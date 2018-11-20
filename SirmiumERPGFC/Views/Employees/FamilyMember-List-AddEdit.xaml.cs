using Ninject;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Messages.Employees;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Employees;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Employees;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SirmiumERPGFC.Views.Employees
{
    public partial class FamilyMember_List_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IFamilyMemberService familyMemberService;
        #endregion

        #region Event
        public event FamilyMemberHandler FamilyMemberCreatedUpdated;
        #endregion

        #region CurrentFamilyMember
        private FamilyMemberViewModel _CurrentFamilyMember;

        public FamilyMemberViewModel CurrentFamilyMember
        {
            get { return _CurrentFamilyMember; }
            set
            {
                if (_CurrentFamilyMember != value)
                {
                    _CurrentFamilyMember = value;
                    NotifyPropertyChanged("CurrentFamilyMember");
                }
            }
        }
        #endregion


        #region IsCreateProcess
        private bool _IsCreateProcess;

        public bool IsCreateProcess
        {
            get { return _IsCreateProcess; }
            set
            {
                if (_IsCreateProcess != value)
                {
                    _IsCreateProcess = value;
                    NotifyPropertyChanged("IsCreateProcess");
                }
            }
        }
        #endregion

        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
                }
            }
        }
        #endregion


        #region SaveButtonContent
        private string _SaveButtonContent = " SAČUVAJ ";

        public string SaveButtonContent
        {
            get { return _SaveButtonContent; }
            set
            {
                if (_SaveButtonContent != value)
                {
                    _SaveButtonContent = value;
                    NotifyPropertyChanged("SaveButtonContent");
                }
            }
        }
        #endregion

        #region SaveButtonEnabled
        private bool _SaveButtonEnabled = true;

        public bool SaveButtonEnabled
        {
            get { return _SaveButtonEnabled; }
            set
            {
                if (_SaveButtonEnabled != value)
                {
                    _SaveButtonEnabled = value;
                    NotifyPropertyChanged("SaveButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public FamilyMember_List_AddEdit(FamilyMemberViewModel familyMemberViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Get required services
            familyMemberService = DependencyResolver.Kernel.Get<IFamilyMemberService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentFamilyMember = familyMemberViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Cancel and Save buttons

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentFamilyMember.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv ";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentFamilyMember.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentFamilyMember.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentFamilyMember.IsSynced = false;
                CurrentFamilyMember.UpdatedAt = DateTime.Now;

                FamilyMemberResponse response = new FamilyMemberSQLiteRepository().Delete(CurrentFamilyMember.Identifier);
                response = new FamilyMemberSQLiteRepository().Create(CurrentFamilyMember);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = familyMemberService.Create(CurrentFamilyMember);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new FamilyMemberSQLiteRepository().UpdateSyncStatus(response.FamilyMember.Identifier, response.FamilyMember.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;

                    FamilyMemberCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentFamilyMember = new FamilyMemberViewModel();
                        CurrentFamilyMember.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtName.Focus();
                            })
                        );
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                if (IsPopup)
                                    FlyoutHelper.CloseFlyoutPopup(this);
                                else
                                    FlyoutHelper.CloseFlyout(this);
                            })
                        );
                    }
                }

            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsPopup)
                FlyoutHelper.CloseFlyoutPopup(this);
            else
                FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
