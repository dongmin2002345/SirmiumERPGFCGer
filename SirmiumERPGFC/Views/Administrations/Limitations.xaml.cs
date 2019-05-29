using Ninject;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Limitations;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Limitations;
using SirmiumERPGFC.Views.Common;
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

namespace SirmiumERPGFC.Views.Administrations
{
    public partial class Limitations : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        ILimitationService limitationService;
        ILimitationEmailService limitationEmailService;
        #endregion

        #region CurrentLimitation
        private LimitationViewModel _CurrentLimitation;

        public LimitationViewModel CurrentLimitation
        {
            get { return _CurrentLimitation; }
            set
            {
                if (_CurrentLimitation != value)
                {
                    _CurrentLimitation = value;
                    NotifyPropertyChanged("CurrentLimitation");
                }
            }
        }
        #endregion

        #region LimitationEmailsFromDB
        private ObservableCollection<LimitationEmailViewModel> _LimitationEmailsFromDB;

        public ObservableCollection<LimitationEmailViewModel> LimitationEmailsFromDB
        {
            get { return _LimitationEmailsFromDB; }
            set
            {
                if (_LimitationEmailsFromDB != value)
                {
                    _LimitationEmailsFromDB = value;
                    NotifyPropertyChanged("LimitationEmailsFromDB");
                }
            }
        }
        #endregion

        #region CurrentLimitationEmail
        private LimitationEmailViewModel _CurrentLimitationEmail = new LimitationEmailViewModel();

        public LimitationEmailViewModel CurrentLimitationEmail
        {
            get { return _CurrentLimitationEmail; }
            set
            {
                if (_CurrentLimitationEmail != value)
                {
                    _CurrentLimitationEmail = value;
                    NotifyPropertyChanged("CurrentLimitationEmail");
                }
            }
        }
        #endregion

        #region CurrentLimitationEmailDG
        private LimitationEmailViewModel _CurrentLimitationEmailDG;

        public LimitationEmailViewModel CurrentLimitationEmailDG
        {
            get { return _CurrentLimitationEmailDG; }
            set
            {
                if (_CurrentLimitationEmailDG != value)
                {
                    _CurrentLimitationEmailDG = value;
                    NotifyPropertyChanged("CurrentLimitationEmailDG");
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

        public Limitations()
        {
            limitationService = DependencyResolver.Kernel.Get<ILimitationService>();
            limitationEmailService = DependencyResolver.Kernel.Get<ILimitationEmailService>();

            InitializeComponent();

            this.DataContext = this;

            Thread th = new Thread(() =>
            {
                Sync();
            });
            th.IsBackground = true;
            th.Start();
        }

        #endregion

        #region Display data

        private void DisplayEmailData()
        {
            var response = new LimitationEmailSQLiteRepository().GetLimitationEmailsByPage(MainWindow.CurrentCompanyId, new LimitationEmailViewModel(), 1, Int32.MaxValue);
            if (response.Success)
            {
                LimitationEmailsFromDB = new ObservableCollection<LimitationEmailViewModel>(
                    response.LimitationEmails ?? new List<LimitationEmailViewModel>());
            }
        }

        private void Sync()
        {
            new LimitationSQLiteRepository().Sync(limitationService);

            new LimitationEmailSQLiteRepository().Sync(limitationEmailService);

            CurrentLimitation = new LimitationSQLiteRepository().GetLimitation(MainWindow.CurrentCompanyId)?.Limitation;

            DisplayEmailData();
        }

        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Wird geschpeichert... ";
                SaveButtonEnabled = false;

                CurrentLimitation.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentLimitation.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentLimitation.IsSynced = false;
                CurrentLimitation.UpdatedAt = DateTime.Now;

                LimitationResponse response = new LimitationSQLiteRepository().Delete(CurrentLimitation.Identifier);
                response = new LimitationSQLiteRepository().Create(CurrentLimitation);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Lokaler Speicherfehler!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = limitationService.Create(CurrentLimitation);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Die Daten wurden im Lokal gespeichert. Server-Speicherfehler!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new LimitationSQLiteRepository().UpdateSyncStatus(response.Limitation.Identifier, response.Limitation.UpdatedAt, response.Limitation.Id, true);
                    MainWindow.SuccessMessage = "Daten wurden erfolgreich gespeichert!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                if (CurrentLimitationEmail.Identifier == Guid.Empty)
                    CurrentLimitationEmail.Identifier = Guid.NewGuid();
                CurrentLimitationEmail.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentLimitationEmail.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentLimitationEmail.IsSynced = false;

                LimitationEmailResponse response = new LimitationEmailSQLiteRepository().Delete(CurrentLimitationEmail.Identifier);
                response = new LimitationEmailSQLiteRepository().Create(CurrentLimitationEmail);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = limitationEmailService.Create(CurrentLimitationEmail);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new LimitationEmailSQLiteRepository().UpdateSyncStatus(CurrentLimitationEmail.Identifier, response.LimitationEmail.UpdatedAt, response.LimitationEmail.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " SAČUVAJ ";
                    SaveButtonEnabled = true;

                    DisplayEmailData();

                    CurrentLimitationEmail = new LimitationEmailViewModel();
                    CurrentLimitationEmail.Identifier = Guid.NewGuid();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            txtName.Focus();
                        })
                    );
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentLimitationEmail = new LimitationEmailViewModel();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            CurrentLimitationEmail = CurrentLimitationEmailDG;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentLimitationEmailDG == null)
            {
                MainWindow.WarningMessage = "Morate odabrati LimitationEmail za brisanje!";
                return;
            }

            SirmiumERPVisualEffects.AddEffectOnDialogShow(this);

            // Create confirmation window
            DeleteConfirmation deleteConfirmationForm = new DeleteConfirmation("LimitationEmail", CurrentLimitationEmailDG.Email);
            var showDialog = deleteConfirmationForm.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                LimitationEmailResponse response = limitationEmailService.Delete(CurrentLimitationEmailDG.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod brisanja sa servera!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                response = new LimitationEmailSQLiteRepository().Delete(CurrentLimitationEmailDG.Identifier);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog brisanja!";
                    SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
                    return;
                }

                MainWindow.SuccessMessage = " LimitationEmail je uspešno obrisan!";

                Thread displayThread = new Thread(() => Sync());
                displayThread.IsBackground = true;
                displayThread.Start();
            }

            SirmiumERPVisualEffects.RemoveEffectOnDialogShow(this);
        }

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

    }
}
