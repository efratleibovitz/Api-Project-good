using Entities;

namespace Repository
{
    public interface IProductRepository
    {
        Task<(List<Product> Items, int TotalCount)> GetProducts(int? pId, string? name, int position, int skip, float? minPrice, float? maxPrice, string? desc, int?[] categoryIds);
    }
}