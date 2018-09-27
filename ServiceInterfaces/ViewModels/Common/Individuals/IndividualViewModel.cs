using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.Individuals
{
    public class IndividualViewModel : BaseEntityViewModel, INotifyPropertyChanged
    {
        #region Code
        private int _Code;

        public int Code
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

        #region SurName
        private string _SurName;

        public string SurName
        {
            get { return _SurName; }
            set
            {
                if (_SurName != value)
                {
                    _SurName = value;
                    NotifyPropertyChanged("SurName");
                }
            }
        }
        #endregion

        #region DateOfBirth
        private DateTime _DateOfBirth = DateTime.Now;

        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    NotifyPropertyChanged("DateOfBirth");
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

        #region Passport
        private string _Passport;

        public string Passport
        {
            get { return _Passport; }
            set
            {
                if (_Passport != value)
                {
                    _Passport = value;
                    NotifyPropertyChanged("Passport");
                }
            }
        }
        #endregion

        #region Interest
        private string _Interest;

        public string Interest
        {
            get { return _Interest; }
            set
            {
                if (_Interest != value)
                {
                    _Interest = value;
                    NotifyPropertyChanged("Interest");
                }
            }
        }
        #endregion

        #region License
        private string _License;

        public string License
        {
            get { return _License; }
            set
            {
                if (_License != value)
                {
                    _License = value;
                    NotifyPropertyChanged("License");
                }
            }
        }
        #endregion

        #region EmbassyDate
        private DateTime _EmbassyDate = DateTime.Now;

        public DateTime EmbassyDate
        {
            get { return _EmbassyDate; }
            set
            {
                if (_EmbassyDate != value)
                {
                    _EmbassyDate = value;
                    NotifyPropertyChanged("EmbassyDate");
                }
            }
        }
        #endregion

        #region VisaFrom
        private DateTime _VisaFrom = DateTime.Now;

        public DateTime VisaFrom
        {
            get { return _VisaFrom; }
            set
            {
                if (_VisaFrom != value)
                {
                    _VisaFrom = value;
                    NotifyPropertyChanged("VisaFrom");
                }
            }
        }
        #endregion

        #region VisaTo
        private DateTime _VisaTo = DateTime.Now;

        public DateTime VisaTo
        {
            get { return _VisaTo; }
            set
            {
                if (_VisaTo != value)
                {
                    _VisaTo = value;
                    NotifyPropertyChanged("VisaTo");
                }
            }
        }
        #endregion

        #region WorkPermitFrom
        private DateTime _WorkPermitFrom = DateTime.Now;

        public DateTime WorkPermitFrom
        {
            get { return _WorkPermitFrom; }
            set
            {
                if (_WorkPermitFrom != value)
                {
                    _WorkPermitFrom = value;
                    NotifyPropertyChanged("WorkPermitFrom");
                }
            }
        }
        #endregion

        #region WorkPermitTo
        private DateTime _WorkPermitTo = DateTime.Now;

        public DateTime WorkPermitTo
        {
            get { return _WorkPermitTo; }
            set
            {
                if (_WorkPermitTo != value)
                {
                    _WorkPermitTo = value;
                    NotifyPropertyChanged("WorkPermitTo");
                }
            }
        }
        #endregion

        #region Family
        private bool _Family;

        public bool Family
        {
            get { return _Family; }
            set
            {
                if (_Family != value)
                {
                    _Family = value;
                    NotifyPropertyChanged("Family");
                }
            }
        }
        #endregion

        #region Search

        #region SearchBy_Code
        private string _SearchBy_Code;

        public string SearchBy_Code
        {
            get { return _SearchBy_Code; }
            set
            {
                if (_SearchBy_Code != value)
                {
                    _SearchBy_Code = value;
                    NotifyPropertyChanged("SearchBy_Code");
                }
            }
        }
        #endregion


        #region SearchBy_Name
        private string _SearchBy_Name;

        public string SearchBy_Name
        {
            get { return _SearchBy_Name; }
            set
            {
                if (_SearchBy_Name != value)
                {
                    _SearchBy_Name = value;
                    NotifyPropertyChanged("SearchBy_Name");
                }
            }
        }
        #endregion

        #region SearchBy_SurName
        private string _SearchBy_SurName;

        public string SearchBy_SurName
        {
            get { return _SearchBy_SurName; }
            set
            {
                if (_SearchBy_SurName != value)
                {
                    _SearchBy_SurName = value;
                    NotifyPropertyChanged("SearchBy_SurName");
                }
            }
        }
        #endregion

        #region SearchBy_Interest
        private string _SearchBy_Interest;

        public string SearchBy_Interest
        {
            get { return _SearchBy_Interest; }
            set
            {
                if (_SearchBy_Interest != value)
                {
                    _SearchBy_Interest = value;
                    NotifyPropertyChanged("SearchBy_Interest");
                }
            }
        }
        #endregion

        #region SearchBy_VisaFrom
        private DateTime? _SearchBy_VisaFrom;

        public DateTime? SearchBy_VisaFrom
        {
            get { return _SearchBy_VisaFrom; }
            set
            {
                if (_SearchBy_VisaFrom != value)
                {
                    _SearchBy_VisaFrom = value;
                    NotifyPropertyChanged("SearchBy_VisaFrom");
                }
            }
        }
        #endregion

        #region SearchBy_VisaTo
        private DateTime? _SearchBy_VisaTo;

        public DateTime? SearchBy_VisaTo
        {
            get { return _SearchBy_VisaTo; }
            set
            {
                if (_SearchBy_VisaTo != value)
                {
                    _SearchBy_VisaTo = value;
                    NotifyPropertyChanged("SearchBy_VisaTo");
                }
            }
        }
        #endregion

        #region SearchBy_PermitFrom
        private DateTime? _SearchBy_PermitFrom;

        public DateTime? SearchBy_PermitFrom
        {
            get { return _SearchBy_PermitFrom; }
            set
            {
                if (_SearchBy_PermitFrom != value)
                {
                    _SearchBy_PermitFrom = value;
                    NotifyPropertyChanged("SearchBy_PermitFrom");
                }
            }
        }
        #endregion

        #region SearchBy_PermitTo
        private DateTime? _SearchBy_PermitTo;

        public DateTime? SearchBy_PermitTo
        {
            get { return _SearchBy_PermitTo; }
            set
            {
                if (_SearchBy_PermitTo != value)
                {
                    _SearchBy_PermitTo = value;
                    NotifyPropertyChanged("SearchBy_PermitTo");
                }
            }
        }
        #endregion


        #endregion

    }
}
