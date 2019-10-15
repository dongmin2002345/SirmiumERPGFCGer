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
    /// Interaction logic for DiscountPopup.xaml
    /// </summary>
    public partial class DiscountPopup : UserControl, INotifyPropertyChanged
    {
        IDiscountService discountService;

        #region CurrentDiscount
        public DiscountViewModel CurrentDiscount
        {
            get { return (DiscountViewModel)GetValue(CurrentDiscountProperty); }
            set { SetValueDp(CurrentDiscountProperty, value); }
        }

        public static readonly DependencyProperty CurrentDiscountProperty = DependencyProperty.Register(
            "CurrentDiscount",
            typeof(DiscountViewModel),
            typeof(DiscountPopup),
            new PropertyMetadata(OnCurrentDiscountPropertyChanged));

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static void OnCurrentDiscountPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            DiscountPopup popup = source as DiscountPopup;
            DiscountViewModel discount = (DiscountViewModel)e.NewValue;
            popup.txtDiscount.Text = discount != null ? discount.Amount.ToString() : "";
        }
        #endregion

        #region DiscountsFromDB
        private ObservableCollection<DiscountViewModel> _DiscountsFromDB;

        public ObservableCollection<DiscountViewModel> DiscountsFromDB
        {
            get { return _DiscountsFromDB; }
            set
            {
                if (_DiscountsFromDB != value)
                {
                    _DiscountsFromDB = value;
                    NotifyPropertyChanged("DiscountsFromDB");
                }
            }
        }
        #endregion

        bool textFieldHasFocus = false;

        public DiscountPopup()
        {
            discountService = DependencyResolver.Kernel.Get<IDiscountService>();

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
                    DiscountListResponse discountResponse = new DiscountSQLiteRepository().GetDiscountsForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (discountResponse.Success)
                    {
                        if (discountResponse.Discounts != null && discountResponse.Discounts.Count > 0)
                        {
                            DiscountsFromDB = new ObservableCollection<DiscountViewModel>(
                                discountResponse.Discounts?.OrderBy(x => Int32.Parse(x.Code))?.ToList() ?? new List<DiscountViewModel>());

                            if (DiscountsFromDB.Count == 1)
                                CurrentDiscount = DiscountsFromDB.FirstOrDefault();
                        }
                        else
                        {
                            DiscountsFromDB = new ObservableCollection<DiscountViewModel>();

                            CurrentDiscount = null;
                        }
                    }
                })
            );
        }

        private void txtDiscount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();

                popDiscount.IsOpen = true;

                txtDiscountFilter.Focus();
            }

            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtDiscount_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popDiscount.IsOpen = true;

            txtDiscountFilter.Focus();
        }

        private void txtFilterDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDiscountFilter.Text))
                PopulateFromDb(txtDiscountFilter.Text);
            else
                PopulateFromDb();
        }

        private void btnChooseDiscount_Click(object sender, RoutedEventArgs e)
        {
            popDiscount.IsOpen = false;

            txtDiscount.Focus();
        }

        private void btnCancleDiscount_Click(object sender, RoutedEventArgs e)
        {
            CurrentDiscount = null;

            popDiscount.IsOpen = false;

            txtDiscount.Focus();

        }

        private void dgDiscountList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            popDiscount.IsOpen = false;

            e.Handled = true;

            txtDiscount.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgDiscountList.SelectedIndex > 0)
                    dgDiscountList.SelectedIndex = dgDiscountList.SelectedIndex - 1;
                if (dgDiscountList.SelectedIndex >= 0)
                    dgDiscountList.ScrollIntoView(dgDiscountList.Items[dgDiscountList.SelectedIndex]);
            }

            if (e.Key == Key.Down)
            {
                if (dgDiscountList.SelectedIndex < dgDiscountList.Items.Count)
                    dgDiscountList.SelectedIndex = dgDiscountList.SelectedIndex + 1;
                if (dgDiscountList.SelectedIndex >= 0)
                    dgDiscountList.ScrollIntoView(dgDiscountList.Items[dgDiscountList.SelectedIndex]);
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleDiscount.IsFocused && !btnChooseDiscountCode.IsFocused)
                {
                    if (popDiscount.IsOpen)
                    {
                        // Close popup
                        popDiscount.IsOpen = false;

                        // Move focus to next element
                        txtDiscount.Focus();

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

        private void btnCloseDiscountPopup_Click(object sender, RoutedEventArgs e)
        {
            popDiscount.IsOpen = false;

            txtDiscount.Focus();
        }

        private void DgDiscountList_LoadingRow(object sender, DataGridRowEventArgs e)
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
