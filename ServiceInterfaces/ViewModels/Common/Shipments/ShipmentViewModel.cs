using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Prices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Shipments
{
    public class ShipmentViewModel : BaseEntityViewModel
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

        #region ShipmentDate
        private DateTime _ShipmentDate = DateTime.Now;

        public DateTime ShipmentDate
        {
            get { return _ShipmentDate; }
            set
            {
                if (_ShipmentDate != value)
                {
                    _ShipmentDate = value;
                    NotifyPropertyChanged("ShipmentDate");
                }
            }
        }
        #endregion

        #region Address
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    NotifyPropertyChanged("Address");
                }
            }
        }
        #endregion

        #region ServiceDelivery
        private ServiceDeliveryViewModel _ServiceDelivery;

        public ServiceDeliveryViewModel ServiceDelivery
        {
            get { return _ServiceDelivery; }
            set
            {
                if (_ServiceDelivery != value)
                {
                    _ServiceDelivery = value;
                    NotifyPropertyChanged("ServiceDelivery");
                }
            }
        }
        #endregion

        #region ShipmentNumber
        private string _ShipmentNumber;

        public string ShipmentNumber
        {
            get { return _ShipmentNumber; }
            set
            {
                if (_ShipmentNumber != value)
                {
                    _ShipmentNumber = value;
                    NotifyPropertyChanged("ShipmentNumber");
                }
            }
        }
        #endregion

        #region Acceptor
        private string _Acceptor;

        public string Acceptor
        {
            get { return _Acceptor; }
            set
            {
                if (_Acceptor != value)
                {
                    _Acceptor = value;
                    NotifyPropertyChanged("Acceptor");
                }
            }
        }
        #endregion

        #region DeliveryDate
        private DateTime _DeliveryDate = DateTime.Now;

        public DateTime DeliveryDate
        {
            get { return _DeliveryDate; }
            set
            {
                if (_DeliveryDate != value)
                {
                    _DeliveryDate = value;
                    NotifyPropertyChanged("DeliveryDate");
                }
            }
        }
        #endregion

        #region ReturnReceipt
        private string _ReturnReceipt;

        public string ReturnReceipt
        {
            get { return _ReturnReceipt; }
            set
            {
                if (_ReturnReceipt != value)
                {
                    _ReturnReceipt = value;
                    NotifyPropertyChanged("ReturnReceipt");
                }
            }
        }
        #endregion

        #region DocumentName
        private string _DocumentName;

        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                if (_DocumentName != value)
                {
                    _DocumentName = value;
                    NotifyPropertyChanged("DocumentName");
                }
            }
        }
        #endregion

        #region Note
        private string _Note;

        public string Note
        {
            get { return _Note; }
            set
            {
                if (_Note != value)
                {
                    _Note = value;
                    NotifyPropertyChanged("Note");
                }
            }
        }
        #endregion

        #region ShipmentDocuments
        private ObservableCollection<ShipmentDocumentViewModel> _ShipmentDocuments;

        public ObservableCollection<ShipmentDocumentViewModel> ShipmentDocuments
        {
            get { return _ShipmentDocuments; }
            set
            {
                if (_ShipmentDocuments != value)
                {
                    _ShipmentDocuments = value;
                    NotifyPropertyChanged("ShipmentDocuments");
                }
            }
        }
        #endregion

        #region Search

        #region SearchBy_Address
        private string _SearchBy_Address;

        public string SearchBy_Address
        {
            get { return _SearchBy_Address; }
            set
            {
                if (_SearchBy_Address != value)
                {
                    _SearchBy_Address = value;
                    NotifyPropertyChanged("SearchBy_Address");
                }
            }
        }
        #endregion

        #region SearchBy_Acceptor
        private string _SearchBy_Acceptor;

        public string SearchBy_Acceptor
        {
            get { return _SearchBy_Acceptor; }
            set
            {
                if (_SearchBy_Acceptor != value)
                {
                    _SearchBy_Acceptor = value;
                    NotifyPropertyChanged("SearchBy_Acceptor");
                }
            }
        }
        #endregion

        #region SearchBy_ShipmentNumber
        private string _SearchBy_ShipmentNumber;

        public string SearchBy_ShipmentNumber
        {
            get { return _SearchBy_ShipmentNumber; }
            set
            {
                if (_SearchBy_ShipmentNumber != value)
                {
                    _SearchBy_ShipmentNumber = value;
                    NotifyPropertyChanged("SearchBy_ShipmentNumber");
                }
            }
        }
        #endregion

        
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
