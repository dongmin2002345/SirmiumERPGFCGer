using Microsoft.Reporting.WinForms;
using Ninject;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Invoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.Invoices;
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

namespace SirmiumERPGFC.Views.Invoices
{
    public delegate void InvoiceHandler();
    /// <summary>
    /// Interaction logic for InvoiceList.xaml
    /// </summary>
    public partial class InvoiceList : UserControl, INotifyPropertyChanged
    {

        #region Attributes

        #region Services
        IInvoiceService invoiceService;
        IInvoiceItemService invoiceItemService;
        #endregion

        #region InvoiceSearchObject
        private InvoiceViewModel _InvoiceSearchObject = new InvoiceViewModel();

        public InvoiceViewModel InvoiceSearchObject
        {
            get { return _InvoiceSearchObject; }
            set
            {
                if (_InvoiceSearchObject != value)
                {
                    _InvoiceSearchObject = value;
                    NotifyPropertyChanged("InvoiceSearchObject");
                }
            }
        }
        #endregion

        #region InvoicesFromDB
        private ObservableCollection<InvoiceViewModel> _InvoicesFromDB;

        public ObservableCollection<InvoiceViewModel> InvoicesFromDB
        {
            get { return _InvoicesFromDB; }
            set
            {
                if (_InvoicesFromDB != value)
                {
                    _InvoicesFromDB = value;
                    NotifyPropertyChanged("InvoicesFromDB");
                }
            }
        }
        #endregion

        #region CurrentInvoice
        private InvoiceViewModel _CurrentInvoice;

        public InvoiceViewModel CurrentInvoice
        {
            get { return _CurrentInvoice; }
            set
            {
                if (_CurrentInvoice != value)
                {
                    _CurrentInvoice = value;
                    NotifyPropertyChanged("CurrentInvoice");
                    if (_CurrentInvoice != null)
                    {
                        Thread displayItemThread = new Thread(() =>
                        {

                            DisplayInvoiceItemData();

                        });
                        displayItemThread.IsBackground = true;
                        displayItemThread.Start();
                    }
                    else
                        InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>();
                }
            }
        }
        #endregion

        #region InvoiceDataLoading
        private bool _InvoiceDataLoading = true;

        public bool InvoiceDataLoading
        {
            get { return _InvoiceDataLoading; }
            set
            {
                if (_InvoiceDataLoading != value)
                {
                    _InvoiceDataLoading = value;
                    NotifyPropertyChanged("InvoiceDataLoading");
                }
            }
        }
        #endregion


        #region InvoiceItemsFromDB
        private ObservableCollection<InvoiceItemViewModel> _InvoiceItemsFromDB;

        public ObservableCollection<InvoiceItemViewModel> InvoiceItemsFromDB
        {
            get { return _InvoiceItemsFromDB; }
            set
            {
                if (_InvoiceItemsFromDB != value)
                {
                    _InvoiceItemsFromDB = value;
                    NotifyPropertyChanged("InvoiceItemsFromDB");
                }
            }
        }
        #endregion

        #region CurrentInvoiceItem
        private InvoiceItemViewModel _CurrentInvoiceItem;

        public InvoiceItemViewModel CurrentInvoiceItem
        {
            get { return _CurrentInvoiceItem; }
            set
            {
                if (_CurrentInvoiceItem != value)
                {
                    _CurrentInvoiceItem = value;
                    NotifyPropertyChanged("CurrentInvoiceItem");
                }
            }
        }
        #endregion

        #region InvoiceItemDataLoading
        private bool _InvoiceItemDataLoading;

        public bool InvoiceItemDataLoading
        {
            get { return _InvoiceItemDataLoading; }
            set
            {
                if (_InvoiceItemDataLoading != value)
                {
                    _InvoiceItemDataLoading = value;
                    NotifyPropertyChanged("InvoiceItemDataLoading");
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

        #region IsInPDV
        //public ObservableCollection<String> StatusOptions
        //{
        //    get
        //    {
        //        return new ObservableCollection<String>(new List<string>() {
        //                   ChooseStatusConverter.ChooseO,
        //                   ChooseStatusConverter.ChooseB,

        //        });
        //    }
        //}
        #endregion

        #region RefreshButtonContent
        private string _RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));

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

        public InvoiceList()
        {
            // Get required services
            this.invoiceService = DependencyResolver.Kernel.Get<IInvoiceService>();
            this.invoiceItemService = DependencyResolver.Kernel.Get<IInvoiceItemService>();

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

        #region Display data

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;

            Thread displayThread = new Thread(() => DisplayData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        public void DisplayData()
        {
            InvoiceDataLoading = true;

            InvoiceListResponse response = new InvoiceSQLiteRepository()
                .GetInvoicesByPage(MainWindow.CurrentCompanyId, InvoiceSearchObject, currentPage, itemsPerPage);

            if (response.Success)
            {
                InvoicesFromDB = new ObservableCollection<InvoiceViewModel>(response.Invoices ?? new List<InvoiceViewModel>());
                totalItems = response.TotalItems;
            }
            else
            {
                InvoicesFromDB = new ObservableCollection<InvoiceViewModel>();
                totalItems = 0;
                MainWindow.ErrorMessage = response.Message;
            }

            int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
            int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

            PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;

            InvoiceDataLoading = false;
        }

        private void DisplayInvoiceItemData()
        {
            InvoiceItemDataLoading = true;

            InvoiceItemListResponse response = new InvoiceItemSQLiteRepository()
                .GetInvoiceItemsByInvoice(MainWindow.CurrentCompanyId, CurrentInvoice.Identifier);

            if (response.Success)
            {
                InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>(
                    response.InvoiceItems ?? new List<InvoiceItemViewModel>());
            }
            else
            {
                InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>();
            }

            InvoiceItemDataLoading = false;
        }
        private void SyncData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Računi ... ";
            new InvoiceSQLiteRepository().Sync(invoiceService, (synced, toSync) => {
                RefreshButtonContent = " Računi (" + synced + " / " + toSync + ")... ";
            });

            RefreshButtonContent = " Stavke ... ";
            new InvoiceItemSQLiteRepository().Sync(invoiceItemService, (synced, toSync) => {
                RefreshButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });


            DisplayData();
            CurrentInvoice = null;
            InvoiceItemsFromDB = new ObservableCollection<InvoiceItemViewModel>();
            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }
        private void SyncItemData()
        {
            RefreshButtonEnabled = false;

            RefreshButtonContent = " Stavke ... ";
            new InvoiceItemSQLiteRepository().Sync(invoiceItemService, (synced, toSync) => {
                RefreshButtonContent = " Stavke (" + synced + " / " + toSync + ")... ";
            });

            DisplayInvoiceItemData();

            RefreshButtonContent = ((string)Application.Current.FindResource("OSVEŽI"));
            RefreshButtonEnabled = true;
        }
        private void DgInvoices_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DgInvoiceItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
       
        #endregion

        #region Add, edit, delete

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            InvoiceViewModel invoice = new InvoiceViewModel();
            invoice.Identifier = Guid.NewGuid();
            invoice.InvoiceDate = DateTime.Now;

            InvoiceAddEdit addEditForm = new InvoiceAddEdit(invoice, true, false);
            addEditForm.InvoiceCreatedUpdated += new InvoiceHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fakturama")), 95, addEditForm);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentInvoice == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fakturu_za_izmenuUzvičnik"));
                return;
            }

            InvoiceAddEdit addEditForm = new InvoiceAddEdit(CurrentInvoice, false);
            addEditForm.InvoiceCreatedUpdated += new InvoiceHandler(SyncData);
            FlyoutHelper.OpenFlyout(this, ((string)Application.Current.FindResource("Podaci_o_fakturama")), 95, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentInvoice == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_fakturu_za_brisanjeUzvičnik"));
                return;
            }

            // Delete data
            var result = invoiceService.Delete(CurrentInvoice.Identifier);
            if (result.Success)
            {
                MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_obrisaniUzvičnik"));

                Thread displayThread = new Thread(() => SyncData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
            {
                MainWindow.ErrorMessage = result.Message;
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentInvoice == null)
            {
                MainWindow.WarningMessage = "Morate odabrati račun!";
                return;
            }

            #endregion

            Invoice_Item_AddEdit invoiceItemAddEditForm = new Invoice_Item_AddEdit(CurrentInvoice);
            invoiceItemAddEditForm.InvoiceCreatedUpdated += new InvoiceHandler(SyncItemData);
            FlyoutHelper.OpenFlyout(this, (string)Application.Current.FindResource("Podaci"), 95, invoiceItemAddEditForm);
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
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    InvoicesExcelReport.Show(InvoicesFromDB.ToList());
            //}
            //catch (Exception ex)
            //{
            //    MainWindow.ErrorMessage = ex.Message;
            //}
            //Invoice_ReportWindow reportWindow = new Invoice_ReportWindow(CurrentInvoice);
            //reportWindow.Show();
        }
        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    InvoiceExcelReport.Show(CurrentInvoice);
            //}
            //catch (Exception ex)
            //{
            //    MainWindow.ErrorMessage = ex.Message;
            //}
        }
        #region Display documents

        //private void btnShowDocument_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //        //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
        //        Uri pdf = new Uri(CurrentInvoiceDocument.Path, UriKind.RelativeOrAbsolute);
        //        process.StartInfo.FileName = pdf.LocalPath;
        //        process.Start();
        //    }
        //    catch (Exception error)
        //    {
        //        MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}

        #endregion

        string GetFormatted(double? value)
        {
            if (value == null)
                return "";
            return value.Value.ToString("#,###,###,###,##0.00").Replace(",", ".");
        }

        private void BtnPrintInvoiceReport_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentInvoice == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Morate_odabrati_racunUzvičnik"));
                return;
            }

            if(CurrentInvoice.PdvType == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("PDV_vrsta_nije_uneta_na_racunuUzvicnik"));
                return;
            }

            #endregion

            List<InvoiceItemViewModel> itemsFromDB = new List<InvoiceItemViewModel>();

            var response = new InvoiceItemSQLiteRepository().GetInvoiceItemsByInvoice(MainWindow.CurrentCompanyId, CurrentInvoice.Identifier);

            if(response.Success)
            {
                itemsFromDB.AddRange(response.InvoiceItems ?? new List<InvoiceItemViewModel>());
            }



            rdlcInvoiceReport.LocalReport.DataSources.Clear();



            List<RdlcReports.Invoices.InvoiceItemViewModel> items = new List<RdlcReports.Invoices.InvoiceItemViewModel>();

            int id = 1;
            foreach(var item in itemsFromDB)
            {
                var quantity = GetFormatted((double)item.Quantity);

                double? priceWithoutPDVSuffix = null;
                double? priceWithPDVSuffix = null;
                double? amountSuffix = null;
                double? pdvValueSuffix = null;
                double? rebateSuffix = null;
                
                if(CurrentInvoice.CurrencyExchangeRate != null)
                {
                    priceWithoutPDVSuffix = (double)((double)item.PriceWithoutPDV / CurrentInvoice.CurrencyExchangeRate.Value);
                    priceWithPDVSuffix = (double)((double)item.PriceWithPDV / CurrentInvoice.CurrencyExchangeRate.Value);
                    amountSuffix = (double)((double)item.Amount / CurrentInvoice.CurrencyExchangeRate.Value);
                    pdvValueSuffix = (double)((double)item.PDV / CurrentInvoice.CurrencyExchangeRate.Value);
                    rebateSuffix = (double)((double)item.Rebate / CurrentInvoice.CurrencyExchangeRate.Value);
                }

                items.Add(new RdlcReports.Invoices.InvoiceItemViewModel()
                {
                    Id = (id++).ToString(),
                    ProductCode = item.Code,
                    ProductName = item.Name + Environment.NewLine
                        + CurrentInvoice.CurrencyCode,
                    UnitOfMeasurement = item.UnitOfMeasure,
                    Quantity = quantity + Environment.NewLine + quantity,
                    PDVPercent = item.PDVPercent + "%",
                    PriceWithoutPDV = GetFormatted((double)item.PriceWithoutPDV) + Environment.NewLine
                        + GetFormatted(priceWithoutPDVSuffix), 
                    PriceWithPDV = GetFormatted((double)item.PriceWithPDV) + Environment.NewLine
                        + GetFormatted(priceWithPDVSuffix),
                    Amount = GetFormatted((double)item.Amount) + Environment.NewLine
                        + GetFormatted(amountSuffix),
                    PDVValue = GetFormatted((double)item.PDV) + Environment.NewLine
                        + GetFormatted(pdvValueSuffix),
                    Rebate = GetFormatted((double)item.Rebate) + Environment.NewLine
                        + GetFormatted(rebateSuffix)
                });
            }

            var buyerResponse = new BusinessPartnerSQLiteRepository().GetBusinessPartner(CurrentInvoice.Buyer.Identifier);

            BusinessPartnerViewModel buyer = buyerResponse?.BusinessPartner;


            double sumOfAmount = (double)itemsFromDB.Sum(x => x.Amount);
            double sumOfAmountInCurrency = (double)itemsFromDB.Sum(x => x.CurrencyPriceWithPDV);
            double sumOfBase = (double)itemsFromDB.Sum(x => x.PriceWithoutPDV * x.Quantity); // sum of base
            double sumOfPDV = (double)itemsFromDB.Sum(x => x.PDV); // sum of base
            double? sumOfBaseInCurrency = null;

            if(CurrentInvoice.CurrencyExchangeRate != null)
            {
                sumOfBaseInCurrency = (double)(sumOfBase / CurrentInvoice.CurrencyExchangeRate);
            }
            WpfAppCommonCode.Helpers.BrojUTekst brText = new WpfAppCommonCode.Helpers.BrojUTekst();
            string textFormTotal = brText.PretvoriBrojUTekst(sumOfAmount.ToString("#0.00"), '.', "RSD", "");

            List<RdlcReports.Invoices.InvoiceViewModel> invoice = new List<RdlcReports.Invoices.InvoiceViewModel>()
            {
                new RdlcReports.Invoices.InvoiceViewModel()
                {
                    InvoiceNumber = "(97) " + CurrentInvoice.InvoiceNumber,
                    CityAndInvoiceDate = CurrentInvoice.City?.Name + ", " + CurrentInvoice.InvoiceDate.ToString("dd.MM.yyyy"),
                    DeliveryDateOfGoodsAndServices = CurrentInvoice.DateOfPayment?.ToString("dd.MM.yyyy"),
                    DueDate = CurrentInvoice.DueDate.ToString("dd.MM.yyyy"),

                    BusinessPartnerCode = buyer?.Code,
                    BusinessPartnerPIB = buyer?.PIB,
                    BusinessPartnerMB = buyer.IdentificationNumber,
                    BusinessPartnerName = buyer?.Name,
                    BusinessPartnerAddress = CurrentInvoice.Address,
                    BusinessPartnerCity = CurrentInvoice.City?.ZipCode + " " + CurrentInvoice.City?.Name,

                    Amount = GetFormatted((double)sumOfBase),
                    AmountInCurrency = GetFormatted((double)sumOfBaseInCurrency),

                    TotalAmount = GetFormatted((double)sumOfAmount),
                    TotalAmountInCurrency = GetFormatted((double)sumOfAmountInCurrency),
                    TotalBase = GetFormatted((double)sumOfBase),

                    TotalPDV = GetFormatted((double)sumOfPDV),

                    TotalAmountInText = textFormTotal
                }
            };


            var rpdsModel = new ReportDataSource()
            {
                Name = "Invoices",
                Value = invoice
            };

            var rdpsitems = new ReportDataSource()
            {
                Name = "InvoiceItems",
                Value = items
            };
            rdlcInvoiceReport.LocalReport.DataSources.Add(rpdsModel);
            rdlcInvoiceReport.LocalReport.DataSources.Add(rdpsitems);

            //List<ReportParameter> reportParams = new List<ReportParameter>();
            //string parameterText = "Dana " + (CurrentInputInvoice?.InvoiceDate.ToString("dd.MM.yyyy") ?? "") + " na stočni depo klanice Bioesen primljeno je:";
            //reportParams.Add(new ReportParameter("txtInputInvoiceDate", parameterText));


            //var businessPartnerList = new List<InvoiceBusinessPartnerViewModel>();
            //businessPartnerList.Add(new InvoiceBusinessPartnerViewModel() { Name = "Pera peric " });
            //var businessPartnerModel = new ReportDataSource() { Name = "DataSet2", Value = businessPartnerList };
            //rdlcInputNoteReport.LocalReport.DataSources.Add(businessPartnerModel);

            //string exeFolder = System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory());
            //string ContentStart = System.IO.Path.Combine(exeFolder, @"SirmiumERPGFC\RdlcReports\Invoices\InvoiceReport.rdlc");

            switch (CurrentInvoice.PdvType)
            {
                case 1: // sa pdv
                    {
                        rdlcInvoiceReport.LocalReport.ReportEmbeddedResource = "SirmiumERPGFC.RdlcReports.Invoices.Invoice_WithPDV.rdlc";
                        break;
                    }
                case 2: // bez pdv
                    {
                        rdlcInvoiceReport.LocalReport.ReportEmbeddedResource = "SirmiumERPGFC.RdlcReports.Invoices.Invoice_WithoutPDV.rdlc";
                        break;
                    }
                case 3: // nije u sistemu pdv
                    {
                        rdlcInvoiceReport.LocalReport.ReportEmbeddedResource = "SirmiumERPGFC.RdlcReports.Invoices.Invoice_NotInPDV.rdlc";
                        break;
                    }
            }
            //rdlcInvoiceReport.LocalReport.ReportPath = ContentStart;
            // rdlcInputInvoiceReport.LocalReport.SetParameters(reportParams);
            rdlcInvoiceReport.SetDisplayMode(DisplayMode.PrintLayout);
            rdlcInvoiceReport.Refresh();
            rdlcInvoiceReport.ZoomMode = ZoomMode.Percent;
            rdlcInvoiceReport.ZoomPercent = 100;
            rdlcInvoiceReport.RefreshReport();
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
    }
}
