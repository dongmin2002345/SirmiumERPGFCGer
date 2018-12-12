using Ninject;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Professions;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Professions;
using System;
using System.Collections.Generic;
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

namespace SirmiumERPGFC.Views.Profession
{
    public partial class ProfessionAddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IProfessionService professionService;
        #endregion


        #region Event
        public event ProfessionHandler ProfessionCreatedUpdated;
        #endregion


        #region CurrentProfession
        private ProfessionViewModel _CurrentProfession;

        public ProfessionViewModel CurrentProfession
        {
            get { return _CurrentProfession; }
            set
            {
                if (_CurrentProfession != value)
                {
                    _CurrentProfession = value;
                    NotifyPropertyChanged("CurrentProfession");
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
        private string _SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));

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

        public ProfessionAddEdit(ProfessionViewModel professionViewModel, bool isCreateProcess, bool isPopup = false)
        {
            professionService = DependencyResolver.Kernel.Get<IProfessionService>();

            // Initialize form components
            InitializeComponent();

            this.DataContext = this;

            CurrentProfession = professionViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Cancel and Save buttons

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentProfession.SecondCode == null)
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Šifra"));
                return;
            }

            if (String.IsNullOrEmpty(CurrentProfession.Name))
            {
                MainWindow.WarningMessage = ((string)Application.Current.FindResource("Obavezno_poljeDvotačka_Naziv_zanimanja"));
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = ((string)Application.Current.FindResource("Čuvanje_u_tokuTriTacke"));
                SaveButtonEnabled = false;

                CurrentProfession.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentProfession.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentProfession.IsSynced = false;
                CurrentProfession.UpdatedAt = DateTime.Now;

                ProfessionResponse response = new ProfessionSQLiteRepository().Delete(CurrentProfession.Identifier);
                response = new ProfessionSQLiteRepository().Create(CurrentProfession);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Greška_kod_lokalnog_čuvanjaUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;
                    return;
                }

                response = professionService.Create(CurrentProfession);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = ((string)Application.Current.FindResource("Podaci_su_sačuvani_u_lokaluUzvičnikTačka_Greška_kod_čuvanja_na_serveruUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new ProfessionSQLiteRepository().UpdateSyncStatus(response.Profession.Identifier, response.Profession.Code, response.Profession.UpdatedAt, response.Profession.Id, true);
                    MainWindow.SuccessMessage = ((string)Application.Current.FindResource("Podaci_su_uspešno_sačuvaniUzvičnik"));
                    SaveButtonContent = ((string)Application.Current.FindResource("Sačuvaj"));
                    SaveButtonEnabled = true;

                    ProfessionCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentProfession = new ProfessionViewModel();
                        CurrentProfession.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                txtSecondCode.Focus();
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
                                    FlyoutHelper.CloseFlyout(this);
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
                FlyoutHelper.CloseFlyout(this);
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
