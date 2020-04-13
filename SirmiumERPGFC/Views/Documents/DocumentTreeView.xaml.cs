using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Azure.Storage.File;
using Microsoft.Win32;
using Ninject;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
using ServiceInterfaces.Messages.Common.Invoices;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.DocumentStores;
using ServiceInterfaces.ViewModels.Common.Identity;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Helpers;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.DocumentStores;
using SirmiumERPGFC.ViewComponents.Dialogs;
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

namespace SirmiumERPGFC.Views.Documents
{
    /// <summary>
    /// Interaction logic for DocumentTreeView.xaml
    /// </summary>
    public partial class DocumentTreeView : UserControl, INotifyPropertyChanged
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

        #region DocumentTreeSelectedItem
        private DirectoryTreeItemViewModel _DocumentTreeSelectedItem;

        public DirectoryTreeItemViewModel DocumentTreeSelectedItem
        {
            get { return _DocumentTreeSelectedItem; }
            set
            {
                if (_DocumentTreeSelectedItem != value)
                {
                    _DocumentTreeSelectedItem = value;
                    NotifyPropertyChanged("DocumentTreeSelectedItem");
                }
            }
        }
        #endregion



        #region DocumentTreeFiles
        private ObservableCollection<DocumentFileViewModel> _DocumentTreeFiles;

        public ObservableCollection<DocumentFileViewModel> DocumentTreeFiles
        {
            get { return _DocumentTreeFiles; }
            set
            {
                if (_DocumentTreeFiles != value)
                {
                    _DocumentTreeFiles = value;
                    NotifyPropertyChanged("DocumentTreeFiles");
                }
            }
        }
        #endregion

        #region SelectedDocumentTreeFile
        private DocumentFileViewModel _SelectedDocumentTreeFile;

        public DocumentFileViewModel SelectedDocumentTreeFile
        {
            get { return _SelectedDocumentTreeFile; }
            set
            {
                if (_SelectedDocumentTreeFile != value)
                {
                    _SelectedDocumentTreeFile = value;
                    NotifyPropertyChanged("SelectedDocumentTreeFile");

                    SelectedDocument = SelectedDocumentTreeFile;
                }
            }
        }
        #endregion

        #region SelectedDocument
        private DocumentFileViewModel _SelectedDocument;

        public DocumentFileViewModel SelectedDocument
        {
            get { return _SelectedDocument; }
            set
            {
                if (_SelectedDocument != value)
                {
                    _SelectedDocument = value;
                    NotifyPropertyChanged("SelectedDocument");
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

        #region FilterText
        private string _FilterText;

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                if (_FilterText != value)
                {
                    _FilterText = value;
                    NotifyPropertyChanged("FilterText");

                    Thread td = new Thread(() => GetFolderDocuments());
                    td.IsBackground = true;
                    td.Start();
                }
            }
        }
        #endregion


        #region FilterDateFrom
        private DateTime? _FilterDateFrom;

        public DateTime? FilterDateFrom
        {
            get { return _FilterDateFrom; }
            set
            {
                if (_FilterDateFrom != value)
                {
                    _FilterDateFrom = value;
                    NotifyPropertyChanged("FilterDateFrom");

                    Thread td = new Thread(() => GetFolderDocuments());
                    td.IsBackground = true;
                    td.Start();
                }
            }
        }
        #endregion

        #region FilterDateTo
        private DateTime? _FilterDateTo;

        public DateTime? FilterDateTo
        {
            get { return _FilterDateTo; }
            set
            {
                if (_FilterDateTo != value)
                {
                    _FilterDateTo = value;
                    NotifyPropertyChanged("FilterDateTo");

                    Thread td = new Thread(() => GetFolderDocuments());
                    td.IsBackground = true;
                    td.Start();
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



        #region FilesToUpload
        private ObservableCollection<DirectoryTreeItemViewModel> _FilesToUpload = new ObservableCollection<DirectoryTreeItemViewModel>();

        public ObservableCollection<DirectoryTreeItemViewModel> FilesToUpload
        {
            get { return _FilesToUpload; }
            set
            {
                if (_FilesToUpload != value)
                {
                    _FilesToUpload = value;
                    NotifyPropertyChanged("FilesToUpload");
                }
            }
        }
        #endregion


        #region IsCopyInProgress
        private bool _IsCopyInProgress;

        public bool IsCopyInProgress
        {
            get { return _IsCopyInProgress; }
            set
            {
                if (_IsCopyInProgress != value)
                {
                    _IsCopyInProgress = value;
                    NotifyPropertyChanged("IsCopyInProgress");
                }
            }
        }
        #endregion

        #region CopyPercentage
        private double _CopyPercentage;

        public double CopyPercentage
        {
            get { return _CopyPercentage; }
            set
            {
                if (_CopyPercentage != value)
                {
                    _CopyPercentage = value;
                    NotifyPropertyChanged("CopyPercentage");
                }
            }
        }
        #endregion


        private IDocumentFolderService documentFolderService;
        private IDocumentFileService documentFileService;
        AzureDataClient azureClient;

        #region CurrentStatus
        private string _CurrentStatus = "OK";

        public string CurrentStatus
        {
            get { return _CurrentStatus; }
            set
            {
                if (_CurrentStatus != value)
                {
                    _CurrentStatus = value;
                    NotifyPropertyChanged("CurrentStatus");
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


        public DocumentTreeView()
        {
            InitializeComponent();

            documentFolderService = DependencyResolver.Kernel.Get<IDocumentFolderService>();
            documentFileService = DependencyResolver.Kernel.Get<IDocumentFileService>();

            this.DataContext = this;


            FolderFilterObject.PropertyChanged += FolderFilterObject_PropertyChanged;

            azureClient = new AzureDataClient();

            Thread td = new Thread(() => DisplayFolderTree());
            td.IsBackground = true;
            td.Start();
        }

        private void FolderFilterObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!String.IsNullOrEmpty(e?.PropertyName))
            {
                if(e.PropertyName == "Search_Name")
                {
                    if(String.IsNullOrEmpty(FolderFilterObject.Search_Name))
                    {
                        FolderFilterObject.Search_MultiLevel = true;
                    } else
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

                new DocumentFolderSQLiteRepository().Sync(documentFolderService, ((done, toDo) => {
                    Debug.WriteLine($"Syncing folders: {done} out of {toDo}");
                
                }));
                new DocumentFileSQLiteRepository().Sync(documentFileService, ((done, toDo) => {
                    Debug.WriteLine($"Syncing documents: {done} out of {toDo}");
                }));

                DocumentFolderListResponse response;
                if(FolderFilterObject.Search_MultiLevel)
                {
                    response = new DocumentFolderSQLiteRepository().GetRootFolder(MainWindow.CurrentCompanyId);
                    if (response.Success)
                    {
                        DocumentTreeItems = new ObservableCollection<DocumentFolderViewModel>(response?.DocumentFolders ?? new List<DocumentFolderViewModel>());

                        if (DocumentTreeItems.Count() > 0)
                        {
                            SelectedTreeItem = DocumentTreeItems[0];
                            GetDirectoryTree(SelectedTreeItem);

                            GetFolderDocuments();
                        }
                    }
                    else
                        MainWindow.ErrorMessage = response.Message;
                } else
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

        void StatusCallback(string status)
        {
            Dispatcher.BeginInvoke((Action)(() => { CurrentStatus = status; }));
        }

        void GetFolderDocuments()
        {
            List<DocumentFileViewModel> documents;

            var response = new DocumentFileSQLiteRepository().GetDocumentFiles(MainWindow.CurrentCompanyId, 
                new DocumentFileViewModel() 
                { 
                    Search_ParentPath = SelectedTreeItem?.Path, 
                    Search_Name = FilterText,
                    Search_DateFrom = FilterDateFrom,
                    Search_DateTo = FilterDateTo
                });
            if (response.Success)
                documents = response?.DocumentFiles ?? new List<DocumentFileViewModel>();
            else
            {
                MainWindow.ErrorMessage = response.Message;
                documents = new List<DocumentFileViewModel>();
            }

            DocumentTreeFiles = new ObservableCollection<DocumentFileViewModel>(documents ?? new List<DocumentFileViewModel>());
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
                        if(SelectedTreeItem != null && SelectedTreeItem.SubDirectories == null || SelectedTreeItem.SubDirectories.Count() < 1)
                        {
                            GetDirectoryTree(SelectedTreeItem);
                        }
                    }

                    GetFolderDocuments();
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

        async Task<string> GetNewFolderName()
        {
            var parentWindow = this.TryFindParent<MetroWindow>();
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
                if (response.Success)
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

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = DocumentTreeFiles?.Where(x => x.IsSelected)?.ToList() ?? new List<DocumentFileViewModel>();
            if (itemsToDelete.Count() > 0)
            {
                try
                {
                    foreach(var item in itemsToDelete)
                    {
                        var response = documentFileService.Delete(item);
                        if (response.Success)
                        {

                            new DocumentFileSQLiteRepository().Sync(documentFileService, ((done, toDo) =>
                            {
                                Debug.WriteLine($"Syncing documents: {done} out of {toDo}");
                            }));

                            azureClient.Delete(azureClient.GetFile(item.Path));
                            DocumentTreeFiles.Remove(item);
                        }
                        else
                            MainWindow.ErrorMessage = response.Message;
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                }
            }
        }

        private void BtnSelectData_Drop(object sender, DragEventArgs e)
        {
            IDataObject data = e.Data;
            if (data != null)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length > 0)
                {
                    AppendToUploadList(files);
                }
            }
        }

        private void AppendToUploadList(string[] files)
        {
            foreach (string file in files)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (!FilesToUpload.Any(y => y.Name == fileInfo.Name))
                    {
                        DirectoryTreeItemViewModel fileToAdd = new DirectoryTreeItemViewModel();
                        fileToAdd.Name = fileInfo.Name;
                        fileToAdd.FullPath = fileInfo.FullName;
                        fileToAdd.FileSize = fileInfo.Length / 1024;
                        FilesToUpload.Add(fileToAdd);
                    }
                } catch(Exception ex)
                {
                    MainWindow.ErrorMessage = "Nije moguće pristupiti odabranom dokumentu!";
                }
            }
        }

        private void BtnSelectData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            bool? result = ofd.ShowDialog();
            if (result != null)
            {
                if (ofd.FileNames != null && ofd.FileNames.Count() > 0)
                {
                    AppendToUploadList(ofd.FileNames);
                }
            }
        }

        private void BtnCopyData_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTreeItem == null)
            {
                MainWindow.ErrorMessage = ("Morate odabrati folder gde ce se smestiti dokumenti!");
                return;
            }


            if (FilesToUpload == null || FilesToUpload.Count() < 1)
            {
                MainWindow.ErrorMessage = ("Morate odabrati dokumente za kopiranje!");
                return;
            }

            Thread td = new Thread(() =>
            {
                try
                {
                    LoadingData = true;
                    IsCopyInProgress = true;
                    List<DirectoryTreeItemViewModel> filesToUpload = FilesToUpload.Where(x => x.IsSelected).ToList();

                    if(filesToUpload.Count() < 1)
                    {
                        MainWindow.ErrorMessage = ("Morate odabrati dokumente za kopiranje!");
                        return;
                    }

                    foreach (DirectoryTreeItemViewModel item in filesToUpload)
                    {
                        CopyPercentage = 0;

                        var copiedFile = azureClient.CopyLocal(azureClient.GetDirectory(SelectedTreeItem.Path), item.FullPath, (progress, total) =>
                        {
                            long percent = (long)(((double)progress / (double)total) * 100);
                            CopyPercentage = percent;

                            Thread.Sleep(25);
                        });

                        copiedFile.FetchAttributes();

                        var docFile = new DocumentFileViewModel()
                        {
                            Identifier = Guid.NewGuid(),
                            Name = copiedFile.Name,
                            Path = copiedFile.Uri.LocalPath,
                            DocumentFolder = SelectedTreeItem,
                            Size = copiedFile.Properties.Length / 1024,
                            Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                            CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId },
                            CreatedAt = copiedFile.Properties.LastModified.Value.DateTime
                        };
                        var response = documentFileService.Create(docFile);
                        if(!response.Success)
                        {
                            IsCopyInProgress = false;
                        }

                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            FilesToUpload.Remove(item);
                        }));

                        if(!IsCopyInProgress)
                        {
                            MainWindow.WarningMessage = "Kopiranje je prekinuto!";
                            break;
                        }
                    }
                    IsCopyInProgress = false;

                    new DocumentFileSQLiteRepository().Sync(documentFileService, ((done, toDo) => {
                        Debug.WriteLine($"Syncing documents: {done} out of {toDo}");
                    }));

                    GetFolderDocuments();
                } catch(Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                } finally
                {
                    IsCopyInProgress = false;
                    LoadingData = false;
                }
            });
            td.IsBackground = true;
            td.Start();
        }

        async Task<string> GetNewFileName(string previousValue)
        {
            var parentWindow = this.TryFindParent<MainWindow>();
            //MetroWindow parentWindow = Window.GetWindow(this) as MetroWindow;
            if (parentWindow != null)
            {
                return await parentWindow.ShowInputAsync("Preimenuj", "Unesite novo ime za fajl",
                    new MetroDialogSettings() 
                    { 
                        DialogResultOnCancel = MessageDialogResult.Canceled, AffirmativeButtonText = "Potvrdi", NegativeButtonText = "Otkaži",
                        DefaultButtonFocus = MessageDialogResult.Affirmative,
                        DefaultText = previousValue
                    });
            }
            return null;
        }

        private async void btnRename_Click(object sender, RoutedEventArgs e)
        {
            DocumentFileViewModel item = SelectedDocumentTreeFile;

            if (item != null)
            {
                var result = await GetNewFileName(item.NewName);

                if (String.IsNullOrEmpty(result))
                    return;

                item = DocumentTreeFiles.FirstOrDefault(x => x.Path == item.Path);
                item.NewName = result;
                
                Thread td = new Thread(() =>
                {
                    try
                    {
                        LoadingData = true;

                        string fileWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(item.Name);
                        string newName = item.NewName + item.Name.Replace(fileWithoutExtension, "");

                        string oldPath = item.Path;
                        item.Path = item.Path?.Replace(item.Name, newName);
                        item.Name = newName;
                        var response = documentFileService.Create(item);
                        if (response.Success)
                        {
                            var file = azureClient.GetFile(oldPath);
                            azureClient.CopyRemote(file, newName);

                            azureClient.Delete(file);

                            new DocumentFileSQLiteRepository().Sync(documentFileService, ((done, toDo) => {
                                Debug.WriteLine($"Syncing documents: {done} out of {toDo}");
                            }));
                        }

                        GetFolderDocuments();
                    }
                    catch (Exception ex)
                    {
                        MainWindow.ErrorMessage = ex.Message;
                    }
                    finally
                    {
                        LoadingData = false;
                    }

                });

                td.IsBackground = true;
                td.Start();
            }
        }

        private void Copy_OnComplete()
        {
            CopyPercentage = 0;
        }

        private void Copy_OnProgressChanged(double Persentage, ref bool Cancel)
        {
            Cancel = !IsCopyInProgress;
            CopyPercentage = Persentage;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() =>
            {
                DocumentFileViewModel item = SelectedDocumentTreeFile;

                if (item != null)
                {
                    try
                    {
                        SelectedDocumentTreeFile = null; LoadingData = true;

                        var file = azureClient.GetFile(item.Path);
                        if(file.Exists())
                        {
                            string copiedFile = azureClient.DownloadFileToOpen(file, (progress, total) => {
                                long percent = (long)(((double)progress / (double)total) * 100);

                                Debug.WriteLine($"{percent}% downloaded");
                                StatusCallback($"Openning documnet: {item.Name}");
                            });

                            if (String.IsNullOrEmpty(copiedFile))
                                return;


                            Debug.WriteLine(item.Path);

                            StatusCallback($"Openned documnet: {item.Name}");

                            //var file = azureClient.GetFile(item.File.Uri.LocalPath);

                            //if (file != null)
                            //{
                            //    MessageBox.Show("Moze fajl!");
                            //}


                            LoadingData = false;
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            Uri pdf = new Uri(copiedFile, UriKind.RelativeOrAbsolute);
                            process.StartInfo.FileName = pdf.LocalPath;

                            process.Start();
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });
            td.IsBackground = true;
            td.Start();
            
        }

        private void BtnCancelCopy_Click(object sender, RoutedEventArgs e)
        {
            IsCopyInProgress = false;
        }

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

        private void btnStopSearch_Click(object sender, RoutedEventArgs e)
        {
            azureClient.CancelOperation = true;
            LoadingData = false;
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            var items = FilesToUpload.ToList();
            foreach (var item in items)
                item.IsSelected = true;

            FilesToUpload = new ObservableCollection<DirectoryTreeItemViewModel>(items);
        }

        private void btnDeselectAll_Click(object sender, RoutedEventArgs e)
        {
            var items = FilesToUpload.ToList();
            foreach (var item in items)
                item.IsSelected = false;

            FilesToUpload = new ObservableCollection<DirectoryTreeItemViewModel>(items);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var itemsToRemove = FilesToUpload.Where(x => x.IsSelected).ToList();

                foreach (var item in itemsToRemove)
                    FilesToUpload.Remove(item);
            } catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        private void btnRecalculateIndexes_Click(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => {
                try
                {
                    LoadingData = true;

                    var clearResponse = documentFolderService.Clear(MainWindow.CurrentCompanyId);

                    if(clearResponse.Success)
                    {

                        var azureClient = new AzureDataClient();
                        var rootFolder = new DocumentFolderViewModel() {
                            Identifier = Guid.NewGuid(),
                            
                            Name = "Documents",
                            Path = azureClient.rootDirectory.Uri.LocalPath,
                            Company = new CompanyViewModel() { Id = MainWindow.CurrentCompanyId },
                            CreatedBy = new UserViewModel() { Id = MainWindow.CurrentUserId }
                        };
                        azureClient.IndexingDirectoryChanged += delegate (string currentPath, int totalIndexed)
                        {
                            CurrentStatus = $"Indexing: {totalIndexed}. {currentPath}";
                        };
                        azureClient.ResetIndexNumber();
                        azureClient.GetDocumentFolders(documentFolderService, documentFileService, rootFolder, true);

                        //DirectoryTreeItemViewModel dirIndex = azureClient.RecalculateIndex();
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.ErrorMessage = ex.Message;
                } finally
                {
                    LoadingData = false;
                }

            });
            td.IsBackground = true;
            td.Start();
        }

        private void btnSelectAllDocuments_Click(object sender, RoutedEventArgs e)
        {
            if(DocumentTreeFiles != null)
            {
                var items = DocumentTreeFiles.ToList();
                foreach (var item in items)
                    item.IsSelected = true;

                DocumentTreeFiles = new ObservableCollection<DocumentFileViewModel>(items);
            }
        }

        private void btnDeselectAllDocuments_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentTreeFiles != null)
            {
                var items = DocumentTreeFiles.ToList();
                foreach (var item in items)
                    item.IsSelected = false;

                DocumentTreeFiles = new ObservableCollection<DocumentFileViewModel>(items);
            }
        }

        private void BtnDeleteFromList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTreeItem != null)
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

                    } catch(Exception ex)
                    {
                        MainWindow.ErrorMessage = ex.Message;
                    }
                });
                td.IsBackground = true;
                td.Start();
            }
        }

        private async void BtnCopyDocuments_Click(object sender, RoutedEventArgs e)
        {
            var selectedDocuments = DocumentTreeFiles.Where(x => x.IsSelected).ToList();

            if(selectedDocuments.Count() < 1)
            {
                MainWindow.ErrorMessage = "Morate označiti dokumente za kopiranje!";
                return;
            }
            try
            {
                await CopyDocuments(selectedDocuments, false);
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        private async void BtnMoveDocuments_List(object sender, RoutedEventArgs e)
        {
            var selectedDocuments = DocumentTreeFiles.Where(x => x.IsSelected).ToList();

            if (selectedDocuments.Count() < 1)
            {
                MainWindow.ErrorMessage = "Morate označiti dokumente za premeštanje!";
                return;
            }
            try
            {
                await CopyDocuments(selectedDocuments, true);
            } catch(Exception ex)
            {
                MainWindow.ErrorMessage = ex.Message;
            }
        }

        async Task CopyDocuments(List<DocumentFileViewModel> documents, bool isMoveInProgress = false)
        {
            var dialog = new DocumentPathDialog();
            bool? selectedResult = dialog.ShowDialog();

            if (selectedResult == true)
            {
                string selectedPath = dialog.SelectedPath;

                var directoryResponse = new DocumentFolderSQLiteRepository().GetDirectoryByPath(MainWindow.CurrentCompanyId, selectedPath);
                if(directoryResponse?.DocumentFolder == null)
                {
                    MainWindow.ErrorMessage = "Došlo je do greške u izboru foldera, molimo ponovite postupak.";
                    return;
                }

                DocumentFolderViewModel selectedFolder = directoryResponse.DocumentFolder;

                var directoryWhereToCopy = azureClient.GetDirectory(selectedPath);
                directoryWhereToCopy.CreateIfNotExists();

                var parentWindow = this.TryFindParent<MetroWindow>();
                if(parentWindow != null)
                {
                    string prefix = isMoveInProgress ? "Premeštanje" : "Kopiranje";
                    string header = $"{prefix} je u toku";
                    string content = isMoveInProgress ? "Premeštaju se odabrane datoteke, molimo sačekajte..." : "Kopiraju se odabrane datoteke, molimo sačekajte...";
                    var progress = await parentWindow.ShowProgressAsync(header, content, true, new MetroDialogSettings()
                    {

                    });
                    progress.SetIndeterminate();

                    try
                    {
                        foreach (var document in documents)
                        {
                            if (progress.IsCanceled)
                            {
                                MainWindow.WarningMessage = "Opracija je prekinuta...";
                                return;
                            }

                            progress.SetMessage($"{prefix} je u toku, trenutna datoteka: {document.Name}");


                            var file = azureClient.GetFile(document.Path);
                            if (!file.Exists())
                            {
                                MainWindow.WarningMessage = $"Datoteka {document.Name} ne postoji na serveru!";
                                continue;
                            }
                            file.FetchAttributes();

                            var newPath = directoryWhereToCopy.GetFileReference(document.Name);
                            if (newPath.Exists())
                            {
                                MainWindow.WarningMessage = $"Datoteka {document.Name} već postoji na odabranoj lokaciji!";
                                continue;
                            }

                            var copyResult = await newPath.StartCopyAsync(file);

                            var newDocument = new DocumentFileViewModel()
                            {
                                Identifier = Guid.NewGuid(),
                                Name = document.Name,
                                Path = newPath.Uri.LocalPath,
                                DocumentFolder = selectedFolder,
                                Size = file.Properties.Length / 1024,
                                CreatedAt = document.CreatedAt,
                                Company = document.Company,
                                CreatedBy = document.CreatedBy
                            };

                            var documentCreateResponse = await Task.FromResult(documentFileService.Create(newDocument));

                            if (documentCreateResponse.Success)
                            {
                                if (!String.IsNullOrEmpty(copyResult))
                                {
                                    if (isMoveInProgress)
                                    {
                                        var response = await Task.FromResult(documentFileService.Delete(document));
                                        if (response.Success)
                                        {
                                            await file.DeleteIfExistsAsync();

                                            DocumentTreeFiles.Remove(document);
                                        }
                                        else
                                            MainWindow.ErrorMessage = response.Message;
                                    }
                                }
                            }
                            else
                                MainWindow.WarningMessage = $"Greška pri kreiranju reda u bazi [{documentCreateResponse.Message}]";
                        }
                        await progress?.CloseAsync();

                    } catch(Exception ex)
                    {
                        await progress?.CloseAsync();
                        MainWindow.ErrorMessage = ex.Message;
                    }
                } else
                {
                    MainWindow.ErrorMessage = "Nije moguće učitati dijalog za kopiranje datoteka!";
                }

                Thread td = new Thread(() => {

                    new DocumentFileSQLiteRepository().Sync(documentFileService, ((done, toDo) => {
                        Debug.WriteLine($"Syncing documents: {done} out of {toDo}");
                    }));
                });
                td.IsBackground = true;
                td.Start();
            }
        }
    }
}
