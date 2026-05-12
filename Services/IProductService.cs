using DTOs;
using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<productDto>> GetProducts(int position, int skip, int? Product_Id, string? name, float? minPrice, float? maxPrice, int[]? CategoryIds, string? description);
        Task<bool> CanUserRateAsync(int userId, int productId);
        Task AddRatingAsync(int userId, int productId, int value);
    }
}