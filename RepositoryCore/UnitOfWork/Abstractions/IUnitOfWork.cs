using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Individuals;
using RepositoryCore.Abstractions.Common.Invoices;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Abstractions.Common.Professions;

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

		ISectorRepository GetSectorRepository();


		void Save();
        IProfessionRepository GetProfessionRepository();


        void Save();
    }
}
