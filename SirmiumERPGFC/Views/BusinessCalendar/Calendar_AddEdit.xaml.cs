using Ninject;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Messages.CalendarAssignments;
using ServiceInterfaces.ViewModels.CalendarAssignments;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.CalendarAssignments;
using System;
using System.Collections.Generic;
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
using static SirmiumERPGFC.Views.BusinessCalendar.Calendar_List;

namespace SirmiumERPGFC.Views.BusinessCalendar
{
    /// <summary>
    /// Interaction logic for Calendar_AddEdit.xaml
    /// </summary>
    public partial class Calendar_AddEdit : UserControl, INotifyPropertyChanged
    {

        #region Attributes


        #region Services
        ICalendarAssignmentService calendarAssignmentService;
        #endregion

        #region Events
        public event CalendarAssignmentHandler CalendarAssignmentCreatedUpdated;
        #endregion


        #region IsCreateProcess
        private bool _IsCreateProcess;

        public bool IsCreateProcess
        {
            get { return _IsCreateProcess; }
            set
            {
                if (_IsCreateProcess != value)
                {
                    _IsCreateProcess = value;
                    NotifyPropertyChanged("IsCreateProcess");
                }
            }
        }
        #endregion

        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
                }
            }
        }
        #endregion

        #region SubmitButtonContent
        private string _SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));

        public string SubmitButtonContent
        {
            get { return _SubmitButtonContent; }
            set
            {
                if (_SubmitButtonContent != value)
                {
                    _SubmitButtonContent = value;
                    NotifyPropertyChanged("SubmitButtonContent");
                }
            }
        }
        #endregion

        #region SubmitButtonEnabled
        private bool _SubmitButtonEnabled = true;

        public bool SubmitButtonEnabled
        {
            get { return _SubmitButtonEnabled; }
            set
            {
                if (_SubmitButtonEnabled != value)
                {
                    _SubmitButtonEnabled = value;
                    NotifyPropertyChanged("SubmitButtonEnabled");
                }
            }
        }
        #endregion

        #region CurrentCalendarAssignment
        private CalendarAssignmentViewModel _CurrentCalendarAssignment;

        public CalendarAssignmentViewModel CurrentCalendarAssignment
        {
            get { return _CurrentCalendarAssignment; }
            set
            {
                if (_CurrentCalendarAssignment != value)
                {
                    _CurrentCalendarAssignment = value;
                    NotifyPropertyChanged("CurrentCalendarAssignment");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor
        public Calendar_AddEdit(CalendarAssignmentViewModel calendarAssignment, bool isCreateProcess, bool isPopup = false)
        {
            InitializeComponent();
            calendarAssignmentService = DependencyResolver.Kernel.Get<ICalendarAssignmentService>();

            this.DataContext = this;

            CurrentCalendarAssignment = calendarAssignment;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Submit and cancel
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentCalendarAssignment.Date == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("ObaveznoPoljeDatum"));
                return;
            }
            if (String.IsNullOrEmpty(CurrentCalendarAssignment.Name))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("ObaveznoPoljeNaziv"));
                return;
            }
            if (String.IsNullOrEmpty(CurrentCalendarAssignment.Description))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("ObaveznoPoljeOpis"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SubmitButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SubmitButtonEnabled = false;

                CurrentCalendarAssignment.IsSynced = false;
                CurrentCalendarAssignment.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentCalendarAssignment.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CalendarAssignmentResponse response = new CalendarAssignmentSQLiteRepository().Delete(CurrentCalendarAssignment.Identifier);
                response = new CalendarAssignmentSQLiteRepository().Create(CurrentCalendarAssignment);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                    return;
                }

                response = calendarAssignmentService.Create(CurrentCalendarAssignment);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SubmitButtonContent = ((string)Application.Current.FindResource("Proknjiži"));
                    SubmitButtonEnabled = true;

                    CalendarAssignmentCreatedUpdated?.Invoke();

                    if (IsCreateProcess)
                    {
                        CurrentCalendarAssignment = new CalendarAssignmentViewModel();
                        CurrentCalendarAssignment.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                dtSelectedDate.Focus();
                            })
                        );
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                if (IsPopup)
                                    FlyoutHelper.CloseFlyoutPopup(this);
                                else
                                    FlyoutHelper.CloseFlyout(this);
                            })
                        );
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
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
}
