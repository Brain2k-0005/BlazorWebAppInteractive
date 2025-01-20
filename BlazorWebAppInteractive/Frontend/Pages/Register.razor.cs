using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Pages
{
    public partial class Register
    {
        /// <summary>
        /// Model to capture user input for registration, including email, password, and other details.
        /// </summary>
        [SupplyParameterFromForm]
        private RegisterInputModel Input { get; set; } = new();

        /// <summary>
        /// Flag to indicate if the register button has been clicked to prevent multiple submissions.
        /// </summary>
        private bool clicked = false;

        /// <summary>
        /// Handles the registration process by calling the `AccountService`.
        /// Navigates to the login page on success or shows an error message on failure.
        /// </summary>
        private async Task HandleRegister()
        {
            clicked = true;
            // Call the AccountService to register the user with the provided input.
            var result = await AccountService.Register(Input);

            if (result.Succeeded)
            {
                Snackbar.Add("Account successfully created.\r\nPlease confirm your email.", Severity.Success);

               await Task.Delay(2000).ContinueWith(t => NavigationManager.NavigateTo("/login"));
            }
            else
            {
                result.Errors.ToList().ForEach(x =>
                {
                    Snackbar.Add(x.Description, Severity.Error);
                });
            }
            clicked = false;
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