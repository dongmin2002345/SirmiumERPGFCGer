﻿using Ninject;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
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
using ServiceInterfaces.ViewModels.Common.Sectors;
using SirmiumERPGFC.Views.Sectors;
using ServiceInterfaces.Messages.Common.Sectors;
using SirmiumERPGFC.Repository.Sectors;
using ServiceInterfaces.Abstractions.Common.Sectors;

namespace SirmiumERPGFC.ViewComponents.Popups
{
    /// <summary>
    /// Interaction logic for AgencyPopup.xaml
    /// </summary>
    public partial class AgencyPopup : UserControl, INotifyPropertyChanged
    {
        IAgencyService agencyService;

        #region CurrentAgency
        public AgencyViewModel CurrentAgency
        {
            get { return (AgencyViewModel)GetValue(CurrentAgencyProperty); }
            set { SetValueDp(CurrentAgencyProperty, value); }
        }

        public static readonly DependencyProperty CurrentAgencyProperty = DependencyProperty.Register(
            "CurrentAgency",
            typeof(AgencyViewModel),
            typeof(AgencyPopup),
            new PropertyMetadata(OnCurrentAgencyPropertyChanged));

        private static void OnCurrentAgencyPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            AgencyPopup popup = source as AgencyPopup;
            AgencyViewModel agency = (AgencyViewModel)e.NewValue;
            popup.txtAgency.Text = agency != null ? agency.Code + " (" + agency.Name + ")" : "";
        }
        #endregion

        #region CurrentSector
        public SectorViewModel CurrentSector
        {
            get { return (SectorViewModel)GetValue(CurrentSectorProperty); }
            set { SetValueDp(CurrentSectorProperty, value); }
        }

        public static readonly DependencyProperty CurrentSectorProperty = DependencyProperty.Register(
            "CurrentSector",
            typeof(SectorViewModel),
            typeof(AgencyPopup),
            new PropertyMetadata(OnCurrentSectorPropertyChanged));

        private static void OnCurrentSectorPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            AgencyPopup popup = source as AgencyPopup;
            SectorViewModel sector = (SectorViewModel)e.NewValue;

            popup.PopulateFromDb("");

        }
        #endregion

        #region AgenciesFromDB
        private ObservableCollection<AgencyViewModel> _AgenciesFromDB;

        public ObservableCollection<AgencyViewModel> AgenciesFromDB
        {
            get { return _AgenciesFromDB; }
            set
            {
                if (_AgenciesFromDB != value)
                {
                    _AgenciesFromDB = value;
                    NotifyPropertyChanged("AgenciesFromDB");
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

        public AgencyPopup()
        {
            agencyService = DependencyResolver.Kernel.Get<IAgencyService>();

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
                    if (CurrentSector != null)
                    {
                        AgencyListResponse response = new AgencySQLiteRepository().GetAgenciesForPopup(MainWindow.CurrentCompanyId, CurrentSector.Identifier, filterString);

                        if (response.Success)
                        {

                            AgenciesFromDB = new ObservableCollection<AgencyViewModel>(response.Agencies ?? new List<AgencyViewModel>());
                            if (AgenciesFromDB.Count == 1)
                                CurrentAgency = AgenciesFromDB.FirstOrDefault();
                        }

                        else
                        
                            AgenciesFromDB = new ObservableCollection<AgencyViewModel>();
                    }
                    else
                        AgenciesFromDB = new ObservableCollection<AgencyViewModel>();
                
                })
            );
        }

        private void txtAgency_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popAgency.IsOpen = true;

                txtFilterAgency.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtAgency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;
            popAgency.IsOpen = true;

            txtFilterAgency.Focus();
        }

        private void txtFilterAgency_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterAgency.Text))
                PopulateFromDb(txtFilterAgency.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseAgency_Click(object sender, RoutedEventArgs e)
        {
            popAgency.IsOpen = false;

            txtAgency.Focus();
        }

        private void btnCancleAgency_Click(object sender, RoutedEventArgs e)
        {
            CurrentAgency = null;
            popAgency.IsOpen = false;

            txtAgency.Focus();
        }

        private void dgAgencyList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popAgency.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtAgency.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                    if (dgAgencyList.SelectedIndex > 0)
                    dgAgencyList.SelectedIndex = dgAgencyList.SelectedIndex - 1;
                    if (dgAgencyList.SelectedIndex >= 0)
                    dgAgencyList.ScrollIntoView(dgAgencyList.Items[dgAgencyList.SelectedIndex]);
                
            }

            if (e.Key == Key.Down)
            {
                    if (dgAgencyList.SelectedIndex < dgAgencyList.Items.Count)
                        dgAgencyList.SelectedIndex = dgAgencyList.SelectedIndex + 1;
                    if (dgAgencyList.SelectedIndex >= 0)
                    dgAgencyList.ScrollIntoView(dgAgencyList.Items[dgAgencyList.SelectedIndex]);
                
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleAgency.IsFocused && !btnChooseAgency.IsFocused)
                {
                    if (popAgency.IsOpen)
                    {
                        popAgency.IsOpen = false;
                        txtAgency.Focus();

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

        private void btnCloseAgency_Click(object sender, RoutedEventArgs e)
        {
            popAgency.IsOpen = false;

            txtAgency.Focus();
        }

        private void DgAgencyList_LoadingRow(object sender, DataGridRowEventArgs e)
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
