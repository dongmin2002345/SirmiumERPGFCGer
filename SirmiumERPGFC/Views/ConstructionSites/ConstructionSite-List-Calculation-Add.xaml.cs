using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ConstructionSites;
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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    public partial class ConstructionSite_List_Calculation_Add : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Service
        IConstructionSiteCalculationService constructionSiteCalculationService;
        #endregion

        #region Events
        public event ConstructionSiteCalculationHandler ConstructionSiteCalculationCreatedUpdated;
        #endregion

        #region CurrentConstructionSiteCalculation
        private ConstructionSiteCalculationViewModel _CurrentConstructionSiteCalculation;

        public ConstructionSiteCalculationViewModel CurrentConstructionSiteCalculation
        {
            get { return _CurrentConstructionSiteCalculation; }
            set
            {
                if (_CurrentConstructionSiteCalculation != value)
                {
                    _CurrentConstructionSiteCalculation = value;
                    NotifyPropertyChanged("CurrentConstructionSiteCalculation");
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

        #endregion

        public ConstructionSite_List_Calculation_Add(ConstructionSiteCalculationViewModel constructionSiteCalculation)
        {
            constructionSiteCalculationService = DependencyResolver.Kernel.Get<IConstructionSiteCalculationService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSiteCalculation = constructionSiteCalculation;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(() =>
            {
                SaveButtonContent = " Čuvanje u toku... ";
                SaveButtonEnabled = false;

                CurrentConstructionSiteCalculation.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
                CurrentConstructionSiteCalculation.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

                CurrentConstructionSiteCalculation.IsSynced = false;
                CurrentConstructionSiteCalculation.UpdatedAt = DateTime.Now;

                ConstructionSiteCalculationResponse response = new ConstructionSiteCalculationSQLiteRepository().Delete(CurrentConstructionSiteCalculation.Identifier);
                response = new ConstructionSiteCalculationSQLiteRepository().Create(CurrentConstructionSiteCalculation);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod lokalnog čuvanja!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                    return;
                }

                response = constructionSiteCalculationService.Create(CurrentConstructionSiteCalculation);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Podaci su sačuvani u lokalu!. Greška kod čuvanja na serveru!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;
                }

                if (response.Success)
                {
                    new ConstructionSiteCalculationSQLiteRepository().UpdateSyncStatus(
                        response.ConstructionSiteCalculation.Identifier, 
                        response.ConstructionSiteCalculation.Id,
                        response.ConstructionSiteCalculation.ValueDifference,
                        response.ConstructionSiteCalculation.NewValue,
                        true);
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SaveButtonContent = " Sačuvaj ";
                    SaveButtonEnabled = true;

                    ConstructionSiteCalculationCreatedUpdated();

                    ConstructionSiteViewModel constructionSite = CurrentConstructionSiteCalculation.ConstructionSite;
                    CurrentConstructionSiteCalculation = new ConstructionSiteCalculationViewModel();
                    CurrentConstructionSiteCalculation.Identifier = Guid.NewGuid();
                    CurrentConstructionSiteCalculation.ConstructionSite = constructionSite;

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            txtNumOfEmployees.Focus();
                        })
                    );                    
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }

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
