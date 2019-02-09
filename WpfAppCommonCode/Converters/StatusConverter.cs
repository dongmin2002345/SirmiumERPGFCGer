using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppCommonCode.Converters
{
    public class StatusConverter : IValueConverter
    {
        public static string Odaberi = "Odaberi";
        public static string UObradi = "U obradi / In Bearbeltubg";
        public static string Odbijen = "Odbijen / Abgelehnt";
        public static string Odobren = "Odobren / Genehmlgt";

        public static List<String> StatusOptions = new List<string>()
        {
            Odaberi, UObradi, Odbijen, Odobren
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int)value == 1)
                    return UObradi;
                else if ((int)value == 2)
                    return Odbijen;
                else if ((int)value == 3)
                    return Odobren;
                return Odaberi;
            }
            return Odaberi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == UObradi)
                    return 1;
                else if (value.ToString() == Odbijen)
                    return 2;
                else if (value.ToString() == Odobren)
                    return 4;
            }

            return 0;
        }
    }
}
