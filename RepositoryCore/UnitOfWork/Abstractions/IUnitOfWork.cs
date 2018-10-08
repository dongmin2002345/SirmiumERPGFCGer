using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Individuals;
using RepositoryCore.Abstractions.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.UnitOfWork.Abstractions
{
    public interface IUnitOfWork //: IDisposable
    {
        ICompanyRepository GetCompanyRepository();

        IAuthenticationRepository GetAuthenticationRepository();
        IUserRepository GetUserRepository();

        IBusinessPartnerRepository GetBusinessPartnerRepository();

        IIndividualsRepository GetIndividualsRepository();

        IOutputInvoiceRepository GetOutputInvoiceRepository();

        ICityRepository GetCityRepository();
        IRegionRepository GetRegionRepository();
        IMunicipalityRepository GetMunicipalityRepository();

        void Save();
    }
}
