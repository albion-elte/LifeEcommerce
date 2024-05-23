using LifeEcommerce.Data.Repository.IRepository;

namespace LifeEcommerce.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IEcommerceRepository<TEntity> Repository<TEntity>() where TEntity : class;

        bool Complete();
    }
}
