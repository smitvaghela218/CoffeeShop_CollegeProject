using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        [StringLength(10, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter product price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be a positive value")]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage = "Please enter product code")]
        [StringLength(20, ErrorMessage = "Product code cannot exceed 20 characters")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Please enter product description")]
        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        [Range(1, int.MaxValue, ErrorMessage = "UserID must be a valid positive number")]
        public int UserID { get; set; }
    }

    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
