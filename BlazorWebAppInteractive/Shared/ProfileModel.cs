using BlazorWebAppInteractive.Backend.Data.Models;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorWebAppInteractive.Shared
{
    public class ProfileModel : INotifyPropertyChanged
    {
        private string _firstname;
        private string _lastname;
        private string _email;
        private bool _emailNotifications = true;
        private bool _smsNotifications = false;
        private bool _websiteNotifications = false;
        private bool _darkMode = false;
        private string? _profilePicturePath;
        private ColorPreset _colorPreset = ColorPreset.Solar;

        public string Firstname
        {
            get => _firstname;
            set => SetProperty(ref _firstname, value);
        }

        public string Lastname
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public bool EmailNotifications
        {
            get => _emailNotifications;
            set => SetProperty(ref _emailNotifications, value);
        }

        public bool SmsNotifications
        {
            get => _smsNotifications;
            set => SetProperty(ref _smsNotifications, value);
        }

        public bool WebsiteNotifications
        {
            get => _websiteNotifications;
            set => SetProperty(ref _websiteNotifications, value);
        }

        public bool DarkMode
        {
            get => _darkMode;
            set => SetProperty(ref _darkMode, value);
        }

        public string? ProfilePicturePath
        {
            get => _profilePicturePath;
            set => SetProperty(ref _profilePicturePath, value);
        }

        public ColorPreset ColorPreset
        {
            get => _colorPreset;
            set => SetProperty(ref _colorPreset, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        public static bool AreModelsEqual(ProfileModel obj1, ProfileModel obj2)
        {
            if (obj1 == null || obj2 == null)
                return obj1 == obj2;

            var type = typeof(ProfileModel);

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .All(prop => Equals(prop.GetValue(obj1), prop.GetValue(obj2)));
        }
    }
}
