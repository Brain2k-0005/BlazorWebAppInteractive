using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Themes
{
    public class DefaultTheme
    {
        public static readonly MudTheme Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Background = Colors.Gray.Lighten5, // Default light background
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Indigo.Default, // Default primary indigo
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Pink.Accent2, // Default secondary pink
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Teal.Lighten1, // Tertiary teal accent
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Blue.Default, // Default blue for info elements
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Default, // Default green for success
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Default, // Default amber for warnings
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Default, // Default red for errors
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken2, // Dark text/icons for contrast
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Darken3, // Primary text color
                TextSecondary = Colors.Gray.Darken1, // Secondary text

                Surface = Colors.Shades.White, // Default card/panel surfaces
                AppbarBackground = Colors.Indigo.Darken1, // Default appbar background
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Lighten4, // Drawer (menu) background
                DrawerText = Colors.Gray.Darken3,
            },
            PaletteDark = new PaletteDark
            {
                Background = Colors.Gray.Darken4, // Default dark background
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Indigo.Darken1, // Default indigo in dark mode
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.Pink.Accent2, // Default secondary pink
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Teal.Darken1, // Tertiary teal in dark mode
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Blue.Default, // Info elements in dark mode
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Darken1, // Success in dark mode
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Amber.Darken1, // Warnings in dark mode
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Darken1, // Errors in dark mode
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken4, // Very dark surfaces
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Lighten3, // Default text color
                TextSecondary = Colors.Gray.Lighten2, // Secondary text

                Surface = Colors.Gray.Darken3, // Panel and card surfaces
                AppbarBackground = Colors.Indigo.Darken3, // Appbar in dark mode
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Darken2, // Drawer (menu) background
                DrawerText = Colors.Shades.White,
            },
            ZIndex = new ZIndex { Snackbar = 3000 }
        };
    }
}
