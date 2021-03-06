﻿using Configurator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryCore.Context;
using RepositoryCore.DbSeed;
using RepositoryCore.DbViews.Banks;
using RepositoryCore.DbViews.CalendarAssignments;
using RepositoryCore.DbViews.Common.BusinessPartners;
using RepositoryCore.DbViews.Common.CallCentars;
using RepositoryCore.DbViews.Common.Companies;
using RepositoryCore.DbViews.Common.Identity;
using RepositoryCore.DbViews.Common.Invoices;
using RepositoryCore.DbViews.Common.Locations;
using RepositoryCore.DbViews.Common.Phonebooks;
using RepositoryCore.DbViews.Common.Prices;
using RepositoryCore.DbViews.Common.Professions;
using RepositoryCore.DbViews.Common.Sectors;
using RepositoryCore.DbViews.Common.Shipments;
using RepositoryCore.DbViews.Common.TaxAdministrations;
using RepositoryCore.DbViews.Common.ToDos;
using RepositoryCore.DbViews.ConstructionSites;
using RepositoryCore.DbViews.Employees;
using RepositoryCore.DbViews.PhysicalPersons;
using RepositoryCore.DbViews.Statuses;
using RepositoryCore.DbViews.Vats;
using RepositoryCore.UnitOfWork.Abstractions;
using RepositoryCore.UnitOfWork.Implementations;
using ServiceCore.Implementations.Banks;
using ServiceCore.Implementations.CalendarAssignments;
using ServiceCore.Implementations.Common;
using ServiceCore.Implementations.Common.BusinessPartners;
using ServiceCore.Implementations.Common.Companies;
using ServiceCore.Implementations.Common.DocumentStores;
using ServiceCore.Implementations.Common.Identity;
using ServiceCore.Implementations.Common.InputInvoices;
using ServiceCore.Implementations.Common.Invoices;
using ServiceCore.Implementations.Common.Locations;
using ServiceCore.Implementations.Common.OutputInvoices;
using ServiceCore.Implementations.Common.Phonebooks;
using ServiceCore.Implementations.Common.Prices;
using ServiceCore.Implementations.Common.Professions;
using ServiceCore.Implementations.Common.Sectors;
using ServiceCore.Implementations.Common.Shipments;
using ServiceCore.Implementations.Common.TaxAdministrations;
using ServiceCore.Implementations.Common.ToDos;
using ServiceCore.Implementations.ConstructionSites;
using ServiceCore.Implementations.Employees;
using ServiceCore.Implementations.Limitations;
using ServiceCore.Implementations.Statuses;
using ServiceCore.Implementations.Vats;
using ServiceInterfaces.Abstractions;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.CalendarAssignments;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Abstractions.Common.Companies;
using ServiceInterfaces.Abstractions.Common.DocumentStores;
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
using SirmiumERPWeb.Hubs;
using SirmiumERPWeb.Tasks;
using System;
using System.Threading;

namespace SirmiumERPWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException; ;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Configuration = configuration;
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                var exc = e.Exception;
                var excType = exc.GetType();
                if (excType == typeof(ArgumentException))
                {
                    var argException = exc as ArgumentException;
                }
                else if (excType == typeof(Exception))
                {
                    var exception = exc as Exception;
                }
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                var exc = e.ExceptionObject;
                if (exc.GetType() == typeof(ArgumentException))
                {
                    var argException = exc as ArgumentException;

                    Console.WriteLine(argException?.Message);
                }
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            /*string dbConnectionString = "Server=46.101.103.32; Port=3306; Database=SirmiumERPWeb; Uid=SirmiumERPWeb; Pwd=Secret123$; CharSet=utf8";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySQL(dbConnectionString);

            services.AddDbContext<ApplicationDbContext>();
            //services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(dbConnectionString));
            */
            ConfigureOtherStuff(services);

            services.AddMvc();


            return services.BuildServiceProvider();
        }

        private void ConfigureOtherStuff(IServiceCollection services)
        {
            /*string dbConnectionString = "Server=46.101.103.32; Port=3306; Database=SirmiumERPWeb; Uid=SirmiumERPWeb; Pwd=Secret123$; CharSet=utf8";
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseMySQL(dbConnectionString);
            services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(builder.Options));

            //services.AddScoped<IDbSeed, DbSeed>();
            */


            //services.AddDbContext<ApplicationDbContext>(
            //    b => b.UseLazyLoadingProxies()
            //        .UseSqlServer(new Config().GetConfiguration()["ConnectionString"] as string));

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(new Config().GetConfiguration()["ConnectionString"] as string); //, b => b.UseRowNumberForPaging(true));
            services.AddScoped<ApplicationDbContext>(c => new ApplicationDbContext(builder.Options));

            /* Repositoriers and other stuff related to them */

            services.AddScoped<IUnitOfWork, UnitOfWork>((sp) => new UnitOfWork(true));

            services.AddScoped<ServiceInterfaces.Abstractions.Common.Identity.IAuthenticationService, ServiceCore.Implementations.Common.Identity.AuthenticationService>();
            services.AddScoped<ISeedData, SeedData>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyUserService, CompanyUserService>();

            services.AddScoped<IToDoService, ToDoService>();

            services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
            services.AddScoped<IBusinessPartnerPhoneService, BusinessPartnerPhoneService>();
            services.AddScoped<IBusinessPartnerInstitutionService, BusinessPartnerInstitutionService>();
            services.AddScoped<IBusinessPartnerBankService, BusinessPartnerBankService>();
            services.AddScoped<IBusinessPartnerOrganizationUnitService, BusinessPartnerOrganizationUnitService>();
            services.AddScoped<IBusinessPartnerTypeService, BusinessPartnerTypeService>();
            services.AddScoped<IBusinessPartnerLocationService, BusinessPartnerLocationService>();
            services.AddScoped<IBusinessPartnerDocumentService, BusinessPartnerDocumentService>();
            services.AddScoped<IBusinessPartnerNoteService, BusinessPartnerNoteService>();

            services.AddScoped<IOutputInvoiceService, OutputInvoiceService>();
            services.AddScoped<IOutputInvoiceNoteService, OutputInvoiceNoteService>();
			services.AddScoped<IOutputInvoiceDocumentService, OutputInvoiceDocumentService>();
			services.AddScoped<IInputInvoiceService, InputInvoiceService>();
            services.AddScoped<IInputInvoiceNoteService, InputInvoiceNoteService>();
			services.AddScoped<IInputInvoiceDocumentService, InputInvoiceDocumentService>();

			services.AddScoped<ICityService, CityService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IMunicipalityService, MunicipalityService>();
            services.AddScoped<ICountryService, CountryService>();

			services.AddScoped<ISectorService, SectorService>();
			services.AddScoped<IAgencyService, AgencyService>();

            services.AddScoped<IProfessionService, ProfessionService>();

			services.AddScoped<IPhysicalPersonService, PhysicalPersonService>();
            services.AddScoped<IPhysicalPersonDocumentService, PhysicalPersonDocumentService>();
            services.AddScoped<IPhysicalPersonCardService, PhysicalPersonCardService>();
            services.AddScoped<IPhysicalPersonItemService, PhysicalPersonItemService>();
            services.AddScoped<IPhysicalPersonNoteService, PhysicalPersonNoteService>();
            services.AddScoped<IPhysicalPersonLicenceService, PhysicalPersonLicenceService>();
            services.AddScoped<IPhysicalPersonProfessionService, PhysicalPersonProfessionService>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeDocumentService, EmployeeDocumentService>();
            services.AddScoped<IEmployeeCardService, EmployeeCardService>();
            services.AddScoped<IEmployeeItemService, EmployeeItemService>();
            services.AddScoped<IEmployeeNoteService, EmployeeNoteService>();
            services.AddScoped<IEmployeeLicenceService, EmployeeLicenceService>();
            services.AddScoped<IEmployeeProfessionService, EmployeeProfessionService>();
            services.AddScoped<IEmployeeByConstructionSiteService, EmployeeByConstructionSiteService>();
            services.AddScoped<IFamilyMemberService, FamilyMemberService>();

            services.AddScoped<IEmployeeByBusinessPartnerService, EmployeeByBusinessPartnerService>();

            services.AddScoped<IBusinessPartnerByConstructionSiteService, BusinessPartnerByConstructionSiteService>();

            services.AddScoped<IBankService, BankService>();
			services.AddScoped<ILicenceTypeService, LicenceTypeService>();
            services.AddScoped<ILimitationService, LimitationService>();
            services.AddScoped<ILimitationEmailService, LimitationEmailService>();

            services.AddScoped<IConstructionSiteService, ConstructionSiteService>();
            services.AddScoped<IConstructionSiteCalculationService, ConstructionSiteCalculationService>();
            services.AddScoped<IConstructionSiteDocumentService, ConstructionSiteDocumentService>();
            services.AddScoped<IConstructionSiteNoteService, ConstructionSiteNoteService>();

            services.AddScoped<ITaxAdministrationService, TaxAdministrationService>();

            services.AddScoped<IVatService, VatService>();
            services.AddScoped<IServiceDeliveryService, ServiceDeliveryService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IStatusService, StatusService>();

            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentDocumentService, ShipmentDocumentService>();

            services.AddScoped<IPhonebookService, PhonebookService>();
            services.AddScoped<IPhonebookDocumentService, PhonebookDocumentService>();
            services.AddScoped<IPhonebookNoteService, PhonebookNoteService>();
            services.AddScoped<IPhonebookPhoneService, PhonebookPhoneService>();

            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IInvoiceItemService, InvoiceItemService>();

            services.AddScoped<ICallCentarService, CallCentarService>();

            services.AddScoped<ICalendarAssignmentService, CalendarAssignmentService>();
            services.AddScoped<IEmployeeAttachmentService, EmployeeAttachmentService>();
            services.AddScoped<IPhysicalPersonAttachmentService, PhysicalPersonAttachmentService>();
            services.AddScoped<IToDoStatusService, ToDoStatusService>();
            services.AddScoped<IDocumentFolderService, DocumentFolderService>();
            services.AddScoped<IDocumentFileService, DocumentFileService>();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISeedData seedData)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSignalR((cfg) => {
                cfg.MapHub<NotificationHub>("/notifications");
            });

            app.UseMvc(routes =>
                routes.MapRoute(
                    name: "WebAPI",
                    template: "api/{controller}/{action}/{id?}"
                )
            //.MapRoute(
            //    name: "WebAPIForYear",
            //    template: "api/foryear/{year:int}/{controller}/{action}/{id?}"
            //)
            );

            seedData.SeedCompanyData();
            seedData.SeedUserData();
            //var tmp1 = UnitOfWork.ContoShemeOptions;

            //seedData.PopulateData();

            // Create views
            BankView.CreateView();

            BusinessPartnerBankView.CreateView();
            BusinessPartnerByConstructionSiteView.CreateView();
            BusinessPartnerInstitutionView.CreateView();
            BusinessPartnerLocationView.CreateView();
            BusinessPartnerDocumentView.CreateView();
            BusinessPartnerNoteView.CreateView();
            BusinessPartnerOrganizationUnitView.CreateView();
            BusinessPartnerPhoneView.CreateView();
            BusinessPartnerView.CreateView();
            BusinessPartnerTypeView.CreateView();

            //CompanyView.CreateView(); //???

            //UserView.CreateView();

            InputInvoiceView.CreateView();
            InputInvoiceNoteView.CreateView();
			InputInvoiceDocumentView.CreateView();
			OutputInvoiceDocumentView.CreateView();
			OutputInvoiceView.CreateView();
            OutputInvoiceNoteView.CreateView();

            CountryView.CreateView();
            RegionView.CreateView();
            MunicipalityView.CreateView();
            CityView.CreateView();

            ProfessionView.CreateView();

            SectorView.CreateView();
            AgencyView.CreateView();

            TaxAdministrationView.CreateView();

            ToDoView.CreateView();

            EmployeeByBusinessPartnerView.CreateView();
            EmployeeByConstructionSiteView.CreateView();
            EmployeeCardView.CreateView();
            EmployeeView.CreateView();
            EmployeeDocumentView.CreateView();
            EmployeeItemView.CreateView();
            EmployeeLicenceView.CreateView();
            EmployeeNoteView.CreateView();
            EmployeeProfessionView.CreateView();
            FamilyMemberView.CreateView();
            LicenceTypeView.CreateView();

            PhysicalPersonView.CreateView();
            PhysicalPersonItemView.CreateView();
            PhysicalPersonNoteView.CreateView();
            PhysicalPersonLicenceView.CreateView();
            PhysicalPersonDocumentView.CreateView();
            PhysicalPersonCardView.CreateView();
            PhysicalPersonProfessionView.CreateView();

            ConstructionSiteCalculationView.CreateView();
            ConstructionSiteDocumentView.CreateView();
            ConstructionSiteNoteView.CreateView();
            ConstructionSiteView.CreateView();

            VatView.CreateView();

            ServiceDeliveryView.CreateView();
            DiscountView.CreateView();
            StatusView.CreateView();

            ShipmentView.CreateView();
            ShipmentDocumentView.CreateView();

            PhonebookView.CreateView();
            PhonebookDocumentView.CreateView();
            PhonebookNoteView.CreateView();
            PhonebookPhoneView.CreateView();

            InvoiceView.CreateView();
            InvoiceItemView.CreateView();

            CallCentarView.CreateView();

            CalendarAssignmentView.CreateView();

            EmployeeAttachmentView.CreateView();

            PhysicalPersonAttachmentView.CreateView();

            ToDoStatusView.CreateView();

            var mailingTime = new Config().GetConfiguration()["MailTime"];
            Console.WriteLine("Sending mails scheduled at: {0}\nCurrent time: {1}", mailingTime, DateTime.Now.ToString("HH:mm:ss"));

            Thread mailThread = new Thread(() => MailTask.SendMailTime(mailingTime));
            mailThread.IsBackground = true;
            mailThread.Start();
        }
    }
}