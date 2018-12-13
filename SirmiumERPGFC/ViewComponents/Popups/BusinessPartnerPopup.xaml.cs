using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Views.BusinessPartners;
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
    public partial class BusinessPartnerPopup : UserControl, INotifyPropertyChanged
    {
        IBusinessPartnerService businessPartnerService;

        #region CurrentBusinessPartner
        public BusinessPartnerViewModel CurrentBusinessPartner
        {
            get { return (BusinessPartnerViewModel)GetValue(CurrentBusinessPartnerProperty); }
            set { SetValueDp(CurrentBusinessPartnerProperty, value); }
        }

        public static readonly DependencyProperty CurrentBusinessPartnerProperty = DependencyProperty.Register(
            "CurrentBusinessPartner",
            typeof(BusinessPartnerViewModel),
            typeof(BusinessPartnerPopup),
            new PropertyMetadata(OnCurrentBusinessPartnerPropertyChanged));

        private static void OnCurrentBusinessPartnerPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BusinessPartnerPopup popup = source as BusinessPartnerPopup;
            BusinessPartnerViewModel businessPartner = (BusinessPartnerViewModel)e.NewValue;
            popup.txtBusinessPartner.Text = businessPartner != null ? businessPartner.Code + " (" + businessPartner.Name + ")" : "";
        }
        #endregion

        #region BusinessPartnersFromDB
        private ObservableCollection<BusinessPartnerViewModel> _BusinessPartnersFromDB;

        public ObservableCollection<BusinessPartnerViewModel> BusinessPartnersFromDB
        {
            get { return _BusinessPartnersFromDB; }
            set
            {
                if (_BusinessPartnersFromDB != value)
                {
                    _BusinessPartnersFromDB = value;
                    NotifyPropertyChanged("BusinessPartnersFromDB");
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

        public BusinessPartnerPopup()
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();

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
                    //new BusinessPartnerSQLiteRepository().Sync(businessPartnerService);

                    BusinessPartnerListResponse businessPartnerResp = new BusinessPartnerSQLiteRepository().GetBusinessPartnersForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (businessPartnerResp.Success)
                    {
                        BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>();
                        foreach (var item in businessPartnerResp.BusinessPartners.Where(x => !String.IsNullOrEmpty(x.Name)))
                        {
                            BusinessPartnersFromDB.Add(item);
                        }
                        foreach (var item in businessPartnerResp.BusinessPartners.Where(x => !String.IsNullOrEmpty(x.NameGer)))
                        {
                            item.Name = item.NameGer;
                            BusinessPartnersFromDB.Add(item);
                        }
                    }
                    else
                        BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>();
                })
            );
        }

        private void txtBusinessPartner_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popBusinessPartner.IsOpen = true;

                txtFilterBusinessPartners.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtBusinessPartner_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popBusinessPartner.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterBusinessPartners.Focus();
        }

        private void txtFilterBusinessPartners_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterBusinessPartners.Text))
                PopulateFromDb(txtFilterBusinessPartners.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            popBusinessPartner.IsOpen = false;

            txtBusinessPartner.Focus();
        }

        private void btnCancleBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            CurrentBusinessPartner = null;
            popBusinessPartner.IsOpen = false;

            txtBusinessPartner.Focus();
        }

        private void btnAddBusinessPartner_Click(object sender, RoutedEventArgs e)
        {
            popBusinessPartner.IsOpen = false;

            BusinessPartnerViewModel BusinessPartner = new BusinessPartnerViewModel();
            BusinessPartner.Identifier = Guid.NewGuid();

            BusinessPartner_List_AddEdit BusinessPartnerAddEditForm = new BusinessPartner_List_AddEdit(BusinessPartner, true, true);
            BusinessPartnerAddEditForm.BusinessPartnerCreatedUpdated += new BusinessPartnerHandler(BusinessPartnerAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o poslovnim partnerima", 95, BusinessPartnerAddEditForm);

            txtBusinessPartner.Focus();
        }

        void BusinessPartnerAdded() { }

        private void dgBusinessPartnerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popBusinessPartner.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtBusinessPartner.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgBusinessPartnerList.Items != null && dgBusinessPartnerList.Items.Count > 0)
                {
                    if (dgBusinessPartnerList.SelectedIndex == -1)
                        dgBusinessPartnerList.SelectedIndex = 0;
                    if (dgBusinessPartnerList.SelectedIndex > 0)
                        dgBusinessPartnerList.SelectedIndex = dgBusinessPartnerList.SelectedIndex - 1;
                    dgBusinessPartnerList.ScrollIntoView(dgBusinessPartnerList.Items[dgBusinessPartnerList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgBusinessPartnerList.Items != null && dgBusinessPartnerList.Items.Count > 0)
                {
                    if (dgBusinessPartnerList.SelectedIndex < dgBusinessPartnerList.Items.Count)
                        dgBusinessPartnerList.SelectedIndex = dgBusinessPartnerList.SelectedIndex + 1;
                    dgBusinessPartnerList.ScrollIntoView(dgBusinessPartnerList.Items[dgBusinessPartnerList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleBusinessPartner.IsFocused && !btnChooseBusinessPartner.IsFocused)
                {
                    if (popBusinessPartner.IsOpen)
                    {
                        popBusinessPartner.IsOpen = false;
                        txtBusinessPartner.Focus();

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

        private void btnAddBusinessPartner_LostFocus(object sender, RoutedEventArgs e)
        {
            popBusinessPartner.IsOpen = false;

            txtBusinessPartner.Focus();
        }

        private void btnCloseBusinessPartnerPopup_Click(object sender, RoutedEventArgs e)
        {
            popBusinessPartner.IsOpen = false;

            txtBusinessPartner.Focus();
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
