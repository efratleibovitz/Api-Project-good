using Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Values;
using Repository;
using System.Collections.Generic;
using DTOs;
using AutoMapper;
using StackExchange.Redis;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        //AutoMapper _mapper;
        private readonly IMapper _mapper;
        private readonly WebApiShop216328971Context _shopContext;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;



        public ProductService(IProductRepository productRepository, IMapper mapper, IConnectionMultiplexer redis, WebApiShop216328971Context shopContext)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _redis = redis;
            _db = _redis.GetDatabase();
            _shopContext = shopContext;

        }
        public async Task<List<productDto>> GetProducts(int position, int skip, int? Product_Id, string? name, float? minPrice, float? maxPrice, int[]? CategoryIds, string? description)
        {
            var listProduct =await _productRepository.GetProducts(position, skip,Product_Id,name, minPrice,maxPrice,CategoryIds,description);
            List<productDto> listProductDto = _mapper.Map<List<productDto>>(listProduct);
            return listProductDto;
        }

        public async Task<bool> CanUserRateAsync(int userId, int productId)
        {
            string key = $"rating:{userId}:{productId}:{DateTime.UtcNow:yyyyMMdd}";
            // Try to increment the counter, set expiry to 1 day
            var count = await _db.StringIncrementAsync(key);
            if (count == 1)
                await _db.KeyExpireAsync(key, TimeSpan.FromDays(1));
            return count == 1; // Only allow the first rating per day
        }
        public async Task AddRatingAsync(int userId, int productId, int value)
        {
            var rating = new Rating
            {
                UserId = userId,
                ProductId = productId,
                Value = value,
                Host = "",     
                Method = "",
                Path = "",
                Referer = "",
                UserAgent = "",
                RecordDate = DateTime.UtcNow
            };

            _shopContext.Ratings.Add(rating);
            await _shopContext.SaveChangesAsync();
        }


    }
}
