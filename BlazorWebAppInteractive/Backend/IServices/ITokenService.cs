using BlazorWebAppInteractive.Backend.Data.Models;

namespace BlazorWebAppInteractive.Backend.IServices
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a secure token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A string representing the generated secure token.</returns>
        Task<string> GenerateSecureToken(ApplicationUser user, int timeInMinutes = 5);

        /// <summary>
        /// Validates a given token and retrieves the associated user if the token is valid.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <returns>The <see cref="ApplicationUser"/> associated with the token, or null if the token is invalid.</returns>
        Task<ApplicationUser?> ValidateToken(string token);

        /// <summary>
        /// Clears the token associated with the specified user.
        /// </summary>
        /// <param name="user">The user whose token should be cleared.</param>
        /// <returns>A boolean indicating whether the token was successfully cleared.</returns>
        Task<bool> ClearToken(ApplicationUser user);
    }
}
