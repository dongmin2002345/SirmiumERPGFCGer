using Ninject;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Messages.Common.OutputInvoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.OutputInvoices;
using ServiceWebApi.Implementations.Common.OutputInvoices;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.OutputInvoices;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using WpfAppCommonCode.Converters;

namespace SirmiumERPGFC.Views.OutputInvoices
{
    /// <summary>
    /// Interaction logic for BusinessPartnerAddEdit.xaml
    /// </summary>
    public partial class OutputInvoiceAddEdit : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Event for handling output invoice create and update
        /// </summary>
        public event OutputInvoiceHandler OutputInvoiceCreatedUpdated;

        /// <summary>
        /// Service for accessing output invoice
        /// </summary>
        IOutputInvoiceService outputInvoiceService;

        #region CurrentOutputInvoice
        private OutputInvoiceViewModel _CurrentOutputInvoice;

        public OutputInvoiceViewModel CurrentOutputInvoice
        {
            get { return _CurrentOutputInvoice; }
            set
            {
                if (_CurrentOutputInvoice != value)
                {
                    _CurrentOutputInvoice = value;
                    NotifyPropertyChanged("CurrentOutputInvoice");
                }
            }
        }
        #endregion

        #region StatusOptions
        public ObservableCollection<String> StatusOptions
        {
            get
            {
                return new ObservableCollection<String>(new List<string>() {
                           ChooseStatusConverter.Choose,
                           ChooseStatusConverter.ChooseO,
                           ChooseStatusConverter.ChooseB,
                });
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
        private string _SaveButtonContent = " Sačuvaj ";

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



        #region Constructor

        public OutputInvoiceAddEdit(OutputInvoiceViewModel OutputInvoiceViewModel, bool isCreateProcess, bool isPopup = false)
        {
            // Initialize service
            outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentOutputInvoice = OutputInvoiceViewModel;
            IsCreateProcess = isCreateProcess;
            IsPopup = isPopup;
        }

        #endregion

        #region Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentOutputInvoice.Supplier))
            {
                MainWindow.WarningMessage = "Obavezno polje: Naziv dobavljača";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentOutputInvoice.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentOutputInvoice.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentOutputInvoice.IsSynced = false;
                CurrentOutputInvoice.UpdatedAt = DateTime.Now;

                OutputInvoiceResponse response = new OutputInvoiceSQLiteRepository().Delete(CurrentOutputInvoice.Identifier);
                response = new OutputInvoiceSQLiteRepository().Create(CurrentOutputInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = outputInvoiceService.Create(CurrentOutputInvoice);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new OutputInvoiceSQLiteRepository().UpdateSyncStatus(response.OutputInvoice.Identifier, response.OutputInvoice.Id, response.OutputInvoice.Code, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    OutputInvoiceCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentOutputInvoice = new OutputInvoiceViewModel();
                        CurrentOutputInvoice.Identifier = Guid.NewGuid();

                        Application.Current.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                               // txtCode.Focus();
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

        ///// <summary>
        ///// Notifier for displaying error and success messages
        ///// </summary>
        //Notifier notifier;

        //#region Constructor

        ///// <summary>
        ///// OutputInvoiceAddEdit constructor
        ///// </summary>
        ///// <param name="OutputInvoiceViewModel"></param>
        //public OutputInvoiceAddEdit(OutputInvoiceViewModel OutputInvoiceViewModel)
        //{
        //    // Initialize service
        //    this.outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>();

        //    // Draw all components
        //    InitializeComponent();

        //    this.DataContext = this;

        //    // Initialize notifications
        //    notifier = new Notifier(cfg =>
        //    {
        //        cfg.PositionProvider = new WindowPositionProvider(
        //            parentWindow: System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(),
        //            corner: Corner.TopRight,
        //            offsetX: 10,
        //            offsetY: 10);

        //        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
        //            notificationLifetime: TimeSpan.FromSeconds(3),
        //            maximumNotificationCount: MaximumNotificationCount.FromCount(3));

        //        cfg.Dispatcher = Application.Current.Dispatcher;
        //    });

        //    CurrentOutputInvoice = OutputInvoiceViewModel;

        //    //if (CurrentOutputInvoice.Code <= 0)
        //    //    CurrentOutputInvoice.Code = outputInvoiceService.GetNewCodeValue().OutputInvoiceCode;

        //    // Add handler for keyboard shortcuts
        //    AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

        //    //txtName.Focus();
        //}
        //#endregion

        //#region Cancel save button 


        ///// <summary>
        ///// Cancel operation and close window
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnCancel_Click(object sender, RoutedEventArgs e)
        //{
        //    FlyoutHelper.CloseFlyout(this);
        //}

        ///// <summary>
        ///// Create or update OutputInvoice
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnSave_Click(object sender, RoutedEventArgs e)
        //{

        //    CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
        //    CurrentOutputInvoice.CreatedBy = new UserViewModel() { Id = customPrincipal.Identity.Id };

        //    //if (String.IsNullOrEmpty(CurrentOutputInvoice.Mobile))
        //    //{
        //    //    MainWindow.WarningMessage = "Morate uneti mobilni!";
        //    //    return;
        //    //}

        //    //int PIB = 0;
        //    //Int32.TryParse(CurrentOutputInvoice.PIB, out PIB);

        //    OutputInvoiceResponse response;

        //    //// If by any chance PIB exists in the database
        //    //if (response.Success == false)
        //    //{
        //    //    if (CurrentOutputInvoice.Id != response.OutputInvoice.Id)
        //    //    {
        //    //        notifier.ShowError("PIB mora biti jedinstven!");
        //    //        return;
        //    //    }
        //    //}
        //    //if (CurrentOutputInvoice.Id > 0)
        //    //    response = outputInvoiceService.Update(CurrentOutputInvoice);
        //    //else
        //    //    response = outputInvoiceService.Create(CurrentOutputInvoice);

        //    //if (response.Success)
        //    //{
        //    //    OutputInvoiceCreatedUpdated(response.OutputInvoice);
        //    //    FlyoutHelper.CloseFlyout(this);
        //    //}
        //    //else
        //    //    notifier.ShowError(response.Message);
        //}

        //#endregion


        //#region Keyboard shortcuts

        //private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //    {
        //        FlyoutHelper.CloseFlyout(this);
        //    }

        //    if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
        //    {
        //        btnSave_Click(sender, e);
        //    }
        //}

        //#endregion

        //#region Validation

        ///// <summary>
        ///// Validation of input fields
        ///// </summary>
        ///// <returns></returns>
        //private bool InputIsValid()
        //{
        //    bool isValid = true;

        //    // Check is CompanyName entered
        //    if (String.IsNullOrEmpty(txtName.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("Ime kompanije je obavezno!");
        //    }

        //    // Check is address entered
        //    if (String.IsNullOrEmpty(txtAddress.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("Adresa je obavezna!");
        //    }

        //    // Check is phone entered
        //    if (String.IsNullOrEmpty(txtPhone.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("Telefon je obavezan!");
        //    }

        //    // Check is Code entered
        //    if (String.IsNullOrEmpty(txtCode.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("Sifra je obavezna!");
        //    }

        //    // Check is Code entered
        //    if (String.IsNullOrEmpty(txtPIBNumber.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("PIB je obavezan!");
        //    }

        //    if(String.IsNullOrEmpty(txtDueDate.Text))
        //    {
        //        isValid = false;
        //        notifier.ShowError("Valuta partnera je obavezna!");
        //    }


        //    return isValid;
        //}

        //#endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

    }
}
