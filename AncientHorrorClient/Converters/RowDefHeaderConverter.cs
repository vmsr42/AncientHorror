using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AncientHorrorClient.Converters
{
    public class RowDefHeaderConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value==null||parameter==null)
                return new GridLength(1, GridUnitType.Star);
            int rowNumber = int.Parse(parameter.ToString());
            int curHeader = int.Parse(value.ToString());
            if (curHeader == rowNumber)
                return GridLength.Auto;
            else
                return new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
