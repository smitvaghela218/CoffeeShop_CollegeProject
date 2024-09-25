using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{

    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
    }

    public class UserModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "User Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password should be between 6 and 12 characters")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters")]
        [MaxLength(12, ErrorMessage = "Password cannot exceed 12 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please enter user address")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Please enter active status")]
        public bool IsActive { get; set; }
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
        
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
