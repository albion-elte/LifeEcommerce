namespace LifeEcommerce.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Seller {  get; set; }
        public string ImageUrl {  get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public decimal Price { get; set; }
    }
}
