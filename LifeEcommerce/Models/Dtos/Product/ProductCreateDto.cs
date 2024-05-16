using LifeEcommerce.Models.Entities;

namespace LifeEcommerce.Models.Dtos.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Seller { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
