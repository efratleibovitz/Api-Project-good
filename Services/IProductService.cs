using DTOs;
using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<productDto>> GetProducts(int? pId, string? name, int position, int skip, float? minPrice, float? maxPrice, string? desc, int?[] categoryIds);

    }
}