namespace LifeEcommerce.Data.Repository.IRepository
{
    public interface IEcommerceRepository<Tentity> where Tentity : class
    {
        IQueryable<Tentity> GetAll();

        void Create(Tentity entity);
        void CreateRange(List<Tentity> entity);
        void Update(Tentity entity);
        void UpdateRange(List<Tentity> entity);
        void Delete(Tentity entity);
        void DeleteRange(List<Tentity> entity);
    }
}
