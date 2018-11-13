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
    public partial class RegionPopup : UserControl, INotifyPropertyChanged
    {
        IRegionService regionService;

        #region CurrentRegions
        public RegionViewModel CurrentRegion
        {
            get { return (RegionViewModel)GetValue(CurrentRegionProperty); }
            set { SetValueDp(CurrentRegionProperty, value); }
        }

        public static readonly DependencyProperty CurrentRegionProperty = DependencyProperty.Register(
            "CurrentRegion",
            typeof(RegionViewModel),
            typeof(RegionPopup),
            new PropertyMetadata(OnCurrentRegionPropertyChanged));

        private static void OnCurrentRegionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RegionPopup popup = source as RegionPopup;
            RegionViewModel region = (RegionViewModel)e.NewValue;
            popup.txtRegion.Text = region != null ? region.RegionCode + " (" + region.Name + ")" : "";
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
            typeof(RegionPopup),
            new PropertyMetadata(OnCurrentCountryPropertyChanged));

        private static void OnCurrentCountryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RegionPopup popup = source as RegionPopup;
            CountryViewModel animalType = (CountryViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region RegionsFromDB
        private ObservableCollection<RegionViewModel> _RegionsFromDB;

        public ObservableCollection<RegionViewModel> RegionsFromDB
        {
            get { return _RegionsFromDB; }
            set
            {
                if (_RegionsFromDB != value)
                {
                    _RegionsFromDB = value;
                    NotifyPropertyChanged("RegionsFromDB");
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

        public RegionPopup()
        {
            regionService = DependencyResolver.Kernel.Get<IRegionService>();

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
                        new RegionSQLiteRepository().Sync(regionService);

                        RegionListResponse regionResp = new RegionSQLiteRepository().GetRegionsForPopup(MainWindow.CurrentCompanyId, CurrentCountry.Identifier, filterString);
                        if (regionResp.Success)
                            RegionsFromDB = new ObservableCollection<RegionViewModel>(regionResp.Regions ?? new List<RegionViewModel>());
                        else
                            RegionsFromDB = new ObservableCollection<RegionViewModel>();
                    }
                    else
                        RegionsFromDB = new ObservableCollection<RegionViewModel>();
                })
            );
        }

        private void txtRegion_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popRegion.IsOpen = true;

                txtRegionFilter.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtRegion_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popRegion.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtRegionFilter.Focus();
        }

        private void txtRegion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtRegionFilter.Text))
                PopulateFromDb(txtRegionFilter.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseRegion_Click(object sender, RoutedEventArgs e)
        {
            popRegion.IsOpen = false;

            txtRegion.Focus();
        }

        private void btnCancleRegion_Click(object sender, RoutedEventArgs e)
        {
            CurrentRegion = null;
            popRegion.IsOpen = false;

            txtRegion.Focus();
        }

        private void btnAddRegion_Click(object sender, RoutedEventArgs e)
        {
            popRegion.IsOpen = false;

            RegionViewModel region = new RegionViewModel();
            //region.Code = new RegionSQLiteRepository().GetNewCodeValue(MainWindow.CurrentCompanyId);
            region.Identifier = Guid.NewGuid();

            RegionAddEdit regionAddEditForm = new RegionAddEdit(region, true, true);
            regionAddEditForm.RegionCreatedUpdated += new RegionHandler(RegionAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o regionu", 95, regionAddEditForm);

            txtRegion.Focus();
        }

        void RegionAdded() { }

        private void dgRegionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popRegion.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtRegion.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgRegionList.Items != null && dgRegionList.Items.Count > 0)
                {
                    if (dgRegionList.SelectedIndex == -1)
                        dgRegionList.SelectedIndex = 0;
                    if (dgRegionList.SelectedIndex > 0)
                        dgRegionList.SelectedIndex = dgRegionList.SelectedIndex - 1;
                    dgRegionList.ScrollIntoView(dgRegionList.Items[dgRegionList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgRegionList.Items != null && dgRegionList.Items.Count > 0)
                {
                    if (dgRegionList.SelectedIndex < dgRegionList.Items.Count)
                        dgRegionList.SelectedIndex = dgRegionList.SelectedIndex + 1;
                    dgRegionList.ScrollIntoView(dgRegionList.Items[dgRegionList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleRegion.IsFocused && !btnChooseRegionCode.IsFocused)
                {
                    if (popRegion.IsOpen)
                    {
                        popRegion.IsOpen = false;
                        txtRegion.Focus();

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

        private void btnAddRegion_LostFocus(object sender, RoutedEventArgs e)
        {
            popRegion.IsOpen = false;

            txtRegion.Focus();
        }

        private void btnCloseRegionPopup_Click(object sender, RoutedEventArgs e)
        {
            popRegion.IsOpen = false;

            txtRegion.Focus();
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
