using System.Reflection.Metadata;
using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        WebApiShop216328971Context _shopContext;
        public ProductRepository(WebApiShop216328971Context context)
        {
            _shopContext = context;
        }

      

        public async Task<(List<Product> Items, int TotalCount)> GetProducts(int? pId, string? name,int position, int skip, float? minPrice, float? maxPrice, string? desc, int?[] categoryIds)
        {
            var query = _shopContext.Products.Where(product =>
            (desc == null ? (true) : (product.Description.Contains(desc)))
            && (name == null ? true : product.ProductName.Contains(name))
            && ((minPrice == null) ? (true) : (product.Price >= minPrice))
            && ((maxPrice == null) ? (true) : (product.Price <= maxPrice))
            && ((categoryIds.Length == 0) ? (true) : (categoryIds.Contains(product.CategoryId))))
            .OrderBy(product => product.Price);

            Console.WriteLine(query.ToQueryString());
            List<Product> products = await query.Skip((position - 1) * skip)
            .Take(skip).Include(product => product.Category).ToListAsync();
            var total = await query.CountAsync();
            return (products, total);
        }

    }
}
