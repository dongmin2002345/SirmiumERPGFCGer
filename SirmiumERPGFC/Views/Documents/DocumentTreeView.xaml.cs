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



                var folder = new DirectoryTreeItemViewModel()
                {
                    Name = $"Dokumenti",
                    IsDirectory = true,
                    IsDirExpanded = true,
                    FullPath = AppConfigurationHelper.GetConfiguration().DefaultNetworkDirectory
                };

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

                List<DirectoryTreeItemViewModel> itemsToHighlightParents = new List<DirectoryTreeItemViewModel>();


                SearchFilesRecursively(FilterText?.ToLower(), SelectedTreeItem, itemsToHighlightParents, shouldSearchRecursively);

                DocumentTreeFiles = new ObservableCollection<DirectoryTreeItemViewModel>(itemsToHighlightParents.OrderBy(x => x.Name));
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

        void SearchFilesRecursively(string filterString, DirectoryTreeItemViewModel item, List<DirectoryTreeItemViewModel> itemsToHighlightParents, bool shouldRecursivelySearch = false)
        {
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(item.FullPath);
            }
            catch (Exception ex)
            {
                files = new List<string>();
            }
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (NameMatches(filterString, fileInfo.Name))
                {
                    itemsToHighlightParents.Add(new DirectoryTreeItemViewModel()
                    {
                        Name = fileInfo.Name,
                        CreatedAt = fileInfo.LastWriteTime,
                        FileSize = fileInfo.Length / 1024,
                        FullPath = file,
                        IsDirectory = false,
                    });
                }
            }
            if (shouldRecursivelySearch)
            {
                if (item.Items != null)
                {
                    foreach (var dir in item.Items)
                    {
                        SearchFilesRecursively(filterString, dir, itemsToHighlightParents, shouldRecursivelySearch);
                    }
                }
            }
        }

        void GetDirectoryTree(DirectoryTreeItemViewModel parent)
        {
            IEnumerable<string> directories;
            try
            {
                directories = Directory.EnumerateDirectories(parent.FullPath);
            }
            catch (Exception ex)
            {
                directories = new List<string>();
            }
            foreach (var item in directories)
            {
                if (String.IsNullOrEmpty(item))
                    continue;

                var attributes = File.GetAttributes(item);
                if (attributes.HasFlag(FileAttributes.Directory))
                {

                    var directory = new DirectoryTreeItemViewModel()
                    {
                        FullPath = item,
                        IsDirectory = true,
                        Name = System.IO.Path.GetFileName(item)
                    };
                    directory.ParentNode = parent;
                    parent.Items.Add(directory);
                    GetDirectoryTree(directory);
                }
            }
        }


        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedTreeItem = e.NewValue as DirectoryTreeItemViewModel;

            Thread td = new Thread(() => {
                try
                {
                    LoadingData = true;
                    List<DirectoryTreeItemViewModel> treeItems = new List<DirectoryTreeItemViewModel>();
                    SearchFilesRecursively(FilterText?.ToLower(), SelectedTreeItem, treeItems, false);

                    DocumentTreeFiles = new ObservableCollection<DirectoryTreeItemViewModel>(treeItems);
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
                MessageBox.Show("Naziv foldera ne moze biti prazan!");
                return;
            }

            var basePath = SelectedTreeItem?.FullPath;
            if (String.IsNullOrEmpty(basePath))
            {
                MessageBox.Show("Bazna putanja ne moze biti prazna!");
                return;
            }

            var newPath = System.IO.Path.Combine(basePath, NewFolderName);
            if (Directory.Exists(newPath))
            {
                MessageBox.Show("Folder sa ovim nazivom vec postoji!");
                return;
            }

            try
            {
                Directory.CreateDirectory(newPath);

                SelectedTreeItem.Items.Add(new DirectoryTreeItemViewModel()
                {
                    Name = NewFolderName,
                    IsDirectory = true,
                    FullPath = newPath,
                    IsDirExpanded = true,
                    ParentNode = SelectedTreeItem,
                });

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
            Dispatcher.BeginInvoke((Action)(() => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
        #endregion

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = SelectedDocumentTreeFile;
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
            var data = e.Data;
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
            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (!FilesToUpload.Any(y => y.Name == fileInfo.Name))
                    {
                        FilesToUpload.Add(new DirectoryTreeItemViewModel()
                        {
                            Name = fileInfo.Name,
                            FullPath = fileInfo.FullName,
                            FileSize = fileInfo.Length / 1024
                        });
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

            var result = ofd.ShowDialog();
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
                MessageBox.Show("Morate odabrati folder gde ce se smestiti dokumenti!");
                return;
            }


            if (FilesToUpload == null || FilesToUpload.Count() < 1)
            {
                MessageBox.Show("Morate odabrati dokumente za kopiranje!");
                return;
            }

            Thread td = new Thread(() =>
            {
                try
                {
                    LoadingData = true;
                    IsCopyInProgress = true;
                    var filesToUpload = FilesToUpload.ToList();

                    foreach (var item in filesToUpload)
                    {
                        CopyPercentage = 0;
                        var newPath = System.IO.Path.Combine(SelectedTreeItem.FullPath, item.Name);

                        if (newPath == item.FullPath)
                        {
                            MainWindow.ErrorMessage = "Ne mozete kopirati datoteku na njenu izvornu lokaciju!";
                            continue;
                        }

                        if (File.Exists(newPath))
                        {
                            var result = MessageBox.Show($"Vec postoji datoteka {item.Name} na odabranoj lokaciji. Zelite li da je prepisete?", "Duplikat!", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                File.Delete(newPath);

                            }
                            else
                                continue;
                        }

                        string oldPath = item.FullPath;
                        var copy = new CopyFileHelper(item.FullPath, newPath);

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
            var item = SelectedDocumentTreeFile;

            if (item != null)
            {
                item = DocumentTreeFiles.FirstOrDefault(x => x.FullPath == item.FullPath);

                if (String.IsNullOrEmpty(item.NewName))
                {
                    MessageBox.Show("Nije moguce postaviti prazan naziv dokumenta!");
                    return;
                }

                if (File.Exists(item.FullPath))
                {
                    var fileInfo = new FileInfo(item.FullPath);

                    if (fileInfo != null)
                    {
                        var fileWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(item.Name);
                        var newName = item.NewName + item.Name.Replace(fileWithoutExtension, "");
                        var newPath = System.IO.Path.Combine(fileInfo.Directory.FullName, newName);

                        if (File.Exists(newPath))
                        {
                            MessageBox.Show($"Dokument sa unetim nazivom \"{newName}\" vec postoji!");
                            return;
                        }

                        Thread td = new Thread(() =>
                        {
                            try
                            {
                                LoadingData = true;
                                string oldPath = item.FullPath;
                                var copy = new CopyFileHelper(item.FullPath, newPath);

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
            var item = SelectedDocumentTreeFile;

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
    }
}
