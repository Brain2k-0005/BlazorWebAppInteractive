namespace BlazorWebAppInteractive.Backend.Services
{
    /// <summary>
    /// The ProfilePictureService class manages the user's profile picture.
    /// It provides a mechanism to update the profile picture and notify subscribers
    /// asynchronously about changes.
    /// </summary>
    public class ProfilePictureService
    {
        // Default profile picture URL.
        private string _currentprofilepicture = "https://picsum.photos/200/200";

        /// <summary>
        /// Event triggered when the profile picture changes. Subscribers can register
        /// to be notified asynchronously of updates.
        /// </summary>
        public event Func<Task>? OnProfileChangedAsync;

        /// <summary>
        /// Gets or sets the current profile picture URL. When set, it triggers the
        /// <see cref="OnProfileChangedAsync"/> event to notify subscribers of the update.
        /// </summary>
        public string CurrentProfilepicture
        {
            get => _currentprofilepicture; // Retrieve the current profile picture URL.
            set
            {
                _currentprofilepicture = value; // Update the profile picture URL.
                NotifyThemeChanged(); // Notify subscribers about the change.
            }
        }

        /// <summary>
        /// Notifies all registered subscribers about the profile picture change asynchronously.
        /// Catches and ignores exceptions for components that have been disposed.
        /// </summary>
        private async void NotifyThemeChanged()
        {
            if (OnProfileChangedAsync != null) // Check if there are subscribers.
            {
                foreach (var subscriber in OnProfileChangedAsync.GetInvocationList()) // Loop through all subscribers.
                {
                    try
                    {
                        // Invoke each subscriber asynchronously.
                        await ((Func<Task>)subscriber)();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore exceptions for disposed components.
                    }
                }
            }
        }
    }
}