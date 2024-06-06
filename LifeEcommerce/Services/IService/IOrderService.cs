using LifeEcommerce.Models.Dtos.Order;
using System.Security.Claims;

namespace LifeEcommerce.Services.IService
{
    public interface IOrderService
    {
        Task CreateOrder(OrderCreateDto orderCreateDto);
        Task OrderSummary(ClaimsPrincipal user);
    }
}
