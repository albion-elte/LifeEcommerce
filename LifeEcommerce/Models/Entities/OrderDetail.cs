using System.ComponentModel.DataAnnotations.Schema;

namespace LifeEcommerce.Models.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderData OrderData { get; set; }

        public int ProductId {  get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
