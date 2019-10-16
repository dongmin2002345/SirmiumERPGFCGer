using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Shipments
{
    public class ShipmentDocumentViewModel : BaseEntityViewModel
    {
        #region Shipment
        private ShipmentViewModel _Shipment;

        public ShipmentViewModel Shipment
        {
            get { return _Shipment; }
            set
            {
                if (_Shipment != value)
                {
                    _Shipment = value;
                    NotifyPropertyChanged("Shipment");
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

        #region CreateDate
        private DateTime? _CreateDate = DateTime.Now;

        public DateTime? CreateDate
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
    }
}
