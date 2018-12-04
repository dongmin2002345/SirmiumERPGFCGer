using DomainCore.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Limitations
{
    public interface ILimitationRepository
    {
        List<Limitation> GetLimitations(int companyId);
        List<Limitation> GetLimitationsNewerThen(int companyId, DateTime lastUpdateTime);

        Limitation Create(Limitation limitation);
    }
}
