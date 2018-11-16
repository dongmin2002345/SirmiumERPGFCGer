using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Abstractions.Employees;

namespace RepositoryCore.UnitOfWork.Abstractions
{
    public interface IUnitOfWork //: IDisposable
    {
        ICompanyRepository GetCompanyRepository();

        IAuthenticationRepository GetAuthenticationRepository();
        IUserRepository GetUserRepository();

        IToDoRepository GetToDoRepository();

        IBusinessPartnerRepository GetBusinessPartnerRepository();
        IBusinessPartnerLocationRepository GetBusinessPartnerLocationRepository();
        IBusinessPartnerPhoneRepository GetBusinessPartnerPhoneRepository();
        IBusinessPartnerInstitutionRepository GetBusinessPartnerInstitutionRepository();
        IBusinessPartnerBankRepository GetBusinessPartnerBankRepository();
        IBusinessPartnerOrganizationUnitRepository GetBusinessPartnerOrganizationUnitRepository();
        IBusinessPartnerTypeRepository GetBusinessPartnerTypeRepository();
        IBusinessPartnerBusinessPartnerTypeRepository GetBusinessPartnerBusinessPartnerTypeRepository();

        IOutputInvoiceRepository GetOutputInvoiceRepository();
		IInputInvoiceRepository GetInputInvoiceRepository();

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
        IConstructionSiteCalculationRepository GetConstructionSiteCalculationRepository();

        IEmployeeRepository GetEmployeeRepository();
        IEmployeeItemRepository GetEmployeeItemRepository();
        IEmployeeCardRepository GetEmployeeCardRepository();
        IEmployeeDocumentRepository GetEmployeeDocumentRepository();
        IEmployeeLicenceRepository GetEmployeeLicenceRepository();
        IEmployeeProfessionRepository GetEmployeeProfessionRepository();
        IEmployeeByConstructionSiteRepository GetEmployeeByConstructionSiteRepository();
        IEmployeeByConstructionSiteHistoryRepository GetEmployeeByConstructionSiteHistoryRepository();
        IFamilyMemberRepository GetFamilyMemberRepository();

        IEmployeeByBusinessPartnerRepository GetEmployeeByBusinessPartnerRepository();
        IEmployeeByBusinessPartnerHistoryRepository GetEmployeeByBusinessPartnerHistoryRepository();

        IBusinessPartnerByConstructionSiteRepository GetBusinessPartnerByConstructionSiteRepository();
        IBusinessPartnerByConstructionSiteHistoryRepository GetBusinessPartnerByConstructionSiteHistoryRepository();

        void Save();
    }
}
