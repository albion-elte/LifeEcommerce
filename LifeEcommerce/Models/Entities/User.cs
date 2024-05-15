using System.ComponentModel.DataAnnotations;

namespace LifeEcommerce.Models.Entities
{
    public class User
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(320)]
        public string EmailAddress { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

    }
}
