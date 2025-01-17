using BlazorWebAppInteractive.Backend.Data;
using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Backend.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppInteractive.Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string secretKey = "o42moN4tFB95DFDGHqyKnCtez0K0atd4";

        public TokenService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        /// <summary>
        /// Generates a signed token using HMAC-SHA256 with the provided content and secret key.
        /// </summary>
        /// <param name="content">The content to be signed.</param>
        /// <param name="secretKey">The secret key used for signing.</param>
        /// <returns>A Base64-encoded string representing the signed token.</returns>
        private string SignToken(string content, string secretKey)
        {
            // Create an HMACSHA256 instance using the provided secret key.
            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretKey));

            // Compute the HMAC hash of the content.
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));

            // Convert the hash to a Base64 string and return it as the signed token.
            return Convert.ToBase64String(hash);
        }

        public async Task<string> GenerateSecureToken(ApplicationUser user, int timeInMinutes = 5)
        {
            // Define the token expiration time (5 minutes from now).
            DateTime expiration = DateTime.UtcNow.AddMinutes(timeInMinutes);

            // Create the token content including the user ID, a unique GUID, and the expiration time.
            string tokenContent = $"{user.Id}|{Guid.NewGuid()}|{expiration}";

            // Sign the token content using the secret key to ensure its integrity.
            string signature = SignToken(tokenContent, secretKey);

            // Combine the token content with the signature to form the complete token.
            string combinedToken = $"{tokenContent}|{signature}";

            // Encode the combined token into a Base64 string for secure transmission or storage.
            string encodedToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(combinedToken));

            // Save the generated token along with its expiration time for the user.
            await SaveToken(user.Id, encodedToken, expiration);

            // Return the Base64-encoded token.
            return encodedToken;
        }

        /// <summary>
        /// Saves a generated token to the database along with its associated user ID and expiration time.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the token belongs.</param>
        /// <param name="token">The token to be saved.</param>
        /// <param name="expiration">The expiration time of the token.</param>
        private async Task SaveToken(string userId, string token, DateTime expiration)
        {
            // Add a new token entry to the database.
            _db.QRTokens.Add(new TokenEntity
            {
                Token = token,          // The generated token.
                UserId = userId,        // The ID of the user associated with the token.
                Expiration = expiration // The expiration time of the token.
            });

            // Save the changes asynchronously to persist the token in the database.
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Validates the signature of a token by comparing it with the expected signature generated using the secret key.
        /// </summary>
        /// <param name="content">The original content of the token to validate.</param>
        /// <param name="signature">The provided signature to compare against.</param>
        /// <param name="secretKey">The secret key used to generate the expected signature.</param>
        /// <returns>
        /// A boolean indicating whether the provided signature matches the expected signature.
        /// </returns>
        private bool ValidateSignature(string content, string signature, string secretKey)
        {
            // Generate the expected signature by signing the content with the secret key.
            string expectedSignature = SignToken(content, secretKey);

            // Compare the provided signature with the expected signature and return the result.
            return expectedSignature == signature;
        }

        public async Task<ApplicationUser?> ValidateToken(string token)
        {
            // Decode the Base64-encoded token.
            string decodedToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));

            // Split the token into its components.
            string[] parts = decodedToken.Split('|');

            // Ensure the token has the correct structure (4 parts: user ID, GUID, expiration, and signature).
            if (parts.Length != 4) return null; // Invalid token.

            // Reconstruct the content part of the token.
            string tokenContent = $"{parts[0]}|{parts[1]}|{parts[2]}";

            // Extract the signature from the token.
            string signature = parts[3];

            // Validate the signature using the secret key.
            if (!ValidateSignature(tokenContent, signature, secretKey))
            {
                return null; // Token has been tampered with.
            }

            // Parse and check the expiration date.
            DateTime expiration = DateTime.Parse(parts[2]);
            if (expiration < DateTime.UtcNow)
            {
                return null; // Token has expired.
            }

            // Check if the token exists in the database.
            var tokenEntity = await _db.QRTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (tokenEntity == null)
            {
                return null; // Token not found in the database.
            }

            // Remove the token from the database to prevent reuse.
            _db.QRTokens.Remove(tokenEntity);
            await _db.SaveChangesAsync();

            // Return the user associated with the token.
            return await _userManager.FindByIdAsync(tokenEntity.UserId);
        }

        public async Task<bool> ClearToken(ApplicationUser user)
        {
            // Retrieve all tokens for the user that have the specified expiration time.
            var tokenEntitys = await _db.QRTokens
                .Where(t => t.UserId == user.Id && t.Expiration == DateTime.Now.AddMinutes(5))
                .ToListAsync();

            // Check if no matching tokens were found.
            if (tokenEntitys == null)
            {
                return false; // No tokens to clear.
            }

            // Remove the retrieved tokens from the database.
            _db.QRTokens.RemoveRange(tokenEntitys);

            // Save changes and return true if at least one token was removed.
            return await _db.SaveChangesAsync() >= 1;
        }
    }
}