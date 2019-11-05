using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.BusinessCalendar
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class Calendar_List : UserControl, INotifyPropertyChanged
    {
        #region CalendarItems
        private ObservableCollection<CalendarDate> _CalendarItems;

        public ObservableCollection<CalendarDate> CalendarItems
        {
            get { return _CalendarItems; }
            set
            {
                if (_CalendarItems != value)
                {
                    _CalendarItems = value;
                    NotifyPropertyChanged("CalendarItems");
                }
            }
        }
        #endregion

        #region StartingMonth
        private DateTime _StartingMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime StartingMonth
        {
            get { return _StartingMonth; }
            set
            {
                if (_StartingMonth != value)
                {
                    _StartingMonth = value;
                    NotifyPropertyChanged("StartingMonth");
                }
            }
        }
        #endregion


        #region TodayDate
        private DateTime? _TodayDate;

        public DateTime? TodayDate
        {
            get { return _TodayDate; }
            set
            {
                if (_TodayDate != value)
                {
                    _TodayDate = value;
                    NotifyPropertyChanged("TodayDate");
                }
            }
        }
        #endregion


        #region TomorrowDate
        private DateTime? _TomorrowDate;

        public DateTime? TomorrowDate
        {
            get { return _TomorrowDate; }
            set
            {
                if (_TomorrowDate != value)
                {
                    _TomorrowDate = value;
                    NotifyPropertyChanged("TomorrowDate");
                }
            }
        }
        #endregion

        #region LoadingData
        private bool _LoadingData;

        public bool LoadingData
        {
            get { return _LoadingData; }
            set
            {
                if (_LoadingData != value)
                {
                    _LoadingData = value;
                    NotifyPropertyChanged("LoadingData");
                }
            }
        }
        #endregion

        public Calendar_List()
        {
            InitializeComponent();

            this.DataContext = this;

            DisplayDates();
        }

        void DisplayDates()
        {

            List<CalendarDate> dates = new List<CalendarDate>();

            var startOfMonth = StartingMonth;
            var dayOfWeek = (int)startOfMonth.DayOfWeek;
            if (dayOfWeek > 0)
                startOfMonth = startOfMonth.AddDays(-dayOfWeek);
            else
                startOfMonth = startOfMonth.AddDays(-1);

            var today = DateTime.Now.Date;


            for (int i = 1; i <= 42; i++) // 6 x 7 grid
            {
                var calculated = startOfMonth.AddDays(i);

                dates.Add(new CalendarDate()
                {
                    Identifier = new Guid(),
                    Date = calculated,
                    IsEnabled = calculated.Date.Month == StartingMonth.Date.Month
                });
            }

            CalendarItems = new ObservableCollection<CalendarDate>(dates);

            var todaysDate = CalendarItems.FirstOrDefault(x => x.Date == today);
            if (todaysDate != null)
            {
                todaysDate.IsSelected = true;
                TodayDate = today;
                TomorrowDate = today.AddDays(1);
            }
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void CalendarDayButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoadingData)
                return;
            var item = (((Button)e.Source).CommandParameter) as CalendarDate;
            if(item != null)
            {
                foreach (var date in CalendarItems)
                    date.IsSelected = false;

                item.IsSelected = true;

                var today = item;

                TodayDate = today.Date;

                var nextDay = CalendarItems.FirstOrDefault(x => x.Date == item.Date.AddDays(1));

                TomorrowDate = nextDay.Date;


                Thread td = new Thread(() => {
                    LoadingData = true;

                    LoadingData = false;                
                });
                td.IsBackground = true;
                td.Start();
            }
        }

        private void btnPrevMonth_Click(object sender, RoutedEventArgs e)
        {
            if (LoadingData)
                return;
            StartingMonth = StartingMonth.AddMonths(-1);
            DisplayDates();
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            if (LoadingData)
                return;
            StartingMonth = StartingMonth.AddMonths(1);
            DisplayDates();
        }
    }

    
    public class CalendarDate : BaseEntityViewModel
    {
        #region IsEnabled
        private bool _IsEnabled;

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    NotifyPropertyChanged("IsEnabled");
                }
            }
        }
        #endregion

        #region Date
        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    NotifyPropertyChanged("Date");
                    NotifyPropertyChanged("DayInAMonth");
                }
            }
        }
        #endregion


        #region DayInAMonth

        public int DayInAMonth
        {
            get { return Date.Date.Day; }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        #endregion



    }
}
