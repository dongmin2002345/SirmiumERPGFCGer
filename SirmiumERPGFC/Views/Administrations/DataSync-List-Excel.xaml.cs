using Microsoft.Win32;
using Newtonsoft.Json;
using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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

namespace SirmiumERPGFC.Views.Administrations
{
    /// <summary>
    /// Interaction logic for DataSync_List_Excel.xaml
    /// </summary>
    public partial class DataSync_List_Excel : UserControl, INotifyPropertyChanged
    {
        public static string BaseApiUrl = "http://localhost:5001/api";

        #region CountryButtonContent
        private string _CountryButtonContent = " Tipovi artikala ";

        public string CountryButtonContent
        {
            get { return _CountryButtonContent; }
            set
            {
                if (_CountryButtonContent != value)
                {
                    _CountryButtonContent = value;
                    NotifyPropertyChanged("CountryButtonContent");
                }
            }
        }
        #endregion

        #region CountryButtonEnabled
        private bool _CountryButtonEnabled = true;

        public bool CountryButtonEnabled
        {
            get { return _CountryButtonEnabled; }
            set
            {
                if (_CountryButtonEnabled != value)
                {
                    _CountryButtonEnabled = value;
                    NotifyPropertyChanged("CountryButtonEnabled");
                }
            }
        }
        #endregion

        public DataSync_List_Excel()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void SendData(string apiUrl, string values)
        {
            BaseResponse response = new BaseResponse();

            using (var client = new WebClient() { Encoding = Encoding.UTF8 })
            {
                client.Proxy = null;
                Uri uri;
                try
                {
                    Uri.TryCreate(apiUrl, UriKind.Absolute, out uri);
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    string jsonResponse = client.UploadString(apiUrl, "POST", values);

                    response = JsonConvert.DeserializeObject<BaseResponse>(jsonResponse);
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = "Dogodila se greska: " + ex.Message;
                    return;
                }

                if (response == null)
                {
                    MainWindow.ErrorMessage = "Dogodila se greška pri učitavanju podataka, proverite mrežu!";
                    return;
                }

                MainWindow.SuccessMessage = "Operacija je uspešno izvršena";
            }
        }

        private void btnCountries_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                CountryButtonContent = " Učitavanje EXCEL fajla... ";
                CountryButtonEnabled = false;

                List<CountryViewModel> Countries = new List<CountryViewModel>();

                #region Excel
                OpenFileDialog oDlg = new OpenFileDialog();
                oDlg.InitialDirectory = "C:\\";
                oDlg.Filter = "xlsx Files (*.xlsx)|*.xlsx";
                if (true == oDlg.ShowDialog())
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(oDlg.FileName);
                    Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                    int rowCount = xlRange.Rows.Count;
                    int colCount = xlRange.Columns.Count;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        CountryViewModel country = new CountryViewModel();
                        country.Mark = xlRange.Cells[i, 1].Text;
                        country.Name = xlRange.Cells[i, 2].Text;
                        country.AlfaCode = xlRange.Cells[i, 3].Text;
                        country.NumericCode = xlRange.Cells[i, 4].Text;

                        country.Identifier = Guid.NewGuid();
                        country.IsSynced = false;
                        country.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        country.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        country.CreatedAt = DateTime.Now;
                        country.UpdatedAt = DateTime.Now;

                        Countries.Add(country);
                    }
                }
                #endregion

                CountryButtonContent = " Unos podataka u toku... ";
                CountryButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedCountries";
                string values = JsonConvert.SerializeObject(
                    Countries,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                CountryButtonContent = " Tipovi artikala ";
                CountryButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}
