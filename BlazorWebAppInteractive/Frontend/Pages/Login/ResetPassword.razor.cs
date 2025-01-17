using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Text;

namespace BlazorWebAppInteractive.Frontend.Pages.Login
{
    public partial class ResetPassword
    {
        /// <summary>
        /// Identity token provided as a query parameter in the URL to identify the user session.
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery(Name = "id")]
        public string IdentityToken { get; set; }

        /// <summary>
        /// Password reset token provided as a query parameter in the URL, used for verification.
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery(Name = "token")]
        public string pwtoken { get; set; }

        /// <summary>
        /// Model to capture the user's input for the new password.
        /// </summary>
        [SupplyParameterFromForm]
        private ResetPasswordModel Input { get; set; } = new();

        /// <summary>
        /// The email address of the user resetting their password.
        /// This is retrieved and validated using the identity token.
        /// </summary>
        private string email = string.Empty;

        /// <summary>
        /// Handles the password reset process by validating the reset token and setting the new password.
        /// </summary>
        private async Task HandleResetPassword()
        {
            // Decode the password reset token from Base64 URL format.
            pwtoken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(pwtoken));

            // Attempt to reset the password using the account service.
            IdentityResult result = await AccountService.RestPassword(pwtoken, email, Input.Password);

            if (result.Succeeded)
            {
                // Navigate to the login page upon successful password reset.
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                // Display error messages for any issues during the reset process.
                result.Errors.ToList().ForEach(x =>
                {
                    Snackbar.Add(x.Description, Severity.Error);
                });
            }
        }

        /// <summary>
        /// Lifecycle method triggered after the component has rendered.
        /// Validates the identity token and retrieves the associated user's email on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates if this is the first render of the component.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Validate the identity token to ensure the session is still valid.
                var result = await TokenService.ValidateToken(IdentityToken);

                if (result == null)
                {
                    // Display an error message if the session has expired.
                    Snackbar.Add("Session expired", Severity.Error);
                    return;
                }

                // Retrieve the email address associated with the validated token.
                email = result.Email;
            }
        }

        // Fields and methods for managing the visibility of the first password input field.
        private bool isShow01; // Flag to track visibility state of the first password field.
        private InputType PasswordInput01 = InputType.Password; // Default input type is password.
        private string PasswordInputIcon01 = Icons.Material.Filled.VisibilityOff; // Default icon for hidden password.

        /// <summary>
        /// Toggles the visibility of the first password field.
        /// </summary>
        private void PW01Vis()
        {
            if (isShow01)
            {
                // Hide the password and update the icon and input type.
                isShow01 = false;
                PasswordInputIcon01 = Icons.Material.Filled.VisibilityOff;
                PasswordInput01 = InputType.Password;
            }
            else
            {
                // Show the password and update the icon and input type.
                isShow01 = true;
                PasswordInputIcon01 = Icons.Material.Filled.Visibility;
                PasswordInput01 = InputType.Text;
            }
        }

        // Fields and methods for managing the visibility of the second password input field (confirmation).
        private bool isShow02; // Flag to track visibility state of the second password field.
        private InputType PasswordInput02 = InputType.Password; // Default input type is password.
        private string PasswordInputIcon02 = Icons.Material.Filled.VisibilityOff; // Default icon for hidden password.

        /// <summary>
        /// Toggles the visibility of the second password field.
        /// </summary>
        private void PW02Vis()
        {
            if (isShow02)
            {
                // Hide the password and update the icon and input type.
                isShow02 = false;
                PasswordInputIcon02 = Icons.Material.Filled.VisibilityOff;
                PasswordInput02 = InputType.Password;
            }
            else
            {
                // Show the password and update the icon and input type.
                isShow02 = true;
                PasswordInputIcon02 = Icons.Material.Filled.Visibility;
                PasswordInput02 = InputType.Text;
            }
        }
    }
}
