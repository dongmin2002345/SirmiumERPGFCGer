using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.ConstructionSites
{
    public class ConstructionSiteNoteViewModel : BaseEntityViewModel
    {
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
        private DateTime _NoteDate;

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
