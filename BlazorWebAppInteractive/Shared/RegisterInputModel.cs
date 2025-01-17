using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Shared
{
    public class RegisterInputModel
    {
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string Firstname { get; set; } = "";

        [Required, MaxLength(100)]
        public string Lastname { get; set; } = "";

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirm { get; set; } = "";
    }
}
