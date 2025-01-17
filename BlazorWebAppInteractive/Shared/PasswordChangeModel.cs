using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Shared
{
    public class PasswordChangeModel
    {
        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
