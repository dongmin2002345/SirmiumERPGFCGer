using Ninject;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Messages.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceWebApi.Implementations.Common.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Users;
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
    /// Interaction logic for CallCentarPopup.xaml
    /// </summary>
    public partial class UserPopup : UserControl, INotifyPropertyChanged
    {
        IUserService UserService;

        #region CurrentUser
        public UserViewModel CurrentUser
        {
            get { return (UserViewModel)GetValue(CurrentUserProperty); }
            set { SetValueDp(CurrentUserProperty, value); }
        }

        public static readonly DependencyProperty CurrentUserProperty = DependencyProperty.Register(
            "CurrentUser",
            typeof(UserViewModel),
            typeof(UserPopup),
            new PropertyMetadata(OnCurrentUserPropertyChanged));

        void SetValueDp(DependencyProperty property, object value, String propName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private static void OnCurrentUserPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            UserPopup popup = source as UserPopup;
            UserViewModel User = (UserViewModel)e.NewValue;
            popup.txtUser.Text = User != null ? User.FirstName.ToString() : "";
            popup.txtUser.Text = User != null ? User.LastName.ToString() : "";
        }
        #endregion

        #region UsersFromDB
        private ObservableCollection<UserViewModel> _UsersFromDB;

        public ObservableCollection<UserViewModel> UsersFromDB
        {
            get { return _UsersFromDB; }
            set
            {
                if (_UsersFromDB != value)
                {
                    _UsersFromDB = value;
                    NotifyPropertyChanged("UsersFromDB");
                }
            }
        }
        #endregion

        bool textFieldHasFocus = false;

        public UserPopup()
        {
            UserService = DependencyResolver.Kernel.Get<UserService>();

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
                    new UserSQLiteRepository().Sync(UserService);

                    UserListResponse regionResp = new UserSQLiteRepository().GetUsersForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (regionResp.Success)
                        UsersFromDB = new ObservableCollection<UserViewModel>(regionResp.Users ?? new List<UserViewModel>());
                    else
                        UsersFromDB = new ObservableCollection<UserViewModel>();
                })
            );
        }

        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();

                popUser.IsOpen = true;

                txtUserFilter.Focus();
            }

            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            popUser.IsOpen = true;

            txtUserFilter.Focus();
        }

        private void txtFilterUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserFilter.Text))
                PopulateFromDb(txtUserFilter.Text);
            else
                PopulateFromDb();
        }

        private void btnChooseUser_Click(object sender, RoutedEventArgs e)
        {
            popUser.IsOpen = false;

            txtUser.Focus();
        }

        private void btnCancleUser_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = null;

            popUser.IsOpen = false;

            txtUser.Focus();

        }

        private void dgUserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            popUser.IsOpen = false;

            e.Handled = true;

            txtUser.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgUserList.SelectedIndex > 0)
                    dgUserList.SelectedIndex = dgUserList.SelectedIndex - 1;
                if (dgUserList.SelectedIndex >= 0)
                    dgUserList.ScrollIntoView(dgUserList.Items[dgUserList.SelectedIndex]);
            }

            if (e.Key == Key.Down)
            {
                if (dgUserList.SelectedIndex < dgUserList.Items.Count)
                    dgUserList.SelectedIndex = dgUserList.SelectedIndex + 1;
                if (dgUserList.SelectedIndex >= 0)
                    dgUserList.ScrollIntoView(dgUserList.Items[dgUserList.SelectedIndex]);
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleUser.IsFocused && !btnChooseUserCode.IsFocused)
                {
                    if (popUser.IsOpen)
                    {
                        // Close popup
                        popUser.IsOpen = false;

                        // Move focus to next element
                        txtUser.Focus();

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

        private void btnCloseUserPopup_Click(object sender, RoutedEventArgs e)
        {
            popUser.IsOpen = false;

            txtUser.Focus();
        }

        private void DgUserList_LoadingRow(object sender, DataGridRowEventArgs e)
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
