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
    public partial class MunicipalityPopup : UserControl, INotifyPropertyChanged
    {
        IMunicipalityService municipalityService;

        #region CurrentMunicipality
        public MunicipalityViewModel CurrentMunicipality
        {
            get { return (MunicipalityViewModel)GetValue(CurrentMunicipalityProperty); }
            set { SetValueDp(CurrentMunicipalityProperty, value); }
        }

        public static readonly DependencyProperty CurrentMunicipalityProperty = DependencyProperty.Register(
            "CurrentMunicipality",
            typeof(MunicipalityViewModel),
            typeof(MunicipalityPopup),
            new PropertyMetadata(OnCurrentMunicipalityPropertyChanged));

        private static void OnCurrentMunicipalityPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            MunicipalityPopup popup = source as MunicipalityPopup;
            MunicipalityViewModel municipality = (MunicipalityViewModel)e.NewValue;
            popup.txtMunicipality.Text = municipality != null ? municipality.MunicipalityCode + " (" + municipality.Name + ")" : "";
        }
        #endregion

        #region CurrentRegion
        public RegionViewModel CurrentRegion
        {
            get { return (RegionViewModel)GetValue(CurrentRegionProperty); }
            set { SetValueDp(CurrentRegionProperty, value); }
        }

        public static readonly DependencyProperty CurrentRegionProperty = DependencyProperty.Register(
            "CurrentRegion",
            typeof(RegionViewModel),
            typeof(MunicipalityPopup),
            new PropertyMetadata(OnCurrentRegionPropertyChanged));

        private static void OnCurrentRegionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            MunicipalityPopup popup = source as MunicipalityPopup;
            RegionViewModel animalType = (RegionViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region MunicipalitysFromDB
        private ObservableCollection<MunicipalityViewModel> _MunicipalitysFromDB;

        public ObservableCollection<MunicipalityViewModel> MunicipalitysFromDB
        {
            get { return _MunicipalitysFromDB; }
            set
            {
                if (_MunicipalitysFromDB != value)
                {
                    _MunicipalitysFromDB = value;
                    NotifyPropertyChanged("MunicipalitysFromDB");
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

        public MunicipalityPopup()
        {
            municipalityService = DependencyResolver.Kernel.Get<IMunicipalityService>();

            InitializeComponent();

            // MVVM Data binding
            (this.Content as FrameworkElement).DataContext = this;

            AddHandler(Keyboard.PreviewKeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private void PopulateFromDb(string filterString = "")
        {
            Thread th = new Thread(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    if (CurrentRegion != null)
                    {
                        //new MunicipalitySQLiteRepository().Sync(municipalityService);

                        MunicipalityListResponse municipalityResp = new MunicipalitySQLiteRepository().GetMunicipalitiesForPopup(MainWindow.CurrentCompanyId, CurrentRegion.Identifier, filterString);
                        if (municipalityResp.Success)
                            MunicipalitysFromDB = new ObservableCollection<MunicipalityViewModel>(municipalityResp.Municipalities ?? new List<MunicipalityViewModel>());
                        else
                            MunicipalitysFromDB = new ObservableCollection<MunicipalityViewModel>();
                    }
                    else
                        MunicipalitysFromDB = new ObservableCollection<MunicipalityViewModel>();
                }));
            });
            th.IsBackground = true;
            th.Start();
        }

        private void txtMunicipality_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popMunicipality.IsOpen = true;

                txtMunicipalityFilter.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtMunicipality_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popMunicipality.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtMunicipalityFilter.Focus();
        }

        private void txtFilterMunicipality_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMunicipalityFilter.Text))
                PopulateFromDb(txtMunicipalityFilter.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseMunicipality_Click(object sender, RoutedEventArgs e)
        {
            popMunicipality.IsOpen = false;

            txtMunicipality.Focus();
        }

        private void btnCancleMunicipality_Click(object sender, RoutedEventArgs e)
        {
            CurrentMunicipality = null;
            popMunicipality.IsOpen = false;

            txtMunicipality.Focus();
        }

        private void btnAddMunicipality_Click(object sender, RoutedEventArgs e)
        {
            popMunicipality.IsOpen = false;

            MunicipalityViewModel municipality = new MunicipalityViewModel();
            //municipality.Code = new MunicipalitySQLiteRepository().GetNewCodeValue(MainWindow.CurrentCompanyId);
            municipality.Identifier = Guid.NewGuid();

            MunicipalityAddEdit municipalityAddEditForm = new MunicipalityAddEdit(municipality, true, true);
            municipalityAddEditForm.MunicipalityCreatedUpdated += new MunicipalityHandler(MunicipalityAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o opštini", 95, municipalityAddEditForm);

            txtMunicipality.Focus();
        }

        void MunicipalityAdded() { }

        private void dgMunicipalityList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popMunicipality.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtMunicipality.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgMunicipalityList.Items != null && dgMunicipalityList.Items.Count > 0)
                {
                    if (dgMunicipalityList.SelectedIndex == -1)
                        dgMunicipalityList.SelectedIndex = 0;
                    if (dgMunicipalityList.SelectedIndex > 0)
                        dgMunicipalityList.SelectedIndex = dgMunicipalityList.SelectedIndex - 1;
                    dgMunicipalityList.ScrollIntoView(dgMunicipalityList.Items[dgMunicipalityList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgMunicipalityList.Items != null && dgMunicipalityList.Items.Count > 0)
                {
                    if (dgMunicipalityList.SelectedIndex < dgMunicipalityList.Items.Count)
                        dgMunicipalityList.SelectedIndex = dgMunicipalityList.SelectedIndex + 1;
                    dgMunicipalityList.ScrollIntoView(dgMunicipalityList.Items[dgMunicipalityList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleMunicipality.IsFocused && !btnChooseMunicipalityCode.IsFocused)
                {
                    if (popMunicipality.IsOpen)
                    {
                        popMunicipality.IsOpen = false;
                        txtMunicipality.Focus();

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

        private void btnAddMunicipality_LostFocus(object sender, RoutedEventArgs e)
        {
            popMunicipality.IsOpen = false;

            txtMunicipality.Focus();
        }

        private void btnCloseMunicipalityPopup_Click(object sender, RoutedEventArgs e)
        {
            popMunicipality.IsOpen = false;

            txtMunicipality.Focus();
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
