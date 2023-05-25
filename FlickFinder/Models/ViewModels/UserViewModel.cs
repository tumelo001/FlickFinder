
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FlickFinder.Models.ViewModels
{
    public class SignInModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }    
        public string ReturnUrl { get; set; } = "/";

    }
    public class RegisterModel
    {
        [Required]
        [UIHint("User name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "password must match")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

    }

}
