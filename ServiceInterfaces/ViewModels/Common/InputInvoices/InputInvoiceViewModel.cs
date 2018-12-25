using ServiceInterfaces.ViewModels.Base;
using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.ViewModels.Common.InputInvoices
{
	public class InputInvoiceViewModel : BaseEntityViewModel, INotifyPropertyChanged
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

		#region BusinessPartner
		private BusinessPartnerViewModel _BusinessPartner;

		public BusinessPartnerViewModel BusinessPartner
		{
			get { return _BusinessPartner; }
			set
			{
				if (_BusinessPartner != value)
				{
					_BusinessPartner = value;
					NotifyPropertyChanged("BusinessPartner");
				}
			}
		}
		#endregion

		#region Supplier
		private string _Supplier;

		public string Supplier
		{
			get { return _Supplier; }
			set
			{
				if (_Supplier != value)
				{
					_Supplier = value;
					NotifyPropertyChanged("Supplier");
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

		#region InvoiceNumber
		private string _InvoiceNumber;

		public string InvoiceNumber
		{
			get { return _InvoiceNumber; }
			set
			{
				if (_InvoiceNumber != value)
				{
					_InvoiceNumber = value;
					NotifyPropertyChanged("InvoiceNumber");
				}
			}
		}
		#endregion

		#region InvoiceDate
		private DateTime _InvoiceDate = DateTime.Now;

		public DateTime InvoiceDate
		{
			get { return _InvoiceDate; }
			set
			{
				if (_InvoiceDate != value)
				{
					_InvoiceDate = value;
					NotifyPropertyChanged("InvoiceDate");
				}
			}
		}
		#endregion

		#region AmountNet
		private decimal _AmountNet;

		public decimal AmountNet
		{
			get { return _AmountNet; }
			set
			{
				if (_AmountNet != value)
				{
					_AmountNet = value;
					NotifyPropertyChanged("AmountNet");
                    PDV = (AmountNet * PDVPercent) / 100;
                    AmountGross = AmountNet + PDV;
				}
			}
		}
		#endregion

		#region PDVPercent
		private int _PDVPercent;

		public int PDVPercent
		{
			get { return _PDVPercent; }
			set
			{
				if (_PDVPercent != value)
				{
					_PDVPercent = value;
					NotifyPropertyChanged("PDVPercent");
					PDV = (AmountNet * PDVPercent) / 100;
				}
			}
		}
		#endregion

		#region PDV
		private decimal _PDV;

		public decimal PDV
		{
			get { return _PDV; }
			set
			{
				if (_PDV != value)
				{
					_PDV = value;
					NotifyPropertyChanged("PDV");
                    AmountGross = AmountNet + PDV;
				}
			}
		}
		#endregion

		#region AmountGross
		private decimal _AmountGross;

		public decimal AmountGross
		{
			get { return _AmountGross; }
			set
			{
				if (_AmountGross != value)
				{
					_AmountGross = value;
					NotifyPropertyChanged("AmountGross");
				}
			}
		}
		#endregion

		#region Currency
		private int _Currency;

		public int Currency
		{
			get { return _Currency; }
			set
			{
				if (_Currency != value)
				{
					_Currency = value;
					NotifyPropertyChanged("Currency");
					DateOfPaymet = InvoiceDate.AddDays(Currency);
				}
			}
		}
		#endregion

		#region DateOfPaymet
		private DateTime _DateOfPaymet = DateTime.Now;

		public DateTime DateOfPaymet
		{
			get { return _DateOfPaymet; }
			set
			{
				if (_DateOfPaymet != value)
				{
					_DateOfPaymet = value;
					NotifyPropertyChanged("DateOfPaymet");
				}
			}
		}
		#endregion

		#region Status
		private string _Status;

		public string Status
		{
			get { return _Status; }
			set
			{
				if (_Status != value)
				{
					_Status = value;
					NotifyPropertyChanged("Status");
				}
			}
		}
		#endregion

		#region StatusDate
		private DateTime _StatusDate = DateTime.Now;

		public DateTime StatusDate
		{
			get { return _StatusDate; }
			set
			{
				if (_StatusDate != value)
				{
					_StatusDate = value;
					NotifyPropertyChanged("StatusDate");
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

		#region Rebate
		private decimal _Rebate;

		public decimal Rebate
		{
			get { return _Rebate; }
			set
			{
				if (_Rebate != value)
				{
					_Rebate = value;
					NotifyPropertyChanged("Rebate");
				}
			}
		}
		#endregion

		#region RebateValue
		private decimal _RebateValue;

		public decimal RebateValue
		{
			get { return _RebateValue; }
			set
			{
				if (_RebateValue != value)
				{
					_RebateValue = value;
					NotifyPropertyChanged("RebateValue");
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

		#region SearchBy_Supplier
		private string _SearchBy_Supplier;

		public string SearchBy_Supplier
		{
			get { return _SearchBy_Supplier; }
			set
			{
				if (_SearchBy_Supplier != value)
				{
					_SearchBy_Supplier = value;
					NotifyPropertyChanged("SearchBy_Supplier");
				}
			}
		}
		#endregion

		#region SearchBy_InvoiceNumber
		private string _SearchBy_InvoiceNumber;

		public string SearchBy_InvoiceNumber
		{
			get { return _SearchBy_InvoiceNumber; }
			set
			{
				if (_SearchBy_InvoiceNumber != value)
				{
					_SearchBy_InvoiceNumber = value;
					NotifyPropertyChanged("SearchBy_InvoiceNumber");
				}
			}
		}
		#endregion

		#region SearchBy_InvoiceDateFrom
		private DateTime? _SearchBy_InvoiceDateFrom;

		public DateTime? SearchBy_InvoiceDateFrom
		{
			get { return _SearchBy_InvoiceDateFrom; }
			set
			{
				if (_SearchBy_InvoiceDateFrom != value)
				{
					_SearchBy_InvoiceDateFrom = value;
					NotifyPropertyChanged("SearchBy_InvoiceDateFrom");
				}
			}
		}
		#endregion

		#region SearchBy_InvoiceDateTo
		private DateTime? _SearchBy_InvoiceDateTo;

		public DateTime? SearchBy_InvoiceDateTo
		{
			get { return _SearchBy_InvoiceDateTo; }
			set
			{
				if (_SearchBy_InvoiceDateTo != value)
				{
					_SearchBy_InvoiceDateTo = value;
					NotifyPropertyChanged("SearchBy_InvoiceDateTo");
				}
			}
		}
		#endregion

		#region SearchBy_DateOfPaymetFrom
		private DateTime? _SearchBy_DateOfPaymetFrom;

		public DateTime? SearchBy_DateOfPaymetFrom
		{
			get { return _SearchBy_DateOfPaymetFrom; }
			set
			{
				if (_SearchBy_DateOfPaymetFrom != value)
				{
					_SearchBy_DateOfPaymetFrom = value;
					NotifyPropertyChanged("SearchBy_DateOfPaymetFrom");
				}
			}
		}
		#endregion

		#region SearchBy_DateOfPaymetTo
		private DateTime? _SearchBy_DateOfPaymetTo;

		public DateTime? SearchBy_DateOfPaymetTo
		{
			get { return _SearchBy_DateOfPaymetTo; }
			set
			{
				if (_SearchBy_DateOfPaymetTo != value)
				{
					_SearchBy_DateOfPaymetTo = value;
					NotifyPropertyChanged("SearchBy_DateOfPaymetTo");
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
