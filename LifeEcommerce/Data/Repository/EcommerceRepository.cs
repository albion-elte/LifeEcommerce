using LifeEcommerce.Data.Repository.IRepository;

namespace LifeEcommerce.Data.Repository
{
    public class EcommerceRepository<Tentity> : IEcommerceRepository<Tentity> where Tentity : class
    {
        private readonly LifeEcommerceDbContext _dbContext;

        public EcommerceRepository(LifeEcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Tentity entity)
        {
            _dbContext.Set<Tentity>().Add(entity);
        }

        public void CreateRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().AddRange(entities);
        }

        public void Delete(Tentity entity)
        {
            _dbContext.Set<Tentity>().Remove(entity);
        }

        public void DeleteRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().RemoveRange(entities);
        }

        public IQueryable<Tentity> GetAll()
        {
            var result = _dbContext.Set<Tentity>();

            return result;
        }

        public void Update(Tentity entity)
        {
            _dbContext.Set<Tentity>().Update(entity);
        }

        public void UpdateRange(List<Tentity> entities)
        {
            _dbContext.Set<Tentity>().UpdateRange(entities);
        }
    }
}
