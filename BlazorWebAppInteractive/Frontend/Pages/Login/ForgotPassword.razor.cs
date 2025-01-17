using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Pages.Login
{
    public partial class ForgotPassword
    {
        /// <summary>
        /// Model to capture the user's email input from the form.
        /// </summary>
        [SupplyParameterFromForm]
        private ForgotPassowordInputModel Input { get; set; } = new();

        /// <summary>
        /// A flag to indicate if the reset password process is currently in progress.
        /// </summary>
        private bool isSend = false;

        /// <summary>
        /// Handles the process of initiating a password reset.
        /// Sends a reset email if the provided email is valid.
        /// </summary>
        private async Task HandleResetPassword()
        {
            isSend = true; // Set the flag to true to indicate processing has started.

            // Call the AccountService to send a reset password email.
            IdentityResult result = await AccountService.ForgotPassword(Input.Email);

            if (result.Succeeded)
            {
                // Inform the user that the reset email has been sent.
                Snackbar.Add("Please check your mailbox", Severity.Info);
            }
            else
            {
                // Display error messages if the operation fails.
                result.Errors.ToList().ForEach(error =>
                {
                    Snackbar.Add(error.Description, Severity.Error);
                });
            }

            isSend = false; // Reset the flag to false to indicate processing is complete.
        }
    }
}