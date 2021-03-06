﻿using Ninject;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Messages.Common.TaxAdministrations;
using ServiceInterfaces.ViewModels.Common.TaxAdministrations;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.TaxAdministrations;
using SirmiumERPGFC.Views.TaxAdministrations;
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
    /// Interaction logic for TaxAdministrationPopup.xaml
    /// </summary>
    public partial class TaxAdministrationPopup : UserControl, INotifyPropertyChanged
    {
        ITaxAdministrationService taxAdministrationService;

        #region CurrentTaxAdministration
        public TaxAdministrationViewModel CurrentTaxAdministration
        {
            get { return (TaxAdministrationViewModel)GetValue(CurrentTaxAdministrationProperty); }
            set { SetValueDp(CurrentTaxAdministrationProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaxAdministrationProperty = DependencyProperty.Register(
            "CurrentTaxAdministration",
            typeof(TaxAdministrationViewModel),
            typeof(TaxAdministrationPopup),
            new PropertyMetadata(OnCurrentTaxAdministrationPropertyChanged));

        private static void OnCurrentTaxAdministrationPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            TaxAdministrationPopup popup = source as TaxAdministrationPopup;
            TaxAdministrationViewModel taxAdministration = (TaxAdministrationViewModel)e.NewValue;
            popup.txtTaxAdministration.Text = taxAdministration != null ? taxAdministration.Code + " (" + taxAdministration.Name + ")" : "";
        }
        #endregion

        #region TaxAdministrationsFromDB
        private ObservableCollection<TaxAdministrationViewModel> _TaxAdministrationsFromDB;

        public ObservableCollection<TaxAdministrationViewModel> TaxAdministrationsFromDB
        {
            get { return _TaxAdministrationsFromDB; }
            set
            {
                if (_TaxAdministrationsFromDB != value)
                {
                    _TaxAdministrationsFromDB = value;
                    NotifyPropertyChanged("TaxAdministrationsFromDB");
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

        public TaxAdministrationPopup()
        {
            taxAdministrationService = DependencyResolver.Kernel.Get<ITaxAdministrationService>();

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
                    TaxAdministrationListResponse TaxAdministrationResponse = new TaxAdministrationSQLiteRepository().TaxAdministrationsForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (TaxAdministrationResponse.Success)
                    {
                        if (TaxAdministrationResponse.TaxAdministrations != null && TaxAdministrationResponse.TaxAdministrations.Count > 0)
                        {
                            TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>(
                                TaxAdministrationResponse.TaxAdministrations.ToList() ?? new List<TaxAdministrationViewModel>());

                            if (TaxAdministrationsFromDB.Count == 1)
                                CurrentTaxAdministration = TaxAdministrationsFromDB.FirstOrDefault();
                        }
                        else
                        {
                            TaxAdministrationsFromDB = new ObservableCollection<TaxAdministrationViewModel>();

                            CurrentTaxAdministration = null;
                        }
                    }
                })
            );
        }

        private void txtTaxAdministration_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popTaxAdministration.IsOpen = true;

                txtFilterTaxAdministration.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtTaxAdministration_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popTaxAdministration.IsOpen = true;

            txtFilterTaxAdministration.Focus();
        }

        private void txtFilterTaxAdministration_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterTaxAdministration.Text))
                PopulateFromDb(txtFilterTaxAdministration.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseTaxAdministration_Click(object sender, RoutedEventArgs e)
        {
            popTaxAdministration.IsOpen = false;

            txtTaxAdministration.Focus();
        }

        private void btnCancleTaxAdministration_Click(object sender, RoutedEventArgs e)
        {
            CurrentTaxAdministration = null;
            popTaxAdministration.IsOpen = false;

            txtTaxAdministration.Focus();
        }

        private void dgTaxAdministrationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popTaxAdministration.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtTaxAdministration.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgTaxAdministrationList.Items != null && dgTaxAdministrationList.Items.Count > 0)
                {
                    if (dgTaxAdministrationList.SelectedIndex > 0)
                        dgTaxAdministrationList.SelectedIndex = dgTaxAdministrationList.SelectedIndex - 1;
                    if (dgTaxAdministrationList.SelectedIndex >= 0)
                    dgTaxAdministrationList.ScrollIntoView(dgTaxAdministrationList.Items[dgTaxAdministrationList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgTaxAdministrationList.Items != null && dgTaxAdministrationList.Items.Count > 0)
                {
                    if (dgTaxAdministrationList.SelectedIndex < dgTaxAdministrationList.Items.Count)
                        dgTaxAdministrationList.SelectedIndex = dgTaxAdministrationList.SelectedIndex + 1;
                    if (dgTaxAdministrationList.SelectedIndex >= 0)

                        dgTaxAdministrationList.ScrollIntoView(dgTaxAdministrationList.Items[dgTaxAdministrationList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleTaxAdministration.IsFocused && !btnChooseTaxAdministration.IsFocused)
                {
                    if (popTaxAdministration.IsOpen)
                    {
                        popTaxAdministration.IsOpen = false;
                        txtTaxAdministration.Focus();

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

        private void DgTaxAdministrationList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnCloseTaxAdministration_Click(object sender, RoutedEventArgs e)
        {
            popTaxAdministration.IsOpen = false;

            txtTaxAdministration.Focus();
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
