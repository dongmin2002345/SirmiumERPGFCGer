using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.OutputInvoices;
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

namespace SirmiumERPGFC.Views.OutputInvoices
{
    public delegate void OutputInvoiceHandler();

    public partial class OutputInvoiceList : UserControl, INotifyPropertyChanged
    {

        #region Attributes
        IOutputInvoiceService outputInvoiceService;

        #region OutputInvoicesFromDB
        private ObservableCollection<OutputInvoiceViewModel> _OutputInvoicesFromDB;

        public ObservableCollection<OutputInvoiceViewModel> OutputInvoicesFromDB
        {
            get { return _OutputInvoicesFromDB; }
            set
            {
                if (_OutputInvoicesFromDB != value)
                {
                    _OutputInvoicesFromDB = value;
                    NotifyPropertyChanged("OutputInvoicesFromDB");
                }
            }
        }
        #endregion

        #region CurrentOutputInvoice
        private OutputInvoiceViewModel _CurrentOutputInvoice;

        public OutputInvoiceViewModel CurrentOutputInvoice
        {
            get { return _CurrentOutputInvoice; }
            set
            {
                if (_CurrentOutputInvoice != value)
                {
                    _CurrentOutputInvoice = value;
                    NotifyPropertyChanged("CurrentOutputInvoice");
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
                           ChooseStatusConverter.ChooseO,
                           ChooseStatusConverter.ChooseB,

                });
            }
        }
        #endregion

        #region OutputInvoiceSearchObject
        private OutputInvoiceViewModel _OutputInvoiceSearchObject = new OutputInvoiceViewModel();

        public OutputInvoiceViewModel OutputInvoiceSearchObject
        {
            get { return _OutputInvoiceSearchObject; }
            set
            {
                if (_OutputInvoiceSearchObject != value)
                {
                    _OutputInvoiceSearchObject = value;
                    NotifyPropertyChanged("OutputInvoiceSearchObject");
                }
            }
        }
        #endregion

        #region OutputInvoiceDataLoading
        private bool _OutputInvoiceDataLoading = true;

        public bool OutputInvoiceDataLoading
        {
            get { return _OutputInvoiceDataLoading; }
            set
            {
                if (_OutputInvoiceDataLoading != value)
                {
                    _OutputInvoiceDataLoading = value;
                    NotifyPropertyChanged("OutputInvoiceDataLoading");
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


        #region RefreshButtonContent
        private string _RefreshButtonContent = " OSVEŽI ";

        public string RefreshButtonContent
        {
            get { return _RefreshButtonContent; }
            set
            {
                if (_RefreshButtonContent != value)
                {
                    _RefreshButtonContent = value;
                    NotifyPropertyChanged("RefreshButtonContent");
                }
            }
        }
        #endregion

        #region RefreshButtonEnabled
        private bool _RefreshButtonEnabled = true;

        public bool RefreshButtonEnabled
        {
            get { return _RefreshButtonEnabled; }
            set
            {
                if (_RefreshButtonEnabled != value)
                {
                    _RefreshButtonEnabled = value;
                    NotifyPropertyChanged("RefreshButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public OutputInvoiceList()
        {
            // Get required services
            this.outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();

            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => SyncData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread syncThread = new Thread(() =>
            {
                SyncData();

                MainWindow.SuccessMessage = "Podaci su uspešno sinhronizovani!";
            });
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayData()
        {
            OutputInvoiceDataLoading = true;

            OutputInvoiceListResponse response = new OutputInvoiceSQLiteRepository()
                .GetOutputInvoicesByPage(MainWindow.CurrentCompanyId, OutputInvoiceSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                OutputInvoicesFromDB = new ObservableCollection<OutputInvoiceViewModel>(response.OutputInvoices ?? new List<OutputInvoiceViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                OutputInvoicesFromDB = new ObservableCollection<OutputInvoiceViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            OutputInvoiceDataLoading = false;
        }

        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Izlazne fakture ... ";
            new OutputInvoiceSQLiteRepository().Sync(outputInvoiceService);

            DisplayData();

            RefreshButtonContent = " OSVEŽI ";
            RefreshButtonEnabled = true;
        }

        #endregion

        #region Add, edit and delete methods

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OutputInvoiceViewModel OutputInvoice = new OutputInvoiceViewModel();
            OutputInvoice.Identifier = Guid.NewGuid();

            OutputInvoiceAddEdit addEditForm = new OutputInvoiceAddEdit(OutputInvoice, true);
            addEditForm.OutputInvoiceCreatedUpdated += new OutputInvoiceHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o izlaznim fakturama", 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOutputInvoice == null)
            {
                MainWindow.WarningMessage = "Morate odabrati fakturu za izmenu!";
                return;
            }

            OutputInvoiceAddEdit addEditForm = new OutputInvoiceAddEdit(CurrentOutputInvoice, false);
            addEditForm.OutputInvoiceCreatedUpdated += new OutputInvoiceHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, "Podaci o izlaznim fakturama", 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOutputInvoice == null)
            {
                MainWindow.WarningMessage = "Morate odabrati fakturu za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("fakturu", CurrentOutputInvoice.Code);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                OutputInvoiceResponse response = outputInvoiceService.Delete(CurrentOutputInvoice.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new OutputInvoiceSQLiteRepository().Delete(CurrentOutputInvoice.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = "Faktura je uspešno obrisana!";

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #endregion

        #region Pagination

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage = 1;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;

                Thread displayThread = new Thread(() => DisplayData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
            {
                currentPage++;

                Thread displayThread = new Thread(() => DisplayData());
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

                Thread displayThread = new Thread(() => DisplayData());
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
        //#region OutputInvoicesLoading
        //private bool _OutputInvoicesLoading;

        //public bool OutputInvoicesLoading
        //{
        //    get { return _OutputInvoicesLoading; }
        //    set
        //    {
        //        if (_OutputInvoicesLoading != value)
        //        {
        //            _OutputInvoicesLoading = value;
        //            NotifyPropertyChanged("OutputInvoicesLoading");
        //        }
        //    }
        //}
        //#endregion

        //#region Pagination data
        //int currentPage = 1;
        //int itemsPerPage = 20;
        //int totalItems = 0;

        //#region PaginationDisplay
        //private string _PaginationDisplay;

        //public string PaginationDisplay
        //{
        //    get { return _PaginationDisplay; }
        //    set
        //    {
        //        if (_PaginationDisplay != value)
        //        {
        //            _PaginationDisplay = value;
        //            NotifyPropertyChanged("PaginationDisplay");
        //        }
        //    }
        //}
        //#endregion
        //#endregion

        //#region OutputInvoiceFilterObject
        //private OutputInvoiceViewModel _OutputInvoiceFilterObject = new OutputInvoiceViewModel();

        //public OutputInvoiceViewModel OutputInvoiceFilterObject
        //{
        //    get { return _OutputInvoiceFilterObject; }
        //    set
        //    {
        //        if (_OutputInvoiceFilterObject != value)
        //        {
        //            _OutputInvoiceFilterObject = value;
        //            NotifyPropertyChanged("OutputInvoiceFilterObject");
        //        }
        //    }
        //}
        //#endregion

        //#region OutputInvoicesFromDB
        //private ObservableCollection<OutputInvoiceViewModel> _OutputInvoicesFromDB;

        //public ObservableCollection<OutputInvoiceViewModel> OutputInvoicesFromDB
        //{
        //    get
        //    {
        //        return _OutputInvoicesFromDB;
        //    }
        //    set
        //    {
        //        if (_OutputInvoicesFromDB != value)
        //        {
        //            _OutputInvoicesFromDB = value;
        //            NotifyPropertyChanged("OutputInvoicesFromDB");
        //        }
        //    }
        //}
        //#endregion

        //#region CurrentOutputInvoice
        //private OutputInvoiceViewModel _CurrentOutputInvoice;

        //public OutputInvoiceViewModel CurrentOutputInvoice
        //{
        //    get { return _CurrentOutputInvoice; }
        //    set
        //    {
        //        if (_CurrentOutputInvoice != value)
        //        {
        //            _CurrentOutputInvoice = value;
        //            NotifyPropertyChanged("CurrentOutputInvoice");
        //        }
        //    }
        //}
        //#endregion

        //#endregion

        //#region Constructor

        //public OutputInvoiceList()
        //{
        //    // Get required service
        //    this.outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();

        //    // Initialize form components
        //    InitializeComponent();

        //    this.DataContext = this;

        //    Thread displayThread = new Thread(() => PopulateData());
        //    displayThread.IsBackground = true;
        //    displayThread.Start();
        //}

        //#endregion

        //#region Add, Edit, Delete buttons click

        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{

        //    OutputInvoiceAddEdit addEditForm = new OutputInvoiceAddEdit(new OutputInvoiceViewModel());
        //    addEditForm.OutputInvoiceCreatedUpdated += new OutputInvoiceHandler((OutputInvoiceViewModel) => {
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    });
        //    FlyoutHelper.OpenFlyout(this, "Podaci o izlaznim računima", 70, addEditForm);
        //}
        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{

        //    OutputInvoiceAddEdit addEditForm = new OutputInvoiceAddEdit(CurrentOutputInvoice);
        //    addEditForm.OutputInvoiceCreatedUpdated += new OutputInvoiceHandler((OutputInvoiceViewModel) => {
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    });
        //    FlyoutHelper.OpenFlyout(this, "Podaci o izlaznim računima", 70, addEditForm);
        //}

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    // Check if any data is selected for delete
        //    if (CurrentOutputInvoice == null)
        //    {
        //        MainWindow.ErrorMessage = ("Morate odabrati iznizni račun za brisanje!");
        //        return;
        //    }

        //    // Show blur effects
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    // Create confirmation window
        //    DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("izlazni račun", CurrentOutputInvoice.Code.ToString());

        //    var showDialog = deleteConfirmationForm.ShowDialog();
        //    if (showDialog != null && showDialog.Value)
        //    {
        //        // Delete business partner
        //        //OutputInvoiceResponse response = outputInvoiceService.Delete(CurrentOutputInvoice.Id);

        //        //// Display data and notifications
        //        //if (response.Success)
        //        //{
        //        //    MainWindow.SuccessMessage = ("Podaci su uspešno obrisani!");
        //        //    Thread displayThread = new Thread(() => PopulateData());
        //        //    displayThread.IsBackground = true;
        //        //    displayThread.Start();
        //        //}
        //        //else
        //        //{
        //        //    MainWindow.ErrorMessage = (response.Message);
        //        //}
        //    }

        //    // Remove blur effects
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        //    dgOutputInvoices.Focus();
        //}

        //#endregion

        //#region Search buttons
        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    Thread displayThread = new Thread(() => PopulateData());
        //    displayThread.IsBackground = true;
        //    displayThread.Start();
        //}

        //private void PopulateData()    
        //{
        //    OutputInvoicesLoading = true;

        //    string SearchObjectJson = JsonConvert.SerializeObject(OutputInvoiceFilterObject,
        //        Formatting.Indented,
        //        new JsonSerializerSettings
        //        {
        //            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
        //        });


        //   // var response = outputInvoiceService.GetOutputInvoicesByPage(currentPage, itemsPerPage, SearchObjectJson);
        //    //if (response.Success)
        //    //{
        //    //    OutputInvoicesFromDB = new ObservableCollection<OutputInvoiceViewModel>(response?.OutputInvoicesByPage ?? new List<OutputInvoiceViewModel>());
        //    //    totalItems = response?.TotalItems ?? 0;

        //    //    int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
        //    //    int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

        //    //    PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;
        //    //}
        //    //else
        //    //{
        //    //    OutputInvoicesFromDB = new ObservableCollection<OutputInvoiceViewModel>(new List<OutputInvoiceViewModel>());
        //    //    MainWindow.ErrorMessage = response.Message;
        //    //    totalItems = 0;
        //    //}
        //    //OutputInvoicesLoading = false;
        //}
        //#endregion

        //#region Export to excel

        ///// <summary>
        ///// Export all data to excel
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    //    // Create excel workbook and sheet
        //    //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    //    excel.Visible = true;
        //    //    Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
        //    //    Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

        //    //    // Load data that will be exported to excel
        //    //    List<OutputInvoiceViewModel> OutputInvoicesForExport = OutputInvoicesFromDB.ToList();

        //    //    // Insert document headers
        //    //    sheet1.Range[sheet1.Cells[1, 1], sheet1.Cells[1, 12]].Merge();
        //    //    sheet1.Cells[1, 1].HorizontalAlignment = XlHAlign.xlHAlignCenter;
        //    //    sheet1.Cells[1, 1].Font.Bold = true;
        //    //    sheet1.Cells[1, 1] = "Podaci o poslovnim partnerima";

        //    //    // Insert row headers
        //    //    sheet1.Rows[3].Font.Bold = true;
        //    //    sheet1.Cells[3, 1] = "Šifra";
        //    //    sheet1.Cells[3, 2] = "Ime poslovnog partnera";
        //    //    sheet1.Cells[3, 3] = "Grad";
        //    //    sheet1.Cells[3, 4] = "Država";
        //    //    sheet1.Cells[3, 5] = "Adresa";
        //    //    sheet1.Cells[3, 6] = "Broj bankovnog računa";
        //    //    sheet1.Cells[3, 7] = "Naziv računa";
        //    //    sheet1.Cells[3, 8] = "PIB";
        //    //    sheet1.Cells[3, 9] = "PIO";
        //    //    sheet1.Cells[3, 10] = "PDV";
        //    //    sheet1.Cells[3, 11] = "Email";
        //    //    sheet1.Cells[3, 12] = "Sajt";

        //    //    // Insert data to excel
        //    //    for (int i = 0; i < OutputInvoicesForExport.Count; i++)
        //    //    {
        //    //        sheet1.Cells[i + 4, 1] = OutputInvoicesForExport[i].Code;
        //    //        sheet1.Cells[i + 4, 2] = OutputInvoicesForExport[i].Name;
        //    //        sheet1.Cells[i + 4, 3] = OutputInvoicesForExport[i].City?.Name;
        //    //        sheet1.Cells[i + 4, 4] = OutputInvoicesForExport[i].Country?.Name;
        //    //        sheet1.Cells[i + 4, 5] = OutputInvoicesForExport[i].Address;
        //    //        sheet1.Cells[i + 4, 6] = OutputInvoicesForExport[i].BankAccountNumber;
        //    //        sheet1.Cells[i + 4, 7] = OutputInvoicesForExport[i].BankAccountName;
        //    //        sheet1.Cells[i + 4, 8] = OutputInvoicesForExport[i].PIB;
        //    //        sheet1.Cells[i + 4, 9] = OutputInvoicesForExport[i].PIO;
        //    //        sheet1.Cells[i + 4, 10] = OutputInvoicesForExport[i].PDV;
        //    //        sheet1.Cells[i + 4, 11] = OutputInvoicesForExport[i].Email;
        //    //        sheet1.Cells[i + 4, 12] = OutputInvoicesForExport[i].WebSite;
        //    //    }

        //    //    // Set additional options
        //    //    sheet1.Columns.AutoFit();
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MainWindow.ErrorMessage = (ex.Message);
        //    //}

        //}

        //#endregion

        //#region Additional options

        ////private void btnOutputInvoicePhones_Click(object sender, RoutedEventArgs e)
        ////{
        ////    // Show blur efects
        ////    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        ////    // Create business partner phones window
        ////    OutputInvoicePhoneList OutputInvoicePhoneListForm = new OutputInvoicePhoneList(CurrentOutputInvoice.Id);

        ////    // Display window
        ////    OutputInvoicePhoneListForm.ShowDialog();

        ////    // Remove blur efects
        ////    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        ////    dgOutputInvoices.Focus();
        ////}

        ////private void btnBankAccounts_Click(object sender, RoutedEventArgs e)
        ////{
        ////    // Show blur efects
        ////    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        ////    // Create business partner bank accounts window
        ////    OutputInvoiceBankAccountList OutputInvoiceBankAccountListForm = new OutputInvoiceBankAccountList(CurrentOutputInvoice.Id);

        ////    // Display window
        ////    OutputInvoiceBankAccountListForm.ShowDialog();

        ////    // Remove blur efects 
        ////    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        ////}

        ////private void btnCities_Click(object sender, RoutedEventArgs e)
        ////{
        ////    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);
        ////    OutputInvoiceLocationList OutputInvoiceLocationListForm = new OutputInvoiceLocationList(CurrentOutputInvoice.Id);
        ////    OutputInvoiceLocationListForm.ShowDialog();
        ////    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        ////}

        ////private void btnDocuments_Click(object sender, RoutedEventArgs e)
        ////{

        ////}

        ////private void btnPrices_Click(object sender, RoutedEventArgs e)
        ////{

        ////}

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    //SiemiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    //OutputInvoicesReportForm OutputInvoicesReportForm = new OutputInvoicesReportForm();

        //    //OutputInvoicesReportForm.ShowDialog();

        //    //SiemiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        //}

        //private void btnExcelReport_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //#endregion


        //#region Pagination

        //private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage > 1)
        //    {
        //        currentPage = 1;
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }
        //}

        //private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage > 1)
        //    {
        //        currentPage--;
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }
        //}

        //private void btnNextPage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage < Math.Ceiling((double)this.totalItems / this.itemsPerPage))
        //    {
        //        currentPage++;
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }
        //}

        //private void btnLastPage_Click(object sender, RoutedEventArgs e)
        //{
        //    int lastPage = (int)Math.Ceiling((double)this.totalItems / this.itemsPerPage);
        //    if (currentPage < lastPage)
        //    {
        //        currentPage = lastPage;
        //        Thread displayThread = new Thread(() => PopulateData());
        //        displayThread.IsBackground = true;
        //        displayThread.Start();
        //    }
        //}
        //#endregion

        //#region INotifyPropertyChanged implementation
        //public event PropertyChangedEventHandler PropertyChanged;


        //// This method is called by the Set accessor of each property.
        //// The CallerMemberName attribute that is applied to the optional propertyName
        //// parameter causes the property name of the caller to be substituted as an argument.
        //private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //#endregion

    }
}


