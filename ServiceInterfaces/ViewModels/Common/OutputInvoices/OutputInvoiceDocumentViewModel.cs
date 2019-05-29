using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.OutputInvoices
{
	public class OutputInvoiceDocumentViewModel : BaseEntityViewModel
	{
		#region OutputInvoice
		private OutputInvoiceViewModel _OutputInvoice;

		public OutputInvoiceViewModel OutputInvoice
		{
			get { return _OutputInvoice; }
			set
			{
				if (_OutputInvoice != value)
				{
					_OutputInvoice = value;
					NotifyPropertyChanged("OutputInvoice");
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
