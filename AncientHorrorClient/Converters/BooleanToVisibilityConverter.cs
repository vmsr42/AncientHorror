using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;


namespace AncientHorrorClient.Converters
{
    /// <summary>
    /// Конвертер значений, который преобразует значение true в значение <see cref="Visibility.Visible"/> и значение false в
    /// значение <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool dir = true;
            if (parameter != null)
                dir = bool.Parse(parameter.ToString());

            if (dir)
                return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            else
                return (value is bool && !(bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
