using DomainCore.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.CallCentars
{
    public interface ICallCentarRepository
    {
        List<CallCentar> GetCallCentars(int companyId);
        List<CallCentar> GetCallCentarsNewerThen(int companyId, DateTime lastUpdateTime);
        CallCentar GetCallCentar(int callCentarId);

        CallCentar Create(CallCentar callCentar);
        CallCentar Delete(Guid identifier);
    }
}
