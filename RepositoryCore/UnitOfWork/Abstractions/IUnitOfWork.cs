using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Abstractions.Employees;

namespace RepositoryCore.UnitOfWork.Abstractions
{
    public interface IUnitOfWork //: IDisposable
    {
        ICompanyRepository GetCompanyRepository();

        IAuthenticationRepository GetAuthenticationRepository();
        IUserRepository GetUserRepository();

        IBusinessPartnerRepository GetBusinessPartnerRepository();
        IBusinessPartnerLocationRepository GetBusinessPartnerLocationRepository();
        IBusinessPartnerPhoneRepository GetBusinessPartnerPhoneRepository();
        IBusinessPartnerOrganizationUnitRepository GetBusinessPartnerOrganizationUnitRepository();
        IBusinessPartnerTypeRepository GetBusinessPartnerTypeRepository();
        IBusinessPartnerBusinessPartnerTypeRepository GetBusinessPartnerBusinessPartnerTypeRepository();

        IOutputInvoiceRepository GetOutputInvoiceRepository();

        ICityRepository GetCityRepository();
        IRegionRepository GetRegionRepository();
        IMunicipalityRepository GetMunicipalityRepository();
        ICountryRepository GetCountryRepository();

        ISectorRepository GetSectorRepository();
        IAgencyRepository GetAgencyRepository();

        IProfessionRepository GetProfessionRepository();
		IBankRepository GetBankRepository();
		ILicenceTypeRepository GetLicenceTypeRepository();

        IConstructionSiteRepository GetConstructionSiteRepository();

        IEmployeeRepository GetEmployeeRepository();
        IEmployeeItemRepository GetEmployeeItemRepository();
        IEmployeeLicenceRepository GetEmployeeLicenceRepository();
        IEmployeeProfessionRepository GetEmployeeProfessionRepository();
        IEmployeeByConstructionSiteRepository GetEmployeeByConstructionSiteRepository();
        IEmployeeByConstructionSiteHistoryRepository GetEmployeeByConstructionSiteHistoryRepository();
        IFamilyMemberRepository GetFamilyMemberRepository();

        void Save();
    }
}
