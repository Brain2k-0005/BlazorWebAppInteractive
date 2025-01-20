using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlazorWebAppInteractive.Backend.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user with the provided registration model.
        /// </summary>
        /// <param name="registerModel">The input model containing registration details.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> Register(RegisterInputModel registerModel);

        public Task<IdentityResult> Confirm(ApplicationUser user, string token);
        public Task<IdentityResult> ConfirmMail(LoginInputModel input);

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="passwordChangeModel">The model containing old and new password details.</param>
        /// <param name="applicationUser">The user whose password is being changed.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> ChangePassword(PasswordChangeModel passwordChangeModel, ApplicationUser applicationUser);

        /// <summary>
        /// Updates user profile information.
        /// </summary>
        /// <param name="profileModel">The model containing updated profile information.</param>
        /// <param name="applicationUser">The user whose information is being updated.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> ChangeInformation(ProfileModel profileModel, ApplicationUser applicationUser);

        /// <summary>
        /// Initiates the forgot password process by sending a reset email to the user.
        /// </summary>
        /// <param name="email">The email address of the user who forgot their password.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> ForgotPassword(string email);

        /// <summary>
        /// Resets a user's password using the provided token.
        /// </summary>
        /// <param name="token">The password reset token.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The new password to set.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the operation.</returns>
        public Task<IdentityResult> RestPassword(string token, string email, string password);

        /// <summary>
        /// Retrieves a user based on claims principal.
        /// </summary>
        /// <param name="principal">The claims principal identifying the user.</param>
        /// <returns>The <see cref="ApplicationUser"/> object, or null if the user is not found.</returns>
        public Task<ApplicationUser?> GetUserByClaim(ClaimsPrincipal principal);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The <see cref="ApplicationUser"/> object, or null if the user is not found.</returns>
        public Task<ApplicationUser?> GetUserByEmail(string email);

        /// <summary>
        /// Validates user login credentials.
        /// </summary>
        /// <param name="loginInputModel">The input model containing login details.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the validation.</returns>
        public Task<IdentityResult> ValidateLogin(LoginInputModel loginInputModel);

        /// <summary>
        /// Deletes the specified user account from the system.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> object representing the user to be deleted.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous operation. 
        /// The task result contains an <see cref="IdentityResult"/> indicating whether the deletion was successful.
        /// </returns>
        /// <remarks>
        /// Use this method to remove a user account permanently. Ensure that proper authorization checks are
        /// performed before calling this method to prevent unauthorized deletions.
        /// </remarks>
        public Task<IdentityResult> DeleteAccount(ApplicationUser user);
    }
}
