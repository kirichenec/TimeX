using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace TimeXv2.Extensions
{
    public class PaletteHelperExtensions
    {
        public static void SetLightDark(bool isDark)
        {
            var existingResourceDictionary = Application.Current.Resources.MergedDictionaries
                .Where(rd => rd.Source != null)
                .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString, @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme.((Light)|(Dark)))").Success);

            if (existingResourceDictionary == null)
            {
                throw new ApplicationException("Unable to find Light/Dark base theme in Application resources.");
            }

            var source = $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{(isDark ? "Dark" : "Light")}.xaml";
            var newResourceDictionary = new ResourceDictionary() { Source = new Uri(source) };

            Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);
        }
    }
}
