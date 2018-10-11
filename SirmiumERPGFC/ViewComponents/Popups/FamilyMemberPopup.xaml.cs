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
    public partial class FamilyMemberPopup : UserControl, INotifyPropertyChanged
    {
        IFamilyMemberService FamilyMemberService;

        #region CurrentFamilyMember
        public FamilyMemberViewModel CurrentFamilyMember
        {
            get { return (FamilyMemberViewModel)GetValue(CurrentFamilyMemberProperty); }
            set { SetValueDp(CurrentFamilyMemberProperty, value); }
        }

        public static readonly DependencyProperty CurrentFamilyMemberProperty = DependencyProperty.Register(
            "CurrentFamilyMember",
            typeof(FamilyMemberViewModel),
            typeof(FamilyMemberPopup),
            new PropertyMetadata(OnCurrentFamilyMemberPropertyChanged));

        private static void OnCurrentFamilyMemberPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            FamilyMemberPopup popup = source as FamilyMemberPopup;
            FamilyMemberViewModel FamilyMember = (FamilyMemberViewModel)e.NewValue;
            popup.txtFamilyMember.Text = FamilyMember != null ? FamilyMember.Code + " (" + FamilyMember.Name + ")" : "";
        }
        #endregion

        #region FamilyMembersFromDB
        private ObservableCollection<FamilyMemberViewModel> _FamilyMembersFromDB;

        public ObservableCollection<FamilyMemberViewModel> FamilyMembersFromDB
        {
            get { return _FamilyMembersFromDB; }
            set
            {
                if (_FamilyMembersFromDB != value)
                {
                    _FamilyMembersFromDB = value;
                    NotifyPropertyChanged("FamilyMembersFromDB");
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

        public FamilyMemberPopup()
        {
            FamilyMemberService = DependencyResolver.Kernel.Get<IFamilyMemberService>();

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
                    FamilyMemberListResponse FamilyMemberResp = new FamilyMemberSQLiteRepository().GetFamilyMembersForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (FamilyMemberResp.Success)
                        FamilyMembersFromDB = new ObservableCollection<FamilyMemberViewModel>(FamilyMemberResp.FamilyMembers ?? new List<FamilyMemberViewModel>());
                    else
                        FamilyMembersFromDB = new ObservableCollection<FamilyMemberViewModel>();
                })
            );
        }

        private void txtFamilyMember_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popFamilyMember.IsOpen = true;

                txtFilterFamilyMember.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtFamilyMember_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popFamilyMember.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterFamilyMember.Focus();
        }

        private void txtFilterFamilyMember_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterFamilyMember.Text))
                PopulateFromDb(txtFilterFamilyMember.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseFamilyMember_Click(object sender, RoutedEventArgs e)
        {
            popFamilyMember.IsOpen = false;

            txtFamilyMember.Focus();
        }

        private void btnCancleFamilyMember_Click(object sender, RoutedEventArgs e)
        {
            CurrentFamilyMember = null;
            popFamilyMember.IsOpen = false;

            txtFamilyMember.Focus();
        }

        private void btnAddFamilyMember_Click(object sender, RoutedEventArgs e)
        {
            popFamilyMember.IsOpen = false;

            FamilyMemberViewModel FamilyMember = new FamilyMemberViewModel();
            FamilyMember.Identifier = Guid.NewGuid();

            FamilyMember_List_AddEdit FamilyMemberAddEditForm = new FamilyMember_List_AddEdit(FamilyMember, true, true);
            FamilyMemberAddEditForm.FamilyMemberCreatedUpdated += new FamilyMemberHandler(FamilyMemberAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o članovima porodice", 95, FamilyMemberAddEditForm);

            txtFamilyMember.Focus();
        }

        void FamilyMemberAdded() { }

        private void dgFamilyMemberList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popFamilyMember.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFamilyMember.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgFamilyMemberList.Items != null && dgFamilyMemberList.Items.Count > 0)
                {
                    if (dgFamilyMemberList.SelectedIndex == -1)
                        dgFamilyMemberList.SelectedIndex = 0;
                    if (dgFamilyMemberList.SelectedIndex > 0)
                        dgFamilyMemberList.SelectedIndex = dgFamilyMemberList.SelectedIndex - 1;
                    dgFamilyMemberList.ScrollIntoView(dgFamilyMemberList.Items[dgFamilyMemberList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgFamilyMemberList.Items != null && dgFamilyMemberList.Items.Count > 0)
                {
                    if (dgFamilyMemberList.SelectedIndex < dgFamilyMemberList.Items.Count)
                        dgFamilyMemberList.SelectedIndex = dgFamilyMemberList.SelectedIndex + 1;
                    dgFamilyMemberList.ScrollIntoView(dgFamilyMemberList.Items[dgFamilyMemberList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleFamilyMember.IsFocused && !btnChooseFamilyMember.IsFocused)
                {
                    if (popFamilyMember.IsOpen)
                    {
                        popFamilyMember.IsOpen = false;
                        txtFamilyMember.Focus();

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

        private void btnAddFamilyMember_LostFocus(object sender, RoutedEventArgs e)
        {
            popFamilyMember.IsOpen = false;

            txtFamilyMember.Focus();
        }

        private void btnCloseFamilyMemberPopup_Click(object sender, RoutedEventArgs e)
        {
            popFamilyMember.IsOpen = false;

            txtFamilyMember.Focus();
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