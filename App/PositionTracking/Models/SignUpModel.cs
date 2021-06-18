using System;
using System.ComponentModel.DataAnnotations;

namespace PositionTracking.Models
{

        public class SignUpModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "REPEAT PASSWORD")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

    }
}