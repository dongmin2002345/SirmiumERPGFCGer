using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
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

namespace SirmiumERPGFC.Views.Employees
{
    /// <summary>
    /// Interaction logic for PhysicalPerson_Item_AddEdit.xaml
    /// </summary>
    public partial class PhysicalPerson_Item_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IPhysicalPersonService physicalPersonService;
        IPhysicalPersonItemService physicalPersonItemService;
        #endregion


        #region Event
        public event PhysicalPersonHandler PhysicalPersonCreatedUpdated;
        #endregion


        #region CurrentPhysicalPerson
        private PhysicalPersonViewModel _CurrentPhysicalPerson = new PhysicalPersonViewModel();

        public PhysicalPersonViewModel CurrentPhysicalPerson
        {
            get { return _CurrentPhysicalPerson; }
            set
            {
                if (_CurrentPhysicalPerson != value)
                {
                    _CurrentPhysicalPerson = value;
                    NotifyPropertyChanged("CurrentPhysicalPerson");
                }
            }
        }
        #endregion


        #region PhysicalPersonItemsFromDB
        private ObservableCollection<PhysicalPersonItemViewModel> _PhysicalPersonItemsFromDB;

        public ObservableCollection<PhysicalPersonItemViewModel> PhysicalPersonItemsFromDB
        {
            get { return _PhysicalPersonItemsFromDB; }
            set
            {
                if (_PhysicalPersonItemsFromDB != value)
                {
                    _PhysicalPersonItemsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonItemForm
        private PhysicalPersonItemViewModel _CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();

        public PhysicalPersonItemViewModel CurrentPhysicalPersonItemForm
        {
            get { return _CurrentPhysicalPersonItemForm; }
            set
            {
                if (_CurrentPhysicalPersonItemForm != value)
                {
                    _CurrentPhysicalPersonItemForm = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonItemForm");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonItemDG
        private PhysicalPersonItemViewModel _CurrentPhysicalPersonItemDG;

        public PhysicalPersonItemViewModel CurrentPhysicalPersonItemDG
        {
            get { return _CurrentPhysicalPersonItemDG; }
            set
            {
                if (_CurrentPhysicalPersonItemDG != value)
                {
                    _CurrentPhysicalPersonItemDG = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonItemDG");
                }
            }
        }
        #endregion

        #region PhysicalPersonItemDataLoading
        private bool _PhysicalPersonItemDataLoading;

        public bool PhysicalPersonItemDataLoading
        {
            get { return _PhysicalPersonItemDataLoading; }
            set
            {
                if (_PhysicalPersonItemDataLoading != value)
                {
                    _PhysicalPersonItemDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonItemDataLoading");
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

        public PhysicalPerson_Item_AddEdit(PhysicalPersonViewModel physicalPerson)
        {
            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>();
            physicalPersonItemService = DependencyResolver.Kernel.Get<IPhysicalPersonItemService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentPhysicalPerson = physicalPerson;
            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
            CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayPhysicalPersonItemData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddNote.Focus();
        }

        #endregion

        #region Display data

        public void DisplayPhysicalPersonItemData()
        {
            PhysicalPersonItemDataLoading = true;

            PhysicalPersonItemListResponse response = new PhysicalPersonItemSQLiteRepository()
                .GetPhysicalPersonItemsByPhysicalPerson(MainWindow.CurrentCompanyId, CurrentPhysicalPerson.Identifier);

            if (response.Success)
            {
                PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>(
                    response.PhysicalPersonItems ?? new List<PhysicalPersonItemViewModel>());
            }
            else
            {
                PhysicalPersonItemsFromDB = new ObservableCollection<PhysicalPersonItemViewModel>();
            }

            PhysicalPersonItemDataLoading = false;
        }

        private void DgPhysicalPersonItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dg_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        #region Add, Edit and Delete 

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentPhysicalPersonItemForm.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Naziv";
                return;
            }

            #endregion

            new PhysicalPersonItemSQLiteRepository().Delete(CurrentPhysicalPersonItemForm.Identifier);

            CurrentPhysicalPersonItemForm.PhysicalPerson = CurrentPhysicalPerson;

            CurrentPhysicalPersonItemForm.IsSynced = false;
            CurrentPhysicalPersonItemForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentPhysicalPersonItemForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new PhysicalPersonItemSQLiteRepository().Create(CurrentPhysicalPersonItemForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
                CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
            CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Added;

            PhysicalPersonCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayPhysicalPersonItemData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtNote.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
            CurrentPhysicalPersonItemForm.Identifier = CurrentPhysicalPersonItemDG.Identifier;
            CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Edited;

            CurrentPhysicalPersonItemForm.FamilyMember = CurrentPhysicalPersonItemDG.FamilyMember;
            CurrentPhysicalPersonItemForm.Name = CurrentPhysicalPersonItemDG.Name;
            CurrentPhysicalPersonItemForm.DateOfBirth = CurrentPhysicalPersonItemDG.DateOfBirth;
            CurrentPhysicalPersonItemForm.Passport = CurrentPhysicalPersonItemDG.Passport;
            CurrentPhysicalPersonItemForm.EmbassyDate = CurrentPhysicalPersonItemDG.EmbassyDate;

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new PhysicalPersonItemSQLiteRepository().SetStatusDeleted(CurrentPhysicalPersonItemDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
                CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
                CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Added;

                CurrentPhysicalPersonItemDG = null;

                PhysicalPersonCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayPhysicalPersonItemData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelNote_Click(object sender, RoutedEventArgs e)
        {
            CurrentPhysicalPersonItemForm = new PhysicalPersonItemViewModel();
            CurrentPhysicalPersonItemForm.Identifier = Guid.NewGuid();
            CurrentPhysicalPersonItemForm.ItemStatus = ItemStatus.Added;
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (PhysicalPersonItemsFromDB == null || PhysicalPersonItemsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentPhysicalPerson.PhysicalPersonItems = PhysicalPersonItemsFromDB;
                PhysicalPersonResponse response = physicalPersonService.Create(CurrentPhysicalPerson);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod čuvanja podataka!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            PhysicalPersonCreatedUpdated();

            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
