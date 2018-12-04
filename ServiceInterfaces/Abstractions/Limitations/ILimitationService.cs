using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Limitations
{
    public interface ILimitationService
    {
        LimitationListResponse GetLimitations(int companyId);
        LimitationListResponse GetLimitationsNewerThen(int companyId, DateTime? lastUpdateTime);

        LimitationResponse Create(LimitationViewModel Limitation);

        LimitationListResponse Sync(SyncLimitationRequest request);
    }
}
