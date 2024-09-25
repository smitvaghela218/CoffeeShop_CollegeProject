using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }

        [Required(ErrorMessage = "Please select an order")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be a valid positive number")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please select a product")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductID must be a valid positive number")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter the amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive value greater than 0")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Please enter the total amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be a positive value greater than 0")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        [Range(1, int.MaxValue, ErrorMessage = "UserID must be a valid positive number")]
        public int UserID { get; set; }
    }
}
