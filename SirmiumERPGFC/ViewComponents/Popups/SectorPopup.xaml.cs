using Ninject;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Locations;
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Sectors;
using SirmiumERPGFC.Views.Sectors;
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
    public partial class SectorPopup : UserControl, INotifyPropertyChanged
    {
        ISectorService SectorService;

        #region CurrentSector
        public SectorViewModel CurrentSector
        {
            get { return (SectorViewModel)GetValue(CurrentSectorProperty); }
            set { SetValueDp(CurrentSectorProperty, value); }
        }

        public static readonly DependencyProperty CurrentSectorProperty = DependencyProperty.Register(
            "CurrentSector",
            typeof(SectorViewModel),
            typeof(SectorPopup),
            new PropertyMetadata(OnCurrentSectorPropertyChanged));

        private static void OnCurrentSectorPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            SectorPopup popup = source as SectorPopup;
            SectorViewModel Sector = (SectorViewModel)e.NewValue;
            popup.txtSector.Text = Sector != null ? Sector.SecondCode + " (" + Sector.Name + ")" : "";
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
            typeof(SectorPopup),
            new PropertyMetadata(OnCurrentCountryPropertyChanged));

        private static void OnCurrentCountryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            SectorPopup popup = source as SectorPopup;
            CountryViewModel animalType = (CountryViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region SectorsFromDB
        private ObservableCollection<SectorViewModel> _SectorsFromDB;

        public ObservableCollection<SectorViewModel> SectorsFromDB
        {
            get { return _SectorsFromDB; }
            set
            {
                if (_SectorsFromDB != value)
                {
                    _SectorsFromDB = value;
                    NotifyPropertyChanged("SectorsFromDB");
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

        public SectorPopup()
        {
            SectorService = DependencyResolver.Kernel.Get<ISectorService>();

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
                        SectorListResponse SectorResp = new SectorSQLiteRepository().GetSectorsForPopup(MainWindow.CurrentCompanyId, CurrentCountry.Identifier, filterString);
                        if (SectorResp.Success)
                            SectorsFromDB = new ObservableCollection<SectorViewModel>(SectorResp.Sectors ?? new List<SectorViewModel>());
                        else
                            SectorsFromDB = new ObservableCollection<SectorViewModel>();
                    }
                    else
                        SectorsFromDB = new ObservableCollection<SectorViewModel>();
                })
            );
        }

        private void txtSector_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popSector.IsOpen = true;

                txtFilterSector.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtSector_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popSector.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterSector.Focus();
        }

        private void txtFilterSector_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterSector.Text))
                PopulateFromDb(txtFilterSector.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseSector_Click(object sender, RoutedEventArgs e)
        {
            popSector.IsOpen = false;

            txtSector.Focus();
        }

        private void btnCancleSector_Click(object sender, RoutedEventArgs e)
        {
            CurrentSector = null;
            popSector.IsOpen = false;

            txtSector.Focus();
        }

        private void btnAddSector_Click(object sender, RoutedEventArgs e)
        {
            popSector.IsOpen = false;

            SectorViewModel Sector = new SectorViewModel();
            Sector.Identifier = Guid.NewGuid();

            Sector_AddEdit SectorAddEditForm = new Sector_AddEdit(Sector, true, true);
            SectorAddEditForm.SectorCreatedUpdated += new SectorHandler(SectorAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o drzavama", 95, SectorAddEditForm);

            txtSector.Focus();
        }

        void SectorAdded() { }

        private void dgSectorList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popSector.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtSector.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgSectorList.Items != null && dgSectorList.Items.Count > 0)
                {
                    if (dgSectorList.SelectedIndex == -1)
                        dgSectorList.SelectedIndex = 0;
                    if (dgSectorList.SelectedIndex > 0)
                        dgSectorList.SelectedIndex = dgSectorList.SelectedIndex - 1;
                    dgSectorList.ScrollIntoView(dgSectorList.Items[dgSectorList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgSectorList.Items != null && dgSectorList.Items.Count > 0)
                {
                    if (dgSectorList.SelectedIndex < dgSectorList.Items.Count)
                        dgSectorList.SelectedIndex = dgSectorList.SelectedIndex + 1;
                    dgSectorList.ScrollIntoView(dgSectorList.Items[dgSectorList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleSector.IsFocused && !btnChooseSector.IsFocused)
                {
                    if (popSector.IsOpen)
                    {
                        popSector.IsOpen = false;
                        txtSector.Focus();

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

        private void btnAddSector_LostFocus(object sender, RoutedEventArgs e)
        {
            popSector.IsOpen = false;

            txtSector.Focus();
        }

        private void btnCloseSectorPopup_Click(object sender, RoutedEventArgs e)
        {
            popSector.IsOpen = false;

            txtSector.Focus();
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
