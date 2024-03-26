using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Deal
    {
        [Key]
        public int DealId { get; set; }
        public int ProductId { get; set; }
        public decimal DealPrice { get; set; }
        public DateTime StartDate { get; set; }
        public string FormattedStartDate => StartDate.ToString("d MMMM yyyy");
        public DateTime EndDate { get; set; }
        public string FormattedEndDate => EndDate.ToString("d MMMM yyyy");
        public Product? Product { get; set; }
        public string? DealThumbnail { get; set; }
    }
}
