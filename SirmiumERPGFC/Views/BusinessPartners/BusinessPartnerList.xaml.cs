using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
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
    public delegate void BusinessPartnerHandler(BusinessPartnerViewModel businessPartner);

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

        #region Add, Edit, Delete buttons click

        private void btnAddBusinessPartner_Click(object sender, RoutedEventArgs e)
        {

            BusinessPartnerAddEdit addEditForm = new BusinessPartnerAddEdit(new BusinessPartnerViewModel());
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler((BusinessPartnerViewModel) => {
                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            });
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 70, addEditForm);
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            BusinessPartnerAddEdit addEditForm = new BusinessPartnerAddEdit(CurrentBusinessPartner);
            addEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler((BusinessPartnerViewModel) => {
                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            });
            FlyoutHelper.OpenFlyout(this, "Podaci o poslovnim partnerima", 70, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Check if any data is selected for delete
            if (CurrentBusinessPartner == null)
            {
                MainWindow.ErrorMessage = ("Morate odabrati poslovnog partnera za brisanje!");
                return;
            }

            // Show blur effects
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("poslovnog partnera", CurrentBusinessPartner.Name);

            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                // Delete business partner
                BusinessPartnerResponse response = businessPartnerService.Delete(CurrentBusinessPartner.Id);

                // Display data and notifications
                if (response.Success)
                {
                    MainWindow.SuccessMessage = ("Podaci su uspešno obrisani!");
                    Thread displayThread = new Thread(() => PopulateData());
                    displayThread.IsBackground = true;
                    displayThread.Start();
                }
                else
                {
                    MainWindow.ErrorMessage = (response.Message);
                }
            }

            // Remove blur effects
            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

            dgBusinessPartners.Focus();
        }

        #endregion

        #region Search buttons
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        private void PopulateData()
        {
            BusinessPartnersLoading = true;

            string SearchObjectJson = JsonConvert.SerializeObject(BusinessPartnerFilterObject,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                });


            var response = businessPartnerService.GetBusinessPartnersByPage(currentPage, itemsPerPage, SearchObjectJson);
            if (response.Success)
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(response?.BusinessPartnersByPage ?? new List<BusinessPartnerViewModel>());
                totalItems = response?.TotalItems ?? 0;

                int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
                int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

                PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;
            }
            else
            {
                BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>(new List<BusinessPartnerViewModel>());
                MainWindow.ErrorMessage = response.Message;
                totalItems = 0;
            }
            BusinessPartnersLoading = false;
        }
        #endregion

        #region Export to excel

        /// <summary>
        /// Export all data to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    // Create excel workbook and sheet
            //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //    excel.Visible = true;
            //    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            //    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            //    // Load data that will be exported to excel
            //    List<BusinessPartnerViewModel> businessPartnersForExport = BusinessPartnersFromDB.ToList();

            //    // Insert document headers
            //    sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, 12]].Merge();
            //    sheet1.Cells[1, 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //    sheet1.Cells[1, 1].Font.Bold = true;
            //    sheet1.Cells[1, 1] = "Podaci o poslovnim partnerima";

            //    // Insert row headers
            //    sheet1.Rows[3].Font.Bold = true;
            //    sheet1.Cells[3, 1] = "Šifra";
            //    sheet1.Cells[3, 2] = "Ime poslovnog partnera";
            //    sheet1.Cells[3, 3] = "Grad";
            //    sheet1.Cells[3, 4] = "Država";
            //    sheet1.Cells[3, 5] = "Adresa";
            //    sheet1.Cells[3, 6] = "Broj bankovnog računa";
            //    sheet1.Cells[3, 7] = "Naziv računa";
            //    sheet1.Cells[3, 8] = "PIB";
            //    sheet1.Cells[3, 9] = "PIO";
            //    sheet1.Cells[3, 10] = "PDV";
            //    sheet1.Cells[3, 11] = "Email";
            //    sheet1.Cells[3, 12] = "Sajt";

            //    // Insert data to excel
            //    for (int i = 0; i < businessPartnersForExport.Count; i++)
            //    {
            //        sheet1.Cells[i + 4, 1] = businessPartnersForExport[i].Code;
            //        sheet1.Cells[i + 4, 2] = businessPartnersForExport[i].Name;
            //        sheet1.Cells[i + 4, 3] = businessPartnersForExport[i].City?.Name;
            //        sheet1.Cells[i + 4, 4] = businessPartnersForExport[i].Country?.Name;
            //        sheet1.Cells[i + 4, 5] = businessPartnersForExport[i].Address;
            //        sheet1.Cells[i + 4, 6] = businessPartnersForExport[i].BankAccountNumber;
            //        sheet1.Cells[i + 4, 7] = businessPartnersForExport[i].BankAccountName;
            //        sheet1.Cells[i + 4, 8] = businessPartnersForExport[i].PIB;
            //        sheet1.Cells[i + 4, 9] = businessPartnersForExport[i].PIO;
            //        sheet1.Cells[i + 4, 10] = businessPartnersForExport[i].PDV;
            //        sheet1.Cells[i + 4, 11] = businessPartnersForExport[i].Email;
            //        sheet1.Cells[i + 4, 12] = businessPartnersForExport[i].WebSite;
            //    }

            //    // Set additional options
            //    sheet1.Columns.AutoFit();
            //}
            //catch (Exception ex)
            //{
            //    MainWindow.ErrorMessage = (ex.Message);
            //}

        }

        #endregion

        #region Additional options

        //private void btnBusinessPartnerPhones_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show blur efects
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    // Create business partner phones window
        //    BusinessPartnerPhoneList businessPartnerPhoneListForm = new BusinessPartnerPhoneList(CurrentBusinessPartner.Id);

        //    // Display window
        //    businessPartnerPhoneListForm.ShowDialog();

        //    // Remove blur efects
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        //    dgBusinessPartners.Focus();
        //}

        //private void btnBankAccounts_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show blur efects
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    // Create business partner bank accounts window
        //    BusinessPartnerBankAccountList businessPartnerBankAccountListForm = new BusinessPartnerBankAccountList(CurrentBusinessPartner.Id);

        //    // Display window
        //    businessPartnerBankAccountListForm.ShowDialog();

        //    // Remove blur efects 
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCities_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);
        //    BusinessPartnerLocationList businessPartnerLocationListForm = new BusinessPartnerLocationList(CurrentBusinessPartner.Id);
        //    businessPartnerLocationListForm.ShowDialog();
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnDocuments_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void btnPrices_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //SiemiumERPVisualEffects.AddEffectOnDialogShow(this);

            //BusinessPartnersReportForm businessPartnersReportForm = new BusinessPartnersReportForm();

            //businessPartnersReportForm.ShowDialog();

            //SiemiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        }

        private void btnExcelReport_Click(object sender, RoutedEventArgs e)
        {

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
