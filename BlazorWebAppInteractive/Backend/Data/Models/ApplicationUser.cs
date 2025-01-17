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

        public bool EmailNotificationsEnabled { get; set; }
        public bool SmsNotificationsEnabled { get; set; }
        public bool WebsiteNotificationsEnabled { get; set; }
        public ColorPreset ColorPreset { get; set; }
        public string? LoginCode { get; set; }
        public Guid? FamilyCode { get; set; }
        public string? ProfilePicturePath { get; set; }
    }

    public enum ColorPreset
    {
        JUSTMAMI,
        Solar,
        Slate,
        Quartz,
        Yeti
    }
}