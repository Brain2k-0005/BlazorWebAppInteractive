using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Themes
{
    public class QuartzTheme
    {
        public static readonly MudTheme Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Background = Colors.Gray.Lighten5, // Light background for the theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Pink.Default, // Main pink color
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Purple.Accent2, // Supporting purple accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.BlueGray.Default, // Third accent color for highlights
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Pink.Lighten2, // Info elements (e.g., tooltips, notifications)
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Lighten1, // Success messages and indicators
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Default, // Warnings and important alerts
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Default, // Error or critical message color
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken2, // Text or icons for darker areas
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Darken3, // Default text color
                TextSecondary = Colors.Gray.Darken1, // Secondary (less important) text

                Surface = Colors.Shades.White, // Card and panel surfaces
                AppbarBackground = Colors.Pink.Darken2, // Application bar background
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Lighten3, // Drawer (menu) background
                DrawerText = Colors.Gray.Darken3,
            },
            PaletteDark = new PaletteDark
            {
                Background = Colors.Gray.Darken4, // Darker background for the theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Pink.Darken1, // Main pink color for dark mode
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Purple.Accent2, // Supporting purple accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.BlueGray.Darken2, // Third accent color for highlights
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Pink.Darken2, // Info elements in dark mode
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Darken1, // Success messages in dark mode
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Darken1, // Warnings in dark mode
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Darken1, // Errors in dark mode
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken4, // Dark text for very dark surfaces
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Lighten3, // Default text color for dark mode
                TextSecondary = Colors.Gray.Lighten2, // Secondary text in dark mode

                Surface = Colors.Gray.Darken3, // Card and panel surfaces in dark mode
                AppbarBackground = Colors.Pink.Darken3, // Appbar background in dark mode
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Darken2, // Drawer (menu) background
                DrawerText = Colors.Shades.White,
            },
            ZIndex = new ZIndex { Snackbar = 3000, Drawer = 10000 }
        };

    }
}
