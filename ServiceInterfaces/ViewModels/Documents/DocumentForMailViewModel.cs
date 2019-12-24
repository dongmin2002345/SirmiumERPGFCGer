using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Documents
{
    public class DocumentForMailViewModel : BaseEntityViewModel
    {

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

        #region Type
        private string _Type;

        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged("Type");
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

        #region CreateDate
        private DateTime _CreateDate;

        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set
            {
                if (_CreateDate != value)
                {
                    _CreateDate = value;
                    NotifyPropertyChanged("CreateDate");
                }
            }
        }
        #endregion


        #region DocumentFor
        private string _DocumentFor;

        public string DocumentFor
        {
            get { return _DocumentFor; }
            set
            {
                if (_DocumentFor != value)
                {
                    _DocumentFor = value;
                    NotifyPropertyChanged("DocumentFor");
                }
            }
        }
        #endregion


    }
}
