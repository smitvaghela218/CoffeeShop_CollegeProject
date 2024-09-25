using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class BillsModel
    {
        public int? BillID { get; set; }

        [Required(ErrorMessage = "Please enter bill number")]
        public string BillNumber { get; set; }

        [Required(ErrorMessage = "Please enter bill date")]
        public DateTime BillDate { get; set; }

        [Required(ErrorMessage = "Please select order")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please enter total amount")]
        public double TotalAmount { get; set; }

        public double? Discount { get; set; }

        [Required(ErrorMessage = "Please enter net amount")]
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "Please select user")]
        public int UserID { get; set; }
    }
}
