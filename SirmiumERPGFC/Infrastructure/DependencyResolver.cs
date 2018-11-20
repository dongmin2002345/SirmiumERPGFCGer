using Ninject;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceInterfaces.Abstractions.Common.TaxAdministrations;
using ServiceInterfaces.Abstractions.Common.ToDos;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceInterfaces.Abstractions.Employees;
using ServiceWebApi.Implementations.Banks;
using ServiceWebApi.Implementations.Common.BusinessPartners;
using ServiceWebApi.Implementations.Common.Companies;
using ServiceWebApi.Implementations.Common.Identity;
using ServiceWebApi.Implementations.Common.InputInvoices;
using ServiceWebApi.Implementations.Common.Locations;
using ServiceWebApi.Implementations.Common.OutputInvoices;
using ServiceWebApi.Implementations.Common.Professions;
using ServiceWebApi.Implementations.Common.Sectors;
using ServiceWebApi.Implementations.Common.TaxAdministrations;
using ServiceWebApi.Implementations.Common.ToDos;
using ServiceWebApi.Implementations.ConstructionSites;
using ServiceWebApi.Implementations.Employees;

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

                Kernel.Bind<IOutputInvoiceService>().To<OutputInvoiceService>();
				Kernel.Bind<IInputInvoiceService>().To<InputInvoiceService>();

				Kernel.Bind<ICityService>().To<CityService>();
                Kernel.Bind<IRegionService>().To<RegionService>();
                Kernel.Bind<IMunicipalityService>().To<MunicipalityService>();
                Kernel.Bind<ICountryService>().To<CountryService>();

				Kernel.Bind<ISectorService>().To<SectorService>();
				Kernel.Bind<IAgencyService>().To<AgencyService>();

                Kernel.Bind<IProfessionService>().To<ProfessionService>();
				Kernel.Bind<IBankService>().To<BankService>();
				Kernel.Bind<ILicenceTypeService>().To<LicenceTypeService>();

				Kernel.Bind<IPhysicalPersonService>().To<PhysicalPersonService>();

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

                Kernel.Bind<ITaxAdministrationService>().To<TaxAdministrationService>();

            }
        }
    }
}
