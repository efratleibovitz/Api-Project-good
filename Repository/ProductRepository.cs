using System;
using System.Reflection.Metadata;
using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebApiShop216328971Context _shopContext;
        public ProductRepository(WebApiShop216328971Context context)
        {
            _shopContext = context;
        }



        public async Task<List<Product>> GetProducts(int position, int skip, int? Product_Id, string? name, float? minPrice, float? maxPrice, int[]? CategoryIds, string? description)
        {
            var query = _shopContext.Products
         .Where(product =>
             (description == null ? true : product.Description.Contains(description))
             && (name == null ? true : product.ProductName.Contains(name))
             && (minPrice == null ? true : product.Price >= minPrice)
             && (maxPrice == null ? true : product.Price <= maxPrice)
//&& ((CategoryIds == null || CategoryIds.Length == 0) ? true : CategoryIds.Contains(product.CategoryId))
&& (CategoryIds == null || CategoryIds.Length == 0 || (product.CategoryId != null && CategoryIds.Contains(product.CategoryId.Value)))
             )
         .OrderBy(product => product.Price);

            //Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.Skip((position - 1) * skip)
            .Take(skip).Include(product => product.Category).ToListAsync();
            // var total = await query.CountAsync();
            return (products);
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _shopContext.Products.FindAsync(id);
        }
    }
}
