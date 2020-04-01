using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.ConstructionSites;
using ServiceInterfaces.ViewModels.Documents;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Helpers;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace SirmiumERPGFC.Views.Documents
{
    /// <summary>
    /// Interaction logic for Documents_Mailing_List.xaml
    /// </summary>
    public partial class Documents_Mailing_List : UserControl, INotifyPropertyChanged
    {

        #region Attributes


        #region BusinessPartner
        #region FilterBusinessPartner
        private BusinessPartnerViewModel _FilterBusinessPartner;

        public BusinessPartnerViewModel FilterBusinessPartner
        {
            get { return _FilterBusinessPartner; }
            set
            {
                if (_FilterBusinessPartner != value)
                {
                    _FilterBusinessPartner = value;
                    NotifyPropertyChanged("FilterBusinessPartner");

                    FilterBusinessPartnerDocuments.Search_Name = _FilterBusinessPartner?.Name;
                }
            }
        }
        #endregion

        #region BusinessPartnerTimer
        CancellationTokenSource cancelBusinessPartnerTimer = new CancellationTokenSource();
        public async void BusinessPartnerTimer()
        {
            Debug.WriteLine(DateTime.Now.Ticks);
            await Task.Delay(1000);


            Debug.WriteLine(DateTime.Now.Ticks);
            Thread td = new Thread(() => {
                DisplayBusinessPartnerDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }
        #endregion


        #region FilterBusinessPartnerDocuments
        private BusinessPartnerDocumentViewModel _FilterBusinessPartnerDocuments;

        public BusinessPartnerDocumentViewModel FilterBusinessPartnerDocuments
        {
            get { return _FilterBusinessPartnerDocuments; }
            set
            {
                if (_FilterBusinessPartnerDocuments != value)
                {
                    _FilterBusinessPartnerDocuments = value;
                    NotifyPropertyChanged("FilterBusinessPartnerDocuments");
                }
            }
        }
        #endregion


        #region BusinessPartnerDocumentsFromDB
        private ObservableCollection<BusinessPartnerDocumentViewModel> _BusinessPartnerDocumentsFromDB;

        public ObservableCollection<BusinessPartnerDocumentViewModel> BusinessPartnerDocumentsFromDB
        {
            get { return _BusinessPartnerDocumentsFromDB; }
            set
            {
                if (_BusinessPartnerDocumentsFromDB != value)
                {
                    _BusinessPartnerDocumentsFromDB = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentBusinessPartnerDocument
        private BusinessPartnerDocumentViewModel _CurrentBusinessPartnerDocument;

        public BusinessPartnerDocumentViewModel CurrentBusinessPartnerDocument
        {
            get { return _CurrentBusinessPartnerDocument; }
            set
            {
                if (_CurrentBusinessPartnerDocument != value)
                {
                    _CurrentBusinessPartnerDocument = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerDocument");
                }
            }
        }
        #endregion

        #region BusinessPartnerDocumentDataLoading
        private bool _BusinessPartnerDocumentDataLoading;

        public bool BusinessPartnerDocumentDataLoading
        {
            get { return _BusinessPartnerDocumentDataLoading; }
            set
            {
                if (_BusinessPartnerDocumentDataLoading != value)
                {
                    _BusinessPartnerDocumentDataLoading = value;
                    NotifyPropertyChanged("BusinessPartnerDocumentDataLoading");
                }
            }
        }
        #endregion

        #endregion


        #region ConstructionSite
        #region FilterConstructionSite
        private ConstructionSiteViewModel _FilterConstructionSite;

        public ConstructionSiteViewModel FilterConstructionSite
        {
            get { return _FilterConstructionSite; }
            set
            {
                if (_FilterConstructionSite != value)
                {
                    _FilterConstructionSite = value;
                    NotifyPropertyChanged("FilterConstructionSite");

                    FilterConstructionSiteDocuments.Search_Name = _FilterConstructionSite?.Name;
                    FilterConstructionSiteDocuments.Search_Code = _FilterConstructionSite?.InternalCode;
                }
            }
        }
        #endregion



        #region FilterConstructionSiteDocuments
        private ConstructionSiteDocumentViewModel _FilterConstructionSiteDocuments;

        public ConstructionSiteDocumentViewModel FilterConstructionSiteDocuments
        {
            get { return _FilterConstructionSiteDocuments; }
            set
            {
                if (_FilterConstructionSiteDocuments != value)
                {
                    _FilterConstructionSiteDocuments = value;
                    NotifyPropertyChanged("FilterConstructionSiteDocuments");
                }
            }
        }
        #endregion


        #region ConstructionSiteTimer
        CancellationTokenSource cancelConstructionSiteTimer = new CancellationTokenSource();
        public async void ConstructionSiteTimer()
        {
            Debug.WriteLine(DateTime.Now.Ticks);
            await Task.Delay(1000);


            Debug.WriteLine(DateTime.Now.Ticks);
            Thread td = new Thread(() => {
                DisplayConstructionSiteDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }
        #endregion


        #region ConstructionSiteDocumentsFromDB
        private ObservableCollection<ConstructionSiteDocumentViewModel> _ConstructionSiteDocumentsFromDB;

        public ObservableCollection<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentsFromDB
        {
            get { return _ConstructionSiteDocumentsFromDB; }
            set
            {
                if (_ConstructionSiteDocumentsFromDB != value)
                {
                    _ConstructionSiteDocumentsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocument
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocument;

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocument
        {
            get { return _CurrentConstructionSiteDocument; }
            set
            {
                if (_CurrentConstructionSiteDocument != value)
                {
                    _CurrentConstructionSiteDocument = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocument");
                }
            }
        }
        #endregion

        #region ConstructionSiteDocumentDataLoading
        private bool _ConstructionSiteDocumentDataLoading;

        public bool ConstructionSiteDocumentDataLoading
        {
            get { return _ConstructionSiteDocumentDataLoading; }
            set
            {
                if (_ConstructionSiteDocumentDataLoading != value)
                {
                    _ConstructionSiteDocumentDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentDataLoading");
                }
            }
        }
        #endregion

        #endregion

        #region Employee
        #region FilterEmployee
        private EmployeeViewModel _FilterEmployee;

        public EmployeeViewModel FilterEmployee
        {
            get { return _FilterEmployee; }
            set
            {
                if (_FilterEmployee != value)
                {
                    _FilterEmployee = value;
                    NotifyPropertyChanged("FilterEmployee");

                    FilterEmployeeDocuments.Search_Name = _FilterEmployee?.Name;
                    FilterEmployeeDocuments.Search_Code = _FilterEmployee?.Code;
                }
            }
        }
        #endregion


        #region EmployeeTimer
        CancellationTokenSource cancelEmployeeTimer = new CancellationTokenSource();
        public async void EmployeeTimer()
        {
            Debug.WriteLine(DateTime.Now.Ticks);
            await Task.Delay(1000);


            Debug.WriteLine(DateTime.Now.Ticks);
            Thread td = new Thread(() => {
                DisplayEmployeeDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }
        #endregion



        #region FilterEmployeeDocuments
        private EmployeeDocumentViewModel _FilterEmployeeDocuments;

        public EmployeeDocumentViewModel FilterEmployeeDocuments
        {
            get { return _FilterEmployeeDocuments; }
            set
            {
                if (_FilterEmployeeDocuments != value)
                {
                    _FilterEmployeeDocuments = value;
                    NotifyPropertyChanged("FilterEmployeeDocuments");
                }
            }
        }
        #endregion


        #region EmployeeDocumentsFromDB
        private ObservableCollection<EmployeeDocumentViewModel> _EmployeeDocumentsFromDB;

        public ObservableCollection<EmployeeDocumentViewModel> EmployeeDocumentsFromDB
        {
            get { return _EmployeeDocumentsFromDB; }
            set
            {
                if (_EmployeeDocumentsFromDB != value)
                {
                    _EmployeeDocumentsFromDB = value;
                    NotifyPropertyChanged("EmployeeDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentEmployeeDocument
        private EmployeeDocumentViewModel _CurrentEmployeeDocument;

        public EmployeeDocumentViewModel CurrentEmployeeDocument
        {
            get { return _CurrentEmployeeDocument; }
            set
            {
                if (_CurrentEmployeeDocument != value)
                {
                    _CurrentEmployeeDocument = value;
                    NotifyPropertyChanged("CurrentEmployeeDocument");
                }
            }
        }
        #endregion

        #region EmployeeDocumentDataLoading
        private bool _EmployeeDocumentDataLoading;

        public bool EmployeeDocumentDataLoading
        {
            get { return _EmployeeDocumentDataLoading; }
            set
            {
                if (_EmployeeDocumentDataLoading != value)
                {
                    _EmployeeDocumentDataLoading = value;
                    NotifyPropertyChanged("EmployeeDocumentDataLoading");
                }
            }
        }
        #endregion

        #endregion



        #region PhysicalPerson
        #region FilterPhysicalPerson
        private PhysicalPersonViewModel _FilterPhysicalPerson;

        public PhysicalPersonViewModel FilterPhysicalPerson
        {
            get { return _FilterPhysicalPerson; }
            set
            {
                if (_FilterPhysicalPerson != value)
                {
                    _FilterPhysicalPerson = value;
                    NotifyPropertyChanged("FilterPhysicalPerson");

                    FilterPhysicalPersonDocuments.Search_Name = _FilterPhysicalPerson?.Name;
                }
            }
        }
        #endregion


        #region PhysicalPersonTimer
        CancellationTokenSource cancelPhysicalPersonTimer = new CancellationTokenSource();
        public async void PhysicalPersonTimer()
        {
            Debug.WriteLine(DateTime.Now.Ticks);
            await Task.Delay(1000);


            Debug.WriteLine(DateTime.Now.Ticks);
            Thread td = new Thread(() => {
                DisplayPhysicalPersonDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }
        #endregion



        #region FilterPhysicalPersonDocuments
        private PhysicalPersonDocumentViewModel _FilterPhysicalPersonDocuments;

        public PhysicalPersonDocumentViewModel FilterPhysicalPersonDocuments
        {
            get { return _FilterPhysicalPersonDocuments; }
            set
            {
                if (_FilterPhysicalPersonDocuments != value)
                {
                    _FilterPhysicalPersonDocuments = value;
                    NotifyPropertyChanged("FilterPhysicalPersonDocuments");
                }
            }
        }
        #endregion


        #region PhysicalPersonDocumentsFromDB
        private ObservableCollection<PhysicalPersonDocumentViewModel> _PhysicalPersonDocumentsFromDB;

        public ObservableCollection<PhysicalPersonDocumentViewModel> PhysicalPersonDocumentsFromDB
        {
            get { return _PhysicalPersonDocumentsFromDB; }
            set
            {
                if (_PhysicalPersonDocumentsFromDB != value)
                {
                    _PhysicalPersonDocumentsFromDB = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentPhysicalPersonDocument
        private PhysicalPersonDocumentViewModel _CurrentPhysicalPersonDocument;

        public PhysicalPersonDocumentViewModel CurrentPhysicalPersonDocument
        {
            get { return _CurrentPhysicalPersonDocument; }
            set
            {
                if (_CurrentPhysicalPersonDocument != value)
                {
                    _CurrentPhysicalPersonDocument = value;
                    NotifyPropertyChanged("CurrentPhysicalPersonDocument");
                }
            }
        }
        #endregion

        #region PhysicalPersonDocumentDataLoading
        private bool _PhysicalPersonDocumentDataLoading;

        public bool PhysicalPersonDocumentDataLoading
        {
            get { return _PhysicalPersonDocumentDataLoading; }
            set
            {
                if (_PhysicalPersonDocumentDataLoading != value)
                {
                    _PhysicalPersonDocumentDataLoading = value;
                    NotifyPropertyChanged("PhysicalPersonDocumentDataLoading");
                }
            }
        }
        #endregion

        #endregion


        #region SelectedDocumentForMail
        private DocumentForMailViewModel _SelectedDocumentForMail;

        public DocumentForMailViewModel SelectedDocumentForMail
        {
            get { return _SelectedDocumentForMail; }
            set
            {
                if (_SelectedDocumentForMail != value)
                {
                    _SelectedDocumentForMail = value;
                    NotifyPropertyChanged("SelectedDocumentForMail");
                }
            }
        }
        #endregion

        #region DocumentsForMail
        private ObservableCollection<DocumentForMailViewModel> _DocumentsForMail;

        public ObservableCollection<DocumentForMailViewModel> DocumentsForMail
        {
            get { return _DocumentsForMail; }
            set
            {
                if (_DocumentsForMail != value)
                {
                    _DocumentsForMail = value;
                    NotifyPropertyChanged("DocumentsForMail");
                }
            }
        }
        #endregion



        #endregion

        #region Constructor
        public Documents_Mailing_List()
        {
            InitializeComponent();

            this.DataContext = this;

            FilterBusinessPartnerDocuments = new BusinessPartnerDocumentViewModel();
            FilterBusinessPartnerDocuments.PropertyChanged += FilterBusinessPartnerDocuments_PropertyChanged;

            FilterConstructionSiteDocuments = new ConstructionSiteDocumentViewModel();
            FilterConstructionSiteDocuments.PropertyChanged += FilterConstructionSiteDocuments_PropertyChanged;

            FilterEmployeeDocuments = new EmployeeDocumentViewModel();
            FilterEmployeeDocuments.PropertyChanged += FilterEmployeeDocuments_PropertyChanged;

            FilterPhysicalPersonDocuments = new PhysicalPersonDocumentViewModel();
            FilterPhysicalPersonDocuments.PropertyChanged += FilterPhysicalPersonDocuments_PropertyChanged;

            DocumentsForMail = new ObservableCollection<DocumentForMailViewModel>();

            Thread td = new Thread(() => {
                DisplayBusinessPartnerDocumentData();

                DisplayConstructionSiteDocumentData();

                DisplayEmployeeDocumentData();

                DisplayPhysicalPersonDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }
        #endregion

        #region Display Data and add items

        #region BusinessPartner

        private void btnRefreshBusinessPartnerDocuments_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => {
                DisplayBusinessPartnerDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }

        private void DgBusinessPartnerDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnShowBusinessPartnerDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentBusinessPartnerDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DisplayBusinessPartnerDocumentData()
        {
            BusinessPartnerDocumentDataLoading = true;

            BusinessPartnerDocumentListResponse response = new BusinessPartnerDocumentSQLiteRepository()
                .GetFilteredBusinessPartnerDocuments(MainWindow.CurrentCompanyId, FilterBusinessPartnerDocuments);

            if (response.Success)
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>(
                    response.BusinessPartnerDocuments ?? new List<BusinessPartnerDocumentViewModel>());
            }
            else
            {
                BusinessPartnerDocumentsFromDB = new ObservableCollection<BusinessPartnerDocumentViewModel>();
            }

            BusinessPartnerDocumentDataLoading = false;
        }



        private void btnAddCheckedBusinessPartnerDocuments_Click(object sender, RoutedEventArgs e)
        {
            var selectedDocs = BusinessPartnerDocumentsFromDB.Where(x => x.IsSelected).ToList();

            foreach(var item in selectedDocs)
            {
                if(DocumentsForMail.Any(x => x.Identifier == item.Identifier))
                {
                    continue;
                }

                DocumentsForMail.Add(new DocumentForMailViewModel()
                {
                    Identifier = item.Identifier,
                    Name = item.Name,
                    Path = item.Path,
                    DocumentFor = item.BusinessPartner?.Name,
                    CreateDate = item.CreateDate ?? DateTime.MinValue
                });
            }
        }
        #endregion

        #region ConstructionSite
        private void btnRefreshConstructionSiteDocuments_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => {
                DisplayConstructionSiteDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }

        private void DgConstructionSiteDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnShowConstructionSiteDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentConstructionSiteDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DisplayConstructionSiteDocumentData()
        {
            ConstructionSiteDocumentDataLoading = true;

            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentSQLiteRepository()
                .GetFilteredConstructionSiteDocuments(MainWindow.CurrentCompanyId, FilterConstructionSiteDocuments);

            if (response.Success)
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>(
                    response.ConstructionSiteDocuments ?? new List<ConstructionSiteDocumentViewModel>());
            }
            else
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>();
            }

            ConstructionSiteDocumentDataLoading = false;
        }

        private void btnAddCheckedConstructionSiteDocuments_Click(object sender, RoutedEventArgs e)
        {
            var selectedDocs = ConstructionSiteDocumentsFromDB.Where(x => x.IsSelected).ToList();

            foreach (var item in selectedDocs)
            {
                if (DocumentsForMail.Any(x => x.Identifier == item.Identifier))
                {
                    continue;
                }

                DocumentsForMail.Add(new DocumentForMailViewModel()
                {
                    Identifier = item.Identifier,
                    Name = item.Name,
                    Path = item.Path,
                    DocumentFor = item.ConstructionSite?.Name,
                    CreateDate = item.CreateDate ?? DateTime.MinValue
                });
            }
        }

        #endregion

        #region Employee
        private void btnRefreshEmployeeDocuments_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => {
                DisplayEmployeeDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }

        private void DgEmployeeDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnShowEmployeeDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentEmployeeDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DisplayEmployeeDocumentData()
        {
            EmployeeDocumentDataLoading = true;

            EmployeeDocumentListResponse response = new EmployeeDocumentSQLiteRepository()
                .GetFilteredEmployeeDocuments(MainWindow.CurrentCompanyId, FilterEmployeeDocuments);

            if (response.Success)
            {
                EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>(
                    response.EmployeeDocuments ?? new List<EmployeeDocumentViewModel>());
            }
            else
            {
                EmployeeDocumentsFromDB = new ObservableCollection<EmployeeDocumentViewModel>();
            }

            EmployeeDocumentDataLoading = false;
        }

        private void btnAddCheckedEmployeeDocuments_Click(object sender, RoutedEventArgs e)
        {
            var selectedDocs = EmployeeDocumentsFromDB.Where(x => x.IsSelected).ToList();

            foreach (var item in selectedDocs)
            {
                if (DocumentsForMail.Any(x => x.Identifier == item.Identifier))
                {
                    continue;
                }

                DocumentsForMail.Add(new DocumentForMailViewModel()
                {
                    Identifier = item.Identifier,
                    Name = item.Name,
                    Path = item.Path,
                    DocumentFor = item.Employee?.Name + " " + item.Employee?.SurName,
                    CreateDate = item.CreateDate ?? DateTime.MinValue
                });
            }
        }

        #endregion

        #region PhysicalPerson
        private void btnRefreshPhysicalPersonDocuments_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => {
                DisplayPhysicalPersonDocumentData();
            });
            td.IsBackground = true;
            td.Start();
        }

        private void DgPhysicalPersonDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnShowPhysicalPersonDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //string path = "C:\\Users\\Zdravko83\\Desktop\\1 ZBORNIK.pdf";
                Uri pdf = new Uri(CurrentPhysicalPersonDocument.Path, UriKind.RelativeOrAbsolute);
                process.StartInfo.FileName = pdf.LocalPath;
                process.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DisplayPhysicalPersonDocumentData()
        {
            PhysicalPersonDocumentDataLoading = true;

            PhysicalPersonDocumentListResponse response = new PhysicalPersonDocumentSQLiteRepository()
                .GetFilteredPhysicalPersonDocuments(MainWindow.CurrentCompanyId, FilterPhysicalPersonDocuments);

            if (response.Success)
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>(
                    response.PhysicalPersonDocuments ?? new List<PhysicalPersonDocumentViewModel>());
            }
            else
            {
                PhysicalPersonDocumentsFromDB = new ObservableCollection<PhysicalPersonDocumentViewModel>();
            }

            PhysicalPersonDocumentDataLoading = false;
        }

        private void btnAddCheckedPhysicalPersonDocuments_Click(object sender, RoutedEventArgs e)
        {

            var selectedDocs = PhysicalPersonDocumentsFromDB.Where(x => x.IsSelected).ToList();

            foreach (var item in selectedDocs)
            {
                if (DocumentsForMail.Any(x => x.Identifier == item.Identifier))
                {
                    continue;
                }

                DocumentsForMail.Add(new DocumentForMailViewModel()
                {
                    Identifier = item.Identifier,
                    Name = item.Name,
                    Path = item.Path,
                    DocumentFor = item.PhysicalPerson?.Name + " " + item.PhysicalPerson?.SurName,
                    CreateDate = item.CreateDate ?? DateTime.MinValue
                });
            }
        }

        #endregion

        #endregion


        #region Other events
        private void dgAddedDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }




        private async void FilterBusinessPartnerDocuments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search_Name" || e.PropertyName == "Search_DateFrom" || e.PropertyName == "Search_DateTo")
            {
                try
                {
                    cancelBusinessPartnerTimer?.Cancel();
                    cancelBusinessPartnerTimer = new CancellationTokenSource();

                    await Task.Run(BusinessPartnerTimer, cancelBusinessPartnerTimer.Token);
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
            }
        }

        private async void FilterConstructionSiteDocuments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search_Name" || e.PropertyName == "Search_Code"
                || e.PropertyName == "Search_DateFrom" || e.PropertyName == "Search_DateTo" )
            {
                try
                {
                    cancelConstructionSiteTimer?.Cancel();
                    cancelConstructionSiteTimer = new CancellationTokenSource();

                    await Task.Run(ConstructionSiteTimer, cancelConstructionSiteTimer.Token);
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
            }
        }

        private async void FilterEmployeeDocuments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search_Name" || e.PropertyName == "Search_DateFrom" || e.PropertyName == "Search_DateTo" 
                || e.PropertyName == "Search_Code")
            {
                try
                {
                    cancelEmployeeTimer?.Cancel();
                    cancelEmployeeTimer = new CancellationTokenSource();

                    await Task.Run(EmployeeTimer, cancelEmployeeTimer.Token);
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
            }
        }

        private async void FilterPhysicalPersonDocuments_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search_Name" || e.PropertyName == "Search_DateFrom" || e.PropertyName == "Search_DateTo")
            {
                try
                {
                    cancelPhysicalPersonTimer?.Cancel();
                    cancelPhysicalPersonTimer = new CancellationTokenSource();

                    await Task.Run(PhysicalPersonTimer, cancelPhysicalPersonTimer.Token);
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
            }
        }


        private void btnRemoveSingleMailDocument_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDocumentForMail != null)
            {
                DocumentsForMail.Remove(SelectedDocumentForMail);
            }
        }

        private void btnOpenFolder_BusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).CommandParameter as BusinessPartnerDocumentViewModel;
            if (item != null)
            {
                if (String.IsNullOrEmpty(item.Path))
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdabranaLokacijaNijeIspravnaUzvicnik"));
                    return;
                }
                OpenPath(item.Path?.Replace("/", "\\"));
            }
        }

        private void btnOpenFolder_ConstructionSite_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).CommandParameter as ConstructionSiteDocumentViewModel;
            if (item != null)
            {
                if (String.IsNullOrEmpty(item.Path))
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdabranaLokacijaNijeIspravnaUzvicnik"));
                    return;
                }
                OpenPath(item.Path?.Replace("/", "\\"));
            }
        }

        private void btnOpenFolder_Employee_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).CommandParameter as EmployeeDocumentViewModel;
            if (item != null)
            {
                if (String.IsNullOrEmpty(item.Path))
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdabranaLokacijaNijeIspravnaUzvicnik"));
                    return;
                }
                OpenPath(item.Path?.Replace("/", "\\"));
            }
        }

        private void btnOpenFolder_PhysicalPerson_Click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).CommandParameter as PhysicalPersonDocumentViewModel;
            if (item != null)
            {
                if (String.IsNullOrEmpty(item.Path))
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdabranaLokacijaNijeIspravnaUzvicnik"));
                    return;
                }
                OpenPath(item.Path?.Replace("/", "\\"));
            }
        }

        void OpenPath(string path)
        {
            var folderPath = path.Replace(System.IO.Path.GetFileName(path), "");
            if (!Directory.Exists(folderPath))
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdabranaLokacijaNijeIspravnaUzvicnik"));
                return;
            }

            try
            {
                Process.Start(folderPath);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }
        #endregion




        #region Select-Deselect events
        private void btnSelectAll_PhysicalPerson_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PhysicalPersonDocumentsFromDB)
            {
                item.IsSelected = true;
            }
        }

        private void btnDeselectAll_PhysicalPerson_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PhysicalPersonDocumentsFromDB)
            {
                item.IsSelected = false;
            }
        }

        private void btnSelectAll_BusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in BusinessPartnerDocumentsFromDB)
            {
                item.IsSelected = true;
            }
        }

        private void btnDeselectAll_BusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in BusinessPartnerDocumentsFromDB)
            {
                item.IsSelected = false;
            }
        }

        private void btnSelectAll_ConstructionSite_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ConstructionSiteDocumentsFromDB)
            {
                item.IsSelected = true;
            }
        }

        private void btnDeselectAll_ConstructionSite_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ConstructionSiteDocumentsFromDB)
            {
                item.IsSelected = false;
            }
        }

        private void btnSelectAll_Employee_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in EmployeeDocumentsFromDB)
            {
                item.IsSelected = true;
            }
        }

        private void btnDeselectAll_Employee_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in EmployeeDocumentsFromDB)
            {
                item.IsSelected = false;
            }
        }

        private void btnSelectAllAdded_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in DocumentsForMail)
                item.IsSelected = true;
        }

        private void btnDeselectAllAdded_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in DocumentsForMail)
                item.IsSelected = false;
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

        private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            var paths = DocumentsForMail.Where(x => x.IsSelected).ToList();

            if (paths.Count() < 1)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdaberiBarJedanDokumentUzvicnik"));
                return;
            }

            foreach(var item in paths)
            {
                DocumentsForMail.Remove(item);
            }
        }

        private void btnZipAndMailTo_Click(object sender, RoutedEventArgs e)
        {

            var paths = DocumentsForMail.Where(x => !String.IsNullOrEmpty(x.Path)).Select(x => x.Path).Distinct().ToList();

            if (paths.Count() < 1)
            {
                MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OdaberiBarJedanDokumentUzvicnik"));
                return;
            }

            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            var result = folderBrowser.ShowDialog();

            if(result == System.Windows.Forms.DialogResult.OK)
            {

                var path = ZipFileHelper.MakeArchiveFromFiles(paths, folderBrowser.SelectedPath);

                if (!String.IsNullOrEmpty(path))
                {
                    try
                    {
                        string outlookPath = AppConfigurationHelper.Configuration?.OutlookDefinedPath ?? "";
                        Process.Start($"{outlookPath}", $"/a \"{path}\" /c ipm.note ");
                    }
                    catch (Exception error)
                    {
                        MainWindow.ErrorMessage = ((string)Application.Current.FindResource("OutlookNijeInstaliranIliNijePovezanUzvicnik"));
                    }
                }
            }
        }

    }
}
