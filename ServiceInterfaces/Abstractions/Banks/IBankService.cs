using ServiceInterfaces.Messages.Banks;
using ServiceInterfaces.ViewModels.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Banks
{
	public interface IBankService
	{
		BankListResponse GetBanks(int companyId);
		BankListResponse GetBanksNewerThen(int companyId, DateTime? lastUpdateTime);

		BankResponse Create(BankViewModel Bank);
		BankResponse Delete(Guid identifier);

		BankListResponse Sync(SyncBankRequest request);
	}
}
