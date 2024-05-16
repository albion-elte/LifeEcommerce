using AutoMapper;
using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LifeEcommerce.Services.IService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task CreateProduct(ProductCreateDto product)
        {
            var productToCreate = _mapper.Map<Product>(product);
             
            _productRepository.Create(productToCreate);

            await _productRepository.SaveChangesAsync();
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

        public async Task<PagedInfo<ProductDto>> ProductsListView(string search, int page, int pageSize, int categoryId)
        {
            Expression<Func<Product, bool>> condition = x => x.Name.Contains(search) || x.Description.Contains(search);

            IQueryable<Product> products;

            if (categoryId is not 0) 
            {
                products = _productRepository.GetByCondition(x => x.CategoryId == categoryId);
            }
            else
            {
                products = _productRepository.GetAll();
            }

            products = products.WhereIf(!string.IsNullOrEmpty(search), condition);

            var productsList = await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedProducts = _mapper.Map<List<ProductDto>>(productsList);

            var pagedProducts = new PagedInfo<ProductDto>()
            {
                TotalCount = await products.CountAsync(),
                Page = page,
                PageSize = pageSize,
                Items = mappedProducts
            };

            return pagedProducts;
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
