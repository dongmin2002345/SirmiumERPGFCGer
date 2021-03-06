﻿using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.DocumentStores
{
    public class DocumentFileViewModel : BaseEntityViewModel
    {
        // Only for Rename operations
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

                    if(!String.IsNullOrEmpty(_Name))
                        NewName = System.IO.Path.GetFileNameWithoutExtension(_Name);
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

        #region DocumentFolder
        private DocumentFolderViewModel _DocumentFolder;

        public DocumentFolderViewModel DocumentFolder
        {
            get { return _DocumentFolder; }
            set
            {
                if (_DocumentFolder != value)
                {
                    _DocumentFolder = value;
                    NotifyPropertyChanged("DocumentFolder");
                }
            }
        }
        #endregion

        #region Size
        private double _Size;

        public double Size
        {
            get { return _Size; }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }
        #endregion


        #region Search_ParentPath
        private string _Search_ParentPath;

        public string Search_ParentPath
        {
            get { return _Search_ParentPath; }
            set
            {
                if (_Search_ParentPath != value)
                {
                    _Search_ParentPath = value;
                    NotifyPropertyChanged("Search_ParentPath");
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

        #region Search_DateFrom
        private DateTime? _Search_DateFrom;

        public DateTime? Search_DateFrom
        {
            get { return _Search_DateFrom; }
            set
            {
                if (_Search_DateFrom != value)
                {
                    _Search_DateFrom = value;
                    NotifyPropertyChanged("Search_DateFrom");
                }
            }
        }
        #endregion

        #region Search_DateTo
        private DateTime? _Search_DateTo;

        public DateTime? Search_DateTo
        {
            get { return _Search_DateTo; }
            set
            {
                if (_Search_DateTo != value)
                {
                    _Search_DateTo = value;
                    NotifyPropertyChanged("Search_DateTo");
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
