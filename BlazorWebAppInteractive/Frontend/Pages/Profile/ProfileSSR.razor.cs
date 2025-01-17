using Microsoft.AspNetCore.Components;

namespace BlazorWebAppInteractive.Frontend.Pages.Profile
{
    public partial class ProfileSSR
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "token")]
        public string IdentityToken { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(IdentityToken))
            {
                await HandleLogin(IdentityToken);
            }
            NavigationManager.NavigateTo("/profile/settings", true);
        }

        private async Task HandleLogin(string qrCodeToken)
        {
            // Step 1: Validate the QR code token using the ITokenService
            var user = await TokenProvider.ValidateToken(qrCodeToken);

            if (user == null)
            {
                Console.WriteLine("Invalid or expired QR code token.");
                return;
            }

            // Step 2: Check if the user is allowed to sign in
            if (!await SignInManager.CanSignInAsync(user))
            {
                Console.WriteLine("User cannot sign in at the moment.");
                return;
            }

            // Step 3: Sign in the user
            await SignInManager.RefreshSignInAsync(user);

            Console.WriteLine($"User {user.UserName} successfully logged in!");

            // Note: If the token should be removed or invalidated after use, ensure `ValidateToken` handles that logic
        }
    }
}
