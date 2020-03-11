using Ninject;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Messages.Common.ToDos;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ToDos;
using SirmiumERPGFC.Views.Home;
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
    /// Interaction logic for ToDoPopup.xaml
    /// </summary>
    public partial class ToDoStatusPopup : UserControl, INotifyPropertyChanged
    {
        IToDoStatusService toDoStatusService;

        #region CurrentToDoStatus
        public ToDoStatusViewModel CurrentToDoStatus
        {
            get { return (ToDoStatusViewModel)GetValue(CurrentToDoStatusProperty); }
            set { SetValueDp(CurrentToDoStatusProperty, value); }
        }

        public static readonly DependencyProperty CurrentToDoStatusProperty = DependencyProperty.Register(
            "CurrentToDoStatus",
            typeof(ToDoStatusViewModel),
            typeof(ToDoStatusPopup),
            new PropertyMetadata(OnCurrentToDoStatusPropertyChanged));

        private static void OnCurrentToDoStatusPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ToDoStatusPopup popup = source as ToDoStatusPopup;
            ToDoStatusViewModel ToDoStatus = (ToDoStatusViewModel)e.NewValue;
            popup.txtToDoStatus.Text = ToDoStatus != null ? ToDoStatus.Name + ")" : "";
        }
        #endregion

        #region ToDoStatusesFromDB
        private ObservableCollection<ToDoStatusViewModel> _ToDoStatusesFromDB;

        public ObservableCollection<ToDoStatusViewModel> ToDoStatusesFromDB
        {
            get { return _ToDoStatusesFromDB; }
            set
            {
                if (_ToDoStatusesFromDB != value)
                {
                    _ToDoStatusesFromDB = value;
                    NotifyPropertyChanged("ToDoStatusesFromDB");
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

        public ToDoStatusPopup()
        {
            toDoStatusService = DependencyResolver.Kernel.Get<IToDoStatusService>();

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
                    new ToDoStatusSQLiteRepository().Sync(toDoStatusService);

                    ToDoStatusListResponse ToDoStatusResp = new ToDoStatusSQLiteRepository().GetToDoStatusesForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (ToDoStatusResp.Success)
                        ToDoStatusesFromDB = new ObservableCollection<ToDoStatusViewModel>(ToDoStatusResp.ToDoStatuses ?? new List<ToDoStatusViewModel>());
                    else
                        ToDoStatusesFromDB = new ObservableCollection<ToDoStatusViewModel>();
                })
            );
        }

        private void txtToDoStatus_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popToDoStatus.IsOpen = true;

                txtFilterToDoStatus.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtToDoStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popToDoStatus.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterToDoStatus.Focus();
        }

        private void txtFilterToDoStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterToDoStatus.Text))
                PopulateFromDb(txtFilterToDoStatus.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseToDoStatus_Click(object sender, RoutedEventArgs e)
        {
            popToDoStatus.IsOpen = false;

            txtToDoStatus.Focus();
        }

        private void btnCancleToDoStatus_Click(object sender, RoutedEventArgs e)
        {
            CurrentToDoStatus = null;
            popToDoStatus.IsOpen = false;

            txtToDoStatus.Focus();
        }

        private void dgToDoStatusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popToDoStatus.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtToDoStatus.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgToDoStatusList.Items != null && dgToDoStatusList.Items.Count > 0)
                {
                    if (dgToDoStatusList.SelectedIndex == -1)
                        dgToDoStatusList.SelectedIndex = 0;
                    if (dgToDoStatusList.SelectedIndex > 0)
                        dgToDoStatusList.SelectedIndex = dgToDoStatusList.SelectedIndex - 1;
                    dgToDoStatusList.ScrollIntoView(dgToDoStatusList.Items[dgToDoStatusList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgToDoStatusList.Items != null && dgToDoStatusList.Items.Count > 0)
                {
                    if (dgToDoStatusList.SelectedIndex < dgToDoStatusList.Items.Count)
                        dgToDoStatusList.SelectedIndex = dgToDoStatusList.SelectedIndex + 1;
                    dgToDoStatusList.ScrollIntoView(dgToDoStatusList.Items[dgToDoStatusList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleToDoStatus.IsFocused && !btnChooseToDoStatus.IsFocused)
                {
                    if (popToDoStatus.IsOpen)
                    {
                        popToDoStatus.IsOpen = false;
                        txtToDoStatus.Focus();

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

        private void BtnCloseToDoStatus_Click(object sender, RoutedEventArgs e)
        {
            popToDoStatus.IsOpen = false;

            txtToDoStatus.Focus();
        }

        private void DgToDoStatusList_LoadingRow(object sender, DataGridRowEventArgs e)
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