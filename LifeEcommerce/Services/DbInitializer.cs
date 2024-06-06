using LifeEcommerce.Data;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly LifeEcommerceDbContext _db;

        public DbInitializer(LifeEcommerceDbContext db)
        {
            _db = db;
        }


        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "",
                LastName = "user.LastName",
                EmailAddress = "user.EmailAddress",
                PhoneNumber = "PhoneNumber",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Now.Date),
                Gender = "user.Gender"
            };
        }
    }
}
