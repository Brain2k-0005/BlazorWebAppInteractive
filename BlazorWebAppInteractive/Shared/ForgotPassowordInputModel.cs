using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Shared
{
    public class ForgotPassowordInputModel
    {
        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
