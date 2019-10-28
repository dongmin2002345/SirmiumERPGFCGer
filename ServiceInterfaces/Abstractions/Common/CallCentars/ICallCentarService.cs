using ServiceInterfaces.Messages.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Common.CallCentars
{
    public interface ICallCentarService
    {
        CallCentarListResponse GetCallCentars(int companyId);

        CallCentarResponse Create(CallCentarViewModel callCentar);
        CallCentarResponse Delete(Guid identifier);

        CallCentarListResponse Sync(SyncCallCentarRequest request);
    }
}
