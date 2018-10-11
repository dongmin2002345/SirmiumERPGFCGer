using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.Locations;
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

		#region Category
		private string _Category;

		public string Category
		{
			get { return _Category; }
			set
			{
				if (_Category != value)
				{
					_Category = value;
					NotifyPropertyChanged("Category");
				}
			}
		}
		#endregion

		#region Description
		private string _Description;

		public string Description
		{
			get { return _Description; }
			set
			{
				if (_Description != value)
				{
					_Description = value;
					NotifyPropertyChanged("Description");
				}
			}
		}
		#endregion

		#region Country
		private CountryViewModel _Country;

		public CountryViewModel Country
		{
			get { return _Country; }
			set
			{
				if (_Country != value)
				{
					_Country = value;
					NotifyPropertyChanged("Country");
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

		#region Search_Category
		private string _Search_Category;

		public string Search_Category
		{
			get { return _Search_Category; }
			set
			{
				if (_Search_Category != value)
				{
					_Search_Category = value;
					NotifyPropertyChanged("Search_Category");
				}
			}
		}
		#endregion

		#region Search_Country
		private string _Search_Country;

		public string Search_Country
		{
			get { return _Search_Country; }
			set
			{
				if (_Search_Country != value)
				{
					_Search_Country = value;
					NotifyPropertyChanged("Search_Country");
				}
			}
		}
		#endregion
	}
}
