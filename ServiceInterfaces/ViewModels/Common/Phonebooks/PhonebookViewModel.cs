using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Phonebooks
{
    public class PhonebookViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region Municipality
        private MunicipalityViewModel _Municipality;

        public MunicipalityViewModel Municipality
        {
            get { return _Municipality; }
            set
            {
                if (_Municipality != value)
                {
                    _Municipality = value;
                    NotifyPropertyChanged("Municipality");
                }
            }
        }
        #endregion

        #region City
        private CityViewModel _City;

        public CityViewModel City
        {
            get { return _City; }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    NotifyPropertyChanged("City");
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

        #region PhonebookDocuments
        private ObservableCollection<PhonebookDocumentViewModel> _PhonebookDocuments;

        public ObservableCollection<PhonebookDocumentViewModel> PhonebookDocuments
        {
            get { return _PhonebookDocuments; }
            set
            {
                if (_PhonebookDocuments != value)
                {
                    _PhonebookDocuments = value;
                    NotifyPropertyChanged("PhonebookDocuments");
                }
            }
        }
        #endregion

        #region PhonebookNotes
        private ObservableCollection<PhonebookNoteViewModel> _PhonebookNotes;

        public ObservableCollection<PhonebookNoteViewModel> PhonebookNotes
        {
            get { return _PhonebookNotes; }
            set
            {
                if (_PhonebookNotes != value)
                {
                    _PhonebookNotes = value;
                    NotifyPropertyChanged("PhonebookNotes");
                }
            }
        }
        #endregion

        #region PhonebookPhones
        private ObservableCollection<PhonebookPhoneViewModel> _PhonebookPhones;

        public ObservableCollection<PhonebookPhoneViewModel> PhonebookPhones
        {
            get { return _PhonebookPhones; }
            set
            {
                if (_PhonebookPhones != value)
                {
                    _PhonebookPhones = value;
                    NotifyPropertyChanged("PhonebookPhones");
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
