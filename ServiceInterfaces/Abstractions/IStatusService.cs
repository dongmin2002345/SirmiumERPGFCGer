using ServiceInterfaces.Messages.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions
{
    public interface IStatusService
    {
        StatusListResponse GetStatuses(int companyId);

        StatusResponse Create(StatusViewModel vat);
        StatusResponse Delete(Guid identifier);

        StatusListResponse Sync(SyncStatusRequest request);
    }
}
