using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Dtos.ShoppingCard;
using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Services.IService
{
    public interface IShoppingCardService
    {
        Task CreateShoppingCard(ShoppingCardCreateDto shoppingCard);
        Task DeleteShoppingCard(int id);
        Task<ShoppingCard> GetShoppingCard(int id);
        Task UpdateShoppingCard(ShoppingCardDto shoppingCard);
        Task<List<ShoppingCard>> GetAllShoppingCards();
        Task<PagedInfo<ShoppingCardDto>> ShoppingCardListView(string search, int page, int pageSize, int categoryId = 0);
        Task<ShoppingCardItemsDto> GetUsersShoppingCardItems(string? userId);
        Task IncreaseItemCount(int shoppingCardItemId);
        Task DecreaseItemCount(int shoppingCardItemId);
        Task EmptyShoppingCard(string? userId);
    }
}
