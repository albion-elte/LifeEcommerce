using System.ComponentModel.DataAnnotations;

namespace LifeEcommerce.Models.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
