using Ninject;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.Individuals;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Infrastructure;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SirmiumERPGFC.Views.Individuals
{
    /// <summary>
    /// Interaction logic for IndividualAddEdit.xaml
    /// </summary>
    public partial class IndividualAddEdit : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Event for handling individual create and update
        /// </summary>
        public event IndividualHandler IndividualCreatedUpdated;

        /// <summary>
        /// Service for accessing individual
        /// </summary>
        IIndividualService individualService;

        #region CurrentIndividual
        private IndividualViewModel _CurrentIndividual;

        public IndividualViewModel CurrentIndividual
        {
            get { return _CurrentIndividual; }
            set
            {
                if (_CurrentIndividual != value)
                {
                    _CurrentIndividual = value;
                    NotifyPropertyChanged("CurrentIndividual");
                }
            }
        }
        #endregion


        /// <summary>
        /// Notifier for displaying error and success messages
        /// </summary>
        Notifier notifier;

        #region Constructor

        /// <summary>
        /// IndividualAddEdit constructor
        /// </summary>
        /// <param name="IndividualViewModel"></param>
        public IndividualAddEdit(IndividualViewModel IndividualViewModel)
        {
            // Initialize service
            this.individualService = DependencyResolver.Kernel.Get<IIndividualService>();

            // Draw all components
            InitializeComponent();

            this.DataContext = this;

            // Initialize notifications
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(),
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            CurrentIndividual = IndividualViewModel;

            if (CurrentIndividual.Code <= 0)
                CurrentIndividual.Code = individualService.GetNewCodeValue().Code;

            // Add handler for keyboard shortcuts
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

            txtName.Focus();
        }
        #endregion

        private void cbxFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (CurrentOutputInvoice.IsFinalInvoice == false)
            //{
            //    lblAdvanceOutputInvoice.Visibility = Visibility.Hidden;
            //    advanceOutputInvoicePopup.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            //    lblAdvanceOutputInvoice.Visibility = Visibility.Visible;
            //    advanceOutputInvoicePopup.Visibility = Visibility.Visible;
            //}
        }

        #region Create, edit Item

        private void btnSaveItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCancelItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region Cancel save button 


        /// <summary>
        /// Cancel operation and close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

        /// <summary>
        /// Create or update Individual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;
            CurrentIndividual.CreatedBy = new UserViewModel() { Id = customPrincipal.Identity.Id };

            //if (String.IsNullOrEmpty(CurrentIndividual.Mobile))
            //{
            //    MainWindow.WarningMessage = "Morate uneti mobilni!";
            //    return;
            //}

            //int PIB = 0;
            //Int32.TryParse(CurrentIndividual.PIB, out PIB);

            IndividualResponse response;

            //// If by any chance PIB exists in the database
            //if (response.Success == false)
            //{
            //    if (CurrentIndividual.Id != response.Individual.Id)
            //    {
            //        notifier.ShowError("PIB mora biti jedinstven!");
            //        return;
            //    }
            //}
            if (CurrentIndividual.Id > 0)
                response = individualService.Update(CurrentIndividual);
            else
                response = individualService.Create(CurrentIndividual);

            if (response.Success)
            {
                IndividualCreatedUpdated(response.Individual);
                FlyoutHelper.CloseFlyout(this);
            }
            else
                notifier.ShowError(response.Message);
        }

        #endregion


        #region Keyboard shortcuts

        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FlyoutHelper.CloseFlyout(this);
            }

            if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                btnSave_Click(sender, e);
            }
        }

        #endregion

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

        #region Mouse scroll events

        private void dgFamilyItems_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "") //[CallerMemberName]
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion

    }
}
