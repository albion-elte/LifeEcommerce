using LifeEcommerce.Models.Dtos.Product;

namespace LifeEcommerce.Models.Dtos.ShoppingCard
{
    public class ShoppingCardItemsDto
    {
        public List<ShoppingCardDto> ShoppingCardItems { get; set; }

        public decimal TotalOnCard { get; set; }
    }
}
