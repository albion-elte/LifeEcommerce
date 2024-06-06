using LifeEcommerce.Data;
using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Helpers
{
    public static class DbMigrationsHelper
    {
        public async static Task<bool> ApplyDbMigrationsWithDataSeed(this IServiceProvider serviceProvider)
        {
            int pendingMigrations = 0;
            bool seedCompleted = false;

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetRequiredService<LifeEcommerceDbContext>())
                {
                    //var pendingMigrationsCount = (await context.Database.GetPendingMigrationsAsync()).Count();

                    //if (pendingMigrationsCount > 0)
                    //{ 
                    //    await context.Database.MigrateAsync();
                    //}

                    await dbContext.Database.MigrateAsync();
                    pendingMigrations += (await dbContext.Database.GetPendingMigrationsAsync()).Count();

                    var dataSeedService = scope.ServiceProvider.GetRequiredService<DataSeed>();

                    var users = new List<User>();

                    if(dbContext.Users != null && dbContext.Users.Count() == 0) 
                    {
                        foreach(var user in dataSeedService.Users)
                        {
                            var newUser = new User
                            {
                                Id = Guid.NewGuid().ToString(),
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                DateOfBirth = user.DateOfBirth,
                                PhoneNumber = user.PhoneNumber,
                                Gender = user.Gender,
                                EmailAddress = user.EmailAddress
                            };

                            users.Add(newUser);
                        }
                    }

                    await dbContext.AddRangeAsync(users);
                    seedCompleted = await dbContext.SaveChangesAsync() > 0;
                }
            }

            return pendingMigrations == 0 && seedCompleted;
        }
    }
}
