﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppCommonCode.Converters
{
    public class GenderConverter : IValueConverter
    {
        public static string Choose = "Odaberi";
        public static string ChooseM = "Muško";
        public static string ChooseF = "Žensko";

        public static List<String> GenderOptions = new List<string>()
        {
            ChooseM, ChooseF
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                int val = 0;
                Int32.TryParse(value.ToString(), out val);

                if (val == 1)
                    return ChooseM;
                else if (val == 2)
                    return ChooseF;
                return Choose;
            }

            return Choose;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == ChooseM)
                    return 1;//ChooseM
                else if (value.ToString() == ChooseF)
                    return 2;//ChooseF
            }

            return 0;
        }
    }
}
