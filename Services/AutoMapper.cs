
using AutoMapper;
using DTOs;
using Entities;
using NHibernate.Mapping.ByCode.Impl;
using Repository;

namespace Services
{
    public class AutoMapper :Profile
    {       
       public AutoMapper() {

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UpdateUser>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<Product, productDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

        }
    }
}



