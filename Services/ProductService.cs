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
        public async Task<List<productDto>> GetProducts(int? pId, string? name, int position, int skip, float? minPrice, float? maxPrice, string? desc, int?[] categoryIds)
        {
            (List<Product>,int) listProduct =await _productRepository.GetProducts(pId, name, position,skip,minPrice,maxPrice, desc,categoryIds);
            List<productDto> listProductDto = _mapper.Map<List<productDto>>(listProduct);
            return listProductDto;
        }

    }
}
