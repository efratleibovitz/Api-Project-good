using AutoMapper;
using Confluent.Kafka;
using DTOs;
using Entities;
using FluentNHibernate.Automapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository;
using System.Text.Json;
namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IConfiguration _configuration;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository, ILogger<OrderService> logger, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<OrderDto> GetOrderById(int id)
        {
            Order order = await _orderRepository.GetOrderById(id);
            OrderDto orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }
        public async Task<OrderDto> addOrder(Order order)
        {
            double calculatedSum = 0;
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                if (product != null)
                {
                    calculatedSum += product.Price * (item.Quantity ?? 0);
                }
            }
            if (calculatedSum != order.OrederSum)
            {
                _logger.LogWarning("Security Alert: Order sum mismatch for User {UserId}. " +
                                   "Received: {ReceivedSum}, Calculated: {CalculatedSum}",
                                   order.UserId, order.OrederSum, calculatedSum);

                order.OrederSum = calculatedSum;
            }

            Order savedOrder = await _orderRepository.addOrder(order);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["KafkaSettings:BootstrapServers"]
            };

            var message = JsonSerializer.Serialize(savedOrder, new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
            var topic = _configuration["KafkaSettings:Topic"];

            using var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            await producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = savedOrder.OrderId.ToString(),
                Value = message
            });

            _logger.LogInformation("Order {OrderId} sent to Kafka topic '{Topic}'", savedOrder.OrderId, topic);

            return _mapper.Map<OrderDto>(savedOrder);
        }

    }
}
