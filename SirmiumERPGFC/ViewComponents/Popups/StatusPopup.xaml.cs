using Ninject;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Statuses;
using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Statuses;
using SirmiumERPGFC.Views.Statuses;
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
    /// Interaction logic for StatusPopup.xaml
    /// </summary>
    public partial class StatusPopup : UserControl, INotifyPropertyChanged
    {
        IStatusService statusService;

        #region CurrentStatus
        public StatusViewModel CurrentStatus
        {
            get { return (StatusViewModel)GetValue(CurrentStatusProperty); }
            set { SetValueDp(CurrentStatusProperty, value); }
        }

        public static readonly DependencyProperty CurrentStatusProperty = DependencyProperty.Register(
            "CurrentStatus",
            typeof(StatusViewModel),
            typeof(StatusPopup),
            new PropertyMetadata(OnCurrentStatusPropertyChanged));

        private static void OnCurrentStatusPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            StatusPopup popup = source as StatusPopup;
            StatusViewModel status = (StatusViewModel)e.NewValue;
            popup.txtStatus.Text = status != null ? status.Code + " (" + status.Name + ")" : "";
        }
        #endregion

        #region StatusesFromDB
        private ObservableCollection<StatusViewModel> _StatusesFromDB;

        public ObservableCollection<StatusViewModel> StatusesFromDB
        {
            get { return _StatusesFromDB; }
            set
            {
                if (_StatusesFromDB != value)
                {
                    _StatusesFromDB = value;
                    NotifyPropertyChanged("StatusesFromDB");
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

        public StatusPopup()
        {
            statusService = DependencyResolver.Kernel.Get<IStatusService>();

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
                    StatusListResponse StatusResponse = new StatusSQLiteRepository().GetStatusesForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (StatusResponse.Success)
                    {
                        if (StatusResponse.Statuses != null && StatusResponse.Statuses.Count > 0)
                        {
                            StatusesFromDB = new ObservableCollection<StatusViewModel>(
                                StatusResponse.Statuses.ToList() ?? new List<StatusViewModel>());

                            if (StatusesFromDB.Count == 1)
                                CurrentStatus = StatusesFromDB.FirstOrDefault();
                        }
                        else
                        {
                            StatusesFromDB = new ObservableCollection<StatusViewModel>();

                            CurrentStatus = null;
                        }
                    }
                })
            );
        }

        private void txtStatus_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popStatus.IsOpen = true;

                txtFilterStatus.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popStatus.IsOpen = true;

            txtFilterStatus.Focus();
        }

        private void txtFilterStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterStatus.Text))
                PopulateFromDb(txtFilterStatus.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseStatus_Click(object sender, RoutedEventArgs e)
        {
            popStatus.IsOpen = false;

            txtStatus.Focus();
        }

        private void btnCancleStatus_Click(object sender, RoutedEventArgs e)
        {
            CurrentStatus = null;
            popStatus.IsOpen = false;

            txtStatus.Focus();
        }


        private void dgStatusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popStatus.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtStatus.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgStatusList.Items != null && dgStatusList.Items.Count > 0)
                {
                    if (dgStatusList.SelectedIndex > 0)
                        dgStatusList.SelectedIndex = dgStatusList.SelectedIndex - 1;
                    if (dgStatusList.SelectedIndex >= 0)
                    dgStatusList.ScrollIntoView(dgStatusList.Items[dgStatusList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgStatusList.Items != null && dgStatusList.Items.Count > 0)
                {
                    if (dgStatusList.SelectedIndex < dgStatusList.Items.Count)
                        dgStatusList.SelectedIndex = dgStatusList.SelectedIndex + 1;
                    if (dgStatusList.SelectedIndex >= 0)

                        dgStatusList.ScrollIntoView(dgStatusList.Items[dgStatusList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleStatus.IsFocused && !btnChooseStatus.IsFocused)
                {
                    if (popStatus.IsOpen)
                    {
                        popStatus.IsOpen = false;
                        txtStatus.Focus();

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

        private void DgStatusList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void btnCloseStatus_Click(object sender, RoutedEventArgs e)
        {
            popStatus.IsOpen = false;

            txtStatus.Focus();
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
