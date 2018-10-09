using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Messages.Banks
{
	public class SyncBankRequest
	{
		public int CompanyId { get; set; }
		public DateTime? LastUpdatedAt { get; set; }
		public List<BankViewModel> UnSyncedBanks { get; set; }
	}
}
