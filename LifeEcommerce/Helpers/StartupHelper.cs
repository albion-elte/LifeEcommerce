using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Services.IService;
using LifeEcommerce.Services;
using LifeProduct.Data.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public static class StartupHelper
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCardService, ShoppingCardService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient<ImageUploadService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddLogging(this ILoggingBuilder logging, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            logging.ClearProviders();
            logging.AddSerilog(logger);

        }

        public static void RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration["AuthoritySettings:Authority"];
                options.RequireHttpsMetadata = false;
                options.Audience = configuration["AuthoritySettings:Scope"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://sso-sts.gjirafa.dev",
                    ValidAudience = configuration["AuthoritySettings:Scope"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4a9db740-2460-471a-b3a1-6d86bb99b279")),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        context.HttpContext.User = context.Principal ?? new ClaimsPrincipal();

                        var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var firstName = context.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
                        var lastName = context.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
                        var email = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                        var gender = context.HttpContext.User.FindFirst(ClaimTypes.Gender)?.Value;
                        var birthdate = context.HttpContext.User.FindFirst(ClaimTypes.DateOfBirth)?.Value;
                        var phoneNumber = context.HttpContext.User.FindFirst("phone_number")?.Value;

                        //DateTime birthdateParsed = DateTime.Parse(birthdate);

                        var userService = context.HttpContext.RequestServices.GetService<IUnitOfWork>();

                        var existingUser = userService.Repository<User>().GetById(x => x.Id == userId).FirstOrDefault();

                        if (existingUser == null)
                        {
                            var userToBeAdded = new User
                            {
                                Id = userId,
                                FirstName = firstName,
                                LastName = lastName,
                                EmailAddress = email,
                                Gender = gender,
                                PhoneNumber = phoneNumber ?? " ",
                                DateOfBirth = DateOnly.FromDateTime(DateTime.Now)
                            };

                            userService.Repository<User>().Create(userToBeAdded);

                            var emailService = context.HttpContext.RequestServices.GetService<IEmailSender>();
                            //var hostEnvironment = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>();

                            if (emailService != null)
                            {
                                var subject = "Welcome to Life Ecommerce";
                                var htmlBody = string.Empty;

                                using (StreamReader streamReader = File.OpenText("Templates/EmailTemplate.html"))
                                {
                                    htmlBody = streamReader.ReadToEnd();
                                }

                                string messageBody = string.Format(htmlBody, "Welcome to life Ecommerce",
                                    "Greetings",
                                    userToBeAdded.FirstName,
                                    "An account has been created for you with the following email:",
                                    userToBeAdded.EmailAddress,
                                    "Best regards,",
                                    "Life Team");

                                emailService.SendEmailAsync(userToBeAdded.EmailAddress, subject, messageBody);
                            }
                        }
                        else
                        {
                            existingUser.FirstName = firstName;
                            existingUser.LastName = lastName;
                            existingUser.PhoneNumber = phoneNumber;

                            userService.Repository<User>().Update(existingUser);
                        }

                        userService.Complete();
                    }
                };

                options.ForwardDefaultSelector = Selector.ForwardReferenceToken("token");
            });
        }
    }
}
