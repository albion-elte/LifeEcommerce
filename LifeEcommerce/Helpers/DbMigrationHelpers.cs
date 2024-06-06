using LifeEcommerce.Data;
using LifeEcommerce.Helpers.Models;
using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LifeEcommerce.Helpers
{
    public static class DbMigrationHelpers
    {
        public static async Task<bool> ApplyDbMigrationsWithDataSeedAsync(this IServiceProvider services)
        {
            int pendingMigrationCount = 0;
            var seedCompleted = false;
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<LifeEcommerceDbContext>())
                {
                    await context.Database.MigrateAsync();
                    pendingMigrationCount += (await context.Database.GetPendingMigrationsAsync()).Count();

                    var seedDataConfiguration = scope.ServiceProvider.GetRequiredService<DataSeed>();

                    var users = new List<User>();
                    if (context.Users != null && context.Users.Count() == 0)
                    {
                        foreach (var user in seedDataConfiguration.Users)
                        {
                            var newUser = new User
                            {
                                Id =Guid.NewGuid().ToString(),
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                EmailAddress = user.EmailAddress,
                                PhoneNumber = user.PhoneNumber,
                                DateOfBirth = user.DateOfBirth,
                                Gender = user.Gender
                            };

                            users.Add(newUser);
                        }

                        await context.Users.AddRangeAsync(users);
                        seedCompleted = await context.SaveChangesAsync() > 0;
                    }
                }
            }

            return pendingMigrationCount > 0 && seedCompleted;
        }
    }
}
