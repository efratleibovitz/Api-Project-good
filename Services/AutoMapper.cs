
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

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserEmail)) // מיפוי עבור Get
                .ReverseMap() 
                .ForPath(dest => dest.UserEmail, opt => opt.MapFrom(src => src.UserName)); CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<User, LoginDTO>().ReverseMap();
            CreateMap<Product, productDto>()
              .ForMember(dest => dest.Category_Name,
               opt => opt.MapFrom(src => src.Category.CategoryName));
            //CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderSum, opt => opt.MapFrom(src => src.OrederSum)) // מגשר על טעות הכתיב
                .ReverseMap();
        }
    }
}



