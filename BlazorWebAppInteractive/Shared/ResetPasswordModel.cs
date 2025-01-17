using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Shared
{
    public class ResetPasswordModel
    {
        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirm { get; set; } = "";
    }
}
