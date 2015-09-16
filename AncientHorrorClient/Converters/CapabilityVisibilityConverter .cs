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
    public sealed class CapabilityVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            bool res = true;
            if (parameter != null)
                res = bool.Parse(parameter.ToString());
            int count = int.Parse(value.ToString());
            if (count > 0)
                if (res)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            else
                if (!res)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
