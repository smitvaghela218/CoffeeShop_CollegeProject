using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }

        [Required(ErrorMessage = "Please enter order date")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date")]
        [DateNotInFuture(ErrorMessage = "Order date cannot be in the future")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "CustomerID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerID must be a valid positive number")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Payment mode is required")]
        [StringLength(50, ErrorMessage = "Payment mode cannot exceed 50 characters")]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be a positive value")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Please enter shipping address")]
        [StringLength(200, ErrorMessage = "Shipping address cannot exceed 200 characters")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        [Range(1, int.MaxValue, ErrorMessage = "UserID must be a valid positive number")]
        public int UserID { get; set; }
    }

    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }

    // Custom validation attribute to ensure the OrderDate is not in the future
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.Today)  // Ensuring order date is not beyond today
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
