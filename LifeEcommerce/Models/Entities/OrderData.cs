using System.ComponentModel.DataAnnotations;

namespace LifeEcommerce.Models.Entities
{
    public class OrderData
    {
        public string Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public string TrackingId { get; set; } = Guid.NewGuid().ToString();
        public string Carrier { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Phone]
        public string PhoneNumber {  get; set; }

        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public short PostalCode { get; set; }
        public string Name { get; set; }
    }
}
