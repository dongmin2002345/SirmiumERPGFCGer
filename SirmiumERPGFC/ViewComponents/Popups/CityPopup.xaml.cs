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
    public partial class CityPopup : UserControl, INotifyPropertyChanged
    {
        ICityService cityService;

        #region CurrentCity
        public CityViewModel CurrentCity
        {
            get { return (CityViewModel)GetValue(CurrentCityProperty); }
            set { SetValueDp(CurrentCityProperty, value); }
        }

        public static readonly DependencyProperty CurrentCityProperty = DependencyProperty.Register(
            "CurrentCity",
            typeof(CityViewModel),
            typeof(CityPopup),
            new PropertyMetadata(OnCurrentCityPropertyChanged));

        private static void OnCurrentCityPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CityPopup popup = source as CityPopup;
            CityViewModel city = (CityViewModel)e.NewValue;
            popup.txtCity.Text = city != null ? city.ZipCode + " (" + city.Name + ")" : "";
        }
        #endregion

        #region CurrentCountry
        public CountryViewModel CurrentCountry
        {
            get { return (CountryViewModel)GetValue(CurrentCountryProperty); }
            set { SetValueDp(CurrentCountryProperty, value); }
        }

        public static readonly DependencyProperty CurrentCountryProperty = DependencyProperty.Register(
            "CurrentCountry",
            typeof(CountryViewModel),
            typeof(CityPopup),
            new PropertyMetadata(OnCurrentCountryPropertyChanged));

        private static void OnCurrentCountryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            CityPopup popup = source as CityPopup;
            CountryViewModel animalType = (CountryViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region CitiesFromDB
        private ObservableCollection<CityViewModel> _CitiesFromDB;

        public ObservableCollection<CityViewModel> CitiesFromDB
        {
            get { return _CitiesFromDB; }
            set
            {
                if (_CitiesFromDB != value)
                {
                    _CitiesFromDB = value;
                    NotifyPropertyChanged("CitiesFromDB");
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

        public CityPopup()
        {
            cityService = DependencyResolver.Kernel.Get<ICityService>();

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
                    if (CurrentCountry != null)
                    {
                        CityListResponse cityResp = new CitySQLiteRepository().GetCitiesForPopup(MainWindow.CurrentCompanyId, CurrentCountry.Identifier, filterString);
                        if (cityResp.Success)
                            CitiesFromDB = new ObservableCollection<CityViewModel>(cityResp.Cities ?? new List<CityViewModel>());
                        else
                            CitiesFromDB = new ObservableCollection<CityViewModel>();
                    }
                    else
                        CitiesFromDB = new ObservableCollection<CityViewModel>();
                })
            );
        }

        private void txtCity_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popCity.IsOpen = true;

                txtFilterCities.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtCity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popCity.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterCities.Focus();
        }

        private void txtFilterCities_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterCities.Text))
                PopulateFromDb(txtFilterCities.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseCity_Click(object sender, RoutedEventArgs e)
        {
            popCity.IsOpen = false;

            txtCity.Focus();
        }

        private void btnCancleCity_Click(object sender, RoutedEventArgs e)
        {
            CurrentCity = null;
            popCity.IsOpen = false;

            txtCity.Focus();
        }

        private void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            popCity.IsOpen = false;

            CityViewModel city = new CityViewModel();
            //city.ZipCode = new CitySQLiteRepository().GetNewCodeValue(MainWindow.CurrentCompanyId);
            city.Identifier = Guid.NewGuid();

            CityAddEdit cityAddEditForm = new CityAddEdit(city, true, true);
            cityAddEditForm.CityCreatedUpdated += new CityHandler(CityAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o gradovima", 95, cityAddEditForm);

            txtCity.Focus();
        }

        void CityAdded() { }

        private void dgCityList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popCity.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtCity.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgCityList.Items != null && dgCityList.Items.Count > 0)
                {
                    if (dgCityList.SelectedIndex == -1)
                        dgCityList.SelectedIndex = 0;
                    if (dgCityList.SelectedIndex > 0)
                        dgCityList.SelectedIndex = dgCityList.SelectedIndex - 1;
                    dgCityList.ScrollIntoView(dgCityList.Items[dgCityList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgCityList.Items != null && dgCityList.Items.Count > 0)
                {
                    if (dgCityList.SelectedIndex < dgCityList.Items.Count)
                        dgCityList.SelectedIndex = dgCityList.SelectedIndex + 1;
                    dgCityList.ScrollIntoView(dgCityList.Items[dgCityList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleCity.IsFocused && !btnChooseCity.IsFocused)
                {
                    if (popCity.IsOpen)
                    {
                        popCity.IsOpen = false;
                        txtCity.Focus();

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

        private void btnAddCity_LostFocus(object sender, RoutedEventArgs e)
        {
            popCity.IsOpen = false;

            txtCity.Focus();
        }

        private void btnCloseCityPopup_Click(object sender, RoutedEventArgs e)
        {
            popCity.IsOpen = false;

            txtCity.Focus();
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
