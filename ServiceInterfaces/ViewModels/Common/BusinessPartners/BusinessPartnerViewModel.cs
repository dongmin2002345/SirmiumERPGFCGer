using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.BusinessPartners
{
    public class BusinessPartnerViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

        #region Director
        private string _Director;

        public string Director
        {
            get { return _Director; }
            set
            {
                if (_Director != value)
                {
                    _Director = value;
                    NotifyPropertyChanged("Director");
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

        #region InoAddress
        private string _InoAddress;

        public string InoAddress
        {
            get { return _InoAddress; }
            set
            {
                if (_InoAddress != value)
                {
                    _InoAddress = value;
                    NotifyPropertyChanged("InoAddress");
                }
            }
        }
        #endregion

        #region PIB
        private string _PIB;

        public string PIB
        {
            get { return _PIB; }
            set
            {
                if (_PIB != value)
                {
                    _PIB = value;
                    NotifyPropertyChanged("PIB");
                }
            }
        }
        #endregion

        #region MatCode
        private string _MatCode;

        public string MatCode
        {
            get { return _MatCode; }
            set
            {
                if (_MatCode != value)
                {
                    _MatCode = value;
                    NotifyPropertyChanged("MatCode");
                }
            }
        }
        #endregion

        #region Mobile
        private string _Mobile;

        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                if (_Mobile != value)
                {
                    _Mobile = value;
                    NotifyPropertyChanged("Mobile");
                }
            }
        }
        #endregion

        #region Phone
        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (_Phone != value)
                {
                    _Phone = value;
                    NotifyPropertyChanged("Phone");
                }
            }
        }
        #endregion

        #region Email
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }
        #endregion

        #region ActivityCode
        private string _ActivityCode;

        public string ActivityCode
        {
            get { return _ActivityCode; }
            set
            {
                if (_ActivityCode != value)
                {
                    _ActivityCode = value;
                    NotifyPropertyChanged("ActivityCode");
                }
            }
        }
        #endregion

        #region BankAccountNumber
        private string _BankAccountNumber;

        public string BankAccountNumber
        {
            get { return _BankAccountNumber; }
            set
            {
                if (_BankAccountNumber != value)
                {
                    _BankAccountNumber = value;
                    NotifyPropertyChanged("BankAccountNumber");
                }
            }
        }
        #endregion

        #region OpeningDate
        private DateTime _OpeningDate = DateTime.Now;

        public DateTime OpeningDate
        {
            get { return _OpeningDate; }
            set
            {
                if (_OpeningDate != value)
                {
                    _OpeningDate = value;
                    NotifyPropertyChanged("OpeningDate");
                }
            }
        }
        #endregion

        #region BranchOpeningDate
        private DateTime? _BranchOpeningDate;

        public DateTime? BranchOpeningDate
        {
            get { return _BranchOpeningDate; }
            set
            {
                if (_BranchOpeningDate != value)
                {
                    _BranchOpeningDate = value;
                    NotifyPropertyChanged("BranchOpeningDate");
                }
            }
        }
        #endregion

        #region Search

        #region SearchBy_BusinessPartnerName
        private string _SearchBy_BusinessPartnerName;

        public string SearchBy_BusinessPartnerName
        {
            get { return _SearchBy_BusinessPartnerName; }
            set
            {
                if (_SearchBy_BusinessPartnerName != value)
                {
                    _SearchBy_BusinessPartnerName = value;
                    NotifyPropertyChanged("SearchBy_BusinessPartnerName");
                }
            }
        }
        #endregion


        #endregion
    }
}
