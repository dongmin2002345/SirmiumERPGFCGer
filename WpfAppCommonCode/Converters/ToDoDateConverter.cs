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
    public class ToDoDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? toDoDate = value as DateTime?;
            if (toDoDate != null && ((DateTime)toDoDate).Date == DateTime.Now.Date)
                return new SolidColorBrush(Colors.Red);
            else if (toDoDate != null && toDoDate < DateTime.Now)
                return new SolidColorBrush(Colors.DarkRed);
            else if (toDoDate != null && toDoDate > DateTime.Now && toDoDate < DateTime.Now.AddDays(3))
                return new SolidColorBrush(Colors.Orange);
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
