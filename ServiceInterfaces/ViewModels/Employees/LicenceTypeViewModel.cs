﻿using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Employees
{
	public class LicenceTypeViewModel : BaseEntityViewModel, INotifyPropertyChanged
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
