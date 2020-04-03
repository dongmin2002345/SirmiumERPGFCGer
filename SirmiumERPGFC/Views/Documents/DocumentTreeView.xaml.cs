using Microsoft.Win32;
using SirmiumERPGFC.Common;
using SirmiumERPGFC.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private ObservableCollection<DirectoryTreeItemViewModel> _DocumentTreeItems;

        public ObservableCollection<DirectoryTreeItemViewModel> DocumentTreeItems
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
        private ObservableCollection<DirectoryTreeItemViewModel> _DocumentTreeFiles;

        public ObservableCollection<DirectoryTreeItemViewModel> DocumentTreeFiles
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
        private DirectoryTreeItemViewModel _SelectedDocumentTreeFile;

        public DirectoryTreeItemViewModel SelectedDocumentTreeFile
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
        private DirectoryTreeItemViewModel _SelectedDocument;

        public DirectoryTreeItemViewModel SelectedDocument
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

                    Thread td = new Thread(() => DoFilteringOfData(!String.IsNullOrEmpty(FilterText)));
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
        private DirectoryTreeItemViewModel _SelectedTreeItem;

        public DirectoryTreeItemViewModel SelectedTreeItem
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



        public DocumentTreeView()
        {
            InitializeComponent();

            this.DataContext = this;


            Thread td = new Thread(() => DisplayFolderTree());
            td.IsBackground = true;
            td.Start();
        }

        private void DisplayFolderTree()
        {
            try
            {
                LoadingData = true;



                DirectoryTreeItemViewModel folder = new DirectoryTreeItemViewModel();
                folder.Name = "Dokumenti";
                folder.IsDirectory = true;
                folder.IsDirExpanded = true;
                folder.FullPath = AppConfigurationHelper.GetConfiguration()?.AzureNetworkDrive?.DriveLetter + "\\";

                GetDirectoryTree(folder);


                DocumentTreeItems = new ObservableCollection<DirectoryTreeItemViewModel>()
                {
                    folder
                };
                SelectedTreeItem = folder;

                DoFilteringOfData(false);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                LoadingData = false;
            }
        }

        private void DoFilteringOfData(bool shouldSearchRecursively = false)
        {
            try
            {
                if (SelectedTreeItem == null)
                    return;

                LoadingData = true;


                DocumentTreeFiles = new ObservableCollection<DirectoryTreeItemViewModel>();
                SearchFilesRecursively(FilterText?.ToLower(), SelectedTreeItem, DocumentTreeFiles, shouldSearchRecursively);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                LoadingData = false;
            }
        }

        bool NameMatches(string filter, string toFilter) => (String.IsNullOrEmpty(filter) || toFilter.ToLower().Contains(filter));

        void SearchFilesRecursively(string filterString, DirectoryTreeItemViewModel item, ObservableCollection<DirectoryTreeItemViewModel> itemsToHighlightParents, bool shouldRecursivelySearch = false)
        {
            FileInfo[] files;
            DirectoryInfo directoryInfo = null;
            try
            {
                directoryInfo = new DirectoryInfo(item.FullPath);
                files = directoryInfo.GetFiles();
            }
            catch (Exception ex)
            {
                files = new FileInfo[0];
            }
            foreach (var fileInfo in files)
            {
                if (NameMatches(filterString, fileInfo.Name))
                {
                    DirectoryTreeItemViewModel itm = new DirectoryTreeItemViewModel();
                    itm.Name = fileInfo.Name;
                    itm.CreatedAt = fileInfo.LastWriteTime; 
                    itm.FileSize = fileInfo.Length / 1024;
                    itm.FullPath = fileInfo.FullName;
                    itm.IsDirectory = false;

                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        itemsToHighlightParents.Add(itm);
                    }));
                }
                if (!LoadingData) break;
            }
            if (shouldRecursivelySearch)
            {
                if(directoryInfo != null)
                {
                    DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
                    if (subDirectories != null)
                    {
                        foreach (DirectoryInfo dir in subDirectories)
                        {
                            DirectoryTreeItemViewModel subDir = new DirectoryTreeItemViewModel();
                            subDir.Name = dir.Name;
                            subDir.FullPath = dir.FullName;
                            subDir.IsDirectory = true;
                            subDir.IsDirExpanded = true;
                            if(item.Items != null)
                                if (!item.Items.Any(x => x.FullPath == subDir.FullPath))
                                    item.Items.Add(subDir);

                            if (!LoadingData) break;
                            
                            SearchFilesRecursively(filterString, subDir, itemsToHighlightParents, shouldRecursivelySearch);
                        }
                    }
                }
            }
        }

        void GetDirectoryTree(DirectoryTreeItemViewModel parent)
        {
            DirectoryInfo[] directories;
            try
            {
                var info = new DirectoryInfo(parent.FullPath);
                directories = info.GetDirectories();
                parent.IsDirExpanded = true;
            }
            catch (Exception ex)
            {
                directories = new DirectoryInfo[0];
            }
            foreach (DirectoryInfo item in directories)
            {
                if (String.IsNullOrEmpty(item.FullName))
                    continue;

                DirectoryTreeItemViewModel directory = new DirectoryTreeItemViewModel();
                directory.FullPath = item.FullName;
                directory.IsDirectory = true;
                directory.Name = item.Name;
                directory.ParentNode = parent;
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (!parent.Items.Any(x => x.FullPath == directory.FullPath))
                        parent.Items.Add(directory);
                }));
                //GetDirectoryTree(directory);
            }
        }


        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedTreeItem = e.NewValue as DirectoryTreeItemViewModel;
            SelectedTreeItem.IsDirExpanded = true;
            Thread td = new Thread(() => {
                try
                {
                    LoadingData = true;

                    GetDirectoryTree(SelectedTreeItem);

                    DocumentTreeFiles = new ObservableCollection<DirectoryTreeItemViewModel>();
                    SearchFilesRecursively(FilterText?.ToLower(), SelectedTreeItem, DocumentTreeFiles, false);

                    Dispatcher.BeginInvoke((Action)(() => {
                        SelectedTreeItem.IsDirExpanded = true;
                    }));
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

            string basePath = SelectedTreeItem?.FullPath;
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

                DirectoryTreeItemViewModel treeItem = new DirectoryTreeItemViewModel();
                treeItem.Name = NewFolderName;
                treeItem.IsDirectory = true;
                treeItem.FullPath = newPath;
                treeItem.IsDirExpanded = true;
                treeItem.ParentNode = SelectedTreeItem;

                SelectedTreeItem.Items.Add(treeItem);

                SelectedTreeItem.IsDirExpanded = true;
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
            DirectoryTreeItemViewModel item = SelectedDocumentTreeFile;
            if (item != null)
            {
                try
                {
                    File.Delete(item.FullPath);
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
                        string newPath = System.IO.Path.Combine(SelectedTreeItem.FullPath, item.Name);

                        if (newPath == item.FullPath)
                        {
                            MainWindow.ErrorMessage = "Ne mozete kopirati datoteku na njenu izvornu lokaciju!";
                            continue;
                        }

                        if (File.Exists(newPath))
                        {
                            MessageBoxResult result = MessageBox.Show($"Vec postoji datoteka {item.Name} na odabranoj lokaciji. Zelite li da je prepisete?", "Duplikat!", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(newPath);

                            }
                            else
                                continue;
                        }

                        string oldPath = item.FullPath;
                        CopyFileHelper copy = new CopyFileHelper(item.FullPath, newPath);

                        copy.OnProgressChanged += Copy_OnProgressChanged;
                        copy.OnComplete += Copy_OnComplete;

                        copy.Copy();
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
                    DoFilteringOfData(!String.IsNullOrEmpty(FilterText));
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
            DirectoryTreeItemViewModel item = SelectedDocumentTreeFile;

            if (item != null)
            {
                item = DocumentTreeFiles.FirstOrDefault(x => x.FullPath == item.FullPath);

                if (String.IsNullOrEmpty(item.NewName))
                {
                    MainWindow.ErrorMessage = ("Nije moguce postaviti prazan naziv dokumenta!");
                    return;
                }

                if (File.Exists(item.FullPath))
                {
                    FileInfo fileInfo = new FileInfo(item.FullPath);

                    if (fileInfo != null)
                    {
                        string fileWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(item.Name);
                        string newName = item.NewName + item.Name.Replace(fileWithoutExtension, "");
                        string newPath = System.IO.Path.Combine(fileInfo.Directory.FullName, newName);

                        if (File.Exists(newPath))
                        {
                            MainWindow.ErrorMessage = ($"Dokument sa unetim nazivom \"{newName}\" vec postoji!");
                            return;
                        }

                        Thread td = new Thread(() =>
                        {
                            try
                            {
                                LoadingData = true;
                                string oldPath = item.FullPath;
                                CopyFileHelper copy = new CopyFileHelper(item.FullPath, newPath);

                                copy.OnProgressChanged += Copy_OnProgressChanged;
                                copy.OnComplete += Copy_OnComplete;

                                copy.Copy();

                                item.Name = item.NewName;

                                item.FullPath = newPath;

                                File.Delete(oldPath);

                                DoFilteringOfData(!String.IsNullOrEmpty(FilterText));
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
            DirectoryTreeItemViewModel item = SelectedDocumentTreeFile;

            if (item != null)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    Uri pdf = new Uri(item.FullPath, UriKind.RelativeOrAbsolute);
                    process.StartInfo.FileName = pdf.LocalPath;
                    process.Start();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Could not open the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
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
    }
}
