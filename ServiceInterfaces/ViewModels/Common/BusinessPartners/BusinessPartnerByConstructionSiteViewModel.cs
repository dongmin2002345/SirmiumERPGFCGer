﻿using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.ConstructionSites;
using System;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerByConstructionSiteViewModel : BaseEntityViewModel
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


        #region StartDate
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                if (_StartDate != value)
                {
                    _StartDate = value;
                    NotifyPropertyChanged("StartDate");
                }
            }
        }
        #endregion

        #region EndDate
        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                if (_EndDate != value)
                {
                    _EndDate = value;
                    NotifyPropertyChanged("EndDate");
                }
            }
        }
        #endregion

        #region RealEndDate
        private DateTime? _RealEndDate;

        public DateTime? RealEndDate
        {
            get { return _RealEndDate; }
            set
            {
                if (_RealEndDate != value)
                {
                    _RealEndDate = value;
                    NotifyPropertyChanged("RealEndDate");
                }
            }
        }
        #endregion


        #region MaxNumOfEmployees
        private int _MaxNumOfEmployees;

        public int MaxNumOfEmployees
        {
            get { return _MaxNumOfEmployees; }
            set
            {
                if (_MaxNumOfEmployees != value)
                {
                    _MaxNumOfEmployees = value;
                    NotifyPropertyChanged("MaxNumOfEmployees");
                }
            }
        }
        #endregion


        #region BusinessPartner
        private BusinessPartnerViewModel _BusinessPartner;

        public BusinessPartnerViewModel BusinessPartner
        {
            get { return _BusinessPartner; }
            set
            {
                if (_BusinessPartner != value)
                {
                    _BusinessPartner = value;
                    NotifyPropertyChanged("BusinessPartner");
                }
            }
        }
        #endregion

        #region BusinessPartnerCount
        private int _BusinessPartnerCount;

        public int BusinessPartnerCount
        {
            get { return _BusinessPartnerCount; }
            set
            {
                if (_BusinessPartnerCount != value)
                {
                    _BusinessPartnerCount = value;
                    NotifyPropertyChanged("BusinessPartnerCount");
                }
            }
        }
        #endregion


        #region ConstructionSite
        private ConstructionSiteViewModel _ConstructionSite;

        public ConstructionSiteViewModel ConstructionSite
        {
            get { return _ConstructionSite; }
            set
            {
                if (_ConstructionSite != value)
                {
                    _ConstructionSite = value;
                    NotifyPropertyChanged("ConstructionSite");
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


        #region Search_BusinessPartner
        private BusinessPartnerViewModel _Search_BusinessPartner;

        public BusinessPartnerViewModel Search_BusinessPartner
        {
            get { return _Search_BusinessPartner; }
            set
            {
                if (_Search_BusinessPartner != value)
                {
                    _Search_BusinessPartner = value;
                    NotifyPropertyChanged("Search_BusinessPartner");
                }
            }
        }
        #endregion


        #region DeletePopupOpened
        private bool _DeletePopupOpened;

        public bool DeletePopupOpened
        {
            get { return _DeletePopupOpened; }
            set
            {
                if (_DeletePopupOpened != value)
                {
                    _DeletePopupOpened = value;
                    NotifyPropertyChanged("DeletePopupOpened");
                }
            }
        }
        #endregion

    }
}
