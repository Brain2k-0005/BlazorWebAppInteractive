using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Themes
{
    public class SolarTheme
    {
        public static readonly MudTheme Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Background = Colors.Gray.Lighten5, // Light background for Solar theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Yellow.Default, // Main yellow color
                PrimaryContrastText = Colors.Shades.Black,

                Secondary = Colors.Orange.Default, // Supporting orange accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Amber.Default, // Tertiary accent for highlights
                TertiaryContrastText = Colors.Shades.Black,

                Info = Colors.Orange.Accent2, // Info elements with vibrant orange
                InfoContrastText = Colors.Shades.Black,

                Success = Colors.Green.Default, // Success indicators
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Yellow.Default, // Warnings in the Solar theme
                WarningContrastText = Colors.Shades.Black,

                Error = Colors.Red.Default, // Error or critical color
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken2, // Text or icons on darker areas
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Darken3, // Default text color
                TextSecondary = Colors.Gray.Darken1, // Secondary text

                Surface = Colors.Shades.White, // Card and panel surfaces
                AppbarBackground = Colors.Yellow.Darken1, // Appbar in Solar theme
                AppbarText = Colors.Shades.Black,
                DrawerBackground = Colors.Gray.Lighten3, // Drawer (menu) background
                DrawerText = Colors.Gray.Darken3,
            },
            PaletteDark = new PaletteDark
            {
                Background = Colors.Gray.Darken4, // Darker background for Solar theme
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Yellow.Darken2, // Main yellow in dark mode
                PrimaryContrastText = Colors.Shades.Black,

                Secondary = Colors.Orange.Darken2, // Supporting orange accent
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Amber.Darken2, // Tertiary accent
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Orange.Darken3, // Info elements in dark mode
                InfoContrastText = Colors.Shades.Black,

                Success = Colors.Green.Darken2, // Success in dark mode
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Yellow.Darken3, // Warnings in dark mode
                WarningContrastText = Colors.Shades.Black,

                Error = Colors.Red.Darken3, // Errors in dark mode
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken4, // Text or icons on very dark surfaces
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Lighten3, // Default text color
                TextSecondary = Colors.Gray.Lighten2, // Secondary text

                Surface = Colors.Gray.Darken3, // Panel and card surfaces
                AppbarBackground = Colors.Yellow.Darken3, // Appbar in dark mode
                AppbarText = Colors.Shades.Black,
                DrawerBackground = Colors.Gray.Darken2, // Drawer (menu) background
                DrawerText = Colors.Shades.White,
            },
            ZIndex = new ZIndex { Snackbar = 3000 }
        };

    }
}
