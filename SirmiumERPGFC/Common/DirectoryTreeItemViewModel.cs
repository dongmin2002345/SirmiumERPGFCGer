using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmiumERPGFC.Common
{
    public class DirectoryTreeItemViewModel
    {
        #region Name
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged("Name");

                    NewName = _Name;
                }
            }
        }
        #endregion

        #region NewName
        private string _NewName;

        public string NewName
        {
            get { return _NewName; }
            set
            {
                if (_NewName != value)
                {
                    _NewName = value;
                    NotifyPropertyChanged("NewName");
                }
            }
        }
        #endregion


        #region FullPath
        private string _FullPath;

        public string FullPath
        {
            get { return _FullPath; }
            set
            {
                if (_FullPath != value)
                {
                    _FullPath = value;
                    NotifyPropertyChanged("FullPath");
                }
            }
        }
        #endregion

        #region IsDirExpanded
        private bool _IsDirExpanded;

        public bool IsDirExpanded
        {
            get { return _IsDirExpanded; }
            set
            {
                if (_IsDirExpanded != value)
                {
                    _IsDirExpanded = value;
                    NotifyPropertyChanged("IsDirExpanded");
                }
            }
        }
        #endregion

        #region IsDirectory
        private bool _IsDirectory;

        public bool IsDirectory
        {
            get { return _IsDirectory; }
            set
            {
                if (_IsDirectory != value)
                {
                    _IsDirectory = value;
                    NotifyPropertyChanged("IsDirectory");
                }
            }
        }
        #endregion


        #region IsSelected
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        #endregion

        #region ParentNode
        private DirectoryTreeItemViewModel _ParentNode;

        public DirectoryTreeItemViewModel ParentNode
        {
            get { return _ParentNode; }
            set
            {
                if (_ParentNode != value)
                {
                    _ParentNode = value;
                    NotifyPropertyChanged("ParentNode");
                }
            }
        }
        #endregion

        #region FileSize
        private decimal _FileSize;

        public decimal FileSize
        {
            get { return _FileSize; }
            set
            {
                if (_FileSize != value)
                {
                    _FileSize = value;
                    NotifyPropertyChanged("FileSize");
                }
            }
        }
        #endregion

        #region CreatedAt
        private DateTime _CreatedAt;

        public DateTime CreatedAt
        {
            get { return _CreatedAt; }
            set
            {
                if (_CreatedAt != value)
                {
                    _CreatedAt = value;
                    NotifyPropertyChanged("CreatedAt");
                }
            }
        }
        #endregion



        #region Items
        private ObservableCollection<DirectoryTreeItemViewModel> _Items = new ObservableCollection<DirectoryTreeItemViewModel>();

        public ObservableCollection<DirectoryTreeItemViewModel> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }
        #endregion




        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {

            Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
        #endregion
    }
}
