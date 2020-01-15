using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;
using SirmiumERPGFC.Repository.Invoices;
using SirmiumERPGFC.Repository.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System.ComponentModel;
using System.Threading;
using System.Reflection;

namespace SirmiumERPGFC.RdlcReports.Invoices
{
    public partial class Invoice_PrintPage : MetroWindow, INotifyPropertyChanged
    {
        #region CurrentInvoice
        private ServiceInterfaces.ViewModels.Common.Invoices.InvoiceViewModel _CurrentInvoice;

        public ServiceInterfaces.ViewModels.Common.Invoices.InvoiceViewModel CurrentInvoice
        {
            get { return _CurrentInvoice; }
            set
            {
                if (_CurrentInvoice != value)
                {
                    _CurrentInvoice = value;
                    NotifyPropertyChanged("CurrentInvoice");
                }
            }
        }
        #endregion



        public Invoice_PrintPage(ServiceInterfaces.ViewModels.Common.Invoices.InvoiceViewModel currentInvoice)
        {
            InitializeComponent();

            this.DataContext = this;

            CurrentInvoice = currentInvoice;
        }


        string GetFormatted(double? value, string decimalZeroes = "000")
        {
            if (value == null)
                return "";

            var tmpStr = value.Value.ToString($"#,###,###,###,##0.{decimalZeroes}");



            if (tmpStr.Length < decimalZeroes.Length + 2) // zbog 0.brojdecimala
                tmpStr = $"0.{decimalZeroes}";


            string end = tmpStr.Substring(tmpStr.Length - decimalZeroes.Length, decimalZeroes.Length);

            string start = tmpStr.Substring(0, tmpStr.Length - (decimalZeroes.Length + 1));
            return String.Format("{0},{1}", start.Replace(",", "."), end);
        }

        private async void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            Thread td = new Thread(() =>
            {

                List<ServiceInterfaces.ViewModels.Common.Invoices.InvoiceItemViewModel> itemsFromDB = new List<ServiceInterfaces.ViewModels.Common.Invoices.InvoiceItemViewModel>();

                var response = new InvoiceItemSQLiteRepository().GetInvoiceItemsByInvoice(MainWindow.CurrentCompanyId, CurrentInvoice.Identifier);

                if (response.Success)
                {
                    itemsFromDB.AddRange(response.InvoiceItems ?? new List<ServiceInterfaces.ViewModels.Common.Invoices.InvoiceItemViewModel>());
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    rdlcInvoiceReport.LocalReport.DataSources.Clear();
                }));



                List<RdlcReports.Invoices.InvoiceItemViewModel> items = new List<RdlcReports.Invoices.InvoiceItemViewModel>();

                int id = 1;
                foreach (var item in itemsFromDB)
                {
                    var quantity = GetFormatted((double)item.Quantity, "00");

                    double? priceWithoutPDVSuffix = null;
                    double? priceWithPDVSuffix = null;
                    double? amountSuffix = null;
                    double? pdvValueSuffix = null;
                    double? rebateSuffix = null;

                    if (CurrentInvoice.CurrencyExchangeRate != null)
                    {
                        priceWithoutPDVSuffix = (double)((double)item.BaseAfterDiscount / CurrentInvoice.CurrencyExchangeRate.Value);
                        priceWithPDVSuffix = (double)((double)(item.BaseAfterDiscount + item.PDV) / CurrentInvoice.CurrencyExchangeRate.Value);
                        amountSuffix = (double)((double)item.Amount / CurrentInvoice.CurrencyExchangeRate.Value);
                        pdvValueSuffix = (double)((double)item.PDV / CurrentInvoice.CurrencyExchangeRate.Value);
                        rebateSuffix = (double)((double)item.TotalDiscount / CurrentInvoice.CurrencyExchangeRate.Value);
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
                        PriceWithoutPDV = GetFormatted((double)item.BaseAfterDiscount) + Environment.NewLine
                            + GetFormatted(priceWithoutPDVSuffix),
                        PriceWithPDV = GetFormatted((double)(item.BaseAfterDiscount + item.PDV)) + Environment.NewLine
                            + GetFormatted(priceWithPDVSuffix),
                        Amount = GetFormatted((double)item.Amount) + Environment.NewLine
                            + GetFormatted(amountSuffix),
                        PDVValue = GetFormatted((double)item.PDV) + Environment.NewLine
                            + GetFormatted(pdvValueSuffix),
                        Rebate = GetFormatted((double)item.TotalDiscount) + Environment.NewLine
                            + GetFormatted(rebateSuffix)
                    });
                }

                var buyerResponse = new BusinessPartnerSQLiteRepository().GetBusinessPartner(CurrentInvoice.Buyer.Identifier);

                BusinessPartnerViewModel buyer = buyerResponse?.BusinessPartner;


                double sumOfAmount = (double)itemsFromDB.Sum(x => x.Amount);
                double sumOfAmountInCurrency = (double)itemsFromDB.Sum(x => x.CurrencyPriceWithPDV);
                double sumOfBase = (double)itemsFromDB.Sum(x => x.BaseAfterDiscount); // sum of base
                double sumOfPDV = (double)itemsFromDB.Sum(x => x.PDV); // sum of base
                double? sumOfBaseInCurrency = null;

                if (CurrentInvoice.CurrencyExchangeRate != null)
                {
                    sumOfBaseInCurrency = (double)(sumOfBase / CurrentInvoice.CurrencyExchangeRate);
                }
                WpfAppCommonCode.Helpers.BrojUTekst brText = new WpfAppCommonCode.Helpers.BrojUTekst()
                {
                    Hrvatskezik = false
                };

                string preFormattedText = GetFormatted(sumOfAmount, "00"); 
                string textFormTotal = brText.PretvoriBrojUTekst(preFormattedText, ',', "RSD", "");

                List<RdlcReports.Invoices.InvoiceViewModel> invoice = new List<RdlcReports.Invoices.InvoiceViewModel>()
                {
                    new RdlcReports.Invoices.InvoiceViewModel()
                    {
                        InvoiceNumber = CurrentInvoice.InvoiceNumber,
                        CityAndInvoiceDate = CurrentInvoice.City?.Name + ", " + CurrentInvoice.InvoiceDate.ToString("dd.MM.yyyy"),
                        DeliveryDateOfGoodsAndServices = CurrentInvoice.DateOfPayment?.ToString("dd.MM.yyyy"),
                        DueDate = CurrentInvoice.DueDate.ToString("dd.MM.yyyy"),

                        BusinessPartnerCode = buyer?.InternalCode,
                        BusinessPartnerPIB = buyer?.PIB,
                        BusinessPartnerMB = buyer.IdentificationNumber,
                        BusinessPartnerName = buyer?.Name,
                        BusinessPartnerAddress = CurrentInvoice.Address,
                        BusinessPartnerCity = CurrentInvoice.City?.ZipCode + " " + CurrentInvoice.City?.Name,

                        Amount = GetFormatted((double)sumOfBase),
                        AmountInCurrency = (CurrentInvoice?.CurrencyExchangeRate == null ? "" : GetFormatted((double)sumOfBaseInCurrency)),

                        TotalAmount = GetFormatted((double)sumOfAmount),
                        TotalAmountInCurrency = (CurrentInvoice?.CurrencyExchangeRate == null ? "" : GetFormatted((double)sumOfAmountInCurrency)),
                        TotalBase = GetFormatted((double)sumOfBase),

                        TotalPDV = GetFormatted((double)sumOfPDV),

                        TotalAmountInText = textFormTotal
                    }
                };


                Dispatcher.BeginInvoke(new Action(() =>
                {

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

                    string exeFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                    switch (CurrentInvoice.PdvType)
                    {
                        case 1: // sa pdv
                            {
                                string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\Invoices\Invoice_WithPDV.rdlc");
                                rdlcInvoiceReport.LocalReport.ReportPath = ContentStart;
                                break;
                            }
                        case 2: // bez pdv
                            {
                                string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\Invoices\Invoice_WithoutPDV.rdlc");
                                rdlcInvoiceReport.LocalReport.ReportPath = ContentStart;
                                break;
                            }
                        case 3: // nije u sistemu pdv
                            {
                                string ContentStart = System.IO.Path.Combine(exeFolder, @"RdlcReports\Invoices\Invoice_NotInPDV.rdlc");
                                rdlcInvoiceReport.LocalReport.ReportPath = ContentStart;
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
                }));
            });

            td.IsBackground = true;
            td.Start();
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
