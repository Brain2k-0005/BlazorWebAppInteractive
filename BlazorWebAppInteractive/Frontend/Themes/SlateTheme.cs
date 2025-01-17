using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Themes
{
    public class SlateTheme
    {
        public static readonly MudTheme Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Background = Colors.Gray.Lighten5, // Light background for Slate theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.BlueGray.Default, // Main slate/blue-gray color
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Gray.Default, // Neutral gray accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Teal.Darken1, // Tertiary accent for highlights
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.BlueGray.Lighten2, // Info elements
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Darken1, // Success messages
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Darken1, // Warnings
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Darken1, // Error or critical messages
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken3, // Text or icons for darker areas
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Darken3, // Default text color
                TextSecondary = Colors.Gray.Darken1, // Secondary text

                Surface = Colors.Gray.Lighten4, // Panel and card surfaces
                AppbarBackground = Colors.BlueGray.Darken1, // Appbar background
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Lighten3, // Drawer (menu) background
                DrawerText = Colors.Gray.Darken3,
            },
            PaletteDark = new PaletteDark
            {
                Background = Colors.Gray.Darken4, // Darker background for Slate theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.BlueGray.Darken2, // Main slate color in dark mode
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Gray.Darken3, // Neutral gray accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Teal.Darken2, // Tertiary accent
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.BlueGray.Darken3, // Info elements in dark mode
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Darken2, // Success in dark mode
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Darken2, // Warnings in dark mode
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Darken2, // Errors in dark mode
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken4, // Very dark surfaces
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Lighten3, // Default text color in dark mode
                TextSecondary = Colors.Gray.Lighten2, // Secondary text

                Surface = Colors.Gray.Darken3, // Panel and card surfaces
                AppbarBackground = Colors.BlueGray.Darken4, // Appbar in dark mode
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Darken2, // Drawer (menu) background
                DrawerText = Colors.Shades.White,
            },
            ZIndex = new ZIndex { Snackbar = 3000 }
        };

    }
}
