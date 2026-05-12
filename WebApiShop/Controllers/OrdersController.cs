using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Entities;
using Services;
using DTOs;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        IOrderService _orderService;
        IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            OrderDto order = await _orderService.GetOrderById(id);
            if (order == null) return NoContent();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Post([FromBody] OrderDto orderDto)
        {
            Order orderEntity = _mapper.Map<Order>(orderDto);
            OrderDto newOrderDto = await _orderService.addOrder(orderEntity);
            if (newOrderDto == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = newOrderDto.orderId },
                new { Message = $"הזמנה מספר {newOrderDto.orderId} בוצעה בהצלחה!", Data = newOrderDto });
        }
    }
}
