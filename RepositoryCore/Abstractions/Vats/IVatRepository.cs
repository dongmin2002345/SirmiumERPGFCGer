using DomainCore.Vats;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Vats
{
    public interface IVatRepository
    {
        List<Vat> GetVats(int companyId);
        List<Vat> GetVatsNewerThen(int companyId, DateTime lastUpdateTime);
        Vat GetVat(int vatId);

        Vat Create(Vat vat);
        Vat Delete(Guid identifier);
    }
}
