using System;
using System.ComponentModel.DataAnnotations;

namespace PositionTracking.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Display(Name = "Rememeber me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}

