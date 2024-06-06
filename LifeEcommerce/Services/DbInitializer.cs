using LifeEcommerce.Data;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly LifeEcommerceDbContext _lifeEcommerceDbContext;

        public DbInitializer(LifeEcommerceDbContext lifeEcommerceDbContext)
        {
            _lifeEcommerceDbContext = lifeEcommerceDbContext;
        }

        public void Initialize()
        {
            if (_lifeEcommerceDbContext.Database.GetPendingMigrations().Any())
            {
                _lifeEcommerceDbContext.Database.Migrate();
            }

            var users = new List<User>
            {
                new User
                            {
                                Id = Guid.NewGuid().ToString(),
                                FirstName = "",
                                LastName = "user.LastName",
                                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                                PhoneNumber = "user.PhoneNumber",
                                Gender = "user.Gender",
                                EmailAddress = "user.EmailAddress"
                            },
                new User
                            {
                                Id = Guid.NewGuid().ToString(),
                                FirstName = "",
                                LastName = "user.LastName",
                                DateOfBirth = DateOnly.FromDateTime(DateTime.Now),
                                PhoneNumber = "user.PhoneNumber",
                                Gender = "user.Gender",
                                EmailAddress = "user.EmailAddress"
                            },
        };

            _lifeEcommerceDbContext.Users.AddRange(users);
            _lifeEcommerceDbContext.SaveChanges();
    }
}
}
