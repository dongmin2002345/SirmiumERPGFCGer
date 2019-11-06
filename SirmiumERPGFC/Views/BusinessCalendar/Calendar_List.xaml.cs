using Ninject;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.CalendarAssignments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.BusinessCalendar
{
    public partial class Calendar_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        private ICalendarAssignmentService calendarAssignmentService;
        #endregion

        #region Events
        public delegate void CalendarAssignmentHandler();
        #endregion

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

        #region SyncButtonContent
        private string _SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

        public string SyncButtonContent
        {
            get { return _SyncButtonContent; }
            set
            {
                if (_SyncButtonContent != value)
                {
                    _SyncButtonContent = value;
                    NotifyPropertyChanged("SyncButtonContent");
                }
            }
        }
        #endregion

        #region SyncButtonEnabled
        private bool _SyncButtonEnabled = true;

        public bool SyncButtonEnabled
        {
            get { return _SyncButtonEnabled; }
            set
            {
                if (_SyncButtonEnabled != value)
                {
                    _SyncButtonEnabled = value;
                    NotifyPropertyChanged("SyncButtonEnabled");
                }
            }
        }
        #endregion

        #region CalendarAssignmentsToday
        private ObservableCollection<CalendarAssignmentViewModel> _CalendarAssignmentsToday;

        public ObservableCollection<CalendarAssignmentViewModel> CalendarAssignmentsToday
        {
            get { return _CalendarAssignmentsToday; }
            set
            {
                if (_CalendarAssignmentsToday != value)
                {
                    _CalendarAssignmentsToday = value;
                    NotifyPropertyChanged("CalendarAssignmentsToday");
                }
            }
        }
        #endregion

        #region CalendarAssignmentsTomorrow
        private ObservableCollection<CalendarAssignmentViewModel> _CalendarAssignmentsTomorrow;

        public ObservableCollection<CalendarAssignmentViewModel> CalendarAssignmentsTomorrow
        {
            get { return _CalendarAssignmentsTomorrow; }
            set
            {
                if (_CalendarAssignmentsTomorrow != value)
                {
                    _CalendarAssignmentsTomorrow = value;
                    NotifyPropertyChanged("CalendarAssignmentsTomorrow");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor
        public Calendar_List()
        {
            InitializeComponent();

            calendarAssignmentService = DependencyResolver.Kernel.Get<ICalendarAssignmentService>();

            this.DataContext = this;

            DisplayDates();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }
        #endregion

        #region Display
        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("CalendarTritacke"));
            new CalendarAssignmentSQLiteRepository().Sync(calendarAssignmentService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = ((string)Application.Current.FindResource("Kalendar")) + "(" + synced + "/" + toSync + ")";
            });

            if(TodayDate != null && TomorrowDate != null)
            {
                DisplayTodayItems();
                DisplayTomorrowItems();
                UpdateDateMarkers(StartingMonth);
            }

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }

        void UpdateDateMarkers(DateTime startOfMOnth)
        {
            if (CalendarItems != null)
            {
                var finalDayOfMonth = StartingMonth.AddMonths(1).AddHours(-1);

                List<DateTime> markedDates = new CalendarAssignmentSQLiteRepository().GetAssignedDates(MainWindow.CurrentCompanyId, StartingMonth, finalDayOfMonth);

                foreach (var item in CalendarItems)
                {
                    if (markedDates.Any(x => x == item.Date))
                        item.MarkedDateVisible = Visibility.Visible;
                    else
                        item.MarkedDateVisible = Visibility.Hidden;
                }
            }
        }

        private void DisplayTodayItems()
        {
            LoadingData = true;

            CalendarAssignmentListResponse response = new CalendarAssignmentSQLiteRepository()
                .GetCalendarAssignmentsByDate(MainWindow.CurrentCompanyId, TodayDate);

            if (response.Success)
            {
                CalendarAssignmentsToday = new ObservableCollection<CalendarAssignmentViewModel>(response.CalendarAssignments ?? new List<CalendarAssignmentViewModel>());
            }
            else
            {
                CalendarAssignmentsToday = new ObservableCollection<CalendarAssignmentViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }
            LoadingData = false;
        }

        private void DisplayTomorrowItems()
        {
            LoadingData = true;

            CalendarAssignmentListResponse response = new CalendarAssignmentSQLiteRepository()
                .GetCalendarAssignmentsByDate(MainWindow.CurrentCompanyId, TomorrowDate);

            if (response.Success)
            {
                CalendarAssignmentsTomorrow = new ObservableCollection<CalendarAssignmentViewModel>(response.CalendarAssignments ?? new List<CalendarAssignmentViewModel>());
            }
            else
            {
                CalendarAssignmentsTomorrow = new ObservableCollection<CalendarAssignmentViewModel>();
                MainWindow.ErrorMessage = response.Message;
            }
            LoadingData = false;
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


        void DisplayDates()
        {

            List<CalendarDate> dates = new List<CalendarDate>();

            var startOfMonth = StartingMonth;
            var dayOfWeek = (int)startOfMonth.DayOfWeek;
            if (dayOfWeek > 0)
                startOfMonth = startOfMonth.AddDays(-dayOfWeek);
            else
                startOfMonth = startOfMonth.AddDays(-7);

            var today = DateTime.Now.Date;


            var finalDayOfMonth = StartingMonth.AddMonths(1).AddHours(-1);

            List<DateTime> markedDates = new CalendarAssignmentSQLiteRepository().GetAssignedDates(MainWindow.CurrentCompanyId, StartingMonth, finalDayOfMonth);


            for (int i = 1; i <= 42; i++) // 6 x 7 grid
            {
                var calculated = startOfMonth.AddDays(i);
                var visibleIcon = markedDates.Any(x => x == calculated);
                dates.Add(new CalendarDate()
                {
                    Identifier = new Guid(),
                    Date = calculated,
                    IsEnabled = calculated.Date.Month == StartingMonth.Date.Month,
                    MarkedDateVisible = visibleIcon ? Visibility.Visible : Visibility.Hidden
                });
            }

            CalendarItems = new ObservableCollection<CalendarDate>(dates);

            var todaysDate = CalendarItems.FirstOrDefault(x => x.Date == today && x.IsEnabled);
            if (todaysDate != null)
            {
                todaysDate.IsSelected = true;
                TodayDate = today;
                TomorrowDate = today.AddDays(1);
            }
            else
            {
                TodayDate = StartingMonth.Date;
                TomorrowDate = TodayDate.Value.AddDays(1);
            }


            Thread td = new Thread(() => {
                DisplayTodayItems();

                DisplayTomorrowItems();
            });
            td.IsBackground = true;
            td.Start();
        }

        private void dgTodayItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dgTomorrowItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        #endregion

        #region Selected Date

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
                    DisplayTodayItems();

                    DisplayTomorrowItems();
                });
                td.IsBackground = true;
                td.Start();
            }
        }

        #endregion

        #region Add, Edit and Delete
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CalendarAssignmentViewModel calendarAssignment = new CalendarAssignmentViewModel();
            calendarAssignment.Identifier = Guid.NewGuid();
            calendarAssignment.Date = TodayDate?.Date ?? DateTime.Now.Date;

            Calendar_AddEdit addEditForm = new Calendar_AddEdit(calendarAssignment, true);
            addEditForm.CalendarAssignmentCreatedUpdated += new CalendarAssignmentHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Kalendar")), 95, addEditForm);
        }

        private void BtnEditToday_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((Button)sender).CommandParameter as CalendarAssignmentViewModel;

            if (selected != null)
            {
                Calendar_AddEdit addEditForm = new Calendar_AddEdit(selected, false);
                addEditForm.CalendarAssignmentCreatedUpdated += new CalendarAssignmentHandler(SyncData);
                FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("CallCentar")), 95, addEditForm);
            }
        }

        private void BtnDeleteToday_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((Button)sender).CommandParameter as CalendarAssignmentViewModel;

            if (selected != null)
            {
                Thread th = new Thread(() =>
                {
                    LoadingData = true;

                    var response = calendarAssignmentService.Delete(selected.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                        LoadingData = false;
                        return;
                    }

                    response = new CalendarAssignmentSQLiteRepository().Delete(selected.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                        LoadingData = false;
                        return;
                    }

                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                    DisplayTodayItems();

                    UpdateDateMarkers(StartingMonth);

                    LoadingData = false;
                });
                th.IsBackground = true;
                th.Start();
            }
        }


        private void BtnEditTomorrow_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((Button)sender).CommandParameter as CalendarAssignmentViewModel;

            if (selected != null)
            {
                Calendar_AddEdit addEditForm = new Calendar_AddEdit(selected, false);
                addEditForm.CalendarAssignmentCreatedUpdated += new CalendarAssignmentHandler(SyncData);
                FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("CallCentar")), 95, addEditForm);
            }
        }

        private void BtnDeleteTomorrow_Click(object sender, RoutedEventArgs e)
        {
            var selected = ((Button)sender).CommandParameter as CalendarAssignmentViewModel;

            if (selected != null)
            {
                Thread th = new Thread(() =>
                {
                    LoadingData = true;

                    var response = calendarAssignmentService.Delete(selected.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                        LoadingData = false;
                        return;
                    }

                    response = new CalendarAssignmentSQLiteRepository().Delete(selected.Identifier);
                    if (!response.Success)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                        LoadingData = false;
                        return;
                    }

                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                    DisplayTomorrowItems();

                    UpdateDateMarkers(StartingMonth);

                    LoadingData = false;
                });
                th.IsBackground = true;
                th.Start();
            }
        }
        #endregion

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

        #region MarkedDateVisible
        private Visibility _MarkedDateVisible;

        public Visibility MarkedDateVisible
        {
            get { return _MarkedDateVisible; }
            set
            {
                if (_MarkedDateVisible != value)
                {
                    _MarkedDateVisible = value;
                    NotifyPropertyChanged("MarkedDateVisible");
                }
            }
        }
        #endregion


    }
}
