using ServiceInterfaces.Messages.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Limitations
{
    public interface ILimitationEmailService
    {
        LimitationEmailListResponse GetLimitationEmails(int companyId);
        LimitationEmailListResponse GetLimitationEmailsNewerThen(int companyId, DateTime? lastUpdateTime);

        LimitationEmailResponse Create(LimitationEmailViewModel LimitationEmail);
        LimitationEmailResponse Delete(Guid identifier);

        LimitationEmailListResponse Sync(SyncLimitationEmailRequest request);
    }
}
