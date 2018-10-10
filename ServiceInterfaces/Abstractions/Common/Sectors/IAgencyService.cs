using ServiceInterfaces.Messages.Common.Sectors;
using ServiceInterfaces.ViewModels.Common.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Sectors
{
    public interface IAgencyService
    {
        AgencyListResponse GetAgencies(int companyId);
        AgencyListResponse GetAgenciesNewerThen(int companyId, DateTime? lastUpdateTime);

        AgencyResponse Create(AgencyViewModel agency);
        AgencyResponse Delete(Guid identifier);

        AgencyListResponse Sync(SyncAgencyRequest request);
    }
}
