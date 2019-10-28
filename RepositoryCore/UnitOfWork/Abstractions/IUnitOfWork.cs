using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.CallCentars;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Phonebooks;
using RepositoryCore.Abstractions.Common.Prices;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Abstractions.Common.Shipments;
using RepositoryCore.Abstractions.Common.TaxAdministrations;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Abstractions.Limitations;
using RepositoryCore.Abstractions.Statuses;
using RepositoryCore.Abstractions.Vats;

namespace RepositoryCore.UnitOfWork.Abstractions
{
    public interface IUnitOfWork //: IDisposable
    {
        ICompanyRepository GetCompanyRepository();
        ICompanyUserRepository GetCompanyUserRepository();

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
        IBusinessPartnerDocumentRepository GetBusinessPartnerDocumentRepository();
        IBusinessPartnerNoteRepository GetBusinessPartnerNoteRepository();

        IOutputInvoiceRepository GetOutputInvoiceRepository();
        IOutputInvoiceNoteRepository GetOutputInvoiceNoteRepository();
		IOutputInvoiceDocumentRepository GetOutputInvoiceDocumentRepository();
		IInputInvoiceRepository GetInputInvoiceRepository();
        IInputInvoiceNoteRepository GetInputInvoiceNoteRepository();
		IInputInvoiceDocumentRepository GetInputInvoiceDocumentRepository();

		ICityRepository GetCityRepository();
        IRegionRepository GetRegionRepository();
        IMunicipalityRepository GetMunicipalityRepository();
        ICountryRepository GetCountryRepository();

        ISectorRepository GetSectorRepository();
        IAgencyRepository GetAgencyRepository();

        IProfessionRepository GetProfessionRepository();
		IBankRepository GetBankRepository();
		ILicenceTypeRepository GetLicenceTypeRepository();
        ILimitationRepository GetLimitationRepository();
        ILimitationEmailRepository GetLimitationEmailRepository();

        IConstructionSiteRepository GetConstructionSiteRepository();
        IConstructionSiteCalculationRepository GetConstructionSiteCalculationRepository();
        IConstructionSiteDocumentRepository GetConstructionSiteDocumentRepository();
        IConstructionSiteNoteRepository GetConstructionSiteNoteRepository();

        IEmployeeRepository GetEmployeeRepository();
        IEmployeeItemRepository GetEmployeeItemRepository();
        IEmployeeNoteRepository GetEmployeeNoteRepository();
        IEmployeeCardRepository GetEmployeeCardRepository();
        IEmployeeDocumentRepository GetEmployeeDocumentRepository();
        IEmployeeLicenceRepository GetEmployeeLicenceRepository();
        IEmployeeProfessionRepository GetEmployeeProfessionRepository();

		IPhysicalPersonRepository GetPhysicalPersonRepository();
        IPhysicalPersonItemRepository GetPhysicalPersonItemRepository();
        IPhysicalPersonNoteRepository GetPhysicalPersonNoteRepository();
        IPhysicalPersonCardRepository GetPhysicalPersonCardRepository();
        IPhysicalPersonDocumentRepository GetPhysicalPersonDocumentRepository();
        IPhysicalPersonLicenceRepository GetPhysicalPersonLicenceRepository();
        IPhysicalPersonProfessionRepository GetPhysicalPersonProfessionRepository();


        IEmployeeByConstructionSiteRepository GetEmployeeByConstructionSiteRepository();
        IFamilyMemberRepository GetFamilyMemberRepository();

        IEmployeeByBusinessPartnerRepository GetEmployeeByBusinessPartnerRepository();

        IBusinessPartnerByConstructionSiteRepository GetBusinessPartnerByConstructionSiteRepository();

        ITaxAdministrationRepository GetTaxAdministrationRepository();

        IVatRepository GetVatRepository();

        IServiceDeliveryRepository GetServiceDeliveryRepository();
        IDiscountRepository GetDiscountRepository();

        IStatusRepository GetStatusRepository();
        IShipmentRepository GetShipmentRepository();
        IShipmentDocumentRepository GetShipmentDocumentRepository();
        IPhonebookRepository GetPhonebookRepository();
        IPhonebookPhoneRepository GetPhonebookPhoneRepository();
        IPhonebookNoteRepository GetPhonebookNoteRepository();
        IPhonebookDocumentRepository GetPhonebookDocumentRepository();
        IInvoiceRepository GetInvoiceRepository();
        IInvoiceItemRepository GetInvoiceItemRepository();

        ICallCentarRepository GetCallCentarRepository();


        void Save();
    }
}
