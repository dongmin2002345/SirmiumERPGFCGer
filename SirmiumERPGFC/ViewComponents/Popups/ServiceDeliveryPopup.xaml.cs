using Ninject;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Messages.Common.Prices;
using ServiceInterfaces.ViewModels.Common.Prices;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Prices;
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
    /// Interaction logic for ServiceDeliveryPopup.xaml
    /// </summary>
    public partial class ServiceDeliveryPopup : UserControl, INotifyPropertyChanged
    {
        IServiceDeliveryService serviceDeliveryService;

        #region CurrentServiceDelivery
        public ServiceDeliveryViewModel CurrentServiceDelivery
        {
            get { return (ServiceDeliveryViewModel)GetValue(CurrentServiceDeliveryProperty); }
            set { SetValueDp(CurrentServiceDeliveryProperty, value); }
        }

        public static readonly DependencyProperty CurrentServiceDeliveryProperty = DependencyProperty.Register(
            "CurrentServiceDelivery",
            typeof(ServiceDeliveryViewModel),
            typeof(ServiceDeliveryPopup),
            new PropertyMetadata(OnCurrentServiceDeliveryPropertyChanged));

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static void OnCurrentServiceDeliveryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ServiceDeliveryPopup popup = source as ServiceDeliveryPopup;
            ServiceDeliveryViewModel serviceDelivery = (ServiceDeliveryViewModel)e.NewValue;
            popup.txtServiceDelivery.Text = serviceDelivery != null ? serviceDelivery.Name.ToString() : "";
        }
        #endregion

        #region ServiceDeliverysFromDB
        private ObservableCollection<ServiceDeliveryViewModel> _ServiceDeliverysFromDB;

        public ObservableCollection<ServiceDeliveryViewModel> ServiceDeliverysFromDB
        {
            get { return _ServiceDeliverysFromDB; }
            set
            {
                if (_ServiceDeliverysFromDB != value)
                {
                    _ServiceDeliverysFromDB = value;
                    NotifyPropertyChanged("ServiceDeliverysFromDB");
                }
            }
        }
        #endregion

        bool textFieldHasFocus = false;

        public ServiceDeliveryPopup()
        {
            serviceDeliveryService = DependencyResolver.Kernel.Get<IServiceDeliveryService>();

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
                    ServiceDeliveryListResponse serviceDeliveryResponse = new ServiceDeliverySQLiteRepository().GetServiceDeliverysForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (serviceDeliveryResponse.Success)
                    {
                        if (serviceDeliveryResponse.ServiceDeliverys != null && serviceDeliveryResponse.ServiceDeliverys.Count > 0)
                        {
                            ServiceDeliverysFromDB = new ObservableCollection<ServiceDeliveryViewModel>(
                                serviceDeliveryResponse.ServiceDeliverys?.OrderBy(x => Int32.Parse(x.Code))?.ToList() ?? new List<ServiceDeliveryViewModel>());

                            if (ServiceDeliverysFromDB.Count == 1)
                                CurrentServiceDelivery = ServiceDeliverysFromDB.FirstOrDefault();
                        }
                        else
                        {
                            ServiceDeliverysFromDB = new ObservableCollection<ServiceDeliveryViewModel>();

                            CurrentServiceDelivery = null;
                        }
                    }
                })
            );
        }

        private void txtServiceDelivery_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();

                popServiceDelivery.IsOpen = true;

                txtServiceDeliveryFilter.Focus();
            }

            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtServiceDelivery_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popServiceDelivery.IsOpen = true;

            txtServiceDeliveryFilter.Focus();
        }

        private void txtFilterServiceDelivery_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtServiceDeliveryFilter.Text))
                PopulateFromDb(txtServiceDeliveryFilter.Text);
            else
                PopulateFromDb();
        }

        private void btnChooseServiceDelivery_Click(object sender, RoutedEventArgs e)
        {
            popServiceDelivery.IsOpen = false;

            txtServiceDelivery.Focus();
        }

        private void btnCancleServiceDelivery_Click(object sender, RoutedEventArgs e)
        {
            CurrentServiceDelivery = null;

            popServiceDelivery.IsOpen = false;

            txtServiceDelivery.Focus();

        }

        private void dgServiceDeliveryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            popServiceDelivery.IsOpen = false;

            e.Handled = true;

            txtServiceDelivery.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgServiceDeliveryList.SelectedIndex > 0)
                    dgServiceDeliveryList.SelectedIndex = dgServiceDeliveryList.SelectedIndex - 1;
                if (dgServiceDeliveryList.SelectedIndex >= 0)
                    dgServiceDeliveryList.ScrollIntoView(dgServiceDeliveryList.Items[dgServiceDeliveryList.SelectedIndex]);
            }

            if (e.Key == Key.Down)
            {
                if (dgServiceDeliveryList.SelectedIndex < dgServiceDeliveryList.Items.Count)
                    dgServiceDeliveryList.SelectedIndex = dgServiceDeliveryList.SelectedIndex + 1;
                if (dgServiceDeliveryList.SelectedIndex >= 0)
                    dgServiceDeliveryList.ScrollIntoView(dgServiceDeliveryList.Items[dgServiceDeliveryList.SelectedIndex]);
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleServiceDelivery.IsFocused && !btnChooseServiceDeliveryCode.IsFocused)
                {
                    if (popServiceDelivery.IsOpen)
                    {
                        // Close popup
                        popServiceDelivery.IsOpen = false;

                        // Move focus to next element
                        txtServiceDelivery.Focus();

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

        private void btnCloseServiceDeliveryPopup_Click(object sender, RoutedEventArgs e)
        {
            popServiceDelivery.IsOpen = false;

            txtServiceDelivery.Focus();
        }

        private void DgServiceDeliveryList_LoadingRow(object sender, DataGridRowEventArgs e)
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
