using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Messages.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.BusinessPartners;
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

namespace SirmiumERPGFC.Views.BusinessPartners
{
    /// <summary>
    /// Interaction logic for BusinessPartner_Type_AddEdit.xaml
    /// </summary>
    public partial class BusinessPartner_Type_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IBusinessPartnerTypeService businessPartnerTypeService;
        #endregion

        #region Events
        public event BusinessPartnerTypeHandler BusinessPartnerTypeCreatedUpdated;
        #endregion

        #region CurrentBusinessPartnerType
        private BusinessPartnerTypeViewModel _CurrentBusinessPartnerType;

        public BusinessPartnerTypeViewModel CurrentBusinessPartnerType
        {
            get { return _CurrentBusinessPartnerType; }
            set
            {
                if (_CurrentBusinessPartnerType != value)
                {
                    _CurrentBusinessPartnerType = value;
                    NotifyPropertyChanged("CurrentBusinessPartnerType");
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

        #region SubmitButtonContent
        private string _SubmitButtonContent = " Sačuvaj i proknjiži ";

        public string SubmitButtonContent
        {
            get { return _SubmitButtonContent; }
            set
            {
                if (_SubmitButtonContent != value)
                {
                    _SubmitButtonContent = value;
                    NotifyPropertyChanged("SubmitButtonContent");
                }
            }
        }
        #endregion

        #region SubmitButtonEnabled
        private bool _SubmitButtonEnabled = true;

        public bool SubmitButtonEnabled
        {
            get { return _SubmitButtonEnabled; }
            set
            {
                if (_SubmitButtonEnabled != value)
                {
                    _SubmitButtonEnabled = value;
                    NotifyPropertyChanged("SubmitButtonEnabled");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public BusinessPartner_Type_AddEdit(BusinessPartnerTypeViewModel businessPartnerTypeViewModel, bool isCreateProcess)
        {
            // Initialize service
            this.businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentBusinessPartnerType = businessPartnerTypeViewModel;
            IsCreateProcess = isCreateProcess;

            txtName.Focus();
        }
        #endregion

        #region Save and Cancel button

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (String.IsNullOrEmpty(CurrentBusinessPartnerType.Name))
            {
                MainWindow.WarningMessage = "Obavezno polje: Ime vrste";
                return;
            }

            #endregion

            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentBusinessPartnerType.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentBusinessPartnerType.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentBusinessPartnerType.IsSynced = false;
                CurrentBusinessPartnerType.UpdatedAt = DateTime.Now;

                BusinessPartnerTypeResponse response = new BusinessPartnerTypeSQLiteRepository().Delete(CurrentBusinessPartnerType.Identifier);
                response = new BusinessPartnerTypeSQLiteRepository().Create(CurrentBusinessPartnerType);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = businessPartnerTypeService.Create(CurrentBusinessPartnerType);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new BusinessPartnerTypeSQLiteRepository().UpdateSyncStatus(CurrentBusinessPartnerType.Identifier, CurrentBusinessPartnerType.Id, true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    BusinessPartnerTypeCreatedUpdated();

                    if (IsCreateProcess)
                    {
                        CurrentBusinessPartnerType = new BusinessPartnerTypeViewModel();
                        CurrentBusinessPartnerType.Identifier = Guid.NewGuid();

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
                                FlyoutHelper.CloseFlyout(this);
                            })
                        );
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
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
