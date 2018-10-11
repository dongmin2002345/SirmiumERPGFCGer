using Configurator;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Individuals;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Context;
using RepositoryCore.Implementations.Common.BusinessPartners;
using RepositoryCore.Implementations.Common.Locations;
using RepositoryCore.Implementations.Common.Companies;
using RepositoryCore.Implementations.Common.Identity;
using RepositoryCore.Implementations.Common.Individuals;
using RepositoryCore.Implementations.Common.Invoices;
using RepositoryCore.UnitOfWork.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Implementations.Common.Sectors;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Implementations.Common.Professions;
using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Implementations.Banks;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Implementations.ConstructionSites;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Implementations.Employees;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Implementations.Employees;

namespace RepositoryCore.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;

        #region Repository variables

        private ICompanyRepository companyRepository;

        private IAuthenticationRepository authenticationRepository;
        private IUserRepository userRepository;

        private IBusinessPartnerRepository businessPartnerRepository;
        private IBusinessPartnerLocationRepository businessPartnerLocationRepository;
        private IBusinessPartnerPhoneRepository businessPartnerPhoneRepository;
        private IBusinessPartnerOrganizationUnitRepository businessPartnerOrganizationUnitRepository;
        private IBusinessPartnerTypeRepository businessPartnerTypeRepository;
        private IBusinessPartnerBusinessPartnerTypeRepository businessPartnerBusinessPartnerTypeRepository;

        private IIndividualsRepository individualsRepository;

        private IOutputInvoiceRepository outputInvoiceRepository;

        private ICityRepository cityRepository;
        private IRegionRepository regionRepository;
        private IMunicipalityRepository municipalityRepository;
        private ICountryRepository countryRepository;

        private IProfessionRepository professionRepository;

        private IConstructionSiteRepository constructionSiteRepository;

        private ISectorRepository sectorRepository;
		private IBankRepository bankRepository;
		private ILicenceTypeRepository licenceTypeRepository;
        private IAgencyRepository agencyRepository;

        private IEmployeeRepository employeeRepository;
        private IEmployeeItemRepository employeeItemRepository;
        private IFamilyMemberRepository familyMemberRepository;

        #endregion
        
		#region Constructor

		public UnitOfWork(bool useSql2005Compatibility = false)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            if (useSql2005Compatibility)
            {
                builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"], b => b.UseRowNumberForPaging(true));

            }
            else
            {
                builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"]);
            }


            this.context = new ApplicationDbContext(builder.Options);


        }

        #endregion

        #region Get methods for repositories

        public ICompanyRepository GetCompanyRepository()
        {
            if (companyRepository == null)
                companyRepository = new CompanyRepository(context);
            return companyRepository;
        }

        public IAuthenticationRepository GetAuthenticationRepository()
        {
            if (authenticationRepository == null)
                authenticationRepository = new AuthenticationRepository(context);
            return authenticationRepository;
        }

        public IUserRepository GetUserRepository()
        {
            if (userRepository == null)
                userRepository = new UserRepository(context);
            return userRepository;
        }

        public IBusinessPartnerRepository GetBusinessPartnerRepository()
        {
            if (businessPartnerRepository == null)
                businessPartnerRepository = new BusinessPartnerRepository(context);
            return businessPartnerRepository;
        }

        public IBusinessPartnerLocationRepository GetBusinessPartnerLocationRepository()
        {
            if (businessPartnerLocationRepository == null)
                businessPartnerLocationRepository = new BusinessPartnerLocationRepository(context);
            return businessPartnerLocationRepository;
        }

        public IBusinessPartnerPhoneRepository GetBusinessPartnerPhoneRepository()
        {
            if (businessPartnerPhoneRepository == null)
                businessPartnerPhoneRepository = new BusinessPartnerPhoneRepository(context);
            return businessPartnerPhoneRepository;
        }

        public IBusinessPartnerOrganizationUnitRepository GetBusinessPartnerOrganizationUnitRepository()
        {
            if (businessPartnerOrganizationUnitRepository == null)
                businessPartnerOrganizationUnitRepository = new BusinessPartnerOrganizationUnitRepository(context);
            return businessPartnerOrganizationUnitRepository;
        }

        public IBusinessPartnerTypeRepository GetBusinessPartnerTypeRepository()
        {
            if (businessPartnerTypeRepository == null)
                businessPartnerTypeRepository = new BusinessPartnerTypeRepository(context);
            return businessPartnerTypeRepository;
        }

        public IBusinessPartnerBusinessPartnerTypeRepository GetBusinessPartnerBusinessPartnerTypeRepository()
        {
            if (businessPartnerBusinessPartnerTypeRepository == null)
                businessPartnerBusinessPartnerTypeRepository = new BusinessPartnerBusinessPartnerTypeRepository(context);
            return businessPartnerBusinessPartnerTypeRepository;
        }

        public IIndividualsRepository GetIndividualsRepository()
        {
            if (individualsRepository == null)
                individualsRepository = new IndividualRepository(context);
            return individualsRepository;
        }

        public IOutputInvoiceRepository GetOutputInvoiceRepository()
        {
            if (outputInvoiceRepository == null)
                outputInvoiceRepository = new OutputInvoiceRepository(context);
            return outputInvoiceRepository;
        }

        public ICountryRepository GetCountryRepository()
        {
            if (countryRepository == null)
                countryRepository = new CountryRepository(context);
            return countryRepository;
        }

        public ICityRepository GetCityRepository()
        {
            if (cityRepository == null)
                cityRepository = new CityRepository(context);
            return cityRepository;
        }

        public IRegionRepository GetRegionRepository()
        {
            if (regionRepository == null)
                regionRepository = new RegionRepository(context);
            return regionRepository;
        }

        public IMunicipalityRepository GetMunicipalityRepository()
        {
            if (municipalityRepository == null)
                municipalityRepository = new MunicipalityRepository(context);
            return municipalityRepository;
        }

        public ISectorRepository GetSectorRepository()
        {
            if (sectorRepository == null)
                sectorRepository = new SectorRepository(context);
            return sectorRepository;
        }

        public IAgencyRepository GetAgencyRepository()
        {
            if (agencyRepository == null)
                agencyRepository = new AgencyRepository(context);
            return agencyRepository;
        }

        public IProfessionRepository GetProfessionRepository()
        {
            if (professionRepository == null)
                professionRepository = new ProfessionRepository(context);
            return professionRepository;
        }

		public IBankRepository GetBankRepository()
		{
			if (bankRepository == null)
				bankRepository = new BankRepository(context);
			return bankRepository;
		}

		public ILicenceTypeRepository GetLicenceTypeRepository()
		{
			if (licenceTypeRepository == null)
				licenceTypeRepository = new LicenceTypeRepository(context);
			return licenceTypeRepository;
		}

        public IConstructionSiteRepository GetConstructionSiteRepository()
        {
            if (constructionSiteRepository == null)
                constructionSiteRepository = new ConstructionSiteRepository(context);
            return constructionSiteRepository;
        }

        public IFamilyMemberRepository GetFamilyMemberRepository()
        {
            if (familyMemberRepository == null)
                familyMemberRepository = new FamilyMemberRepository(context);
            return familyMemberRepository;
        }

        public IEmployeeRepository GetEmployeeRepository()
        {
            if (employeeRepository == null)
                employeeRepository = new EmployeeRepository(context);
            return employeeRepository;
        }

        public IEmployeeItemRepository GetEmployeeItemRepository()
        {
            if (employeeItemRepository == null)
                employeeItemRepository = new EmployeeItemRepository(context);
            return employeeItemRepository;
        }

        #endregion

        #region Save method

        public void Save()
        {
            context.SaveChanges();
        }

        #endregion

        #region Dispose 

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
