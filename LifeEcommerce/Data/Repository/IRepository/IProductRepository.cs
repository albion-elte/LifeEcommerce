using LifeEcommerce.Models.Entities;
using System.Linq.Expressions;

namespace LifeEcommerce.Data.Repository.IRepository
{
    public interface IProductRepository
    {
        IQueryable<Product> GetByCondition(Expression<Func<Product, bool>> expression);
        IQueryable<Product> GetById(Expression<Func<Product, bool>> expression);
        IQueryable<Product> GetAll();
        void Create(Product entity);
        void CreateRange(List<Product> entity);
        void Update(Product entity);
        void UpdateRange(List<Product> entity);
        void Delete(Product entity);
        void DeleteRange(List<Product> entity);
        Task SaveChangesAsync();
    }
}
