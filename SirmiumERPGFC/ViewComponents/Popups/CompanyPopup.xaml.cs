using Ninject;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Messages.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Companies;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Companies;
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

namespace SirmiumERPGFC.ViewComponents.Popups
{
    /// <summary>
    /// Interaction logic for CompanyPopup.xaml
    /// </summary>
    public partial class CompanyPopup : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Service for accessing companies
        /// </summary>
        ICompanyService companyService;

        #region CurrentCompany
        public CompanyViewModel CurrentCompany
        {
            get { return (CompanyViewModel)GetValue(CurrentCompanyProperty); }
            set { SetValueDp(CurrentCompanyProperty, value); }
        }

        public static readonly DependencyProperty CurrentCompanyProperty = DependencyProperty.Register(
            "CurrentCompany",
            typeof(CompanyViewModel),
            typeof(CompanyPopup),
            new PropertyMetadata(OnCurrentCompanyPropertyChanged));

        private static void OnCurrentCompanyPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CompanyPopup popup = source as CompanyPopup;
            CompanyViewModel company = (CompanyViewModel)e.NewValue;
            popup.txtCompany.Text = company != null ? company.CompanyCode + " (" + company.CompanyName + ")" : "";
        }
        #endregion

        #region CompaniesFromDB
        private ObservableCollection<CompanyViewModel> _CompaniesFromDB;

        public ObservableCollection<CompanyViewModel> CompaniesFromDB
        {
            get { return _CompaniesFromDB; }
            set
            {
                if (_CompaniesFromDB != value)
                {
                    _CompaniesFromDB = value;
                    NotifyPropertyChanged("CompaniesFromDB");
                }
            }
        }
        #endregion

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Is focus already set
        /// </summary>
        bool textFieldHasFocus = false;


        public CompanyPopup()
        {
            companyService = DependencyResolver.Kernel.Get<ICompanyService>();

            InitializeComponent();

            // MVVM Data binding
            (this.Content as FrameworkElement).DataContext = this;

            // Add handler for keyboard shortcuts
            AddHandler(Keyboard.PreviewKeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private void PopulateFromDb(string filterString = "")
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        CompanyListResponse response = companyService.GetCompanies();
                        if (response.Success)
                            CompaniesFromDB = new ObservableCollection<CompanyViewModel>(response.Companies ?? new List<CompanyViewModel>());
                    })
                );
            }
            catch (Exception) { }
        }

        private void txtCompany_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                // Show popup
                popCompany.IsOpen = true;

                // Set focus to filter field in popup
                txtFilterCompanies.Focus();
            }

            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtCompany_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            // Show popup
            popCompany.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            // Set focus to filter field in popup
            txtFilterCompanies.Focus();
        }

        private void txtFilterCompanies_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If text in filter field is not empty, filter items, otherwise display all items
            if (!String.IsNullOrEmpty(txtFilterCompanies.Text))
                PopulateFromDb(txtFilterCompanies.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseCompany_Click(object sender, RoutedEventArgs e)
        {
            // Close popup
            popCompany.IsOpen = false;

            // Set focus on top text box
            txtCompany.Focus();
        }

        private void btnCancleCompany_Click(object sender, RoutedEventArgs e)
        {
            // Set text fields
            CurrentCompany = null;

            // Close popup
            popCompany.IsOpen = false;

            // Set focus on top text box
            txtCompany.Focus();
        }


        private void CompanyAdded(CompanyViewModel comp)
        {
            CurrentCompany = comp;
        }


        private void btnAddCompany_LostFocus(object sender, RoutedEventArgs e)
        {
            // Close popup
            popCompany.IsOpen = false;

            // Move focus to next element
            txtCompany.Focus();
        }

        private void dgCompaniesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            // Close popup
            popCompany.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            // Set focus on top text box
            txtCompany.Focus();
        }

        private void btnCloseCompanyPopup_Click(object sender, RoutedEventArgs e)
        {
            // Close popup
            popCompany.IsOpen = false;

            // Move focus to next element
            txtCompany.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgCompaniesList.Items != null && dgCompaniesList.Items.Count > 0)
                {
                    if (dgCompaniesList.SelectedIndex == -1)
                        dgCompaniesList.SelectedIndex = 0;
                    if (dgCompaniesList.SelectedIndex > 0)
                        dgCompaniesList.SelectedIndex = dgCompaniesList.SelectedIndex - 1;
                    dgCompaniesList.ScrollIntoView(dgCompaniesList.Items[dgCompaniesList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgCompaniesList.Items != null && dgCompaniesList.Items.Count > 0)
                {
                    if (dgCompaniesList.SelectedIndex < dgCompaniesList.Items.Count)
                        dgCompaniesList.SelectedIndex = dgCompaniesList.SelectedIndex + 1;
                    dgCompaniesList.ScrollIntoView(dgCompaniesList.Items[dgCompaniesList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleCompany.IsFocused && !btnChooseCompany.IsFocused)
                {
                    if (popCompany.IsOpen)
                    {
                        // Close popup
                        popCompany.IsOpen = false;

                        // Move focus to text box
                        popCompany.Focus();

                        e.Handled = true;
                    }
                    else
                    {
                        // Move focus to next element
                        var uie = e.OriginalSource as UIElement;
                        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                        e.Handled = true;
                    }
                }
            }
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string inPropName) //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }

        #endregion
    }
}
