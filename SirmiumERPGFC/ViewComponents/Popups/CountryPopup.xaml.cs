using Ninject;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Messages.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Locations;
using SirmiumERPGFC.Views.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.ViewComponents.Popups
{
    public partial class CountryPopup : UserControl, INotifyPropertyChanged
    {
        ICountryService countryService;

        #region CurrentCountry
        public CountryViewModel CurrentCountry
        {
            get { return (CountryViewModel)GetValue(CurrentCountryProperty); }
            set { SetValueDp(CurrentCountryProperty, value); }
        }

        public static readonly DependencyProperty CurrentCountryProperty = DependencyProperty.Register(
            "CurrentCountry",
            typeof(CountryViewModel),
            typeof(CountryPopup),
            new PropertyMetadata(OnCurrentCountryPropertyChanged));

        private static void OnCurrentCountryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CountryPopup popup = source as CountryPopup;
            CountryViewModel country = (CountryViewModel)e.NewValue;
            popup.txtCountry.Text = country != null ? country.Mark + " (" + country.Name + ")" : "";
        }
        #endregion

        #region CountriesFromDB
        private ObservableCollection<CountryViewModel> _CountriesFromDB;

        public ObservableCollection<CountryViewModel> CountriesFromDB
        {
            get { return _CountriesFromDB; }
            set
            {
                if (_CountriesFromDB != value)
                {
                    _CountriesFromDB = value;
                    NotifyPropertyChanged("CountriesFromDB");
                }
            }
        }
        #endregion

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        bool textFieldHasFocus = false;

        public CountryPopup()
        {
            countryService = DependencyResolver.Kernel.Get<ICountryService>();

            InitializeComponent();

            // MVVM Data binding
            (this.Content as FrameworkElement).DataContext = this;

            AddHandler(Keyboard.PreviewKeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private void PopulateFromDb(string filterString = "")
        {
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    CountryListResponse countryResp = new CountrySQLiteRepository().GetCountriesForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (countryResp.Success)
                        CountriesFromDB = new ObservableCollection<CountryViewModel>(countryResp.Countries ?? new List<CountryViewModel>());
                    else
                        CountriesFromDB = new ObservableCollection<CountryViewModel>();
                })
            );
        }

        private void txtCountry_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popCountry.IsOpen = true;

                txtFilterCountry.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtCountry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popCountry.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterCountry.Focus();
        }

        private void txtFilterCountry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterCountry.Text))
                PopulateFromDb(txtFilterCountry.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseCountry_Click(object sender, RoutedEventArgs e)
        {
            popCountry.IsOpen = false;

            txtCountry.Focus();
        }

        private void btnCancleCountry_Click(object sender, RoutedEventArgs e)
        {
            CurrentCountry = null;
            popCountry.IsOpen = false;

            txtCountry.Focus();
        }

        private void btnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            popCountry.IsOpen = false;

            CountryViewModel country = new CountryViewModel();
            country.Identifier = Guid.NewGuid();

            CountryAddEdit countryAddEditForm = new CountryAddEdit(country, true, true);
            countryAddEditForm.CountryCreatedUpdated += new CountryHandler(CountryAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o drzavama", 95, countryAddEditForm);

            txtCountry.Focus();
        }

        void CountryAdded() { }

        private void dgCountryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popCountry.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtCountry.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgCountryList.Items != null && dgCountryList.Items.Count > 0)
                {
                    if (dgCountryList.SelectedIndex == -1)
                        dgCountryList.SelectedIndex = 0;
                    if (dgCountryList.SelectedIndex > 0)
                        dgCountryList.SelectedIndex = dgCountryList.SelectedIndex - 1;
                    dgCountryList.ScrollIntoView(dgCountryList.Items[dgCountryList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgCountryList.Items != null && dgCountryList.Items.Count > 0)
                {
                    if (dgCountryList.SelectedIndex < dgCountryList.Items.Count)
                        dgCountryList.SelectedIndex = dgCountryList.SelectedIndex + 1;
                    dgCountryList.ScrollIntoView(dgCountryList.Items[dgCountryList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleCountry.IsFocused && !btnChooseCountry.IsFocused)
                {
                    if (popCountry.IsOpen)
                    {
                        popCountry.IsOpen = false;
                        txtCountry.Focus();

                        e.Handled = true;
                    }
                    else
                    {
                        var uie = e.OriginalSource as UIElement;
                        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                        e.Handled = true;
                    }
                }
            }
        }

        #endregion

        private void btnAddCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            popCountry.IsOpen = false;

            txtCountry.Focus();
        }

        private void btnCloseCountryPopup_Click(object sender, RoutedEventArgs e)
        {
            popCountry.IsOpen = false;

            txtCountry.Focus();
        }

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;


        public void NotifyPropertyChanged(string inPropName) //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }

        #endregion
    }
}
