using LifeEcommerce.Data.Repository;
using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Models.Entities;
using System.Collections;

namespace LifeEcommerce.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LifeEcommerceDbContext _dbContext;

        private Hashtable _repositories;

        public UnitOfWork(LifeEcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Complete()
        {
            var numberOfAffectedRows = _dbContext.SaveChanges();
            return numberOfAffectedRows > 0;
        }

        public IEcommerceRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.Contains(type))
            {
                var repositoryType = typeof(EcommerceRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IEcommerceRepository<TEntity>)_repositories[type];
        }
    }
}
