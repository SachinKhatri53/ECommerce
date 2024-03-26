using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; } = "profule.png";
        public int Rate { get; set; }
    }
}
