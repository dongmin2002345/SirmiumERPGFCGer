using ServiceInterfaces.Messages.Common.Professions;
using ServiceInterfaces.ViewModels.Common.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Professions
{
    public interface IProfessionService
    {
        ProfessionListResponse GetProfessions(int companyId);
        ProfessionListResponse GetProfessionsNewerThen(int companyId, DateTime? lastUpdateTime);

        ProfessionResponse Create(ProfessionViewModel City);
        ProfessionResponse Delete(Guid identifier);

        ProfessionListResponse Sync(SyncProfessionRequest request);
    }
}
