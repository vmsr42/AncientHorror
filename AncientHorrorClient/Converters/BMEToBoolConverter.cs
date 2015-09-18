using AncientHorrorClient.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AncientHorrorClient.Converters
{
    public class BMEToBoolConverter : IValueConverter
    {


        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            if (value==null)
                return true;
            bool res = true;
            if (parameter!=null)
                res = bool.Parse(parameter.ToString());
            BusyMessageEnum bme = (BusyMessageEnum)value;
            if (bme== BusyMessageEnum.None )
            {
                if (res)
                    return true;
                else
                    return false;
            }
            else
            {
                if (res)
                    return false;
                else
                    return true;
            }
             
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
