using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Shared
{
    public sealed class LoginInputModel
    {
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
