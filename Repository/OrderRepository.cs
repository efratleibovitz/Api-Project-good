using System.Reflection.Metadata;
using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebApiShop216328971Context _shopContext;
        public OrderRepository(WebApiShop216328971Context context)
        {
            _shopContext = context;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _shopContext.Orders.FirstOrDefaultAsync(o => o.OrderId== id);
        }

       
        public async Task<Order> addOrder(Order? order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null");

            await _shopContext.Orders.AddAsync(order);
            await _shopContext.SaveChangesAsync();
            return order;
        }

    }
}
