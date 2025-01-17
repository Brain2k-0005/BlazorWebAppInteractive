using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Pages.Login
{
    public partial class Login
    {
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
        private async Task HandleLogin()
        {
            clicked = true; // Disable login button while processing.

            // Validate login credentials through the AccountService.
            var result = await AccountService.ValidateLogin(Input);

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

            // Retrieve the user based on the provided email.
            var user = await AccountService.GetUserByEmail(Input.Email);
            if (user == null)
            {
                // Show an error if the user cannot be found.
                Snackbar.Add("You could not be logged in!", Severity.Error);
                return;
            }

            // Generate a secure token for the user and navigate to the SSR login endpoint.
            var token = await TokenProvider.GenerateSecureToken(user);
            NavigationManager.NavigateTo($"/login/ssr?token={token}", true);
        }

        // State variables for toggling password visibility.
        bool _revealPassword = false;
        InputType PasswordInputType = InputType.Password; // Default input type is password.
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
