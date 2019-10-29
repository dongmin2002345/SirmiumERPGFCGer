using Ninject;
using ServiceInterfaces.Abstractions.Vats;
using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Vats;
using SirmiumERPGFC.Views.Vats;
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
    /// Interaction logic for VatPopup.xaml
    /// </summary>
    public partial class VatPopup : UserControl, INotifyPropertyChanged
    {
        IVatService vatService;

        #region CurrentVat
        public VatViewModel CurrentVat
        {
            get { return (VatViewModel)GetValue(CurrentVatProperty); }
            set { SetValueDp(CurrentVatProperty, value); }
        }

        public static readonly DependencyProperty CurrentVatProperty = DependencyProperty.Register(
            "CurrentVat",
            typeof(VatViewModel),
            typeof(VatPopup),
            new PropertyMetadata(OnCurrentVatPropertyChanged));

        private static void OnCurrentVatPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            VatPopup popup = source as VatPopup;
            VatViewModel vat = (VatViewModel)e.NewValue;
            popup.txtVat.Text = vat != null ? vat.Code + " (" + vat.Amount + ")" : "";
        }
        #endregion

        #region VatsFromDB
        private ObservableCollection<VatViewModel> _VatsFromDB;

        public ObservableCollection<VatViewModel> VatsFromDB
        {
            get { return _VatsFromDB; }
            set
            {
                if (_VatsFromDB != value)
                {
                    _VatsFromDB = value;
                    NotifyPropertyChanged("VatsFromDB");
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

        public VatPopup()
        {
            vatService = DependencyResolver.Kernel.Get<IVatService>();

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
                    new VatSQLiteRepository().Sync(vatService);

                    VatListResponse vatResp = new VatSQLiteRepository().GetVatsForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (vatResp.Success)
                        VatsFromDB = new ObservableCollection<VatViewModel>(vatResp.Vats ?? new List<VatViewModel>());
                    else
                        VatsFromDB = new ObservableCollection<VatViewModel>();
                })
            );
        }

        private void txtVat_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popVat.IsOpen = true;

                txtFilterVat.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtVat_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popVat.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterVat.Focus();
        }

        private void txtFilterVat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterVat.Text))
                PopulateFromDb(txtFilterVat.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseVat_Click(object sender, RoutedEventArgs e)
        {
            popVat.IsOpen = false;

            txtVat.Focus();
        }

        private void btnCancleVat_Click(object sender, RoutedEventArgs e)
        {
            CurrentVat = null;
            popVat.IsOpen = false;

            txtVat.Focus();
        }

        private void btnAddVat_Click(object sender, RoutedEventArgs e)
        {
            popVat.IsOpen = false;

            VatViewModel vat = new VatViewModel();
            vat.Identifier = Guid.NewGuid();

            Vat_AddEdit vatAddEditForm = new Vat_AddEdit(vat, true, true);
            vatAddEditForm.VatCreatedUpdated += new VatHandler(VatAdded);
            FlyoutHelper.OpenFlyoutPopup(this, (string)Application.Current.FindResource("Podaci_o_PDV_u"), 95, vatAddEditForm);

            txtVat.Focus();
        }

        void VatAdded() { }

        private void dgVatList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popVat.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtVat.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgVatList.Items != null && dgVatList.Items.Count > 0)
                {
                    if (dgVatList.SelectedIndex == -1)
                        dgVatList.SelectedIndex = 0;
                    if (dgVatList.SelectedIndex > 0)
                        dgVatList.SelectedIndex = dgVatList.SelectedIndex - 1;
                    dgVatList.ScrollIntoView(dgVatList.Items[dgVatList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgVatList.Items != null && dgVatList.Items.Count > 0)
                {
                    if (dgVatList.SelectedIndex < dgVatList.Items.Count)
                        dgVatList.SelectedIndex = dgVatList.SelectedIndex + 1;
                    dgVatList.ScrollIntoView(dgVatList.Items[dgVatList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleVat.IsFocused && !btnChooseVat.IsFocused)
                {
                    if (popVat.IsOpen)
                    {
                        popVat.IsOpen = false;
                        txtVat.Focus();

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

        private void btnAddVat_LostFocus(object sender, RoutedEventArgs e)
        {
            popVat.IsOpen = false;

            txtVat.Focus();
        }

        private void btnCloseVatPopup_Click(object sender, RoutedEventArgs e)
        {
            popVat.IsOpen = false;

            txtVat.Focus();
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
