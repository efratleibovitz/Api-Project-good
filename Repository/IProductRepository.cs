using Entities;

namespace Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(int position, int skip, int? Product_Id, string? name, float? minPrice, float? maxPrice, int[]? CategoryIds, string? description);
    }
}