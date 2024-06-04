using AutoMapper;
using LifeEcommerce.Models.Dtos.Category;
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


            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

            CreateMap<ShoppingCard, ShoppingCardCreateDto>().ReverseMap();
            CreateMap<ShoppingCardCreateDto, ShoppingCard>().ReverseMap();

            CreateMap<ShoppingCard, ShoppingCardDto>().ReverseMap();
            CreateMap<ShoppingCardDto, ShoppingCard>().ReverseMap();
        }
    }
}
