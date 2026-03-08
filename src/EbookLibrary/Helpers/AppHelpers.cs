using System;
using System.Linq;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace EbookLibrary.Helpers
{
    public static class LocalizationHelper
    {
        public static void ApplyLanguage(string language)
        {
            var uri = language == "en"
                ? new Uri("pack://application:,,,/EbookLibrary;component/Resources/Strings.en.xaml")
                : new Uri("pack://application:,,,/EbookLibrary;component/Resources/Strings.pl.xaml");

            var existing = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                    (d.Source.ToString().Contains("Strings.pl") ||
                     d.Source.ToString().Contains("Strings.en")));

            if (existing != null)
                Application.Current.Resources.MergedDictionaries.Remove(existing);

            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = uri });
        }
    }

    public static class ThemeHelper
    {
        public static void ApplyTheme(string theme)
        {
            var paletteHelper = new PaletteHelper();
            var currentTheme = paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(theme == "Dark" ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(currentTheme);
        }
    }
}
