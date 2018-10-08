using DomainCore.Common.Professions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Professions
{
    public interface IProfessionRepository
    {
        List<Profession> GetProfessions(int companyId);
        List<Profession> GetProfessionsNewerThen(int companyId, DateTime lastUpdateTime);

        Profession Create(Profession profession);
        Profession Delete(Guid identifier);
    }
}
