using Ninject;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Abstractions.Common.Invoices;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Abstractions.Common.Phonebook;
using ServiceInterfaces.Abstractions.Common.Prices;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Abstractions.Common.Shipments;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Abstractions.Employees;
using ServiceInterfaces.Abstractions.Limitations;
using ServiceInterfaces.Abstractions.Statuses;
using ServiceInterfaces.Abstractions.Vats;
using ServiceWebApi.Implementations.Banks;
using ServiceWebApi.Implementations.CalendarAssignments;
using ServiceWebApi.Implementations.Common.BusinessPartners;
using ServiceWebApi.Implementations.Common.CallCentars;
using ServiceWebApi.Implementations.Common.Companies;
using ServiceWebApi.Implementations.Common.Identity;
using ServiceWebApi.Implementations.Common.InputInvoices;
using ServiceWebApi.Implementations.Common.Invoices;
using ServiceWebApi.Implementations.Common.Locations;
using ServiceWebApi.Implementations.Common.OutputInvoices;
using ServiceWebApi.Implementations.Common.Phonebooks;
using ServiceWebApi.Implementations.Common.Prices;
using ServiceWebApi.Implementations.Common.Professions;
using ServiceWebApi.Implementations.Common.Sectors;
using ServiceWebApi.Implementations.Common.Shipments;
using ServiceWebApi.Implementations.Common.TaxAdministrations;
using ServiceWebApi.Implementations.Common.ToDos;
using ServiceWebApi.Implementations.ConstructionSites;
using ServiceWebApi.Implementations.Employees;
using ServiceWebApi.Implementations.Limitations;
using ServiceWebApi.Implementations.Statuses;
using ServiceWebApi.Implementations.Vats;

namespace SirmiumERPGFC.Infrastructure
{
    public class DependencyResolver
    {
        public static readonly IKernel Kernel;

        static DependencyResolver()
        {
            if (Kernel == null)
            {
                Kernel = new StandardKernel();

                Kernel.Bind<ICompanyService>().To<CompanyService>();
                Kernel.Bind<ICompanyUserService>().To<CompanyUserService>();

                //Kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
                //Kernel.Bind<ISeedDataService>().To<SeedDataService>();

                Kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
                Kernel.Bind<IUserService>().To<UserService>();

                Kernel.Bind<IToDoService>().To<ToDoService>();

                Kernel.Bind<IBusinessPartnerService>().To<BusinessPartnerService>();
                Kernel.Bind<IBusinessPartnerTypeService>().To<BusinessPartnerTypeService>();
                Kernel.Bind<IBusinessPartnerOrganizationUnitService>().To<BusinessPartnerOrganizationUnitService>();
                Kernel.Bind<IBusinessPartnerPhoneService>().To<BusinessPartnerPhoneService>();
                Kernel.Bind<IBusinessPartnerInstitutionService>().To<BusinessPartnerInstitutionService>();
                Kernel.Bind<IBusinessPartnerBankService>().To<BusinessPartnerBankService>();
                Kernel.Bind<IBusinessPartnerLocationService>().To<BusinessPartnerLocationService>();
                Kernel.Bind<IBusinessPartnerDocumentService>().To<BusinessPartnerDocumentService>();
                Kernel.Bind<IBusinessPartnerNoteService>().To<BusinessPartnerNoteService>();

                Kernel.Bind<IOutputInvoiceService>().To<OutputInvoiceService>();
                Kernel.Bind<IOutputInvoiceNoteService>().To<OutputInvoiceNoteService>();
				Kernel.Bind<IOutputInvoiceDocumentService>().To<OutputInvoiceDocumentService>();
				Kernel.Bind<IInputInvoiceService>().To<InputInvoiceService>();
                Kernel.Bind<IInputInvoiceNoteService>().To<InputInvoiceNoteService>();
				Kernel.Bind<IInputInvoiceDocumentService>().To<InputInvoiceDocumentService>();

				Kernel.Bind<ICityService>().To<CityService>();
                Kernel.Bind<IRegionService>().To<RegionService>();
                Kernel.Bind<IMunicipalityService>().To<MunicipalityService>();
                Kernel.Bind<ICountryService>().To<CountryService>();

				Kernel.Bind<ISectorService>().To<SectorService>();
				Kernel.Bind<IAgencyService>().To<AgencyService>();

                Kernel.Bind<IProfessionService>().To<ProfessionService>();
				Kernel.Bind<IBankService>().To<BankService>();
				Kernel.Bind<ILicenceTypeService>().To<LicenceTypeService>();
				Kernel.Bind<ILimitationService>().To<LimitationService>();
				Kernel.Bind<ILimitationEmailService>().To<LimitationEmailService>();

                Kernel.Bind<IPhysicalPersonService>().To<PhysicalPersonService>();
                Kernel.Bind<IPhysicalPersonItemService>().To<PhysicalPersonItemService>();
                Kernel.Bind<IPhysicalPersonNoteService>().To<PhysicalPersonNoteService>();
                Kernel.Bind<IPhysicalPersonDocumentService>().To<PhysicalPersonDocumentService>();
                Kernel.Bind<IPhysicalPersonCardService>().To<PhysicalPersonCardService>();
                Kernel.Bind<IPhysicalPersonLicenceService>().To<PhysicalPersonLicenceService>();
                Kernel.Bind<IPhysicalPersonProfessionService>().To<PhysicalPersonProfessionService>();

                Kernel.Bind<IEmployeeService>().To<EmployeeService>();
                Kernel.Bind<IEmployeeItemService>().To<EmployeeItemService>();
                Kernel.Bind<IEmployeeNoteService>().To<EmployeeNoteService>();
                Kernel.Bind<IEmployeeDocumentService>().To<EmployeeDocumentService>();
                Kernel.Bind<IEmployeeCardService>().To<EmployeeCardService>();
                Kernel.Bind<IEmployeeLicenceService>().To<EmployeeLicenceService>();
                Kernel.Bind<IEmployeeProfessionService>().To<EmployeeProfessionService>();
                Kernel.Bind<IEmployeeByConstructionSiteService>().To<EmployeeByConstructionSiteService>();
                Kernel.Bind<IFamilyMemberService>().To<FamilyMemberService>();

                Kernel.Bind<IEmployeeByBusinessPartnerService>().To<EmployeeByBusinessPartnerService>();

                Kernel.Bind<IBusinessPartnerByConstructionSiteService>().To<BusinessPartnerByConstructionSiteService>();

                Kernel.Bind<IConstructionSiteService>().To<ConstructionSiteService>();
                Kernel.Bind<IConstructionSiteCalculationService>().To<ConstructionSiteCalculationService>();
                Kernel.Bind<IConstructionSiteDocumentService>().To<ConstructionSiteDocumentService>();
                Kernel.Bind<IConstructionSiteNoteService>().To<ConstructionSiteNoteService>();

                Kernel.Bind<ITaxAdministrationService>().To<TaxAdministrationService>();

                Kernel.Bind<IVatService>().To<VatService>();

                Kernel.Bind<IServiceDeliveryService>().To<ServiceDeliveryService>();
                Kernel.Bind<IDiscountService>().To<DiscountService>();
                Kernel.Bind<IStatusService>().To<StatusService>();

                Kernel.Bind<IShipmentService>().To<ShipmentService>();
                Kernel.Bind<IShipmentDocumentService>().To<ShipmentDocumentService>();

                Kernel.Bind<IPhonebookService>().To<PhonebookService>();
                Kernel.Bind<IPhonebookDocumentService>().To<PhonebookDocumentService>();
                Kernel.Bind<IPhonebookNoteService>().To<PhonebookNoteService>();
                Kernel.Bind<IPhonebookPhoneService>().To<PhonebookPhoneService>();

                Kernel.Bind<IInvoiceService>().To<InvoiceService>();
                Kernel.Bind<IInvoiceItemService>().To<InvoiceItemService>();
                Kernel.Bind<ICallCentarService>().To<CallCentarService>();
                Kernel.Bind<ICalendarAssignmentService>().To<CalendarAssignmentService>();

                Kernel.Bind<IEmployeeAttachmentService>().To<EmployeeAttachmentService>();
                Kernel.Bind<IPhysicalPersonAttachmentService>().To<PhysicalPersonAttachmentService>();
            }
        }
    }
}
