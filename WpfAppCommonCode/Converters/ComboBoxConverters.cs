using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAppCommonCode.Converters
{
    public class BoolStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool tmpValue = (bool)value;
                if (tmpValue)
                    return (string)Application.Current.FindResource("DA");
                else
                    return (string)Application.Current.FindResource("NE");
            }
            return (string)Application.Current.FindResource("NE");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem)
            {
                if (((ComboBoxItem)value).Content.ToString() == (string)Application.Current.FindResource("DA"))
                {
                    return true;
                }
            }
            else if (value.ToString() == (string)Application.Current.FindResource("DA"))
                return true;

            return false;
        }
    }

    public class VatEnabledByPdvTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int?)
            {
                int? val = (int?)value;

                return val == 2 ? false : true;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolGrossBalanceOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool tmpValue = (bool)value;
                if (tmpValue)
                    return "Pojednostavljen";
                else
                    return "Proširen";
            }
            return "Proširen";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem)
            {
                if (((ComboBoxItem)value).Content.ToString() == "Pojednostavljen")
                {
                    return true;
                }
            }
            else if (value.ToString() == "Pojednostavljen")
                return true;

            return false;
        }
    }

    public class BoolIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool tmpValue = (bool)value;
                if (tmpValue)
                    return 1;
                else
                    return 0;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tmpValue;
            Int32.TryParse(value.ToString(), out tmpValue);
            if (tmpValue == 1)
                return true;

            return false;
        }
    }

    public class IntStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int)value == 1)
                    return (string)Application.Current.FindResource("DA");
                else
                    return (string)Application.Current.FindResource("NE");
            }
            return (string)Application.Current.FindResource("NE");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == (string)Application.Current.FindResource("DA"))
                return 1;

            return 0;
        }
    }

    public class StringDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toRet = "";
            if (value != null)
            {
                double doubleValue = 0.0;
                if (value.GetType() == typeof(decimal)) { decimal tmpValue = (decimal)value; doubleValue = (double)tmpValue; }
                else if (value.GetType() == typeof(double)) { doubleValue = (double)value; }

                toRet = doubleValue.ToString("#,###,###,###,##0.00");
            }
            return toRet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            #region Sa NE nulabilnim numerckim vrednostima. Stari kod.
            //var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            //clone.NumberFormat.NumberDecimalSeparator = ".";
            //clone.NumberFormat.NumberGroupSeparator = ",";

            //double toRet = 0;
            //if (value != null)
            //{
            //    bool success = Double.TryParse(value.ToString().Replace(',', '.'), NumberStyles.AllowDecimalPoint, clone, out toRet);
            //    if (success)
            //    {
            //        toRet = Math.Round(toRet, 2);
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //return toRet; 
            #endregion

            #region Sa nulabilnim numerckim vrednostima. Stari kod.
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ".";
            clone.NumberFormat.NumberGroupSeparator = ",";

            double? toRet = null;
            if (value != null && value.ToString().Length > 0)
            {
                double toRetTmp = 0;
                bool success = Double.TryParse(value.ToString().Replace("'", "").Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, clone, out toRetTmp);
                if (success)
                {
                    toRet = Math.Round(toRetTmp, 2);
                    return toRet;
                }
                else
                {
                    //throw new Exception("Format nije dobar.");
                    return "No value";
                }
            }
            return toRet;
            #endregion
        }
    }

    public class StringDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toRet = "";
            if (value != null)
            {
                decimal decimalValue = 0.0M;
                if (value.GetType() == typeof(decimal)) { decimalValue = (decimal)value; }
                else if (value.GetType() == typeof(double)) { double tmpValue = (double)value; decimalValue = (decimal)value; }

                toRet = decimalValue.ToString("#,###,###,###,##0.00");
            }
            return toRet;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var clone = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ".";
            clone.NumberFormat.NumberGroupSeparator = ",";

            decimal toRet = 0;
            if (value != null)
            {
                bool success = Decimal.TryParse(value.ToString().Replace(',', '.'), NumberStyles.AllowDecimalPoint, clone, out toRet);
                if (success)
                {
                    toRet = Math.Round(toRet, 2);
                }
                else
                {
                    return null;
                }
            }
            return toRet;
        }
    }

    public class StringDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toRet = "";
            if (value != null)
            {
                DateTime dateTimeValue = DateTime.Now;
                if (value.GetType() == typeof(DateTime)) { dateTimeValue = (DateTime)value; }

                toRet = dateTimeValue.ToString("dd.MM.yyyy HH:mm:ss");
            }
            return toRet;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime toRet = DateTime.Now;
            if (value != null)
            {
                toRet = DateTime.Parse(toRet.ToString());
                bool success = DateTime.TryParse(value.ToString(), out toRet);
                if (success)
                {
                    toRet = DateTime.Parse(toRet.ToString());
                }
                else
                {
                    return null;
                }
            }
            return toRet;
        }
    }

    public class StringDateTimeShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toRet = "";
            if (value != null)
            {
                DateTime dateTimeValue = DateTime.Now;
                if (value.GetType() == typeof(DateTime)) { dateTimeValue = (DateTime)value; }

                toRet = dateTimeValue.ToString("dd.MM.yyyy");
            }
            return toRet;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime toRet = DateTime.Now;
            if (value != null)
            {
                toRet = DateTime.Parse(toRet.ToString());
                bool success = DateTime.TryParse(value.ToString(), out toRet);
                if (success)
                {
                    toRet = DateTime.Parse(toRet.ToString());
                }
                else
                {
                    return null;
                }
            }
            return toRet;
        }
    }

    public class StringDateForCalendarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string toRet = "";
            if (value != null)
            {
                DateTime dateTimeValue = DateTime.Now;
                if (value.GetType() == typeof(DateTime)) { dateTimeValue = (DateTime)value; }

                toRet = dateTimeValue.ToString("MMM yyyy");
            }
            return toRet;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ManuelWarrantConverter : IValueConverter
    {
        public static string Zakup = "Zakup";
        public static string Rucni = "Ručni";
        public static string Mesovit = "Mešovit";
        public static string NalogZaZaradeIBolovanja = "Nalog za zarade i bolovanja";
        public static string NalogZaZatvaranje = "Nalog za zatvaranje";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                //return Izdato;
                return "Ne sme!";
            }

            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == 1)
                    return Zakup;
                else if (result == 2)
                    return Rucni;
                else if (result == 3)
                    return Mesovit;
                else if (result == 4)
                    return NalogZaZaradeIBolovanja;
                else if (result == 5)
                    return NalogZaZatvaranje;
                return Zakup;
            }
            return Zakup;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == Zakup)
                return 1;
            else if (value.ToString() == Rucni)
                return 2;
            else if (value.ToString() == Mesovit)
                return 3;
            else if (value.ToString() == NalogZaZaradeIBolovanja)
                return 4;
            else if (value.ToString() == NalogZaZatvaranje)
                return 5;
            return 0;
        }
    }

    public class CreditNoteTypeConverter : IValueConverter
    {
        public static string Izdato = "Izdato";
        public static string Primljeno = "Primljeno";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Ne sme!";
            }
            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == 0)
                    return Izdato;
                else if (result == 1)
                    return Primljeno;
                return Izdato;
            }
            return Izdato;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == Izdato)
                return 0;
            else if (value.ToString() == Primljeno)
                return 1;
            return 0;
        }
    }

    public class CreditNoteApprovedConverter : IValueConverter
    {
        public static string Odobrenje = "Odobrenje";
        public static string Zaduzenje = "Zaduzenje";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Ne sme!";
            }
            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == 0)
                    return Odobrenje;
                else if (result == 1)
                    return Zaduzenje;
                return Odobrenje;
            }
            return Odobrenje;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == Odobrenje)
                return 0;
            else if (value.ToString() == Zaduzenje)
                return 1;
            return 0;
        }
    }

    public class InoOutputInvoiceUnitOfMeasureConverter : IValueConverter
    {
        public static string PraznoPolje = "";
        public static string Kg = "Kg";
        public static string kom = "kom";
        public static string m = "m";
        public static string Kutija = "Kutija";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Ne sme!";
            }
            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == 1)
                    return PraznoPolje;
                else if (result == 2)
                    return Kg;
                else if (result == 3)
                    return kom;
                else if (result == 4)
                    return m;
                else if (result == 5)
                    return Kutija;
                return PraznoPolje;
            }
            return PraznoPolje;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == PraznoPolje)
                return 1;
            else if (value.ToString() == Kg)
                return 2;
            else if (value.ToString() == kom)
                return 3;
            else if (value.ToString() == m)
                return 4;
            else if (value.ToString() == Kutija)
                return 5;
            return 0;
        }
    }

    public class SelectedBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isSelected = (value as bool?);
            if (isSelected != null && (bool)isSelected)
                return new SolidColorBrush(Colors.White);
            else
                return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class PdvTypeConverter : IValueConverter
    {
        public static string PraznoPolje = "PdvVrsta_Odaberi";
        public static string SA_PDV = "BPSaPdv";
        public static string BEZ_PDV = "BPBezPdv";
        public static string NIJE_OBVEZNIK = "BPNijeObveznik";

        public PdvTypeConverter() : base()
        {
            SA_PDV = ((string)Application.Current.FindResource("BPSaPdv"));
            BEZ_PDV = ((string)Application.Current.FindResource("BPBezPdv"));
            NIJE_OBVEZNIK = ((string)Application.Current.FindResource("BPNijeObveznik"));
            PraznoPolje = ((string)Application.Current.FindResource("PdvVrsta_Odaberi"));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
                return PraznoPolje;
            if (Int32.TryParse(value.ToString(), out int result))
            {
                if (result == 1)
                    return SA_PDV;
                else if (result == 2)
                    return BEZ_PDV;
                else if (result == 3)
                    return NIJE_OBVEZNIK;
                return PraznoPolje;
            }
            return PraznoPolje;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == PraznoPolje)
                return null;
            else if (value.ToString() == SA_PDV)
                return 1;
            else if (value.ToString() == BEZ_PDV)
                return 2;
            else if (value.ToString() == NIJE_OBVEZNIK)
                return 3;
            return null;
        }
    }
}
