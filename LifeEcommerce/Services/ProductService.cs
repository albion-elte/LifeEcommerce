using AutoMapper;
using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LifeEcommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProduct(ProductCreateDto product)
        {
            var productToCreate = _mapper.Map<Product>(product);

            _unitOfWork.Repository<Product>().Create(productToCreate);

            _unitOfWork.Complete();
        }

        public async Task DeleteProduct(int id)
        {
            var productToDelete = await _productRepository.GetById(x => x.Id == id).FirstOrDefaultAsync();

            _productRepository.Delete(productToDelete);

            await _productRepository.SaveChangesAsync();
        }

        public Task<List<Product>> GetAllProducts()
        {
            var products = _productRepository.GetAll();

            return products.ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            Expression<Func<Product, bool>> expression = product => product.Id == id;

            var product = _productRepository.GetById(expression);

            return await product.FirstAsync();
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

        public async Task UpdateProduct(ProductDto product)
        {
            var productToUpdate = await GetProduct(product.Id);

            if (productToUpdate != null)
            {
                productToUpdate.Price = product.Price;
                productToUpdate.Seller = product.Seller;
                productToUpdate.Name = product.Name;
                productToUpdate.ImageUrl = product.ImageUrl;
                productToUpdate.CategoryId = product.CategoryId;
            }

            _productRepository.Update(productToUpdate);

            await _productRepository.SaveChangesAsync();
        }
    }
}
