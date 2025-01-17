using BlazorWebAppInteractive.Backend.Data.Models;
using BlazorWebAppInteractive.Frontend.Themes;
using MudBlazor;

namespace BlazorWebAppInteractive.Backend.Services
{
    /// <summary>
    /// The ThemeService class is responsible for managing application themes.
    /// It allows updating and notifying subscribers about theme changes, 
    /// initializing themes based on predefined color presets, and dynamically
    /// selecting specific themes.
    /// </summary>
    public class ThemeService
    {
        private MudTheme _currentTheme;

        /// <summary>
        /// Event triggered when the theme changes. Subscribers can register 
        /// to be notified asynchronously of theme updates.
        /// </summary>
        public event Func<Task>? OnThemeChangedAsync;

        /// <summary>
        /// Gets or sets the current application theme.
        /// When the theme is updated, subscribers are notified.
        /// </summary>
        public MudTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                NotifyThemeChanged();
            }
        }

        /// <summary>
        /// Notifies all registered subscribers about the theme change asynchronously.
        /// Catches and ignores exceptions for components that have been disposed.
        /// </summary>
        private async void NotifyThemeChanged()
        {
            if (OnThemeChangedAsync != null)
            {
                foreach (var subscriber in OnThemeChangedAsync.GetInvocationList())
                {
                    try
                    {
                        await ((Func<Task>)subscriber)();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore exceptions for disposed components.
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the theme based on the provided color preset.
        /// </summary>
        /// <param name="preset">The color preset to initialize the theme.</param>
        public void InitializeTheme(ColorPreset preset)
        {
            CurrentTheme = GetThemeByPreset(preset);
        }

        /// <summary>
        /// Retrieves a theme corresponding to a given color preset.
        /// </summary>
        /// <param name="preset">The color preset for which the theme is retrieved.</param>
        /// <returns>The corresponding <see cref="MudTheme"/> for the provided preset.</returns>
        public MudTheme GetThemeByPreset(ColorPreset preset)
        {
            return preset switch
            {
                ColorPreset.JUSTMAMI => DefaultTheme.Theme,
                ColorPreset.Quartz => QuartzTheme.Theme,
                ColorPreset.Solar => SolarTheme.Theme,
                ColorPreset.Slate => SlateTheme.Theme,
                ColorPreset.Yeti => YetiTheme.Theme,
                _ => DefaultTheme.Theme
            };
        }
    }
}