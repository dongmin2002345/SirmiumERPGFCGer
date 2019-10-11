using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerTypeViewModel : BaseEntityViewModel
    {

        #region Code    
        private string _Code;

        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    NotifyPropertyChanged("Code");
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


        #region IsBuyer
        private bool _IsBuyer;

        public bool IsBuyer
        {
            get { return _IsBuyer; }
            set
            {
                if (_IsBuyer != value)
                {
                    _IsBuyer = value;
                    NotifyPropertyChanged("IsBuyer");
                }
            }
        }
        #endregion

        #region IsSupplier
        private bool _IsSupplier;

        public bool IsSupplier
        {
            get { return _IsSupplier; }
            set
            {
                if (_IsSupplier != value)
                {
                    _IsSupplier = value;
                    NotifyPropertyChanged("IsSupplier");
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

        #region ItemStatus
        private int _ItemStatus;

        public int ItemStatus
        {
            get { return _ItemStatus; }
            set
            {
                if (_ItemStatus != value)
                {
                    _ItemStatus = value;
                    NotifyPropertyChanged("ItemStatus");
                }
            }
        }
        #endregion


        #region IsSynced
        private bool _IsSynced;

        public bool IsSynced
        {
            get { return _IsSynced; }
            set
            {
                if (_IsSynced != value)
                {
                    _IsSynced = value;
                    NotifyPropertyChanged("IsSynced");
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

    }
}
