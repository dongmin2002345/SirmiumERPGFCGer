using Configurator;
using Microsoft.EntityFrameworkCore;
using RepositoryCore.Abstractions.Banks;
using RepositoryCore.Abstractions.Common.BusinessPartners;
using RepositoryCore.Abstractions.Common.Companies;
using RepositoryCore.Abstractions.Common.Identity;
using RepositoryCore.Abstractions.Common.Invoices;
using RepositoryCore.Abstractions.Common.Locations;
using RepositoryCore.Abstractions.Common.Professions;
using RepositoryCore.Abstractions.Common.Sectors;
using RepositoryCore.Abstractions.Common.TaxAdministrations;
using RepositoryCore.Abstractions.Common.ToDos;
using RepositoryCore.Abstractions.ConstructionSites;
using RepositoryCore.Abstractions.Employees;
using RepositoryCore.Abstractions.Limitations;
using RepositoryCore.Context;
using RepositoryCore.Implementations.Banks;
using RepositoryCore.Implementations.Common.BusinessPartners;
using RepositoryCore.Implementations.Common.Companies;
using RepositoryCore.Implementations.Common.Identity;
using RepositoryCore.Implementations.Common.Invoices;
using RepositoryCore.Implementations.Common.Locations;
using RepositoryCore.Implementations.Common.Professions;
using RepositoryCore.Implementations.Common.Sectors;
using RepositoryCore.Implementations.Common.TaxAdministrations;
using RepositoryCore.Implementations.Common.ToDos;
using RepositoryCore.Implementations.ConstructionSites;
using RepositoryCore.Implementations.Employees;
using RepositoryCore.Implementations.Limitations;
using RepositoryCore.Implementations.PhysicalPersons;
using RepositoryCore.UnitOfWork.Abstractions;
using System;

namespace RepositoryCore.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;

        #region Repository variables

        private ICompanyRepository companyRepository;
        private ICompanyUserRepository companyUserRepository;

        private IToDoRepository toDoRepository;

        private IAuthenticationRepository authenticationRepository;
        private IUserRepository userRepository;

        private IBusinessPartnerRepository businessPartnerRepository;
        private IBusinessPartnerLocationRepository businessPartnerLocationRepository;
        private IBusinessPartnerPhoneRepository businessPartnerPhoneRepository;
        private IBusinessPartnerInstitutionRepository businessPartnerInstitutionRepository;
        private IBusinessPartnerBankRepository businessPartnerBankRepository;
        private IBusinessPartnerOrganizationUnitRepository businessPartnerOrganizationUnitRepository;
        private IBusinessPartnerTypeRepository businessPartnerTypeRepository;
        private IBusinessPartnerBusinessPartnerTypeRepository businessPartnerBusinessPartnerTypeRepository;
        private IBusinessPartnerDocumentRepository businessPartnerDocumentRepository;
        private IBusinessPartnerNoteRepository businessPartnerNoteRepository;

        private IOutputInvoiceRepository outputInvoiceRepository;
		private IInputInvoiceRepository inputInvoiceRepository;

		private ICityRepository cityRepository;
        private IRegionRepository regionRepository;
        private IMunicipalityRepository municipalityRepository;
        private ICountryRepository countryRepository;

        private IProfessionRepository professionRepository;

        private IConstructionSiteRepository constructionSiteRepository;
        private IConstructionSiteCalculationRepository constructionSiteCalculationRepository;
        private IConstructionSiteDocumentRepository constructionSiteDocumentRepository;
        private IConstructionSiteNoteRepository constructionSiteNoteRepository;

        private ISectorRepository sectorRepository;
		private IBankRepository bankRepository;
		private ILicenceTypeRepository licenceTypeRepository;
        private IAgencyRepository agencyRepository;
        private ILimitationRepository limitationRepository;

		private IPhysicalPersonRepository physicalPersonRepository;
	

		private IEmployeeRepository employeeRepository;
        private IEmployeeItemRepository employeeItemRepository;
        private IEmployeeNoteRepository employeeNoteRepository;
        private IEmployeeCardRepository employeeCardRepository;
        private IEmployeeDocumentRepository employeeDocumentRepository;
        private IEmployeeLicenceRepository employeeLicenceRepository; 
        private IEmployeeProfessionRepository employeeProfessionRepository;
        private IEmployeeByConstructionSiteRepository employeeByConstructionSiteRepository;
        private IFamilyMemberRepository familyMemberRepository;

        private IEmployeeByBusinessPartnerRepository employeeByBusinessPartnerRepository;

        private IBusinessPartnerByConstructionSiteRepository businessPartnerByConstructionSiteRepository;

        private ITaxAdministrationRepository taxAdministrationRepository;


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

        public ICompanyUserRepository GetCompanyUserRepository()
        {
            if (companyUserRepository == null)
                companyUserRepository = new CompanyUserRepository(context);

            return companyUserRepository;
        }

        public IToDoRepository GetToDoRepository()
        {
            if (toDoRepository == null)
                toDoRepository = new ToDoRepository(context);
            return toDoRepository;
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
                businessPartnerRepository = new BusinessPartnerViewRepository(context);
            return businessPartnerRepository;
        }

        public IBusinessPartnerLocationRepository GetBusinessPartnerLocationRepository()
        {
            if (businessPartnerLocationRepository == null)
                businessPartnerLocationRepository = new BusinessPartnerLocationViewRepository(context);
            return businessPartnerLocationRepository;
        }

        public IBusinessPartnerPhoneRepository GetBusinessPartnerPhoneRepository()
        {
            if (businessPartnerPhoneRepository == null)
                businessPartnerPhoneRepository = new BusinessPartnerPhoneViewRepository(context);
            return businessPartnerPhoneRepository;
        }

        public IBusinessPartnerInstitutionRepository GetBusinessPartnerInstitutionRepository()
        {
            if (businessPartnerInstitutionRepository == null)
                businessPartnerInstitutionRepository = new BusinessPartnerInstitutionViewRepository(context);
            return businessPartnerInstitutionRepository;
        }

        public IBusinessPartnerBankRepository GetBusinessPartnerBankRepository()
        {
            if (businessPartnerBankRepository == null)
                businessPartnerBankRepository = new BusinessPartnerBankViewRepository(context);
            return businessPartnerBankRepository;
        }

        public IBusinessPartnerOrganizationUnitRepository GetBusinessPartnerOrganizationUnitRepository()
        {
            if (businessPartnerOrganizationUnitRepository == null)
                businessPartnerOrganizationUnitRepository = new BusinessPartnerOrganizationUnitViewRepository(context);
            return businessPartnerOrganizationUnitRepository;
        }

        public IBusinessPartnerTypeRepository GetBusinessPartnerTypeRepository()
        {
            if (businessPartnerTypeRepository == null)
                businessPartnerTypeRepository = new BusinessPartnerTypeViewRepository(context);
            return businessPartnerTypeRepository;
        }

        public IBusinessPartnerBusinessPartnerTypeRepository GetBusinessPartnerBusinessPartnerTypeRepository()
        {
            if (businessPartnerBusinessPartnerTypeRepository == null)
                businessPartnerBusinessPartnerTypeRepository = new BusinessPartnerBusinessPartnerTypeRepository(context);
            return businessPartnerBusinessPartnerTypeRepository;
        }

        public IBusinessPartnerDocumentRepository GetBusinessPartnerDocumentRepository()
        {
            if (businessPartnerDocumentRepository == null)
                businessPartnerDocumentRepository = new BusinessPartnerDocumentViewRepository(context);
            return businessPartnerDocumentRepository;
        }

        public IBusinessPartnerNoteRepository GetBusinessPartnerNoteRepository()
        {
            if (businessPartnerNoteRepository == null)
                businessPartnerNoteRepository = new BusinessPartnerNoteViewRepository(context);
            return businessPartnerNoteRepository;
        }


        public IOutputInvoiceRepository GetOutputInvoiceRepository()
        {
            if (outputInvoiceRepository == null)
                outputInvoiceRepository = new OutputInvoiceViewRepository(context);
            return outputInvoiceRepository;
        }

		public IInputInvoiceRepository GetInputInvoiceRepository()
		{
			if (inputInvoiceRepository == null)
				inputInvoiceRepository = new InputInvoiceViewRepository(context);
			return inputInvoiceRepository;
		}

		public ICountryRepository GetCountryRepository()
        {
            if (countryRepository == null)
                countryRepository = new CountryViewRepository(context);
            return countryRepository;
        }

        public ICityRepository GetCityRepository()
        {
            if (cityRepository == null)
                cityRepository = new CityViewRepository(context);
            return cityRepository;
        }

        public IRegionRepository GetRegionRepository()
        {
            if (regionRepository == null)
                regionRepository = new RegionViewRepository(context);
            return regionRepository;
        }

        public IMunicipalityRepository GetMunicipalityRepository()
        {
            if (municipalityRepository == null)
                municipalityRepository = new MunicipalityViewRepository(context);
            return municipalityRepository;
        }

        public ISectorRepository GetSectorRepository()
        {
            if (sectorRepository == null)
                sectorRepository = new SectorViewRepository(context);
            return sectorRepository;
        }

        public IAgencyRepository GetAgencyRepository()
        {
            if (agencyRepository == null)
                agencyRepository = new AgencyViewRepository(context);
            return agencyRepository;
        }

        public IProfessionRepository GetProfessionRepository()
        {
            if (professionRepository == null)
                professionRepository = new ProfessionViewRepository(context);
            return professionRepository;
        }

		public IBankRepository GetBankRepository()
		{
			if (bankRepository == null)
				bankRepository = new BankViewRepository(context);
			return bankRepository;
		}

		public ILicenceTypeRepository GetLicenceTypeRepository()
		{
			if (licenceTypeRepository == null)
				licenceTypeRepository = new LicenceTypeViewRepository(context);
			return licenceTypeRepository;
		}

        public IConstructionSiteRepository GetConstructionSiteRepository()
        {
            if (constructionSiteRepository == null)
                constructionSiteRepository = new ConstructionSiteViewRepository(context);
            return constructionSiteRepository;
        }

        public ILimitationRepository GetLimitationRepository()
        {
            if (limitationRepository == null)
                limitationRepository = new LimitationRepository(context);
            return limitationRepository;
        }

        public IConstructionSiteCalculationRepository GetConstructionSiteCalculationRepository()
        {
            if (constructionSiteCalculationRepository == null)
                constructionSiteCalculationRepository = new ConstructionSiteCalculationViewRepository(context);
            return constructionSiteCalculationRepository;
        }

        public IConstructionSiteDocumentRepository GetConstructionSiteDocumentRepository()
        {
            if (constructionSiteDocumentRepository == null)
                constructionSiteDocumentRepository = new ConstructionSiteDocumentViewRepository(context);
            return constructionSiteDocumentRepository;
        }

        public IConstructionSiteNoteRepository GetConstructionSiteNoteRepository()
        {
            if (constructionSiteNoteRepository == null)
                constructionSiteNoteRepository = new ConstructionSiteNoteViewRepository(context);
            return constructionSiteNoteRepository;
        }


        public IFamilyMemberRepository GetFamilyMemberRepository()
        {
            if (familyMemberRepository == null)
                familyMemberRepository = new FamilyMemberViewRepository(context);
            return familyMemberRepository;
        }

        public IEmployeeRepository GetEmployeeRepository()
        {
            if (employeeRepository == null)
                employeeRepository = new EmployeeViewRepository(context);
            return employeeRepository;
        }

        public IEmployeeProfessionRepository GetEmployeeProfessionRepository()
        {
            if (employeeProfessionRepository == null)
                employeeProfessionRepository = new EmployeeProfessionViewRepository(context);
            return employeeProfessionRepository;
        }

        public IEmployeeLicenceRepository GetEmployeeLicenceRepository()
        {
            if (employeeLicenceRepository == null)
                employeeLicenceRepository = new EmployeeLicenceViewRepository(context);
            return employeeLicenceRepository;
        }

        public IEmployeeItemRepository GetEmployeeItemRepository()
        {
            if (employeeItemRepository == null)
                employeeItemRepository = new EmployeeItemViewRepository(context);
            return employeeItemRepository;
        }

        public IEmployeeNoteRepository GetEmployeeNoteRepository()
        {
            if (employeeNoteRepository == null)
                employeeNoteRepository = new EmployeeNoteViewRepository(context);
            return employeeNoteRepository;
        }

        public IEmployeeCardRepository GetEmployeeCardRepository()
        {
            if (employeeCardRepository == null)
                employeeCardRepository = new EmployeeCardViewRepository(context);
            return employeeCardRepository;
        }

        public IEmployeeDocumentRepository GetEmployeeDocumentRepository()
        {
            if (employeeDocumentRepository == null)
                employeeDocumentRepository = new EmployeeDocumentViewRepository(context);
            return employeeDocumentRepository;
        }

        public IEmployeeByConstructionSiteRepository GetEmployeeByConstructionSiteRepository()
        {
            if (employeeByConstructionSiteRepository == null)
                employeeByConstructionSiteRepository = new EmployeeByConstructionSiteViewRepository(context);
            return employeeByConstructionSiteRepository;
        }

        public IEmployeeByBusinessPartnerRepository GetEmployeeByBusinessPartnerRepository()
        {
            if (employeeByBusinessPartnerRepository == null)
                employeeByBusinessPartnerRepository = new EmployeeByBusinessPartnerViewRepository(context);
            return employeeByBusinessPartnerRepository;
        }

		public IPhysicalPersonRepository GetPhysicalPersonRepository()
		{
			if (physicalPersonRepository == null)
				physicalPersonRepository = new PhysicalPersonViewRepository(context);
			return physicalPersonRepository;
		}

		
		public IBusinessPartnerByConstructionSiteRepository GetBusinessPartnerByConstructionSiteRepository()
        {
            if (businessPartnerByConstructionSiteRepository == null)
                businessPartnerByConstructionSiteRepository = new BusinessPartnerByConstructionSiteViewRepository(context);
            return businessPartnerByConstructionSiteRepository;
        }

        public ITaxAdministrationRepository GetTaxAdministrationRepository()
        {
            if (taxAdministrationRepository == null)
                taxAdministrationRepository = new TaxAdministrationViewRepository(context);
            return taxAdministrationRepository;
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
