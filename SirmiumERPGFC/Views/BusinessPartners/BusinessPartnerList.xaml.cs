using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
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

namespace SirmiumERPGFC.Views.BusinessPartners
{
    public delegate void BusinessPartnerHandler();

    public partial class BusinessPartnerList : UserControl, INotifyPropertyChanged
    {

        #region Attributes
        IBusinessPartnerService businessPartnerService;

        #region BusinessPartnersLoading
        private bool _BusinessPartnersLoading;

        public bool BusinessPartnersLoading
        {
            get { return _BusinessPartnersLoading; }
            set
            {
                if (_BusinessPartnersLoading != value)
                {
                    _BusinessPartnersLoading = value;
                    NotifyPropertyChanged("BusinessPartnersLoading");
                }
            }
        }
        #endregion

        #region Pagination data
        int currentPage = 1;
        int itemsPerPage = 20;
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

        #region BusinessPartnerFilterObject
        private BusinessPartnerViewModel _BusinessPartnerFilterObject = new BusinessPartnerViewModel();

        public BusinessPartnerViewModel BusinessPartnerFilterObject
        {
            get { return _BusinessPartnerFilterObject; }
            set
            {
                if (_BusinessPartnerFilterObject != value)
                {
                    _BusinessPartnerFilterObject = value;
                    NotifyPropertyChanged("BusinessPartnerFilterObject");
                }
            }
        }
        #endregion

        #region BusinessPartnersFromDB
        private ObservableCollection<BusinessPartnerViewModel> _BusinessPartnersFromDB;

        public ObservableCollection<BusinessPartnerViewModel> BusinessPartnersFromDB
        {
            get
            {
                return _BusinessPartnersFromDB;
            }
            set
            {
                if (_BusinessPartnersFromDB != value)
                {
                    _BusinessPartnersFromDB = value;
                    NotifyPropertyChanged("BusinessPartnersFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartner
        private BusinessPartnerViewModel _CurrentBusinessPartner;

        public BusinessPartnerViewModel CurrentBusinessPartner
        {
            get { return _CurrentBusinessPartner; }
            set
            {
                if (_CurrentBusinessPartner != value)
                {
                    _CurrentBusinessPartner = value;
                    NotifyPropertyChanged("CurrentBusinessPartner");
                }
            }
        }
        #endregion

        #region BusinessPartnerButtonContent
        private string _BusinessPartnerButtonContent = " Sinhronizacija firmi sa serverom ";

        public string BusinessPartnerButtonContent
        {
            get { return _BusinessPartnerButtonContent; }
            set
            {
                if (_BusinessPartnerButtonContent != value)
                {
                    _BusinessPartnerButtonContent = value;
                    NotifyPropertyChanged("BusinessPartnerButtonContent");
                }
            }
        }
        #endregion

        #region BusinessPartnerButtonEnabled
        private bool _BusinessPartnerButtonEnabled = true;

        public bool BusinessPartnerButtonEnabled
        {
            get { return _BusinessPartnerButtonEnabled; }
            set
            {
                if (_BusinessPartnerButtonEnabled != value)
                {
                    _BusinessPartnerButtonEnabled = value;
                    NotifyPropertyChanged("BusinessPartnerButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public BusinessPartnerList()
        {
            // Get required service
            this.businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void PopulateData()
        {
            BusinessPartnersLoading = true;

            BusinessPartnerListResponse response = new BusinessPartnerSQLiteRepository()
                .GetBusinessPartnersByPage(BusinessPartnerFilterObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response.BusinessPartners ?? new List<BusinessPartnerViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            BusinessPartnersLoading = false;
        }
        #endregion

        #region Add, Edit, Delete buttons click

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            BusinessPartnerViewModel businessPartner = new BusinessPartnerViewModel();
            businessPartner.Code = new BusinessPartnerSQLiteRepository().GetNewCodeValue();
            businessPartner.Identifier = Guid.NewGuid();

            BusinessPartnerAddEdit addEditForm = new BusinessPartnerAddEdit(businessPartner, true);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(PopulateData);
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 95, addEditForm);
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati firmu za izmenu!";
                return;
            }

            BusinessPartnerAddEdit addEditForm = new BusinessPartnerAddEdit(CurrentBusinessPartner, false);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(PopulateData);
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBusinessPartner == null)
            {
                MainWindow.WarningMessage = "Morate odabrati firmu za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("firma", CurrentBusinessPartner.Name + CurrentBusinessPartner.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                BusinessPartnerResponse response = businessPartnerService.Delete(CurrentBusinessPartner.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new BusinessPartnerSQLiteRepository().Delete(CurrentBusinessPartner.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Firma je uspešno obrisana!";

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            // Remove blur effects
            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

            dgBusinessPartners.Focus();
        }

        #endregion

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
            if (currentPage < lastPage)
            {
                currentPage = lastPage;

                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        #endregion

        #region Sync data

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                BusinessPartnerButtonContent = " Sinhronizacija u toku... ";
                BusinessPartnerButtonEnabled = false;

                BusinessPartnerSQLiteRepository sqlLite = new BusinessPartnerSQLiteRepository();

                SyncBusinessPartnerRequest request = new SyncBusinessPartnerRequest();
                request.UnSyncedBusinessPartners = sqlLite.GetUnSyncedBusinessPartners().BusinessPartners;
                request.LastUpdatedAt = sqlLite.GetLastUpdatedAt(MainWindow.CurrentCompanyId);

                BusinessPartnerListResponse response = businessPartnerService.Sync(request);
                if (response.Success)
                {
                    List<BusinessPartnerViewModel> businessPartnersFromDB = response.BusinessPartners;
                    int total = businessPartnersFromDB.Count;
                    int counter = 1;
                    foreach (var businessPartner in businessPartnersFromDB.OrderBy(x => x.Id))
                    {
                        BusinessPartnerButtonContent = " Sinhronizacija u toku " + counter++ + " od " + total;
                        sqlLite.Delete(businessPartner.Identifier);
                        businessPartner.IsSynced = true;
                        sqlLite.Create(businessPartner);
                    }

                    MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani (" + businessPartnersFromDB.Count + ")!";
                }

                PopulateData();

                BusinessPartnerButtonContent = " Sinhronizacija lekova sa serverom ";
                BusinessPartnerButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
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
