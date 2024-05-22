using System;
using System.ComponentModel.DataAnnotations;

namespace LifeEcommerce.Models.Entities
{
    public class OrderData
    {
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal OrderTotal { get; set; }

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
