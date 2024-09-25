using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [StringLength(200, ErrorMessage = "Home address cannot exceed 200 characters")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please enter GST number")]
        [StringLength(15, ErrorMessage = "GST number cannot exceed 15 characters")]
        public string GST_NO { get; set; }

        [Required(ErrorMessage = "Please enter city name")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please enter pincode")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be exactly 6 digits")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Please enter net amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Net amount must be a positive value")]
        public double NetAmount { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        [Range(1, int.MaxValue, ErrorMessage = "UserID must be a valid positive number")]
        public int UserID { get; set; }
    }

    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
