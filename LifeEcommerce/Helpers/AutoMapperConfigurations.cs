using AutoMapper;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations() 
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<ProductCreateDto, Product>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
