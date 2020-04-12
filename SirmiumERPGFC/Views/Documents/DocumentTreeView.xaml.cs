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

            var response = new DocumentFileSQLiteRepository().GetDocumentFiles(MainWindow.CurrentCompanyId, new DocumentFileViewModel() { Search_ParentPath = SelectedTreeItem?.Path, Search_Name = FilterText });
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
                        if(SelectedTreeItem.SubDirectories == null || SelectedTreeItem.SubDirectories.Count() < 1)
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


        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NewFolderName))
            {
                MainWindow.ErrorMessage = ("Naziv foldera ne moze biti prazan!");
                return;
            }

            string basePath = SelectedTreeItem?.Path;
            if (String.IsNullOrEmpty(basePath))
            {
                MainWindow.ErrorMessage = ("Bazna putanja ne moze biti prazna!");
                return;
            }

            string newPath = System.IO.Path.Combine(basePath, NewFolderName);
            if (Directory.Exists(newPath))
            {
                MainWindow.ErrorMessage = ("Folder sa ovim nazivom vec postoji!");
                return;
            }

            try
            {
                Directory.CreateDirectory(newPath);

                //////DirectoryTreeItemViewModel treeItem = new DirectoryTreeItemViewModel();
                //////treeItem.Name = NewFolderName;
                //////treeItem.IsDirectory = true;
                //////treeItem.FullPath = newPath;
                //////treeItem.IsDirExpanded = true;
                //////treeItem.ParentNode = SelectedTreeItem;

                //////SelectedTreeItem.Items.Add(treeItem);

                //////SelectedTreeItem.IsDirExpanded = true;
            } catch(Exception ex)
            {
                MainWindow.ErrorMessage = "Nije moguće kreirati folder, greška na vezi sa mrežnim diskom!";
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
            DocumentFileViewModel item = SelectedDocumentTreeFile;
            if (item != null)
            {
                try
                {
                    azureClient.Delete(azureClient.GetFile(item.Path));
                    DocumentTreeFiles.Remove(item);
                }
                catch (Exception ex)
                {

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

                        azureClient.CopyLocal(azureClient.GetDirectory(SelectedTreeItem.Path), item.FullPath, (progress, total) =>
                        {
                            long percent = (long)(((double)progress / (double)total) * 100);
                            CopyPercentage = percent;

                            Thread.Sleep(25);
                        });

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


        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            DocumentFileViewModel item = SelectedDocumentTreeFile;

            if (item != null)
            {
                item = DocumentTreeFiles.FirstOrDefault(x => x.Path == item.Path);

                if (String.IsNullOrEmpty(item.NewName))
                {
                    MainWindow.ErrorMessage = ("Nije moguce postaviti prazan naziv dokumenta!");
                    return;
                }

                Thread td = new Thread(() =>
                {
                    try
                    {
                        LoadingData = true;


                        string fileWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(item.Name);
                        string newName = item.NewName + item.Name.Replace(fileWithoutExtension, "");
                        var file = azureClient.GetFile(item.Path);
                        azureClient.CopyRemote(file, newName);

                        azureClient.Delete(file);

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


    }
}
