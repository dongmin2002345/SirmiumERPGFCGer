using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Azure.Storage.File;
using Microsoft.Win32;
using Ninject;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Helpers;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.DocumentStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace SirmiumERPGFC.ViewComponents.Dialogs
{
    /// <summary>
    /// Interaction logic for DocumentTreeView.xaml
    /// </summary>
    public partial class DocumentPathDialog : MetroWindow, INotifyPropertyChanged
    {
        #region DocumentTreeItems
        private ObservableCollection<DocumentFolderViewModel> _DocumentTreeItems;

        public ObservableCollection<DocumentFolderViewModel> DocumentTreeItems
        {
            get { return _DocumentTreeItems; }
            set
            {
                if (_DocumentTreeItems != value)
                {
                    _DocumentTreeItems = value;
                    NotifyPropertyChanged("DocumentTreeItems");
                }
            }
        }
        #endregion


        #region LoadingData
        private bool _LoadingData;

        public bool LoadingData
        {
            get { return _LoadingData; }
            set
            {
                if (_LoadingData != value)
                {
                    _LoadingData = value;
                    NotifyPropertyChanged("LoadingData");

                    CanInteract = !_LoadingData;
                }
            }
        }
        #endregion

        #region CanInteract
        private bool _CanInteract = true;

        public bool CanInteract
        {
            get { return _CanInteract; }
            set
            {
                if (_CanInteract != value)
                {
                    _CanInteract = value;
                    NotifyPropertyChanged("CanInteract");
                }
            }
        }
        #endregion

        #region CancelDialog
        private bool _CancelDialog;

        public bool CancelDialog
        {
            get { return _CancelDialog; }
            set
            {
                if (_CancelDialog != value)
                {
                    _CancelDialog = value;
                    NotifyPropertyChanged("CancelDialog");
                }
            }
        }
        #endregion


        #region SelectedTreeItem
        private DocumentFolderViewModel _SelectedTreeItem;

        public DocumentFolderViewModel SelectedTreeItem
        {
            get { return _SelectedTreeItem; }
            set
            {
                if (_SelectedTreeItem != value)
                {
                    _SelectedTreeItem = value;
                    NotifyPropertyChanged("SelectedTreeItem");

                    if (_SelectedTreeItem != null)
                        CanCreateFolder = true;
                    else
                        CanCreateFolder = false;

                    FolderFilterObject.Search_ParentId = SelectedTreeItem?.Id;

                    SelectedPath = _SelectedTreeItem?.Path;
                    //if(_SelectedTreeItem )
                }
            }
        }
        #endregion

        #region NewFolderName
        private string _NewFolderName;

        public string NewFolderName
        {
            get { return _NewFolderName; }
            set
            {
                if (_NewFolderName != value)
                {
                    _NewFolderName = value;
                    NotifyPropertyChanged("NewFolderName");
                }
            }
        }
        #endregion

        #region CanCreateFolder
        private bool _CanCreateFolder = false;

        public bool CanCreateFolder
        {
            get { return _CanCreateFolder; }
            set
            {
                if (_CanCreateFolder != value)
                {
                    _CanCreateFolder = value;
                    NotifyPropertyChanged("CanCreateFolder");
                }
            }
        }
        #endregion

        #region SelectedPath
        private string _SelectedPath;

        public string SelectedPath
        {
            get { return _SelectedPath; }
            set
            {
                if (_SelectedPath != value)
                {
                    _SelectedPath = value;
                    NotifyPropertyChanged("SelectedPath");
                }
            }
        }
        #endregion

        #region FolderFilterObject
        private DocumentFolderViewModel _FolderFilterObject = new DocumentFolderViewModel() { Search_MultiLevel = true, Search_ParentId = null };

        public DocumentFolderViewModel FolderFilterObject
        {
            get { return _FolderFilterObject; }
            set
            {
                if (_FolderFilterObject != value)
                {
                    _FolderFilterObject = value;
                    NotifyPropertyChanged("FolderFilterObject");
                }
            }
        }
        #endregion

        AzureDataClient azureClient;
        IDocumentFolderService documentFolderService;
        public DocumentPathDialog()
        {
            InitializeComponent();
            documentFolderService = DependencyResolver.Kernel.Get<IDocumentFolderService>();

            this.DataContext = this;
            FolderFilterObject.PropertyChanged += FolderFilterObject_PropertyChanged;

            azureClient = new AzureDataClient();

            Thread td = new Thread(() => DisplayFolderTree());
            td.IsBackground = true;
            td.Start();
        }
        private void FolderFilterObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e?.PropertyName))
            {
                if (e.PropertyName == "Search_Name")
                {
                    if (String.IsNullOrEmpty(FolderFilterObject.Search_Name))
                    {
                        FolderFilterObject.Search_MultiLevel = true;
                    }
                    else
                    {
                        FolderFilterObject.Search_MultiLevel = false;
                        SelectedTreeItem = null;
                    }

                    Thread td = new Thread(() => DisplayFolderTree());
                    td.IsBackground = true;
                    td.Start();
                }
            }
        }

        private void DisplayFolderTree()
        {
            try
            {
                LoadingData = true;

                DocumentFolderListResponse response;
                if (FolderFilterObject.Search_MultiLevel)
                {
                    response = new DocumentFolderSQLiteRepository().GetRootFolder(MainWindow.CurrentCompanyId);
                    if (response.Success)
                    {
                        DocumentTreeItems = new ObservableCollection<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());

                        if (DocumentTreeItems.Count() > 0)
                        {
                            SelectedTreeItem = DocumentTreeItems[0];
                            GetDirectoryTree(SelectedTreeItem);
                        }
                    }
                    else
                        MainWindow.ErrorMessage = response.Message;
                }
                else
                {
                    response = new DocumentFolderSQLiteRepository().GetDocumentFolders(MainWindow.CurrentCompanyId, FolderFilterObject);
                    if (response.Success)
                    {
                        DocumentTreeItems = new ObservableCollection<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());
                    }
                    else
                        MainWindow.ErrorMessage = response.Message;
                }


            }
            catch (Exception ex)
            {
            }
            finally
            {
                LoadingData = false;
            }
        }

        void GetDirectoryTree(DocumentFolderViewModel parent)
        {
            List<DocumentFolderViewModel> directories;
            try
            {
                var response = new DocumentFolderSQLiteRepository().GetDocumentFolders(MainWindow.CurrentCompanyId, FolderFilterObject, false);

                directories = response?.DocumentFolders ?? new List<DocumentFolderViewModel>();
            }
            catch (Exception ex)
            {
                directories = new List<DocumentFolderViewModel>();
            }

            parent.SubDirectories = new ObservableCollection<DocumentFolderViewModel>(directories);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedTreeItem = e.NewValue as DocumentFolderViewModel;

            Thread td = new Thread(() => {
                try
                {
                    LoadingData = true;

                    if (FolderFilterObject.Search_MultiLevel)
                    {
                        FolderFilterObject.Search_ParentId = SelectedTreeItem?.Id;
                        if (SelectedTreeItem != null && SelectedTreeItem.SubDirectories == null || SelectedTreeItem.SubDirectories.Count() < 1)
                        {
                            GetDirectoryTree(SelectedTreeItem);
                        }
                    }
                }
                catch (Exception ex) { }
                finally
                {
                    LoadingData = false;
                }
            });
            td.IsBackground = true;
            td.Start();
        }




        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            Dispatcher.BeginInvoke((Action)(() => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
        #endregion

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem tvi = e.OriginalSource as TreeViewItem;

                if (tvi == null || e.Handled) return;
                tvi.IsExpanded = true;

                e.Handled = true;
            }
            catch (Exception ex) { }
        }


        async Task<string> GetNewFolderName()
        {
            var parentWindow = this;
            //MetroWindow parentWindow = Window.GetWindow(this) as MetroWindow;
            if (parentWindow != null)
            {
                return await parentWindow.ShowInputAsync("Kreiraj folder", "Unesite ime foldera",
                    new MetroDialogSettings()
                    {
                        DialogResultOnCancel = MessageDialogResult.Canceled,
                        AffirmativeButtonText = "Potvrdi",
                        NegativeButtonText = "Otkaži",
                        DefaultButtonFocus = MessageDialogResult.Affirmative,
                    });
            }
            return null;
        }

        private async void btnCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            var result = await GetNewFolderName();

            NewFolderName = result;

            if (String.IsNullOrEmpty(NewFolderName))
            {
                MessageBox.Show("Naziv foldera ne moze biti prazan!");
                return;
            }

            string basePath = SelectedTreeItem?.Path;
            if (String.IsNullOrEmpty(basePath))
            {
                MessageBox.Show("Bazna putanja ne moze biti prazna!");
                return;
            }

            try
            {
                var parent = azureClient.GetDirectory(basePath);
                if (!parent.Exists())
                    throw new Exception("Odabrani folder ne postoji!");

                var childFolder = parent.GetDirectoryReference(NewFolderName);
                if (childFolder.Exists())
                    throw new Exception("Folder sa zadatim imenom već postoji!");

                childFolder.Create();

                var newDir = new DocumentFolderViewModel()
                {
                    Identifier = Guid.NewGuid(),
                    ParentFolder = SelectedTreeItem,
                    Name = NewFolderName,
                    Path = childFolder.Uri.LocalPath,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Company = new ServiceInterfaces.ViewModels.Common.Companies.CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                    CreatedBy = new ServiceInterfaces.ViewModels.Common.Identity.UserViewModel() { Id = MainWindow.CurrentUserId }
                };

                var response = documentFolderService.Create(newDir);
                if(response.Success)
                {
                    newDir.Id = response?.DocumentFolder?.Id ?? 0;
                    new DocumentFolderSQLiteRepository().Sync(documentFolderService, ((done, toDo) => {
                        Debug.WriteLine($"Syncing folders: {done} out of {toDo}");
                    }));
                    SelectedTreeItem.SubDirectories.Add(newDir);
                    SelectedTreeItem.IsDirExpanded = true;
                    MainWindow.SuccessMessage = "Folder je uspešno kreiran!";
                }
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }

            NewFolderName = "";
        }

        private void btnConfirmFolder_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTreeItem == null || String.IsNullOrEmpty(SelectedPath))
            {
                MainWindow.ErrorMessage = "Morate odabrati folder da biste potvrdili putanju!";
                return;
            }
            this.DialogResult = true;
        }

        private void btnCancelFolder_Click(object sender, RoutedEventArgs e)
        {
            CancelDialog = true;
            this.DialogResult = false;
            this.Close();
        }

        private void btnDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTreeItem != null)
            {
                Thread td = new Thread(() => {
                    try
                    {
                        var directory = azureClient.GetDirectory(SelectedTreeItem.Path);

                        var subItemsCount = directory.ListFilesAndDirectories().Count();
                        if (subItemsCount > 0)
                        {
                            MainWindow.ErrorMessage = "Možete brisati samo prazne foldere!";
                            return;
                        }

                        var response = documentFolderService.Delete(SelectedTreeItem);
                        if (response.Success)
                        {
                            directory.Delete();

                            new DocumentFolderSQLiteRepository().Sync(documentFolderService, ((done, toDo) => {
                                Debug.WriteLine($"Syncing folders: {done} out of {toDo}");
                            }));
                            DisplayFolderTree();
                        }
                        else
                            MainWindow.ErrorMessage = response.Message;

                    }
                    catch (Exception ex)
                    {
                        MainWindow.ErrorMessage = ex.Message;
                    }
                });
                td.IsBackground = true;
                td.Start();
            }
        }
    }
}
