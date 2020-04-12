using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.DocumentStores
{
    public class DocumentFolderViewModel : BaseEntityViewModel
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
                }
            }
        }
        #endregion

        #region Path
        private string _Path;

        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    NotifyPropertyChanged("Path");
                }
            }
        }
        #endregion

        #region ParentFolder
        private DocumentFolderViewModel _ParentFolder;

        public DocumentFolderViewModel ParentFolder
        {
            get { return _ParentFolder; }
            set
            {
                if (_ParentFolder != value)
                {
                    _ParentFolder = value;
                    NotifyPropertyChanged("ParentFolder");
                }
            }
        }
        #endregion

        #region SubDirectories
        private ObservableCollection<DocumentFolderViewModel> _SubDirectories;

        public ObservableCollection<DocumentFolderViewModel> SubDirectories
        {
            get { return _SubDirectories; }
            set
            {
                if (_SubDirectories != value)
                {
                    _SubDirectories = value;
                    NotifyPropertyChanged("SubDirectories");
                }
            }
        }
        #endregion

        #region Search_ParentId
        private int? _Search_ParentId;

        public int? Search_ParentId
        {
            get { return _Search_ParentId; }
            set
            {
                if (_Search_ParentId != value)
                {
                    _Search_ParentId = value;
                    NotifyPropertyChanged("Search_ParentId");
                }
            }
        }
        #endregion


        #region Search_Name
        private string _Search_Name;

        public string Search_Name
        {
            get { return _Search_Name; }
            set
            {
                if (_Search_Name != value)
                {
                    _Search_Name = value;
                    NotifyPropertyChanged("Search_Name");
                }
            }
        }
        #endregion

        #region Search_ShouldLoadSubDirectories
        public bool Search_ShouldLoadSubDirectories
        {
            get { return String.IsNullOrEmpty(Search_Name); }
        }
        #endregion

        #region Search_MultiLevel
        private bool _Search_MultiLevel = true;

        public bool Search_MultiLevel
        {
            get { return _Search_MultiLevel; }
            set
            {
                if (_Search_MultiLevel != value)
                {
                    _Search_MultiLevel = value;
                    NotifyPropertyChanged("Search_MultiLevel");
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

    }
}
