using BlazorWebAppInteractive.Backend.Data;
using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Backend.IServices;
using BlazorWebAppInteractive.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Security.Claims;
using System.Text;

namespace BlazorWebAppInteractive.Backend.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        ITokenService _tokenService;

        public AccountService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IEmailSender emailSender, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _userStore = new UserStore<ApplicationUser>(_context);
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _tokenService = tokenService;
        }

        public async Task<IdentityResult> Register(RegisterInputModel registerModel)
        {
            try
            {
                // Create a new user instance using the factory method.
                var user = CreateUser();

                // Set the username using a combination of first name, last name, and email.
                await _userStore.SetUserNameAsync(user, $"{registerModel.Firstname}_{registerModel.Lastname}_{registerModel.Email}", CancellationToken.None);

                // Set the email address for the user.
                await _emailStore.SetEmailAsync(user, registerModel.Email, CancellationToken.None);

                // Assign first name and last name to the user.
                user.Firstname = registerModel.Firstname;
                user.Lastname = registerModel.Lastname;

                // Attempt to create the user in the identity system.
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                // If user creation fails, return the result containing error details.
                if (!result.Succeeded)
                {
                    return result;
                }

                // Assign the "role1" role to the newly created user.
                var roleResult = await _userManager.AddToRoleAsync(user, "role1");

                // If role assignment fails, return the result containing error details.
                if (!roleResult.Succeeded)
                {
                    return roleResult;
                }

                var confirmEmail = await ConfirmMail(new LoginInputModel { Email = registerModel.Email, Password = registerModel.Password });

                if (!confirmEmail.Succeeded)
                {
                    IdentityResult.Failed(new IdentityError
                    {
                        Code = "####",
                        Description = "Account successfully created.\r\nBut no confirmation email could be sent. Please visit /confirm"
                    });

                }


                // If all steps succeed, return a successful identity result.
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failed identity result with the exception details.
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "#500", // Internal server error code.
                    Description = ex.Message // Provide the exception message as the description.
                });
            }
        }

        public async Task<IdentityResult> DeleteAccount(ApplicationUser user)
        {
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####",
                    Description = "User not found"
                });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####",
                    Description = "Could not delete user"
                });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Confirm(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> ConfirmMail(LoginInputModel input)
        {
            try
            {
                // Attempt to find the user by their email address.
                var user = await _userManager.FindByEmailAsync(input.Email);

                // If no user is found, return a failed result with an error message.
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code for incorrect login.
                        Description = "Login information is not correct." // Error message for invalid login.
                    });
                }

                // Verify the provided password against the stored hash.
                if (!await _userManager.CheckPasswordAsync(user, input.Password))
                {
                    // If the password is incorrect, return a failed result with an error message.
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code for incorrect login.
                        Description = "Login information is not correct." // Error message for invalid login.
                    });
                }

                // Check if the user's email is confirmed.
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    // If the email is not confirmed, return a failed result with an error message.
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code for unconfirmed email.
                        Description = "Your account is already confirmed" // Error message prompting email confirmation.
                    });
                }

                var confirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                confirmtoken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmtoken));

                var identitytoken = await _tokenService.GenerateSecureToken(user);

                var emailresult = await _emailSender.SendConfirmation(user, $"https://localhost:7221/confirm?id={identitytoken}&token={confirmtoken}");

                if (!emailresult.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "####",
                        Description = "Email could not be send"
                    });
                }

                // If all checks pass, return a successful identity result.
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failed identity result with the exception details.
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####", // General error code for exceptions.
                    Description = ex.Message // Provide the exception message as the description.
                });
            }
        }

        public async Task<IdentityResult> ChangeInformation(ProfileModel profileModel, ApplicationUser applicationUser)
        {
            try
            {
                // Update the user's first name.
                applicationUser.Firstname = profileModel.Firstname;

                // Update the user's last name.
                applicationUser.Lastname = profileModel.Lastname;

                // Update the user's email address.
                applicationUser.Email = profileModel.Email;

                // Update the user's selected color preset.
                applicationUser.ColorPreset = profileModel.ColorPreset;

                // Save the updated user information in the identity system.
                return await _userManager.UpdateAsync(applicationUser);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failed identity result with the exception details.
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "#500", // Internal server error code.
                    Description = ex.Message // Provide the exception message as the description.
                });
            }
        }

        public async Task<IdentityResult> ChangePassword(PasswordChangeModel passwordChangeModel, ApplicationUser applicationUser)
        {
            try
            {
                // Verify the user's current password.
                if (!await _userManager.CheckPasswordAsync(applicationUser, passwordChangeModel.CurrentPassword))
                {
                    // If the current password does not match, return a failed result with an error message.
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code indicating password mismatch.
                        Description = "Your current password does not match!" // Error message to inform the user.
                    });
                }

                // Generate a password reset token for the user.
                var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

                // Reset the user's password using the generated token and the new password.
                var result = await _userManager.ResetPasswordAsync(applicationUser, token, passwordChangeModel.NewPassword);

                // Return the result of the password reset operation.
                return result;
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failed identity result with the exception details.
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "#500", // Internal server error code.
                    Description = ex.Message // Provide the exception message as the description.
                });
            }
        }

        public async Task<ApplicationUser?> GetUserByClaim(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> ValidateLogin(LoginInputModel loginInputModel)
        {
            try
            {
                // Attempt to find the user by their email address.
                var user = await _userManager.FindByEmailAsync(loginInputModel.Email);

                // If no user is found, return a failed result with an error message.
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code for incorrect login.
                        Description = "Login information is not correct." // Error message for invalid login.
                    });
                }

                // Verify the provided password against the stored hash.
                if (!await _userManager.CheckPasswordAsync(user, loginInputModel.Password))
                {
                    // If the password is incorrect, return a failed result with an error message.
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#000", // Custom error code for incorrect login.
                        Description = "Login information is not correct." // Error message for invalid login.
                    });
                }

                // Check if the user's email is confirmed.
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    // If the email is not confirmed, return a failed result with an error message.
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "#405", // Custom error code for unconfirmed email.
                        Description = "Please confirm your registration first.\r\nunder /registration/confirm" // Error message prompting email confirmation.
                    });
                }

                // If all checks pass, return a successful identity result.
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failed identity result with the exception details.
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####", // General error code for exceptions.
                    Description = ex.Message // Provide the exception message as the description.
                });
            }
        }
        public async Task<IdentityResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####",
                    Description = "Could not send email"
                });
            }

            var pwtoken = await _userManager.GeneratePasswordResetTokenAsync(user);
            pwtoken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(pwtoken));
            var IdToken = await _tokenService.GenerateSecureToken(user);

            return await _emailSender.SendResetPassword(user, $"https://localhost:7221/password/reset?id={IdToken}&token={pwtoken}");
        }
        public async Task<IdentityResult> RestPassword(string token, string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "####",
                    Description = "Resetting password failed"
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            return result;
        }
        #region Helper
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
        #endregion
    }
}