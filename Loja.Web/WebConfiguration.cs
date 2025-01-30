using Loja.Core;
using MudBlazor;

namespace Dima.Web;

public static class WebConfiguration
{
    public const string HttpClientName = "loja";

    public static string BackendUrl { get; set; } = string.Empty;

    public static MudTheme theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Open Sans", "sans-serif"]
            }
        },
        PaletteLight = new PaletteLight
        {
            Primary = "#bf0603",
            Secondary = "#FFFFFF",
            Background = Colors.Gray.Lighten4,
            AppbarBackground = "#FFFFFF",
            AppbarText = Colors.Shades.Black,
            TextPrimary = Colors.Shades.Black,
            PrimaryContrastText = Colors.Shades.Black
        }
    };
}