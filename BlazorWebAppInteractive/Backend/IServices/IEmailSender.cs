using BlazorWebAppInteractive.Backend.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorWebAppInteractive.Backend.IServices
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an event reminder email to the specified user.
        /// </summary>
        /// <param name="user">The user to whom the reminder email is sent.</param>
        /// <param name="message">The content of the reminder email.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> SendEventRemider(ApplicationUser user, string message);

        /// <summary>
        /// Sends an email notification to the specified user about family updates.
        /// </summary>
        /// <param name="user">The user to whom the update email is sent.</param>
        /// <param name="message">The content of the update email.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> SendFamilyUpdated(ApplicationUser user, string message);

        /// <summary>
        /// Sends a password reset email to the specified user.
        /// </summary>
        /// <param name="user">The user to whom the password reset email is sent.</param>
        /// <param name="message">The content of the reset email, including a reset link or instructions.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> SendResetPassword(ApplicationUser user, string message);

        /// <summary>
        /// Sends an email confirmation message to the specified user.
        /// </summary>
        /// <param name="user">The user to whom the confirmation email is sent.</param>
        /// <param name="message">The content of the confirmation email, typically including a confirmation link.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> SendConfirmation(ApplicationUser user, string message);
    }

}
