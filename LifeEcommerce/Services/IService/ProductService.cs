using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Services.IService
{
    public class ProductService : IProductService
    {
        public Task CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedInfo<Product>> ProductsListView(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
