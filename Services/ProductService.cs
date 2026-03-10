using Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Testing.Values;
using Repository;
using System.Collections.Generic;
using DTOs;
using AutoMapper;

namespace Services
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        //AutoMapper _mapper;
        IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }
        public async Task<List<productDto>> GetProducts(int position, int skip, int? Product_Id, string? name, float? minPrice, float? maxPrice, int[]? CategoryIds, string? description)
        {
            var listProduct =await _productRepository.GetProducts(position, skip,Product_Id,name, minPrice,maxPrice,CategoryIds,description);
            List<productDto> listProductDto = _mapper.Map<List<productDto>>(listProduct);
            return listProductDto;
        }

    }
}
