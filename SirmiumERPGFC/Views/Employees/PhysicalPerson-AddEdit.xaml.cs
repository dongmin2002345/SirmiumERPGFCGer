using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Views.Common;
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
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.Employees
{
	public partial class PhysicalPerson_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService outputInvoiceService;
        #endregion

        #region Events
        public event PhysicalPersonHandler PhysicalPersonCreatedUpdated;
        #endregion

        #region currentPhysicalPerson
        private PhysicalPersonViewModel _currentPhysicalPerson;

        public PhysicalPersonViewModel CurrentPhysicalPerson
        {
            get { return _currentPhysicalPerson; }
            set
            {
                if (_currentPhysicalPerson != value)
                {
                    _currentPhysicalPerson = value;
                    NotifyPropertyChanged("currentPhysicalPerson");
                }
            }
        }
        #endregion

        #region StatusOptions
        public ObservableCollection<String> StatusOptions
        {
            get
            {
                return new ObservableCollection<String>(new List<string>() {
                           ChooseStatusConverter.Choose,
                           ChooseStatusConverter.ChooseO,
                           ChooseStatusConverter.ChooseB,
                           ChooseStatusConverter.ChooseM,
                });
            }
        }
        #endregion

        #region GenderOptions
        public ObservableCollection<String> GenderOptions
        {
            get
            {
                return new ObservableCollection<String>(new List<string>() {
                           GenderConverter.Choose,
                           GenderConverter.ChooseM,
                           GenderConverter.ChooseF});
            }
        }
        #endregion

        #region IsHeaderCreated
        private bool _IsHeaderCreated;

        public bool IsHeaderCreated
        {
            get { return _IsHeaderCreated; }
            set
            {
                if (_IsHeaderCreated != value)
                {
                    _IsHeaderCreated = value;
                    NotifyPropertyChanged("IsHeaderCreated");
                }
            }
        }
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
        private string _SubmitButtonContent = " PROKNJIŽI ";

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

        #endregion



        #region Constructor

        public PhysicalPerson_AddEdit(PhysicalPersonViewModel PhysicalPersonViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            outputInvoiceService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();

            InitializeComponent();

            this.DataContext = this;

            IsHeaderCreated = !isCreateProcess;

            CurrentPhysicalPerson = PhysicalPersonViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Submit and Cancel 

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            //if (CurrentPhysicalPerson?.BusinessPartner == null)
            //{
            //    MainWindow.WarningMessage = "Obavezno polje: Poslovni partner";
            //    return;
            //}

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.IsSynced = false;
                CurrentPhysicalPerson.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentPhysicalPerson.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                PhysicalPersonResponse response = new PhysicalPersonSQLiteRepository().Create(CurrentPhysicalPerson);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod čuvanja podataka!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                response = outputInvoiceService.Create(CurrentPhysicalPerson);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu. Greška kod čuvanja na serveru!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

                    new PhysicalPersonSQLiteRepository().Sync(outputInvoiceService);

                    PhysicalPersonCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            FlyoutHelper.CloseFlyout(this);
                        })
                    );
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region Mouse wheel event 

        private void PreviewMouseWheelEv(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion


    }
}
