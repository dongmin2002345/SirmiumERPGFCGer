using Ninject;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Common.Locations;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Abstractions.Common.Individuals;
using ServiceInterfaces.Abstractions.Common.OutputInvoices;
using ServiceWebApi.Implementations.Common.BusinessPartners;
using ServiceWebApi.Implementations.Common.Locations;
using ServiceWebApi.Implementations.Common.Companies;
using ServiceWebApi.Implementations.Common.Identity;
using ServiceWebApi.Implementations.Common.Individuals;
using ServiceWebApi.Implementations.Common.OutputInvoices;
using ServiceInterfaces.Abstractions.Common.Sectors;
using ServiceWebApi.Implementations.Common.Sectors;
using ServiceInterfaces.Abstractions.Common.Professions;
using ServiceWebApi.Implementations.Common.Professions;
using ServiceInterfaces.Abstractions.Banks;
using ServiceWebApi.Implementations.Banks;
using ServiceInterfaces.Abstractions.ConstructionSites;
using ServiceWebApi.Implementations.ConstructionSites;
using ServiceWebApi.Implementations.Employees;
using ServiceInterfaces.Abstractions.Employees;

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

                Kernel.Bind<IBusinessPartnerService>().To<BusinessPartnerService>();

                Kernel.Bind<IIndividualService>().To<IndividualService>();

                Kernel.Bind<IOutputInvoiceService>().To<OutputInvoiceService>();

                Kernel.Bind<ICityService>().To<CityService>();
                Kernel.Bind<IRegionService>().To<RegionService>();
                Kernel.Bind<IMunicipalityService>().To<MunicipalityService>();
                Kernel.Bind<ICountryService>().To<CountryService>();

				Kernel.Bind<ISectorService>().To<SectorService>();

                Kernel.Bind<IProfessionService>().To<ProfessionService>();
				Kernel.Bind<IBankService>().To<BankService>();

                Kernel.Bind<IFamilyMemberService>().To<FamilyMemberService>();

                Kernel.Bind<IConstructionSiteService>().To<ConstructionSiteService>();

            }
        }
    }
}
