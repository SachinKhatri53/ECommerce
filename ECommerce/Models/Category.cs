using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName {  get; set; }
        public string Description { get; set; }
        public IList<Product> Products { get; set; } = new List<Product>();

    }
}
