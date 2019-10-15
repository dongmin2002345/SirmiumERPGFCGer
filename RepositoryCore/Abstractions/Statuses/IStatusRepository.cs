using DomainCore.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Statuses
{
    public interface IStatusRepository
    {
        List<Status> GetStatuses(int companyId);
        List<Status> GetStatusesNewerThen(int companyId, DateTime lastUpdateTime);
        Status GetStatus(int statusId);

        Status Create(Status status);
        Status Delete(Guid identifier);
    }
}
