using ServiceInterfaces.Gloabals;
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
    public class ItemStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = value != null ? Int32.Parse(value.ToString()) : 0;
            if (status == ItemStatus.Added || status == ItemStatus.Edited || status == ItemStatus.StockForUpdate)
                return new SolidColorBrush(Colors.White);
            else if (status == ItemStatus.Submited || status == ItemStatus.StockUpdated)
                return new SolidColorBrush(Colors.LightGray);
            else if (status == ItemStatus.Deleted)
                return new SolidColorBrush(Colors.LightPink);
            else
                return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
