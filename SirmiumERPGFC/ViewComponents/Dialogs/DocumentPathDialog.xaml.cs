using MahApps.Metro.Controls;
using Microsoft.Azure.Storage.File;
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

namespace SirmiumERPGFC.ViewComponents.Dialogs
{
    /// <summary>
    /// Interaction logic for DocumentTreeView.xaml
    /// </summary>
    public partial class DocumentPathDialog : MetroWindow, INotifyPropertyChanged
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

                    SelectedPath = _SelectedTreeItem?.FullPath;
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

        AzureDataClient azureClient;
        public DocumentPathDialog()
        {
            InitializeComponent();

            this.DataContext = this;

            azureClient = new AzureDataClient();

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
                folder.Directory = azureClient.rootDirectory;


                GetDirectoryTree(folder);



                DocumentTreeItems = new ObservableCollection<DirectoryTreeItemViewModel>()
                {
                    folder
                };
                SelectedTreeItem = folder;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                LoadingData = false;
            }
        }

        void GetDirectoryTree(DirectoryTreeItemViewModel parent)
        {
            List<CloudFileDirectory> directories;
            try
            {
                directories = azureClient.GetSubDirectories(parent.Directory);


                //var info = new DirectoryInfo(parent.FullPath);
                //directories = info.GetDirectories();
                //parent.IsDirExpanded = true;
            }
            catch (Exception ex)
            {
                directories = new List<CloudFileDirectory>();
            }
            foreach (CloudFileDirectory item in directories)
            {
                if (String.IsNullOrEmpty(item.Name))
                    continue;

                DirectoryTreeItemViewModel directory = new DirectoryTreeItemViewModel();
                directory.FullPath = item.Uri.LocalPath;
                directory.IsDirectory = true;
                directory.Name = item.Name;
                directory.ParentNode = parent;
                directory.Directory = item;
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



        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NewFolderName))
            {
                MessageBox.Show("Naziv foldera ne moze biti prazan!");
                return;
            }

            string basePath = SelectedTreeItem?.FullPath;
            if (String.IsNullOrEmpty(basePath))
            {
                MessageBox.Show("Bazna putanja ne moze biti prazna!");
                return;
            }

            string newPath = System.IO.Path.Combine(basePath, NewFolderName);
            if (Directory.Exists(newPath))
            {
                MessageBox.Show("Folder sa ovim nazivom vec postoji!");
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
            }
            catch (Exception ex)
            {
                MainWindow.ErrorMessage = "Nije moguće kreirati folder, greška na vezi sa mrežnim diskom!";
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
    }
}
