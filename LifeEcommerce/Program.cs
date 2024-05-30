using Amazon.Runtime.Internal.Transform;
using AutoMapper;
using LifeEcommerce.Data;
using LifeEcommerce.Data.Repository.IRepository;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services;
using LifeEcommerce.Services.IService;
using LifeProduct.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

var mapperConfiguration = new MapperConfiguration(
                        mc => mc.AddProfile(new AutoMapperConfigurations()));

IMapper mapper = mapperConfiguration.CreateMapper();

builder.Services.AddSingleton(mapper);

// Add services to the container.
builder.Services.AddDbContext<LifeEcommerceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ImageUploadService>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://sso-sts.gjirafa.dev";
    options.RequireHttpsMetadata = false;
    options.Audience = "life_2024_api";
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

            DateTime birthdateParsed = DateTime.Parse(birthdate);

            var userService = context.HttpContext.RequestServices.GetService<IUnitOfWork>();

            var existingUser = userService.Repository<User>().GetById(x => x.Id == userId).FirstOrDefault();

            if(existingUser == null)
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

                if(emailService != null) 
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Life Ecommerce", Version = "v1"});
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://sso-sts.gjirafa.dev/connect/authorize"),
                TokenUrl = new Uri("https://sso-sts.gjirafa.dev/connect/token"),
                Scopes = new Dictionary<string, string> { { "life_2024_api", "LifeApi" } }
            }
        }
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();
}
);

var app = builder.Build();

var supportedCultures = new[] {"en-US","sq-AL","cs-CS" };
var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.DefaultModelExpandDepth(0);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Life Ecommerce");
        c.OAuthClientId("d8ce3b13-d539-4816-8d07-b1e4c7087fda");
        c.OAuthClientSecret("4a9db740-2460-471a-b3a1-6d86bb99b279");
        c.OAuthAppName("Life Ecommerce");
        c.OAuthUsePkce();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
