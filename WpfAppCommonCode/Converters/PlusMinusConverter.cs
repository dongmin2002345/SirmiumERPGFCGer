using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAppCommonCode.Converters
{
    public class PlusMinusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string plusMinus = (value as string);
            if (plusMinus != "" && (string)plusMinus == "+")
                return new SolidColorBrush(Colors.LightGreen);
            else if (plusMinus != "" && (string)plusMinus == "-")
                return "#ffc3ac";
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
