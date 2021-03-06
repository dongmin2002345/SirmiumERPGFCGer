﻿using Ninject;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using ServiceInterfaces.ViewModels.Common.Locations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Banks;
using SirmiumERPGFC.Views.Banks;
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
    public partial class BankPopup : UserControl, INotifyPropertyChanged
    {
        IBankService bankService;

        #region CurrentBanks
        public BankViewModel CurrentBank
        {
            get { return (BankViewModel)GetValue(CurrentBankProperty); }
            set { SetValueDp(CurrentBankProperty, value); }
        }

        public static readonly DependencyProperty CurrentBankProperty = DependencyProperty.Register(
            "CurrentBank",
            typeof(BankViewModel),
            typeof(BankPopup),
            new PropertyMetadata(OnCurrentBankPropertyChanged));

        private static void OnCurrentBankPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BankPopup popup = source as BankPopup;
            BankViewModel region = (BankViewModel)e.NewValue;
            popup.txtBank.Text = region != null ? region.Swift + " (" + region.Name + ")" : "";
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
            typeof(BankPopup),
            new PropertyMetadata(OnCurrentCountryPropertyChanged));

        private static void OnCurrentCountryPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BankPopup popup = source as BankPopup;
            CountryViewModel animalType = (CountryViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region BanksFromDB
        private ObservableCollection<BankViewModel> _BanksFromDB;

        public ObservableCollection<BankViewModel> BanksFromDB
        {
            get { return _BanksFromDB; }
            set
            {
                if (_BanksFromDB != value)
                {
                    _BanksFromDB = value;
                    NotifyPropertyChanged("BanksFromDB");
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

        public BankPopup()
        {
            bankService = DependencyResolver.Kernel.Get<IBankService>();

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
                    BankListResponse Response = new BankSQLiteRepository().GetBanksForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (Response.Success)
                    {
                        if (Response.Banks != null && Response.Banks.Count > 0)
                        {
                            BanksFromDB = new ObservableCollection<BankViewModel>(
                                Response.Banks?.ToList() ?? new List<BankViewModel>());

                            if (BanksFromDB.Count == 1)
                                CurrentBank = BanksFromDB.FirstOrDefault();
                        }
                        else
                        {
                            BanksFromDB = new ObservableCollection<BankViewModel>();

                            CurrentBank = null;
                        }
                    }
                })
            );
        }

        private void txtBank_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popBank.IsOpen = true;

                txtBankFilter.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtBank_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popBank.IsOpen = true;

            txtBankFilter.Focus();
        }

        private void txtBank_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBankFilter.Text))
                PopulateFromDb(txtBankFilter.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseBank_Click(object sender, RoutedEventArgs e)
        {
            popBank.IsOpen = false;

            txtBank.Focus();
        }

        private void btnCancleBank_Click(object sender, RoutedEventArgs e)
        {
            CurrentBank = null;
            popBank.IsOpen = false;

            txtBank.Focus();
        }


        private void dgBankList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popBank.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtBank.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgBankList.Items != null && dgBankList.Items.Count > 0)
                {
                    if (dgBankList.SelectedIndex > 0)
                        dgBankList.SelectedIndex = dgBankList.SelectedIndex - 1;
                    if (dgBankList.SelectedIndex >= 0)
                       
                    dgBankList.ScrollIntoView(dgBankList.Items[dgBankList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgBankList.Items != null && dgBankList.Items.Count > 0)
                {
                    if (dgBankList.SelectedIndex < dgBankList.Items.Count)
                        dgBankList.SelectedIndex = dgBankList.SelectedIndex + 1;
                    if (dgBankList.SelectedIndex >= 0)
                        dgBankList.ScrollIntoView(dgBankList.Items[dgBankList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleBank.IsFocused && !btnChooseBankCode.IsFocused)
                {
                    if (popBank.IsOpen)
                    {
                        popBank.IsOpen = false;
                        txtBank.Focus();

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

        private void btnCloseBank_Click(object sender, RoutedEventArgs e)
        {
            popBank.IsOpen = false;

            txtBank.Focus();
        }
        private void DgBankList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnCloseBankPopup_Click(object sender, RoutedEventArgs e)
        {
            popBank.IsOpen = false;

            txtBank.Focus();
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
