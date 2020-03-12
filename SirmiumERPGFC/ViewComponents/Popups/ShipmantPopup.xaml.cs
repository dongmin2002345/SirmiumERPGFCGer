using Ninject;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Messages.Common.Shipments;
using ServiceInterfaces.ViewModels.Common.Shipments;
using ServiceWebApi.Implementations.Common.Shipments;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Shipments;
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
    /// <summary>
    /// Interaction logic for ShipmantPopup.xaml
    /// </summary>
    public partial class ShipmantPopup : UserControl, INotifyPropertyChanged
    {
        IShipmentService ShipmentService;

        #region CurrentShipment
        public ShipmentViewModel CurrentShipment
        {
            get { return (ShipmentViewModel)GetValue(CurrentShipmentProperty); }
            set { SetValueDp(CurrentShipmentProperty, value); }
        }

        public static readonly DependencyProperty CurrentShipmentProperty = DependencyProperty.Register(
            "CurrentShipment",
            typeof(ShipmentViewModel),
            typeof(ShipmantPopup),
            new PropertyMetadata(OnCurrentShipmentPropertyChanged));

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static void OnCurrentShipmentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ShipmantPopup popup = source as ShipmantPopup;
            ShipmentViewModel Shipment = (ShipmentViewModel)e.NewValue;
            //popup.txtShipmant.Text = Shipment != null ? Shipment.Code.ToString() : "";
            //popup.txtShipmant.Text = Shipment != null ? Shipment.ShipmentDate.ToString() : "";
            //popup.txtShipmant.Text = Shipment != null ? Shipment.Address.ToString() : "";
            ////popup.txtShipmant.Text = Shipment != null ? Shipment.ServiceDelivery.Name.ToString() : "";
            popup.txtShipmant.Text = Shipment != null ? Shipment.ShipmentNumber.ToString() : "";
        }
        #endregion

        #region ShipmentsFromDB
        private ObservableCollection<ShipmentViewModel> _ShipmentsFromDB;

        public ObservableCollection<ShipmentViewModel> ShipmentsFromDB
        {
            get { return _ShipmentsFromDB; }
            set
            {
                if (_ShipmentsFromDB != value)
                {
                    _ShipmentsFromDB = value;
                    NotifyPropertyChanged("ShipmentsFromDB");
                }
            }
        }
        #endregion

        bool textFieldHasFocus = false;

        public ShipmantPopup()
        {
            ShipmentService = DependencyResolver.Kernel.Get<ShipmentService>();

            InitializeComponent();

            (this.Content as FrameworkElement).DataContext = this;

            AddHandler(Keyboard.PreviewKeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }

        private void PopulateFromDb(string filterString = "")
        {
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    new ShipmentSQLiteRepository().Sync(ShipmentService);

                    ShipmentListResponse regionResp = new ShipmentSQLiteRepository().GetShipmentsForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (regionResp.Success)
                        ShipmentsFromDB = new ObservableCollection<ShipmentViewModel>(regionResp.Shipments ?? new List<ShipmentViewModel>());
                    else
                        ShipmentsFromDB = new ObservableCollection<ShipmentViewModel>();
                })
            );
        }

        private void txtShipmant_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();

                popShipmant.IsOpen = true;

                txtShipmantFilter.Focus();
            }

            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtShipmant_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popShipmant.IsOpen = true;

            txtShipmantFilter.Focus();
        }

        private void txtFilterShipmant_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtShipmantFilter.Text))
                PopulateFromDb(txtShipmantFilter.Text);
            else
                PopulateFromDb();
        }

        private void btnChooseShipmant_Click(object sender, RoutedEventArgs e)
        {
            popShipmant.IsOpen = false;

            txtShipmant.Focus();
        }

        private void btnCancleShipmant_Click(object sender, RoutedEventArgs e)
        {
            CurrentShipment = null;

            popShipmant.IsOpen = false;

            txtShipmant.Focus();

        }

        private void dgShipmantList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            popShipmant.IsOpen = false;

            e.Handled = true;

            txtShipmant.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgShipmantList.SelectedIndex > 0)
                    dgShipmantList.SelectedIndex = dgShipmantList.SelectedIndex - 1;
                if (dgShipmantList.SelectedIndex >= 0)
                    dgShipmantList.ScrollIntoView(dgShipmantList.Items[dgShipmantList.SelectedIndex]);
            }

            if (e.Key == Key.Down)
            {
                if (dgShipmantList.SelectedIndex < dgShipmantList.Items.Count)
                    dgShipmantList.SelectedIndex = dgShipmantList.SelectedIndex + 1;
                if (dgShipmantList.SelectedIndex >= 0)
                    dgShipmantList.ScrollIntoView(dgShipmantList.Items[dgShipmantList.SelectedIndex]);
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleShipmant.IsFocused && !btnChooseShipmantCode.IsFocused)
                {
                    if (popShipmant.IsOpen)
                    {
                        // Close popup
                        popShipmant.IsOpen = false;

                        // Move focus to next element
                        txtShipmant.Focus();

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

        private void btnCloseShipmantPopup_Click(object sender, RoutedEventArgs e)
        {
            popShipmant.IsOpen = false;

            txtShipmant.Focus();
        }

        private void DgShipmantList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
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
