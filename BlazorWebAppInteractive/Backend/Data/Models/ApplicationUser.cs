using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebAppInteractive.Backend.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string Firstname { get; set; }

        [Required, MaxLength(50)]
        public string Lastname { get; set; }
        public ColorPreset ColorPreset { get; set; }
        public string? ProfilePicturePath { get; set; }
    }

    public enum ColorPreset
    {
        Default,
        Solar,
        Slate,
        Quartz,
        Yeti
    }
}