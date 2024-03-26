using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAdmin { get; set; } = false;
        [Column(TypeName = "Date")]
        public DateTime Birthdate { get; set; }
        public string FormattedBirthdate => Birthdate.ToString("d MMMM yyyy");

        public DateTime RegisteredAt { get; set; } = DateTime.Today;
        public string FormattedRegistrationDate => RegisteredAt.ToString("d MMMM yyyy");

        public string ImageUrl { get; set; } = "profile.png";
        public string Contact { get; set; } = "N/A";
        public string Country { get; set; } = "N/A";
        public string City { get; set; } = "N/A";
        public string State { get; set; } = "N/A";
        public string Postal { get; set; } = "N/A";
        public string Street { get; set; } = "N/A";

        public string CompleteAddress => Street + ", " + City + ", " + Postal + ", " + State;

        [NotMapped]
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
