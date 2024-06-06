using Hangfire;
using LifeEcommerce.Data;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerceBackgroundServcices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LifeEcommerceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))); builder.Services.AddHangfireServer();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

var scope  = app.Services.CreateScope();

var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>(); 
LifeService lifeService = new LifeService(unitOfWork);

RecurringJob.AddOrUpdate("UpdateOrderStatus", () => lifeService.UpdateOrderStatus(), Cron.Minutely);


app.Run();
