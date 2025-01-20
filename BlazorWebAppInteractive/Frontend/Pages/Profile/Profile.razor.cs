using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Frontend.Dialogs;
using BlazorWebAppInteractive.Frontend.Snackbar;
using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using MudBlazor;
using SixLabors.ImageSharp;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

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
                        Phonenumber = _user.PhoneNumber,
                        Email = _user.Email,
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
                Phonenumber = source.Phonenumber,
                Email = source.Email,
                ColorPreset = source.ColorPreset,
                ProfilePicturePath = source.ProfilePicturePath,
            };
        }

        private async Task ConfirmDeleteAccount()
        {
            var confirmed = await DialogService.ShowMessageBox(
                title: "Confirm Deletion",
                message: "Are you sure you want to delete your account? This action cannot be undone.",
                yesText: "Delete",
                cancelText: "Cancel"
            );

            if (confirmed == null)
            {
                Snackbar.Add("Action cancelled", Severity.Info);
                return;
            }

            if (confirmed == false)
            {
                Snackbar.Add("Account deletion cancelled", Severity.Info);
                return;
            }

            if (confirmed == true)
            {
                var result = await AccountService.DeleteAccount(_user);

                if (result.Succeeded)
                {
                    Snackbar.Add("Account deleted successfully", Severity.Success);
                    await Task.Delay(2000).ContinueWith(_ =>
                    {
                        NavigationManager.NavigateTo("/logout");
                    });
                }
                else
                {
                    result.Errors.ToList().ForEach(x =>
                    {
                        Snackbar.Add(x.Description, Severity.Error);
                    });

                }
            }
        }

        private void OpenEditDialog()
        {
            DialogService.Show<ProfileImageChange>(title: "Upload Image");
        }

        private string SelectedFileType = "json";
        private string? DownloadLink;

        private async Task GenerateDownloadLink()
        {
            // Serialize user data to JSON
            string dataJson = JsonSerializer.Serialize(new
            {
                Id = _user.Id,
                Username = _user.UserName,
                Firstname = _user.Firstname,
                Lastname = _user.Lastname,
                Email = _user.Email,
                EmailConfirmed = _user.EmailConfirmed,
                Phonenumber = _user.PhoneNumber,
                PhoneNumberConfirmed = _user.PhoneNumberConfirmed,
                TwoFactorEnabled = _user.TwoFactorEnabled,
            },
            new JsonSerializerOptions { WriteIndented = true });

            // Prepare file content and metadata
            string fileContent;
            string fileExtension;

            if (SelectedFileType == "txt")
            {
                fileContent = dataJson.Replace("{", "").Replace("}", "").Replace(",", Environment.NewLine);
                fileExtension = "txt";
            }
            else if (SelectedFileType == "json")
            {
                fileContent = dataJson;
                fileExtension = "json";
            }
            else
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            string fileName = $"AccountData_{_user.UserName}_{DateTime.UtcNow:yyyyMMddHHmmss}.{fileExtension}";
            var tempFilePath = Path.Combine("wwwroot/personal_data", fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));

            // Save the file
            await File.WriteAllBytesAsync(tempFilePath, Encoding.UTF8.GetBytes(fileContent));

            // Generate a download link
            DownloadLink = $"/personal_data/{fileName}";

            DialogService.Show<DownloadDialog>(title: "Download Account Data", parameters: new DialogParameters
            {
                { "DownloadLink", DownloadLink },
                { "FileName", fileName }
            }, new DialogOptions
            {
                BackdropClick = false,
                NoHeader = true,
            });
        }
    }
}