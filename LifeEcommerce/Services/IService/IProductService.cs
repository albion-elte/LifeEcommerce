using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Services.IService
{
    public interface IProductService
    {
        Task CreateProduct(ProductCreateDto product);
        Task DeleteProduct(int id);
        Task GetProduct(int id);
        Task UpdateProduct(Product product);
        Task<List<Product>> GetAllProducts();
        Task<PagedInfo<ProductDto>> ProductsListView(string search, int page, int pageSize, int categoryId = 0);
    }
}
