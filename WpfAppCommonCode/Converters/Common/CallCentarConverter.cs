using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAppCommonCode.Converters.Common
{
    public class CallCentarConverter : IValueConverter
    {
        public CallCentarConverter() => Today = DateTime.Now.Date;

        public DateTime Today { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (value as CallCentarViewModel);
            if (item != null)
            {
                if(item.CheckedDone)
                    return new SolidColorBrush(Colors.LightGreen);
                else
                {
                    if (item.EndingDate.Date < Today)
                    {
                        return new SolidColorBrush(Colors.IndianRed);
                    }
                    else
                        return new SolidColorBrush(Colors.Yellow);
                }
            }


            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CallCentarCompletedConverter : IValueConverter
    {
        public static string Odradjeno = (string) Application.Current.FindResource("DA");
        public static string NijeOdradjeno = (string)Application.Current.FindResource("NE");
        public static string PrekoracenRok = (string)Application.Current.FindResource("NE");

        public static DateTime Today = DateTime.Now.Date;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (value as CallCentarViewModel);
            if (item != null)
            {
                if (item.CheckedDone)
                    return Odradjeno;
                else
                {
                    if (item.EndingDate.Date < Today)
                        return PrekoracenRok;
                    else
                        return NijeOdradjeno;
                }
            }
            return NijeOdradjeno;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
