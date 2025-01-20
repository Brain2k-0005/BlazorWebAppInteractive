using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Text;

namespace BlazorWebAppInteractive.Frontend.Pages.Login
{
    public partial class ConfirmPage
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public string IdentityToken { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "token")]
        public string ConfirmToken { get; set; }

        private bool isValid = false;
        private ApplicationUser? User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/profile/settings", true);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (string.IsNullOrEmpty(ConfirmToken) && string.IsNullOrEmpty(IdentityToken))
                {
                    isValid = false;
                    return;
                }

                ConfirmToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(ConfirmToken));
                User = await TokenService.ValidateToken(IdentityToken);

                if (User == null)
                {
                    Snackbar.Add("Session expired", Severity.Error);
                    isValid = false;
                    return;
                }

                isValid = true;
                StateHasChanged();
            }
        }

        private async void PressConfirm()
        {
            if (User != null)
            {
                var result = await AccountService.Confirm(User, ConfirmToken);

                if (!result.Succeeded)
                {
                    Snackbar.Add("Failed to Confirm your account", Severity.Error);
                    return;
                }

                Snackbar.Add("Your Account is confirmed", Severity.Success);

                await Task.Delay(2000).ContinueWith(t => NavigationManager.NavigateTo("/login"));
            }
        }


        //Login verfy

        /// <summary>
        /// Model to capture login input fields (email and password) from the form.
        /// </summary>
        [SupplyParameterFromForm]
        private LoginInputModel Input { get; set; } = new();

        // Flag to disable the login button after it's clicked to prevent multiple submissions.
        private bool clicked = false;

        /// <summary>
        /// Handles the login process, including validation and navigation.
        /// </summary>
        private async Task HandleVerification()
        {
            clicked = true; // Disable login button while processing.

            // Validate login credentials through the AccountService.
            var result = await AccountService.ConfirmMail(Input);

            if (!result.Succeeded)
            {
                // Show error messages for validation failures in the Snackbar.
                result.Errors.ToList().ForEach(x =>
                {
                    Snackbar.Add(x.Description, Severity.Error);
                });

                clicked = false; // Re-enable the button for retries.
                return;
            }

            Snackbar.Add("Check your mailbox", Severity.Success);
        }

        // State variables for toggling password visibility.
        bool _revealPassword = false;
        MudBlazor.InputType PasswordInputType = InputType.Password; // Default input type is password.
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff; // Default icon for hidden password.

        /// <summary>
        /// Toggles the visibility of the password field.
        /// </summary>
        void RevealPassword()
        {
            if (_revealPassword)
            {
                // Hide the password and update icon.
                _revealPassword = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInputType = InputType.Password;
            }
            else
            {
                // Show the password and update icon.
                _revealPassword = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInputType = InputType.Text;
            }
        }
    }
}
