using DomainCore.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Limitations
{
    public interface ILimitationEmailRepository
    {
        List<LimitationEmail> GetLimitationEmails(int companyId);
        List<LimitationEmail> GetLimitationEmailsNewerThen(int companyId, DateTime lastUpdateTime);

        LimitationEmail Create(LimitationEmail LimitationEmail);
        LimitationEmail Delete(Guid identifier);
    }
}
