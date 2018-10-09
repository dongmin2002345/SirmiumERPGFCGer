using Newtonsoft.Json;
using Ninject;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;
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

namespace SirmiumERPGFC.Views.Employees
{
    public delegate void IndividualHandler(IndividualViewModel individual);

    public partial class Employee_List : UserControl, INotifyPropertyChanged
    {

        #region Attributes
        IIndividualService individualService;

        #region IndividualsLoading
        private bool _IndividualsLoading;

        public bool IndividualsLoading
        {
            get { return _IndividualsLoading; }
            set
            {
                if (_IndividualsLoading != value)
                {
                    _IndividualsLoading = value;
                    NotifyPropertyChanged("IndividualsLoading");
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

        #region IndividualFilterObject
        private IndividualViewModel _IndividualFilterObject = new IndividualViewModel();

        public IndividualViewModel IndividualFilterObject
        {
            get { return _IndividualFilterObject; }
            set
            {
                if (_IndividualFilterObject != value)
                {
                    _IndividualFilterObject = value;
                    NotifyPropertyChanged("IndividualFilterObject");
                }
            }
        }
        #endregion

        #region IndividualsFromDB
        private ObservableCollection<IndividualViewModel> _IndividualsFromDB;

        public ObservableCollection<IndividualViewModel> IndividualsFromDB
        {
            get
            {
                return _IndividualsFromDB;
            }
            set
            {
                if (_IndividualsFromDB != value)
                {
                    _IndividualsFromDB = value;
                    NotifyPropertyChanged("IndividualsFromDB");
                }
            }
        }
        #endregion

        #region CurrentIndividual
        private IndividualViewModel _CurrentIndividual;

        public IndividualViewModel CurrentIndividual
        {
            get { return _CurrentIndividual; }
            set
            {
                if (_CurrentIndividual != value)
                {
                    _CurrentIndividual = value;
                    NotifyPropertyChanged("CurrentIndividual");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public Employee_List()
        {
            // Get required service
            this.individualService = DependencyResolver.Kernel.Get<IIndividualService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            Thread displayThread = new Thread(() => PopulateData());
            displayThread.IsBackground = true;
            displayThread.Start();
        }

        #endregion

        #region Add, Edit, Delete buttons click

        private void btnAddIndividual_Click(object sender, RoutedEventArgs e)
        {

            Employee_List_AddEdit addEditForm = new Employee_List_AddEdit(new IndividualViewModel());
            addEditForm.IndividualCreatedUpdated += new IndividualHandler((IndividualViewModel) => {
                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            });
            FlyoutHelper.OpenFlyout(this, "Podaci o radnicima", 70, addEditForm);
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            Employee_List_AddEdit addEditForm = new Employee_List_AddEdit(CurrentIndividual);
            addEditForm.IndividualCreatedUpdated += new IndividualHandler((IndividualViewModel) => {
                Thread displayThread = new Thread(() => PopulateData());
                displayThread.IsBackground = true;
                displayThread.Start();
            });
            FlyoutHelper.OpenFlyout(this, "Podaci o radnicima", 70, addEditForm);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Check if any data is selected for delete
            if (CurrentIndividual == null)
            {
                MainWindow.ErrorMessage = ("Morate odabrati radnika za brisanje!");
                return;
            }

            // Show blur effects
            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("radnika", CurrentIndividual.Name);

            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                // Delete business partner
                IndividualResponse response = individualService.Delete(CurrentIndividual.Id);

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

            dgIndividuals.Focus();
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
            IndividualsLoading = true;

            string SearchObjectJson = JsonConvert.SerializeObject(IndividualFilterObject,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
                });


            var response = individualService.GetIndividualsByPage(currentPage, itemsPerPage, SearchObjectJson);
            if (response.Success)
            {
                IndividualsFromDB = new ObservableCollection<IndividualViewModel>(response?.IndividualsByPage ?? new List<IndividualViewModel>());
                totalItems = response?.TotalItems ?? 0;

                int itemFrom = totalItems != 0 ? (currentPage - 1) * itemsPerPage + 1 : 0;
                int itemTo = currentPage * itemsPerPage < totalItems ? currentPage * itemsPerPage : totalItems;

                PaginationDisplay = itemFrom + " - " + itemTo + " od " + totalItems;
            }
            else
            {
                IndividualsFromDB = new ObservableCollection<IndividualViewModel>(new List<IndividualViewModel>());
                MainWindow.ErrorMessage = response.Message;
                totalItems = 0;
            }
            IndividualsLoading = false;
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
            //    List<IndividualViewModel> IndividualsForExport = IndividualsFromDB.ToList();

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
            //    for (int i = 0; i < IndividualsForExport.Count; i++)
            //    {
            //        sheet1.Cells[i + 4, 1] = IndividualsForExport[i].Code;
            //        sheet1.Cells[i + 4, 2] = IndividualsForExport[i].Name;
            //        sheet1.Cells[i + 4, 3] = IndividualsForExport[i].City?.Name;
            //        sheet1.Cells[i + 4, 4] = IndividualsForExport[i].Country?.Name;
            //        sheet1.Cells[i + 4, 5] = IndividualsForExport[i].Address;
            //        sheet1.Cells[i + 4, 6] = IndividualsForExport[i].BankAccountNumber;
            //        sheet1.Cells[i + 4, 7] = IndividualsForExport[i].BankAccountName;
            //        sheet1.Cells[i + 4, 8] = IndividualsForExport[i].PIB;
            //        sheet1.Cells[i + 4, 9] = IndividualsForExport[i].PIO;
            //        sheet1.Cells[i + 4, 10] = IndividualsForExport[i].PDV;
            //        sheet1.Cells[i + 4, 11] = IndividualsForExport[i].Email;
            //        sheet1.Cells[i + 4, 12] = IndividualsForExport[i].WebSite;
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

        //private void btnIndividualPhones_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show blur efects
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    // Create business partner phones window
        //    IndividualPhoneList IndividualPhoneListForm = new IndividualPhoneList(CurrentIndividual.Id);

        //    // Display window
        //    IndividualPhoneListForm.ShowDialog();

        //    // Remove blur efects
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);

        //    dgIndividuals.Focus();
        //}

        //private void btnBankAccounts_Click(object sender, RoutedEventArgs e)
        //{
        //    // Show blur efects
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

        //    // Create business partner bank accounts window
        //    IndividualBankAccountList IndividualBankAccountListForm = new IndividualBankAccountList(CurrentIndividual.Id);

        //    // Display window
        //    IndividualBankAccountListForm.ShowDialog();

        //    // Remove blur efects 
        //    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        //}

        //private void btnCities_Click(object sender, RoutedEventArgs e)
        //{
        //    SirmiumERPVisualEffects.AddEffectOnDialogShow(this);
        //    IndividualLocationList IndividualLocationListForm = new IndividualLocationList(CurrentIndividual.Id);
        //    IndividualLocationListForm.ShowDialog();
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

            //IndividualsReportForm IndividualsReportForm = new IndividualsReportForm();

            //IndividualsReportForm.ShowDialog();

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

