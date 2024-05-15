using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Services.IService
{
    public interface IProductService
    {
        Task CreateProduct(Product product);
        Task DeleteProduct(int id);
        Task GetProduct(int id);
        Task UpdateProduct(Product product);
        Task<List<Product>> GetAllProducts();

        Task<PagedInfo<Product>> ProductsListView(int page, int pageSize); //To Do make fully functional
    }
}
