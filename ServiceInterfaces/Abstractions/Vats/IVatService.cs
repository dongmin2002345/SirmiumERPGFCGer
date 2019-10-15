using ServiceInterfaces.Messages.Vats;
using ServiceInterfaces.ViewModels.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Abstractions.Vats
{
    public interface IVatService
    {
        VatListResponse GetVats(int companyId);

        VatResponse Create(VatViewModel vat);
        VatResponse Delete(Guid identifier);

        VatListResponse Sync(SyncVatRequest request);
    }
}
