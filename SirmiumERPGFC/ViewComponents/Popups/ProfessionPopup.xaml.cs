using Ninject;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Professions;
using SirmiumERPGFC.Views.Profession;
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
    public partial class ProfessionPopup : UserControl, INotifyPropertyChanged
    {
        IProfessionService ProfessionService;

        #region CurrentProfession
        public ProfessionViewModel CurrentProfession
        {
            get { return (ProfessionViewModel)GetValue(CurrentProfessionProperty); }
            set { SetValueDp(CurrentProfessionProperty, value); }
        }

        public static readonly DependencyProperty CurrentProfessionProperty = DependencyProperty.Register(
            "CurrentProfession",
            typeof(ProfessionViewModel),
            typeof(ProfessionPopup),
            new PropertyMetadata(OnCurrentProfessionPropertyChanged));

        private static void OnCurrentProfessionPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ProfessionPopup popup = source as ProfessionPopup;
            ProfessionViewModel Profession = (ProfessionViewModel)e.NewValue;
            popup.txtProfession.Text = Profession != null ? Profession.SecondCode + " (" + Profession.Name + ")" : "";
        }
        #endregion

        #region ProfessionsFromDB
        private ObservableCollection<ProfessionViewModel> _ProfessionsFromDB;

        public ObservableCollection<ProfessionViewModel> ProfessionsFromDB
        {
            get { return _ProfessionsFromDB; }
            set
            {
                if (_ProfessionsFromDB != value)
                {
                    _ProfessionsFromDB = value;
                    NotifyPropertyChanged("ProfessionsFromDB");
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

        public ProfessionPopup()
        {
            ProfessionService = DependencyResolver.Kernel.Get<IProfessionService>();

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
                    ProfessionListResponse ProfessionResp = new ProfessionSQLiteRepository().GetProfessionsForPopup(MainWindow.CurrentCompanyId, filterString);
                    if (ProfessionResp.Success)
                        ProfessionsFromDB = new ObservableCollection<ProfessionViewModel>(ProfessionResp.Professions ?? new List<ProfessionViewModel>());
                    else
                        ProfessionsFromDB = new ObservableCollection<ProfessionViewModel>();
                })
            );
        }

        private void txtProfession_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!textFieldHasFocus)
            {
                PopulateFromDb();
                popProfession.IsOpen = true;

                txtFilterProfession.Focus();
            }
            textFieldHasFocus = !textFieldHasFocus;
        }

        private void txtProfession_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textFieldHasFocus = true;

            PopulateFromDb();
            popProfession.IsOpen = true;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtFilterProfession.Focus();
        }

        private void txtFilterProfession_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFilterProfession.Text))
                PopulateFromDb(txtFilterProfession.Text.ToLower());
            else
                PopulateFromDb();
        }

        private void btnChooseProfession_Click(object sender, RoutedEventArgs e)
        {
            popProfession.IsOpen = false;

            txtProfession.Focus();
        }

        private void btnCancleProfession_Click(object sender, RoutedEventArgs e)
        {
            CurrentProfession = null;
            popProfession.IsOpen = false;

            txtProfession.Focus();
        }

        private void btnAddProfession_Click(object sender, RoutedEventArgs e)
        {
            popProfession.IsOpen = false;

            ProfessionViewModel Profession = new ProfessionViewModel();
            Profession.Identifier = Guid.NewGuid();

            ProfessionAddEdit ProfessionAddEditForm = new ProfessionAddEdit(Profession, true, true);
            ProfessionAddEditForm.ProfessionCreatedUpdated += new ProfessionHandler(ProfessionAdded);
            FlyoutHelper.OpenFlyoutPopup(this, "Podaci o drzavama", 95, ProfessionAddEditForm);

            txtProfession.Focus();
        }

        void ProfessionAdded() { }

        private void dgProfessionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            popProfession.IsOpen = false;

            // Hendled is set to true, in order to stop on mouse up event and to set focus 
            e.Handled = true;

            txtProfession.Focus();
        }

        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (dgProfessionList.Items != null && dgProfessionList.Items.Count > 0)
                {
                    if (dgProfessionList.SelectedIndex == -1)
                        dgProfessionList.SelectedIndex = 0;
                    if (dgProfessionList.SelectedIndex > 0)
                        dgProfessionList.SelectedIndex = dgProfessionList.SelectedIndex - 1;
                    dgProfessionList.ScrollIntoView(dgProfessionList.Items[dgProfessionList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Down)
            {
                if (dgProfessionList.Items != null && dgProfessionList.Items.Count > 0)
                {
                    if (dgProfessionList.SelectedIndex < dgProfessionList.Items.Count)
                        dgProfessionList.SelectedIndex = dgProfessionList.SelectedIndex + 1;
                    dgProfessionList.ScrollIntoView(dgProfessionList.Items[dgProfessionList.SelectedIndex]);
                }
            }

            if (e.Key == Key.Enter)
            {
                if (!btnCancleProfession.IsFocused && !btnChooseProfession.IsFocused)
                {
                    if (popProfession.IsOpen)
                    {
                        popProfession.IsOpen = false;
                        txtProfession.Focus();

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

        private void btnAddProfession_LostFocus(object sender, RoutedEventArgs e)
        {
            popProfession.IsOpen = false;

            txtProfession.Focus();
        }

        private void btnCloseProfessionPopup_Click(object sender, RoutedEventArgs e)
        {
            popProfession.IsOpen = false;

            txtProfession.Focus();
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
