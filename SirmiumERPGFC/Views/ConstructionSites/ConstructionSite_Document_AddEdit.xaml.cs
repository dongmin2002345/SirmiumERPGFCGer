﻿using Ninject;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Gloabals;
using ServiceInterfaces.Messages.ConstructionSites;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.ConstructionSites;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.ConstructionSites;
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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    /// <summary>
    /// Interaction logic for ConstructionSite_Document_AddEdit.xaml
    /// </summary>
    public partial class ConstructionSite_Document_AddEdit : UserControl, INotifyPropertyChanged
    {
        #region Attributes

        #region Services
        IConstructionSiteService constructionSiteService;
        IConstructionSiteDocumentService constructionSiteDocumentService;
        #endregion


        #region Event
        public event ConstructionSiteHandler ConstructionSiteCreatedUpdated;
        #endregion


        #region CurrentConstructionSite
        private ConstructionSiteViewModel _CurrentConstructionSite = new ConstructionSiteViewModel();

        public ConstructionSiteViewModel CurrentConstructionSite
        {
            get { return _CurrentConstructionSite; }
            set
            {
                if (_CurrentConstructionSite != value)
                {
                    _CurrentConstructionSite = value;
                    NotifyPropertyChanged("CurrentConstructionSite");
                }
            }
        }
        #endregion


        #region ConstructionSiteDocumentsFromDB
        private ObservableCollection<ConstructionSiteDocumentViewModel> _ConstructionSiteDocumentsFromDB;

        public ObservableCollection<ConstructionSiteDocumentViewModel> ConstructionSiteDocumentsFromDB
        {
            get { return _ConstructionSiteDocumentsFromDB; }
            set
            {
                if (_ConstructionSiteDocumentsFromDB != value)
                {
                    _ConstructionSiteDocumentsFromDB = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentsFromDB");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocumentForm
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel() { CreateDate = DateTime.Now };

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocumentForm
        {
            get { return _CurrentConstructionSiteDocumentForm; }
            set
            {
                if (_CurrentConstructionSiteDocumentForm != value)
                {
                    _CurrentConstructionSiteDocumentForm = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocumentForm");
                }
            }
        }
        #endregion

        #region CurrentConstructionSiteDocumentDG
        private ConstructionSiteDocumentViewModel _CurrentConstructionSiteDocumentDG;

        public ConstructionSiteDocumentViewModel CurrentConstructionSiteDocumentDG
        {
            get { return _CurrentConstructionSiteDocumentDG; }
            set
            {
                if (_CurrentConstructionSiteDocumentDG != value)
                {
                    _CurrentConstructionSiteDocumentDG = value;
                    NotifyPropertyChanged("CurrentConstructionSiteDocumentDG");
                }
            }
        }
        #endregion

        #region ConstructionSiteDocumentDataLoading
        private bool _ConstructionSiteDocumentDataLoading;

        public bool ConstructionSiteDocumentDataLoading
        {
            get { return _ConstructionSiteDocumentDataLoading; }
            set
            {
                if (_ConstructionSiteDocumentDataLoading != value)
                {
                    _ConstructionSiteDocumentDataLoading = value;
                    NotifyPropertyChanged("ConstructionSiteDocumentDataLoading");
                }
            }
        }
        #endregion




        #region SubmitButtonContent
        private string _SubmitButtonContent = " PROKNJIŽI ";

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

        public ConstructionSite_Document_AddEdit(ConstructionSiteViewModel constructionSite)
        {
            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>();
            constructionSiteDocumentService = DependencyResolver.Kernel.Get<IConstructionSiteDocumentService>();

            InitializeComponent();

            this.DataContext = this;

            CurrentConstructionSite = constructionSite;
            CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
            CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Added;

            Thread displayThread = new Thread(() => DisplayConstructionSiteDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            btnAddDocument.Focus();
        }

        #endregion

        #region Display data

        public void DisplayConstructionSiteDocumentData()
        {
            ConstructionSiteDocumentDataLoading = true;

            ConstructionSiteDocumentListResponse response = new ConstructionSiteDocumentSQLiteRepository()
                .GetConstructionSiteDocumentsByConstructionSite(MainWindow.CurrentCompanyId, CurrentConstructionSite.Identifier);

            if (response.Success)
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>(
                    response.ConstructionSiteDocuments ?? new List<ConstructionSiteDocumentViewModel>());
            }
            else
            {
                ConstructionSiteDocumentsFromDB = new ObservableCollection<ConstructionSiteDocumentViewModel>();
            }

            ConstructionSiteDocumentDataLoading = false;
        }

        private void DgConstructionSiteDocuments_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void dg_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
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

        #region Add, Edit and Delete 

        private void btnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (CurrentConstructionSiteDocumentForm.Name == null)
            {
                MainWindow.ErrorMessage = "Obavezno polje: Naziv";
                return;
            }

            #endregion

            new ConstructionSiteDocumentSQLiteRepository().Delete(CurrentConstructionSiteDocumentForm.Identifier);

            CurrentConstructionSiteDocumentForm.ConstructionSite = CurrentConstructionSite;

            CurrentConstructionSiteDocumentForm.IsSynced = false;
            CurrentConstructionSiteDocumentForm.Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId };
            CurrentConstructionSiteDocumentForm.CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId };

            var response = new ConstructionSiteDocumentSQLiteRepository().Create(CurrentConstructionSiteDocumentForm);
            if (!response.Success)
            {
                MainWindow.ErrorMessage = response.Message;

                CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
                CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Added;

                return;
            }

            CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
            CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Added;

            ConstructionSiteCreatedUpdated();

            Thread displayThread = new Thread(() => DisplayConstructionSiteDocumentData());
            displayThread.IsBackground = true;
            displayThread.Start();

            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    txtDocumentName.Focus();
                })
            );

            SubmitButtonEnabled = true;
        }

        private void btnEditDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
            CurrentConstructionSiteDocumentForm.Identifier = CurrentConstructionSiteDocumentDG.Identifier;
            CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Edited;

            CurrentConstructionSiteDocumentForm.Name = CurrentConstructionSiteDocumentDG.Name;
            CurrentConstructionSiteDocumentForm.CreateDate = CurrentConstructionSiteDocumentDG.CreateDate;
            CurrentConstructionSiteDocumentForm.Path = CurrentConstructionSiteDocumentDG.Path;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var response = new ConstructionSiteDocumentSQLiteRepository().SetStatusDeleted(CurrentConstructionSiteDocumentDG.Identifier);
            if (response.Success)
            {
                MainWindow.SuccessMessage = "Stavka je uspešno obrisana!";

                CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
                CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
                CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Added;

                CurrentConstructionSiteDocumentDG = null;

                ConstructionSiteCreatedUpdated();

                Thread displayThread = new Thread(() => DisplayConstructionSiteDocumentData());
                displayThread.IsBackground = true;
                displayThread.Start();
            }
            else
                MainWindow.ErrorMessage = response.Message;
        }

        private void btnCancelDocument_Click(object sender, RoutedEventArgs e)
        {
            CurrentConstructionSiteDocumentForm = new ConstructionSiteDocumentViewModel();
            CurrentConstructionSiteDocumentForm.Identifier = Guid.NewGuid();
            CurrentConstructionSiteDocumentForm.ItemStatus = ItemStatus.Added;
        }

        private void FileDIalog_FileOk(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = (System.Windows.Forms.OpenFileDialog)sender;
            string[] fileNames = dialog.FileNames;

            if (fileNames.Length > 0)
                CurrentConstructionSiteDocumentForm.Path = fileNames[0];
        }

        private void btnChooseDocument_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDIalog = new System.Windows.Forms.OpenFileDialog();

            fileDIalog.Multiselect = true;
            fileDIalog.FileOk += FileDIalog_FileOk;
            fileDIalog.Filter = "Image Files | *.pdf";
            fileDIalog.ShowDialog();
        }

        #endregion

        #region Submit and Cancel 

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            #region Validation

            if (ConstructionSiteDocumentsFromDB == null || ConstructionSiteDocumentsFromDB.Count == 0)
            {
                MainWindow.WarningMessage = "Ne postoje stavke za proknjižavanje!";
                return;
            }

            #endregion

            Thread td = new Thread(() => {

                SubmitButtonContent = " Čuvanje u toku... ";
                SubmitButtonEnabled = false;

                CurrentConstructionSite.ConstructionSiteDocuments = ConstructionSiteDocumentsFromDB;
                ConstructionSiteResponse response = constructionSiteService.Create(CurrentConstructionSite);
                if (!response.Success)
                {
                    MainWindow.ErrorMessage = "Greška kod čuvanja podataka!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;
                }

                if (response.Success)
                {
                    MainWindow.SuccessMessage = "Podaci su uspešno sačuvani!";
                    SubmitButtonContent = " PROKNJIŽI ";
                    SubmitButtonEnabled = true;

                    ConstructionSiteCreatedUpdated();

                    Application.Current.Dispatcher.BeginInvoke(
                        System.Windows.Threading.DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            FlyoutHelper.CloseFlyout(this);
                        })
                    );
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ConstructionSiteCreatedUpdated();

            FlyoutHelper.CloseFlyout(this);
        }

        #endregion

        #region INotifyPropertyChange implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string inPropName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(inPropName));
        }
        #endregion
    }
}