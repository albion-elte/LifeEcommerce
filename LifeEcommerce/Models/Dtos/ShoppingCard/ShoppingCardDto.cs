namespace LifeEcommerce.Models.Dtos.Product
{
    public class ShoppingCardDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Count { get; set; }
        public decimal ItemTotal { get; set; }
    }
}
