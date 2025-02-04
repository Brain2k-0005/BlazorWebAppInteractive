using Microsoft.AspNetCore.Components;

namespace BlazorWebAppInteractive.Frontend.Pages.Login
{
    public partial class LoginSSR
    {
        /// <summary>
        /// The identity token provided as a query parameter in the URL.
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery(Name = "token")]
        public string IdentityToken { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "rmbrme")]
        public bool RememberMe { get; set; } = false;

        /// <summary>
        /// Lifecycle method that initializes the component.
        /// Attempts to handle the login using the identity token and navigates to the home page.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Attempt to process the login using the token from the query parameter.
                await HandleLogin(IdentityToken);
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the login process.
                Console.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                // Redirect the user to the home page regardless of the login result.
                NavigationManager.NavigateTo("/",true);
            }
        }

        /// <summary>
        /// Handles the login process using the provided token.
        /// </summary>
        /// <param name="token">The token used for user authentication.</param>
        private async Task HandleLogin(string token)
        {
            // Step 1: Validate the token using the TokenProvider service.
            var user = await TokenProvider.ValidateToken(token);

            // If the token is invalid or expired.
            if (user == null)
            {
                return;
            }

            // Step 2: Check if the user is eligible to sign in using the SignInManager.
            if (!await SignInManager.CanSignInAsync(user))
            {
                // If the user is not allowed to sign in.
                return;
            }

            // Step 3: Sign in the user without creating a persistent authentication cookie.
            await SignInManager.SignInAsync(user, isPersistent: RememberMe);
        }
    }

}