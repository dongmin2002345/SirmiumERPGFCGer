using DomainCore.Banks;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Banks
{
    public interface IBankRepository
    {
		List<Bank> GetBanks(int companyId);
		List<Bank> GetBanksNewerThen(int companyId, DateTime lastUpdateTime);

		Bank Create(Bank bank);
		Bank Delete(Guid identifier);
	}
}
