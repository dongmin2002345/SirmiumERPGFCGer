using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppCommonCode.Converters
{
    public class ChooseStatusConverter : IValueConverter
    {
        public static string Choose = "ODABRATI (Auswählen)"; //odaberi
        public static string ChooseO = "OTVORENO (Offen)";
        public static string ChooseB = "PLAĆEN (Bezahlt)";
        public static string ChooseM = "OPOMENA (Mähnung)";

        public static List<String> GenderOptions = new List<string>()
        {
            ChooseO, ChooseB
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            //if (value is int) //---vraca 1 i 2
            //{
            //    if ((int)value == 1)
            //        return ChooseO;
            //    else if ((int)value == 2)
            //        return ChooseB;
            //    return Choose;
            //}
            if (value != null && value is string)
            {
                int val = 0;
                Int32.TryParse(value.ToString(), out val);

                if (val == 1)
                    return ChooseO;
                else if (val == 2)
                    return ChooseB;
                else if (val == 3)
                    return ChooseM;
                return Choose;
            }

            return Choose;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                if (value.ToString() == ChooseO)
                    return 1;//ChooseO
                else if (value.ToString() == ChooseB)
                    return 2;//ChooseB
                else if (value.ToString() == ChooseM)
                    return 3;//ChooseM
            }

            return 0;
        }
    }
}
