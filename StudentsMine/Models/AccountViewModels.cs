using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StudentsMine.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public virtual RegistrationStatus Verify()
        {
            RegistrationStatus status = new RegistrationStatus(this, true, StudentRegResults.OK, new List<string>());
            status.Result = true;
            var regexFormat = new Regex("^[a-zA-Z0-9 ]*$");
            if (!Mailer.Mailer.VerifyEmail(this.Email))
            {
                status.Result = false;
                status.ErrorMessage += "The email format is inncorrect.";
            };
            if (this.UserName.Length < 8 || this.UserName.Length > 20)
            {
                status.Result = false;
                status.ErrorMessage += "The Name field length is inncorrect(from 8 and until 20 symbols).";
            }
            else if (!regexFormat.IsMatch(this.UserName))
            {
                status.Result = false;
                status.ErrorMessage += "The UserName can hold only Latinica symbols and numbers(A-Z,a-z,0-9).";
            }
            return status;
        }
    }
}
