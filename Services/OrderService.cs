using AutoMapper;
using Confluent.Kafka;
using DTOs;
using Entities;
using FluentNHibernate.Automapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository;
using System.Threading.Tasks;
using System.Text.Json;
namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IKafkaProducerService _kafkaProducer;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository, ILogger<OrderService> logger, IKafkaProducerService kafkaProducer)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _logger = logger;
            _kafkaProducer = kafkaProducer;
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

            await _kafkaProducer.PublishOrderCreatedAsync(savedOrder);

            return _mapper.Map<OrderDto>(savedOrder);
        }

    }
}
