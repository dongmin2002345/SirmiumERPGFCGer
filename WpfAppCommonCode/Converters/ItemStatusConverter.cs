using ServiceInterfaces.Gloabals;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppCommonCode.Converters
{
    public class ItemStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }
            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == ItemStatus.Submited)
                    return ItemStatus.SubmitedText;
                else if (result == ItemStatus.Added)
                    return ItemStatus.AddedText;
                else if (result == ItemStatus.Edited)
                    return ItemStatus.EditedText;
                else if (result == ItemStatus.Deleted)
                    return ItemStatus.DeletedText;
                else if (result == ItemStatus.StockForUpdate)
                    return ItemStatus.StockForUpdateText;
                else if (result == ItemStatus.StockUpdated)
                    return ItemStatus.StockUpdatedText;
                return 0;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == ItemStatus.SubmitedText)
                return ItemStatus.Submited;
            else if (value.ToString() == ItemStatus.AddedText)
                return ItemStatus.Added;
            else if (value.ToString() == ItemStatus.EditedText)
                return ItemStatus.Edited;
            else if (value.ToString() == ItemStatus.DeletedText)
                return ItemStatus.Deleted;
            else if (value.ToString() == ItemStatus.StockForUpdateText)
                return ItemStatus.StockForUpdate;
            else if (value.ToString() == ItemStatus.StockUpdatedText)
                return ItemStatus.StockUpdated;
            return 0;
        }
    }
}
