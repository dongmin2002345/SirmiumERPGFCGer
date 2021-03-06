﻿using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
    public class PhysicalPersonItemViewModel : BaseEntityViewModel
    {
        #region PhysicalPerson
        private PhysicalPersonViewModel _PhysicalPerson;

        public PhysicalPersonViewModel PhysicalPerson
        {
            get { return _PhysicalPerson; }
            set
            {
                if (_PhysicalPerson != value)
                {
                    _PhysicalPerson = value;
                    NotifyPropertyChanged("PhysicalPerson");
                }
            }
        }
        #endregion

        #region FamilyMember
        private FamilyMemberViewModel _FamilyMember;

        public FamilyMemberViewModel FamilyMember
        {
            get { return _FamilyMember; }
            set
            {
                if (_FamilyMember != value)
                {
                    _FamilyMember = value;
                    NotifyPropertyChanged("FamilyMember");
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

        #region DateOfBirth
        private DateTime? _DateOfBirth = DateTime.Now;

        public DateTime? DateOfBirth
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

        #region EmbassyDate
        private DateTime? _EmpassyDate;

        public DateTime? EmbassyDate
        {
            get { return _EmpassyDate; }
            set
            {
                if (_EmpassyDate != value)
                {
                    _EmpassyDate = value;
                    NotifyPropertyChanged("EmbassyDate");
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
