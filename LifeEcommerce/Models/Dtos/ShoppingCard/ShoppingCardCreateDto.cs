namespace LifeEcommerce.Models.Dtos.Product
{
    public class ShoppingCardCreateDto
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
