using Microsoft.Win32;
using Newtonsoft.Json;
using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Employees;
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
    public partial class DataSync_List_Excel : UserControl, INotifyPropertyChanged
    {
        public static string BaseApiUrl = "http://localhost:5005/api";

        #region CountryButtonContent
        private string _CountryButtonContent = " Države ";

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


        #region BankButtonContent
        private string _BankButtonContent = " Banke ";

        public string BankButtonContent
        {
            get { return _BankButtonContent; }
            set
            {
                if (_BankButtonContent != value)
                {
                    _BankButtonContent = value;
                    NotifyPropertyChanged("BankButtonContent");
                }
            }
        }
        #endregion

        #region BankButtonEnabled
        private bool _BankButtonEnabled = true;

        public bool BankButtonEnabled
        {
            get { return _BankButtonEnabled; }
            set
            {
                if (_BankButtonEnabled != value)
                {
                    _BankButtonEnabled = value;
                    NotifyPropertyChanged("BankButtonEnabled");
                }
            }
        }
        #endregion


        #region CityButtonContent
        private string _CityButtonContent = " Gradovi ... ";

        public string CityButtonContent
        {
            get { return _CityButtonContent; }
            set
            {
                if (_CityButtonContent != value)
                {
                    _CityButtonContent = value;
                    NotifyPropertyChanged("CityButtonContent");
                }
            }
        }
        #endregion

        #region CityButtonEnabled
        private bool _CityButtonEnabled = true;

        public bool CityButtonEnabled
        {
            get { return _CityButtonEnabled; }
            set
            {
                if (_CityButtonEnabled != value)
                {
                    _CityButtonEnabled = value;
                    NotifyPropertyChanged("CityButtonEnabled");
                }
            }
        }
        #endregion


        #region ProfessionButtonContent
        private string _ProfessionButtonContent = " Profesije ";

        public string ProfessionButtonContent
        {
            get { return _ProfessionButtonContent; }
            set
            {
                if (_ProfessionButtonContent != value)
                {
                    _ProfessionButtonContent = value;
                    NotifyPropertyChanged("ProfessionButtonContent");
                }
            }
        }
        #endregion

        #region ProfessionButtonEnabled
        private bool _ProfessionButtonEnabled = true;

        public bool ProfessionButtonEnabled
        {
            get { return _ProfessionButtonEnabled; }
            set
            {
                if (_ProfessionButtonEnabled != value)
                {
                    _ProfessionButtonEnabled = value;
                    NotifyPropertyChanged("ProfessionButtonEnabled");
                }
            }
        }
        #endregion


        #region LicenceTypeButtonContent
        private string _LicenceTypeButtonContent = " Tip dozvole ";

        public string LicenceTypeButtonContent
        {
            get { return _LicenceTypeButtonContent; }
            set
            {
                if (_LicenceTypeButtonContent != value)
                {
                    _LicenceTypeButtonContent = value;
                    NotifyPropertyChanged("LicenceTypeButtonContent");
                }
            }
        }
        #endregion

        #region LicenceTypeButtonEnabled
        private bool _LicenceTypeButtonEnabled = true;

        public bool LicenceTypeButtonEnabled
        {
            get { return _LicenceTypeButtonEnabled; }
            set
            {
                if (_LicenceTypeButtonEnabled != value)
                {
                    _LicenceTypeButtonEnabled = value;
                    NotifyPropertyChanged("LicenceTypeButtonEnabled");
                }
            }
        }
        #endregion


        #region SectorButtonContent
        private string _SectorButtonContent = " Sektor ";

        public string SectorButtonContent
        {
            get { return _SectorButtonContent; }
            set
            {
                if (_SectorButtonContent != value)
                {
                    _SectorButtonContent = value;
                    NotifyPropertyChanged("SectorButtonContent");
                }
            }
        }
        #endregion

        #region SectorButtonEnabled
        private bool _SectorButtonEnabled = true;

        public bool SectorButtonEnabled
        {
            get { return _SectorButtonEnabled; }
            set
            {
                if (_SectorButtonEnabled != value)
                {
                    _SectorButtonEnabled = value;
                    NotifyPropertyChanged("SectorButtonEnabled");
                }
            }
        }
        #endregion


        #region TaxAdministrationButtonContent
        private string _TaxAdministrationButtonContent = " Poreska uprava ";

        public string TaxAdministrationButtonContent
        {
            get { return _TaxAdministrationButtonContent; }
            set
            {
                if (_TaxAdministrationButtonContent != value)
                {
                    _TaxAdministrationButtonContent = value;
                    NotifyPropertyChanged("TaxAdministrationButtonContent");
                }
            }
        }
        #endregion

        #region TaxAdministrationButtonEnabled
        private bool _TaxAdministrationButtonEnabled = true;

        public bool TaxAdministrationButtonEnabled
        {
            get { return _TaxAdministrationButtonEnabled; }
            set
            {
                if (_TaxAdministrationButtonEnabled != value)
                {
                    _TaxAdministrationButtonEnabled = value;
                    NotifyPropertyChanged("TaxAdministrationButtonEnabled");
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

                CountryButtonContent = " Države ";
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

        private void btnBanks_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                BankButtonContent = " Učitavanje EXCEL fajla... ";
                BankButtonEnabled = false;

                List<BankViewModel> banks = new List<BankViewModel>();

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
                        BankButtonContent = i + " od " + rowCount;

                        BankViewModel bank = new BankViewModel();
                        bank.Swift = xlRange.Cells[i, 1].Text;
                        bank.Name = xlRange.Cells[i, 2].Text;
                        bank.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 3].Text };

                        bank.Identifier = Guid.NewGuid();
                        bank.IsSynced = false;
                        bank.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        bank.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        bank.CreatedAt = DateTime.Now;
                        bank.UpdatedAt = DateTime.Now;

                        if (banks.Where(x => x.Swift == bank.Swift).Count() == 0)
                        {
                            banks.Add(bank);

                            if (i % 100 == 0)
                            {
                                BankButtonContent = " Unos podataka u toku... ";
                                BankButtonEnabled = false;

                                string apiUrlTmp = BaseApiUrl + "/SeedData/SeedBanks";
                                string valuesTmp = JsonConvert.SerializeObject(
                                    banks,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);

                                banks.Clear();
                            }
                        }
                    }
                }
                #endregion

                BankButtonContent = " Unos podataka u toku... ";
                BankButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedBanks";
                string values = JsonConvert.SerializeObject(
                    banks,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                BankButtonContent = " Banke ";
                BankButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCities_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                CityButtonContent = " Učitavanje EXCEL fajla... ";
                CityButtonEnabled = false;

                DateTime createTime = DateTime.Now;

                List<CityViewModel> cities = new List<CityViewModel>();
                List<MunicipalityViewModel> municipalities = new List<MunicipalityViewModel>();
                List<RegionViewModel> regions = new List<RegionViewModel>();

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
                        CityButtonContent = i + " od " + rowCount;

                        RegionViewModel region = new RegionViewModel();
                        region.Code = xlRange.Cells[i, 3].Text;
                        region.RegionCode = xlRange.Cells[i, 3].Text;
                        region.Name = xlRange.Cells[i, 4].Text;
                        region.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 7].Text };

                        region.Identifier = Guid.NewGuid();
                        region.IsSynced = false;
                        region.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        region.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        region.CreatedAt = createTime;
                        region.UpdatedAt = createTime;

                        if (regions.Where(x => x.RegionCode == region.RegionCode).Count() == 0)
                            regions.Add(region);

                        MunicipalityViewModel municipality = new MunicipalityViewModel();
                        municipality.Code = xlRange.Cells[i, 5].Text;
                        municipality.MunicipalityCode = xlRange.Cells[i, 5].Text;
                        municipality.Name = xlRange.Cells[i, 6].Text;
                        municipality.Region = new RegionViewModel() { RegionCode = xlRange.Cells[i, 3].Text };
                        municipality.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 7].Text };

                        municipality.Identifier = Guid.NewGuid();
                        municipality.IsSynced = false;
                        municipality.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        municipality.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        municipality.CreatedAt = createTime;
                        municipality.UpdatedAt = createTime;

                        if (municipalities.Where(x => x.MunicipalityCode == municipality.MunicipalityCode).Count() == 0)
                            municipalities.Add(municipality);

                        CityViewModel city = new CityViewModel();
                        city.Code = xlRange.Cells[i, 1].Text;
                        city.ZipCode = xlRange.Cells[i, 1].Text;
                        city.Name = xlRange.Cells[i, 2].Text;
                        city.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 7].Text };
                        city.Municipality = new MunicipalityViewModel() { MunicipalityCode = xlRange.Cells[i, 5].Text };
                        city.Region = new RegionViewModel() { RegionCode = xlRange.Cells[i, 3].Text };

                        city.Identifier = Guid.NewGuid();
                        city.IsSynced = false;
                        city.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        city.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        city.CreatedAt = createTime;
                        city.UpdatedAt = createTime;

                        if (cities.Where(x => x.ZipCode == city.ZipCode).Count() == 0)
                        {
                            cities.Add(city);

                            if (i % 100 == 0)
                            {
                                CityButtonContent = " Unos regiona u toku... ";
                                CityButtonEnabled = false;

                                string apiUrlTmp = BaseApiUrl + "/SeedData/SeedRegions";
                                string valuesTmp = JsonConvert.SerializeObject(
                                    regions,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);
                                regions.Clear();


                                CityButtonContent = " Unos opstina u toku... ";
                                CityButtonEnabled = false;

                                apiUrlTmp = BaseApiUrl + "/SeedData/SeedMunicipalities";
                                valuesTmp = JsonConvert.SerializeObject(
                                    municipalities,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);
                                municipalities.Clear();


                                CityButtonContent = " Unos gradova u toku... ";
                                CityButtonEnabled = false;

                                apiUrlTmp = BaseApiUrl + "/SeedData/SeedCities";
                                valuesTmp = JsonConvert.SerializeObject(
                                    cities,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);
                                cities.Clear();
                            }
                        }

                    }
                }
                #endregion

                CityButtonContent = " Unos podataka u toku... ";
                CityButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedRegions";
                string values = JsonConvert.SerializeObject(
                    regions,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                apiUrl = BaseApiUrl + "/SeedData/SeedMunicipalities";
                values = JsonConvert.SerializeObject(
                    municipalities,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                apiUrl = BaseApiUrl + "/SeedData/SeedCities";
                values = JsonConvert.SerializeObject(
                    cities,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                CityButtonContent = " Gradovi ";
                CityButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnProfessions_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                ProfessionButtonContent = " Učitavanje EXCEL fajla... ";
                ProfessionButtonEnabled = false;

                List<ProfessionViewModel> banks = new List<ProfessionViewModel>();

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
                        ProfessionViewModel profession = new ProfessionViewModel();
                        profession.Code = xlRange.Cells[i, 1].Text;
                        profession.SecondCode = xlRange.Cells[i, 1].Text;
                        profession.Name = xlRange.Cells[i, 2].Text;
                        profession.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 3].Text };

                        profession.Identifier = Guid.NewGuid();
                        profession.IsSynced = false;
                        profession.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        profession.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        profession.CreatedAt = DateTime.Now;
                        profession.UpdatedAt = DateTime.Now;

                        banks.Add(profession);
                    }
                }
                #endregion

                ProfessionButtonContent = " Unos podataka u toku... ";
                ProfessionButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedProfessions";
                string values = JsonConvert.SerializeObject(
                    banks,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                ProfessionButtonContent = " Profesije ";
                ProfessionButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnLicenceTypes_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                LicenceTypeButtonContent = " Učitavanje EXCEL fajla... ";
                LicenceTypeButtonEnabled = false;

                List<LicenceTypeViewModel> licenceTypes = new List<LicenceTypeViewModel>();

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
                        LicenceTypeButtonContent = i + " od " + rowCount;

                        LicenceTypeViewModel licenceType = new LicenceTypeViewModel();
                        licenceType.Category = xlRange.Cells[i, 1].Text;
                        licenceType.Description = xlRange.Cells[i, 2].Text;
                        licenceType.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 3].Text };

                        licenceType.Identifier = Guid.NewGuid();
                        licenceType.IsSynced = false;
                        licenceType.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        licenceType.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        licenceType.CreatedAt = DateTime.Now;
                        licenceType.UpdatedAt = DateTime.Now;

                        if (licenceTypes.Where(x => x.Code == licenceType.Code).Count() == 0)
                        {
                            licenceTypes.Add(licenceType);

                            if (i % 100 == 0)
                            {
                                BankButtonContent = " Unos podataka u toku... ";
                                BankButtonEnabled = false;

                                string apiUrlTmp = BaseApiUrl + "/SeedData/SeedLicenceTypes";
                                string valuesTmp = JsonConvert.SerializeObject(
                                    licenceTypes,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);

                                licenceTypes.Clear();
                            }
                        }
                    }
                }
                #endregion

                LicenceTypeButtonContent = " Unos podataka u toku... ";
                LicenceTypeButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedLicenceTypes";
                string values = JsonConvert.SerializeObject(
                    licenceTypes,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                LicenceTypeButtonContent = " Tip dozvole ";
                LicenceTypeButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnSectors_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                SectorButtonContent = " Učitavanje EXCEL fajla... ";
                SectorButtonEnabled = false;

                DateTime createTime = DateTime.Now;

                List<AgencyViewModel> agencies = new List<AgencyViewModel>();
                List<SectorViewModel> sectors = new List<SectorViewModel>(); 

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
                        SectorButtonContent = i + " od " + rowCount;

                        SectorViewModel sector = new SectorViewModel(); 
                        sector.Code = xlRange.Cells[i, 3].Text;
                        sector.SecondCode = xlRange.Cells[i, 3].Text;
                        sector.Name = xlRange.Cells[i, 4].Text;
                        sector.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 5].Text };

                        sector.Identifier = Guid.NewGuid();
                        sector.IsSynced = false;
                        sector.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        sector.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        sector.CreatedAt = createTime;
                        sector.UpdatedAt = createTime;

                        if (sectors.Where(x => x.Code == sector.Code).Count() == 0)
                            sectors.Add(sector);

                        AgencyViewModel agency = new AgencyViewModel();
                        agency.Code = xlRange.Cells[i, 1].Text;
                        agency.Name = xlRange.Cells[i, 2].Text;
                        agency.Country = new CountryViewModel() { Mark = xlRange.Cells[i, 5].Text };
                        agency.Sector = new SectorViewModel() { SecondCode = xlRange.Cells[i, 3].Text };

                        agency.Identifier = Guid.NewGuid();
                        agency.IsSynced = false;
                        agency.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        agency.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        agency.CreatedAt = createTime;
                        agency.UpdatedAt = createTime;

                        if (agencies.Where(x => x.Code == agency.Code).Count() == 0)
                            agencies.Add(agency);
                    }
                }
                #endregion

                SectorButtonContent = " Unos podataka u toku... ";
                SectorButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedSectors";
                string values = JsonConvert.SerializeObject(
                    sectors,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                apiUrl = BaseApiUrl + "/SeedData/SeedAgencies";
                values = JsonConvert.SerializeObject(
                    agencies,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                SectorButtonContent = " Sektori ";
                SectorButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnTaxAdministrations_Insert_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                TaxAdministrationButtonContent = " Priprema ... ";
                TaxAdministrationButtonEnabled = false;

                DateTime createTime = DateTime.Now;

                List<TaxAdministrationViewModel> taxAdministrations = new List<TaxAdministrationViewModel>();

                #region Excel
                OpenFileDialog oDlg = new OpenFileDialog();
                oDlg.InitialDirectory = "C:\\";
                oDlg.Filter = "xlsx Files (*.xlsx)|*.xlsx";
                if (true == oDlg.ShowDialog())
                {
                    TaxAdministrationButtonContent = " Brisanje postojecih podataka ... ";

                    string apiUrlDelete = BaseApiUrl + "/SeedData/DeleteTaxAdministrations";
                    SendData(apiUrlDelete, MainWindow.CurrentCompanyId.ToString());

                    TaxAdministrationButtonContent = " Učitavanje EXCEL fajla ... ";

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(oDlg.FileName);
                    Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

                    int rowCount = xlRange.Rows.Count;
                    int colCount = xlRange.Columns.Count;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        TaxAdministrationButtonContent = i + " od " + rowCount;

                        TaxAdministrationViewModel taxAdministration = new TaxAdministrationViewModel();
                        taxAdministration.SecondCode = xlRange.Cells[i, 1]?.Text;
                        taxAdministration.Name = xlRange.Cells[i, 2]?.Text;
                        taxAdministration.City = new CityViewModel() { Name = xlRange.Cells[i, 3]?.Text };
                        taxAdministration.Address1 = xlRange.Cells[i, 4]?.Text;
                        taxAdministration.Address2 = xlRange.Cells[i, 5]?.Text;
                        taxAdministration.Address3 = xlRange.Cells[i, 6]?.Text;
                        taxAdministration.Bank1 = new BankViewModel() { Name = xlRange.Cells[i, 7]?.Text };
                        taxAdministration.IBAN1 = xlRange.Cells[i, 8]?.Text;
                        taxAdministration.SWIFT = xlRange.Cells[i, 9]?.Text;
                        taxAdministration.Bank2 = new BankViewModel() { Name = xlRange.Cells[i, 10]?.Text };

                        taxAdministration.Identifier = Guid.NewGuid();
                        taxAdministration.IsSynced = false;
                        taxAdministration.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };
                        taxAdministration.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                        taxAdministration.CreatedAt = createTime;
                        taxAdministration.UpdatedAt = createTime;

                        if (taxAdministrations.Where(x => x.SecondCode == taxAdministration.SecondCode).Count() == 0)
                        {
                            taxAdministrations.Add(taxAdministration);

                            if (i % 50 == 0)
                            {
                                TaxAdministrationButtonContent = " Unos podataka u toku... ";
                                TaxAdministrationButtonEnabled = false;

                                string apiUrlTmp = BaseApiUrl + "/SeedData/SeedTaxAdministrations";
                                string valuesTmp = JsonConvert.SerializeObject(
                                    taxAdministrations,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                SendData(apiUrlTmp, valuesTmp);

                                taxAdministrations.Clear();
                            }
                        }
                    }
                }
                #endregion

                TaxAdministrationButtonContent = " Unos podataka u toku... ";
                TaxAdministrationButtonEnabled = false;

                string apiUrl = BaseApiUrl + "/SeedData/SeedTaxAdministrations";
                string values = JsonConvert.SerializeObject(
                    taxAdministrations,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                SendData(apiUrl, values);

                TaxAdministrationButtonContent = " Poreska uprava ";
                TaxAdministrationButtonEnabled = true;
            });
            th.IsBackground = true;
            th.Start();
        }
    }
}
