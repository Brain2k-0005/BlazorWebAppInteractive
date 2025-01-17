using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Frontend.Snackbar;
using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SixLabors.ImageSharp;
using System.ComponentModel;

namespace BlazorWebAppInteractive.Frontend.Pages.Profile
{
    public partial class Profile
    {
        private bool isValid;
        private bool passwordValid;
        private ProfileModel profileModel = new ProfileModel();
        private ProfileModel baseModel = new ProfileModel();
        private PasswordChangeModel passwordModel = new PasswordChangeModel();

        private ApplicationUser _user;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                _user = await AccountService.GetUserByClaim(authState.User);
                if (_user != null)
                {
                    profileModel = new ProfileModel
                    {
                        Firstname = _user.Firstname,
                        Lastname = _user.Lastname,
                        Email = _user.Email,
                        EmailNotifications = _user.EmailNotificationsEnabled,
                        SmsNotifications = _user.SmsNotificationsEnabled,
                        WebsiteNotifications = _user.WebsiteNotificationsEnabled,
                        ColorPreset = _user.ColorPreset,
                        ProfilePicturePath = _user.ProfilePicturePath ?? ProfilePictureService.CurrentProfilepicture,
                    };

                    // Usage
                    baseModel = CloneProfileModel(profileModel);


                    StateHasChanged();

                    profileModel.PropertyChanged += HandleProfileChange;
                }
            }
        }
        private void HandleProfileChange(object? sender, PropertyChangedEventArgs e)
        {
            if (!ProfileModel.AreModelsEqual(baseModel, profileModel))
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;

                Snackbar.Add<SaveProfile>(new Dictionary<string, object>()
                            {
                                { "OnSaveClicked", EventCallback.Factory.Create(this, SaveSettings) },
                                { "OnCancelClicked", EventCallback.Factory.Create(this, ResetSetting) }
                            },
                            Severity.Normal,
                            config =>
                            {
                                config.HideIcon = true;
                                config.ShowCloseIcon = false;
                                config.RequireInteraction = false;
                                config.VisibleStateDuration = int.MaxValue;
                                config.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
                                config.HideTransitionDuration = 500;
                            },
                            key: "SAVERESTSNACKBAR");
            }
            else
            {
                Snackbar.RemoveByKey("SAVERESTSNACKBAR");
            }
        }

        private async Task SaveSettings()
        {
            Snackbar.RemoveByKey("SAVERESTSNACKBAR");
            var result = await AccountService.ChangeInformation(profileModel, _user);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x =>
                {
                    Snackbar.Add(x.Description);
                });
            }
            else
            {
                Snackbar.Add("Profile updated successfully");
                baseModel = CloneProfileModel(profileModel);
            }
            StateHasChanged();
        }

        private void ResetSetting()
        {
            profileModel.PropertyChanged -= HandleProfileChange;
            profileModel = CloneProfileModel(baseModel);
            Snackbar.RemoveByKey("SAVERESTSNACKBAR");
            profileModel.PropertyChanged += HandleProfileChange;
        }

        private async Task ChangePassword()
        {
            if (passwordValid)
            {
                var result = await AccountService.ChangePassword(passwordModel, _user);

                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(x =>
                    {
                        Snackbar.Add(x.Description);
                    });
                }
                else
                {
                    Snackbar.Add("Profile updated successfully");

                    var token = await TokenProvider.GenerateSecureToken(_user);
                    NavigationManager.NavigateTo($"/profile/ssr?token={token}", true);
                }
                StateHasChanged();
            }
        }

        private void ColorSwitch()
        {
            ThemeService.CurrentTheme = ThemeService.GetThemeByPreset(profileModel.ColorPreset);
        }

        private IBrowserFile browserFile;

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var uploadPath = Path.Combine($"wwwroot/pictures/profiles/", _user.Id);

            // Ensure the directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save file to the specified path
            var filePath = Path.Combine(uploadPath, file.Name);

            try
            {
                using var memoryStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memoryStream); // 10 MB max size

                // Validate dimensions using ImageSharp
                memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position
                using var image = Image.Load(memoryStream);

                if (image.Width > 200 || image.Height > 200)
                {
                    Snackbar.Add("The image must be smaller than or equal to 200x200 pixels.", Severity.Error);
                    return;
                }

                // Save the image to the specified path
                memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position
                using var fileStream = new FileStream(filePath, FileMode.Create);
                memoryStream.CopyTo(fileStream);

                profileModel.ProfilePicturePath = filePath;
                Snackbar.Add("Profile picture uploaded successfully!", Severity.Success);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error uploading file: {ex.Message}", Severity.Error);
            }
        }

        private ProfileModel CloneProfileModel(ProfileModel source)
        {
            return new ProfileModel
            {
                Firstname = source.Firstname,
                Lastname = source.Lastname,
                Email = source.Email,
                EmailNotifications = source.EmailNotifications,
                SmsNotifications = source.SmsNotifications,
                WebsiteNotifications = source.WebsiteNotifications,
                ColorPreset = source.ColorPreset,
                ProfilePicturePath = source.ProfilePicturePath,
            };
        }
    }
}
