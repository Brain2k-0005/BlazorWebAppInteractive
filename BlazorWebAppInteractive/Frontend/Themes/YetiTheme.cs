using MudBlazor;

namespace BlazorWebAppInteractive.Frontend.Themes
{
    public class YetiTheme
    {
        public static readonly MudTheme Theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Background = Colors.Gray.Lighten5,
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Blue.Default,
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.LightBlue.Accent2,
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Cyan.Lighten1,
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Blue.Lighten2,
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Default,
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Yellow.Default,
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Default,
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken2,
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Darken3,
                TextSecondary = Colors.Gray.Darken1,

                Surface = Colors.Shades.White,
                AppbarBackground = Colors.Blue.Darken2,
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Lighten3,
                DrawerText = Colors.Gray.Darken3,
            },
            PaletteDark = new PaletteDark
            {
                Background = Colors.Gray.Darken4,
                Black = Colors.Shades.Black,
                White = Colors.Shades.White,

                Primary = Colors.Blue.Darken1,
                PrimaryContrastText = Colors.Shades.White,

                Secondary = Colors.LightBlue.Accent3,
                SecondaryContrastText = Colors.Shades.White,

                Tertiary = Colors.Cyan.Darken1,
                TertiaryContrastText = Colors.Shades.White,

                Info = Colors.Blue.Darken2,
                InfoContrastText = Colors.Shades.White,

                Success = Colors.Green.Darken1,
                SuccessContrastText = Colors.Shades.White,

                Warning = Colors.Yellow.Darken1,
                WarningContrastText = Colors.Shades.White,

                Error = Colors.Red.Darken1,
                ErrorContrastText = Colors.Shades.White,

                Dark = Colors.Gray.Darken4,
                DarkContrastText = Colors.Shades.White,

                TextPrimary = Colors.Gray.Lighten3,
                TextSecondary = Colors.Gray.Lighten2,

                Surface = Colors.Gray.Darken3,
                AppbarBackground = Colors.Blue.Darken3,
                AppbarText = Colors.Shades.White,
                DrawerBackground = Colors.Gray.Darken2,
                DrawerText = Colors.Shades.White,
            },
            ZIndex = new ZIndex { Snackbar = 3000 }
        };
    }
}
