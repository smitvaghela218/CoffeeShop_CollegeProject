using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class NewProductModel
    {
        public int? ProductID { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter product price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please select product category")]
        public string Category { get; set; } /* (Note: Checkboxes) */

        [Required(ErrorMessage = "Please enter product rating")] /* (Note: Number upto 5) */
        public double Rating { get; set; }

        [Required(ErrorMessage = "Please enter product description")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Please enter product company")]
        public string ProductCompany { get; set; } /* (Note: Dropdown) */


    }
}
