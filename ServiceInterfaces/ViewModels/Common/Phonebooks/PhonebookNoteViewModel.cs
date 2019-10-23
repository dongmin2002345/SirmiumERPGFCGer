using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ServiceInterfaces.ViewModels.Common.Phonebooks
{
    public class PhonebookNoteViewModel : BaseEntityViewModel, INotifyPropertyChanged
    {
        #region Phonebook
        private PhonebookViewModel _Phonebook;

        public PhonebookViewModel Phonebook
        {
            get { return _Phonebook; }
            set
            {
                if (_Phonebook != value)
                {
                    _Phonebook = value;
                    NotifyPropertyChanged("Phonebook");
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

        #region NoteDate
        private DateTime _NoteDate = DateTime.Now;

        public DateTime NoteDate
        {
            get { return _NoteDate; }
            set
            {
                if (_NoteDate != value)
                {
                    _NoteDate = value;
                    NotifyPropertyChanged("NoteDate");
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
