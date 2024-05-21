using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Services.IService
{
    public interface IProductService
    {
        Task CreateProduct(ProductCreateDto product);
        Task DeleteProduct(int id);
        Task<Product> GetProduct(int id);
        Task UpdateProduct(ProductDto product);
        Task<List<Product>> GetAllProducts();
        Task<PagedInfo<ProductDto>> ProductsListView(string search, int page, int pageSize, int categoryId = 0);
    }
}
