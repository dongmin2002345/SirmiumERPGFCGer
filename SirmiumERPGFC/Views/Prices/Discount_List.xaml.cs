using Ninject;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Prices;
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

namespace SirmiumERPGFC.Views.Prices
{
    /// <summary>
    /// Interaction logic for Discount_List.xaml
    /// </summary>
    public delegate void DiscountHandler();
    public partial class Discount_List : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IDiscountService DiscountService;
        #endregion


        #region DiscountsFromDB
        private ObservableCollection<DiscountViewModel> _DiscountsFromDB;

        public ObservableCollection<DiscountViewModel> DiscountsFromDB
        {
            get { return _DiscountsFromDB; }
            set
            {
                if (_DiscountsFromDB != value)
                {
                    _DiscountsFromDB = value;
                    NotifyPropertyChanged("DiscountsFromDB");
                }
            }
        }
        #endregion

        #region CurrentDiscount
        private DiscountViewModel _CurrentDiscount;

        public DiscountViewModel CurrentDiscount
        {
            get { return _CurrentDiscount; }
            set
            {
                if (_CurrentDiscount != value)
                {
                    _CurrentDiscount = value;
                    NotifyPropertyChanged("CurrentDiscount");
                }
            }
        }
        #endregion

        #region DiscountSearchObject
        private DiscountViewModel _DiscountSearchObject = new DiscountViewModel();

        public DiscountViewModel DiscountSearchObject
        {
            get { return _DiscountSearchObject; }
            set
            {
                if (_DiscountSearchObject != value)
                {
                    _DiscountSearchObject = value;
                    NotifyPropertyChanged("DiscountSearchObject");
                }
            }
        }
        #endregion

        #region DiscountDataLoading
        private bool _DiscountDataLoading;

        public bool DiscountDataLoading
        {
            get { return _DiscountDataLoading; }
            set
            {
                if (_DiscountDataLoading != value)
                {
                    _DiscountDataLoading = value;
                    NotifyPropertyChanged("DiscountDataLoading");
                }
            }
        }
        #endregion

        #region Pagination data
        int currentPage = 1;
        int itemsPerPage = 50;
        int totalItems = 0;

        #region PaginationDisplay
        private string _PaginationDisplay;

        public string PaginationDisplay
        {
            get { return _PaginationDisplay; }
            set
            {
                if (_PaginationDisplay != value)
                {
                    _PaginationDisplay = value;
                    NotifyPropertyChanged("PaginationDisplay");
                }
            }
        }
        #endregion
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

        #endregion

        #region Constructor
        public Discount_List()
        {
            DiscountService = DependencyResolver.Kernel.Get<IDiscountService>();

            InitializeComponent();

            this.DataContext = this;
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
            currentPage = 1;

            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sinhronizovaniUzvičnik"));
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayDiscountData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }
        
        public void DisplayDiscountData()
        {
            DiscountDataLoading = true;

            DiscountListResponse response = new DiscountSQLiteRepository()
                .GetDiscountsByPage(MainWindow.CurrentCompanyId, DiscountSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                DiscountsFromDB = new ObservableCollection<DiscountViewModel>(response.Discounts ?? new List<DiscountViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                DiscountsFromDB = new ObservableCollection<DiscountViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            DiscountDataLoading = false;
        }

        private void SyncData()
        {
            SyncButtonEnabled = false;

            SyncButtonContent = ((string)Application.Current.FindResource("Popust_TriTacke"));
            new DiscountSQLiteRepository().Sync(DiscountService, (synced, toSync) =>
            {
                if (toSync > 0)
                    SyncButtonContent = ((string)Application.Current.FindResource("Popust")) +"(" + synced + "/" + toSync + ")";
            });

            DisplayDiscountData();

            SyncButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            SyncButtonEnabled = true;
        }
        private void DgDiscounts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        #endregion


        #region Add, Edit and delete 
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            DiscountViewModel Discount = new DiscountViewModel();
            Discount.Identifier = Guid.NewGuid();

            Discount_AddEdit addEditForm = new Discount_AddEdit(Discount, true);
            addEditForm.DiscountCreatedUpdated += new DiscountHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_popustu")), 95, addEditForm);
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDiscount == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_popust_za_izmenuUzvičnik"));
                return;
            }

            Discount_AddEdit addEditForm = new Discount_AddEdit(CurrentDiscount, false);
            addEditForm.DiscountCreatedUpdated += new DiscountHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_popustu")), 95, addEditForm);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                DiscountDataLoading = true;

                if (CurrentDiscount == null)
                {
                    MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_stavku_za_brisanjeUzvičnik"));
                    DiscountDataLoading = false;
                    return;
                }

                DiscountResponse response = DiscountService.Delete(CurrentDiscount.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_brisanja_sa_serveraUzvičnik"));
                    DiscountDataLoading = false;
                    return;
                }

                response = new DiscountSQLiteRepository().Delete(CurrentDiscount.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_brisanjaUzvičnik"));
                    DiscountDataLoading = false;
                    return;
                }

                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                DisplayDiscountData();

                DiscountDataLoading = false;
            });
            th.IsBackground = true;
            th.Start();
        }
        #endregion

        #region Pagination
        private void BtnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => DisplayDiscountData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayDiscountData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayDiscountData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void BtnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;

                Thread displayThread = new Thread(() => DisplayDiscountData());
                displayThread.IsBackground = true;
                displayThread.Start();
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
}
