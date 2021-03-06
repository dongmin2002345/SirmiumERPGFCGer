﻿using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Views.Employees;
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
    public partial class LicenceTypePopup : UserControl, INotifyPropertyChanged
    {
        ILicenceTypeService LicenceTypeService;

        #region CurrentLicenceType
        public LicenceTypeViewModel CurrentLicenceType
        {
            get { return (LicenceTypeViewModel)GetValue(CurrentLicenceTypeProperty); }
            set { SetValueDp(CurrentLicenceTypeProperty, value); }
        }

        public static readonly DependencyProperty CurrentLicenceTypeProperty = DependencyProperty.Register(
            "CurrentLicenceType",
            typeof(LicenceTypeViewModel),
            typeof(LicenceTypePopup),
            new PropertyMetadata(OnCurrentLicenceTypePropertyChanged));

        private static void OnCurrentLicenceTypePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            LicenceTypePopup popup = source as LicenceTypePopup;
            LicenceTypeViewModel LicenceType = (LicenceTypeViewModel)e.NewValue;
            popup.txtLicenceType.Text = LicenceType != null ? LicenceType.Category + " (" + LicenceType.Description + ")" : "";
        }
        #endregion

        #region LicenceTypesFromDB
        private ObservableCollection<LicenceTypeViewModel> _LicenceTypesFromDB;

        public ObservableCollection<LicenceTypeViewModel> LicenceTypesFromDB
        {
            get { return _LicenceTypesFromDB; }
            set
            {
                if (_LicenceTypesFromDB != value)
                {
                    _LicenceTypesFromDB = value;
                    NotifyPropertyChanged("LicenceTypesFromDB");
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

        public LicenceTypePopup()
        {
            LicenceTypeService = DependencyResolver.Kernel.Get<ILicenceTypeService>();

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
                    LicenceTypeListResponse LicenceTypeResponse = new LicenceTypeSQLiteRepository().GetLicenceTypesForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (LicenceTypeResponse.Success)
                    {
                        if (LicenceTypeResponse.LicenceTypes != null && LicenceTypeResponse.LicenceTypes.Count > 0)
                        {
                            LicenceTypesFromDB = new ObservableCollection<LicenceTypeViewModel>(
                                LicenceTypeResponse.LicenceTypes.ToList() ?? new List<LicenceTypeViewModel>());

                            if (LicenceTypesFromDB.Count == 1)
                                CurrentLicenceType = LicenceTypesFromDB.FirstOrDefault();
                        }
                        else
                        {
                            LicenceTypesFromDB = new ObservableCollection<LicenceTypeViewModel>();

                            CurrentLicenceType = null;
                        }
                    }
                })
            );
        }

        private void txtLicenceType_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popLicenceType.IsOpen = true;

                txtFilterLicenceType.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtLicenceType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popLicenceType.IsOpen = true;

            txtFilterLicenceType.Focus();
        }

        private void txtFilterLicenceType_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterLicenceType.Text))
                PopulateFromDb(txtFilterLicenceType.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseLicenceType_Click(object sender, RoutedEventArgs e)
        {
            popLicenceType.IsOpen = false;

            txtLicenceType.Focus();
        }

        private void btnCancleLicenceType_Click(object sender, RoutedEventArgs e)
        {
            CurrentLicenceType = null;
            popLicenceType.IsOpen = false;

            txtLicenceType.Focus();
        }

        private void dgLicenceTypeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popLicenceType.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtLicenceType.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgLicenceTypeList.Items != null && dgLicenceTypeList.Items.Count > 0)
                {
                    if (dgLicenceTypeList.SelectedIndex > 0)
                        dgLicenceTypeList.SelectedIndex = dgLicenceTypeList.SelectedIndex - 1;
                    if (dgLicenceTypeList.SelectedIndex >= 0)
                    dgLicenceTypeList.ScrollIntoView(dgLicenceTypeList.Items[dgLicenceTypeList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgLicenceTypeList.Items != null && dgLicenceTypeList.Items.Count > 0)
                {
                    if (dgLicenceTypeList.SelectedIndex < dgLicenceTypeList.Items.Count)
                        dgLicenceTypeList.SelectedIndex = dgLicenceTypeList.SelectedIndex + 1;
                    if (dgLicenceTypeList.SelectedIndex >= 0)
                        dgLicenceTypeList.ScrollIntoView(dgLicenceTypeList.Items[dgLicenceTypeList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleLicenceType.IsFocused && !btnChooseLicenceType.IsFocused)
                {
                    if (popLicenceType.IsOpen)
                    {
                        popLicenceType.IsOpen = false;
                        txtLicenceType.Focus();

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

        private void DgLicenceTypeList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnCloseLicenceType_Click(object sender, RoutedEventArgs e)
        {
            popLicenceType.IsOpen = false;

            txtLicenceType.Focus();
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
