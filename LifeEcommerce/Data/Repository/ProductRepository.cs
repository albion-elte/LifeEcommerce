using LifeEcommerce.Data;
using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Models.Entities;
using System.Linq.Expressions;

namespace LifeProduct.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly LifeEcommerceDbContext _dbContext;

        public ProductRepository(LifeEcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Product entity)
        {
            _dbContext.Set<Product>().Add(entity);
        }

        public void CreateRange(List<Product> entities)
        {
            _dbContext.Set<Product>().AddRange(entities);
        }

        public void Delete(Product entity)
        {
            _dbContext.Set<Product>().Remove(entity);
        }

        public void DeleteRange(List<Product> entities)
        {
            _dbContext.Set<Product>().RemoveRange(entities);
        }

        public IQueryable<Product> GetAll()
        {
            var result = _dbContext.Set<Product>();

            return result;
        }

        public IQueryable<Product> GetByCondition(Expression<Func<Product, bool>> expression)
        {
            return _dbContext.Set<Product>().Where(expression);
        }

        public IQueryable<Product> GetById(Expression<Func<Product, bool>> expression)
        {
            return _dbContext.Set<Product>().Where(expression);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Product entity)
        {
            _dbContext.Set<Product>().Update(entity);
        }

        public void UpdateRange(List<Product> entities)
        {
            _dbContext.Set<Product>().UpdateRange(entities);
        }
    }
}
