using Ninject;
using ServiceInterfaces.Abstractions.Banks;
using ServiceInterfaces.Abstractions.Common.BusinessPartners;
using ServiceInterfaces.Abstractions.Common.CallCentars;
using ServiceInterfaces.Abstractions.Common.Identity;
using ServiceInterfaces.Abstractions.Common.InputInvoices;
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
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.Repository.Banks;
using SirmiumERPGFC.Repository.BusinessPartners;
using SirmiumERPGFC.Repository.CallCentars;
using SirmiumERPGFC.Repository.ConstructionSites;
using SirmiumERPGFC.Repository.Employees;
using SirmiumERPGFC.Repository.InputInvoices;
using SirmiumERPGFC.Repository.Limitations;
using SirmiumERPGFC.Repository.Locations;
using SirmiumERPGFC.Repository.OutputInvoices;
using SirmiumERPGFC.Repository.Phonebooks;
using SirmiumERPGFC.Repository.Prices;
using SirmiumERPGFC.Repository.Professions;
using SirmiumERPGFC.Repository.Sectors;
using SirmiumERPGFC.Repository.Shipments;
using SirmiumERPGFC.Repository.Statuses;
using SirmiumERPGFC.Repository.TaxAdministrations;
using SirmiumERPGFC.Repository.ToDos;
using SirmiumERPGFC.Repository.Users;
using SirmiumERPGFC.Repository.Vats;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls;

namespace SirmiumERPGFC.Views.Home
{
    public delegate void ToDoHandler();

    public partial class Home : UserControl, INotifyPropertyChanged
    {
        #region Atributes

        #region Services

        private IBusinessPartnerService businessPartnerService; 
        private IBusinessPartnerTypeService businessPartnerTypeService; 
        private IBusinessPartnerBankService businessPartnerBankService; 
        private IBusinessPartnerDocumentService businessPartnerDocumentService;
        private IBusinessPartnerInstitutionService businessPartnerInstitutionService; 
        private IBusinessPartnerLocationService businessPartnerLocationService; 
        private IBusinessPartnerNoteService businessPartnerNoteService; 
        //private IBusinessPartnerOrganizationUnitService businessPartnerOrganizationUnitService; // FALI SYNC METOD u SQLiteRepository?
        private IBusinessPartnerPhoneService businessPartnerPhoneService; 


        private IEmployeeService employeeService; 
        private IEmployeeProfessionService employeeProfessionService; 
        private IEmployeeNoteService employeeNoteService; 
        private IEmployeeLicenceService employeeLicenceService; 
        private IEmployeeItemService employeeItemService; 
        private IEmployeeDocumentService employeeDocumentService; 
        private IEmployeeCardService employeeCardService; 

        private ILicenceTypeService licenceTypeService; 
        private IFamilyMemberService familyMemberService; 

        private IPhysicalPersonService physicalPersonService; 
        private IPhysicalPersonProfessionService physicalPersonProfessionService; 
        private IPhysicalPersonNoteService physicalPersonNoteService;
        private IPhysicalPersonLicenceService physicalPersonLicenceService; 
        private IPhysicalPersonItemService physicalPersonItemService;
        private IPhysicalPersonDocumentService physicalPersonDocumentService;
        private IPhysicalPersonCardService physicalPersonCardService;



        private IOutputInvoiceService outputInvoiceService; 
        //private IOutputInvoiceDocumentService outputInvoiceDocumentService;
        //private IOutputInvoiceNoteService outputInvoiceNoteService;

        private IInputInvoiceService inputInvoiceService; 
        //private IInputInvoiceDocumentService inputInvoiceDocumentService;
        //private IInputInvoiceNoteService inputInvoiceNoteService;


        private ICountryService countryService; 
        private IRegionService regionService; 
        private IMunicipalityService municipalityService; 
        private ICityService cityService; 

        private ISectorService sectorService; 
        private IProfessionService professionService; 
        private IBankService bankService; 
        private IAgencyService agencyService; 


        private IConstructionSiteService constructionSiteService; 
        private IConstructionSiteDocumentService constructionSiteDocumentService; 
        private IConstructionSiteCalculationService constructionSiteCalculationService; 
        private IConstructionSiteNoteService constructionSiteNoteService; 

        //private IEmployeeByBusinessPartnerService employeeByBusinessPartnerService; //radnici po firmama?
        //private IEmployeeByConstructionSiteService employeeByConstructionSiteService; // radnici po gradilistu
        //private IBusinessPartnerByConstructionSiteService businessPartnerByConstructionSiteService; //firme po gradilistu

        private ITaxAdministrationService taxAdministrationService; 
       
        private ILimitationService limitationService; 
        private IUserService userService; 
        private IVatService vatService; 
        private IDiscountService discountService; 
        private IServiceDeliveryService serviceDeliveryService; 
        private IStatusService statusService; 
        
        private IShipmentService shipmentService; 
        //private IShipmentDocumentService shipmentDocumentService;

        private IToDoService toDoService;

        private IPhonebookService phonebookService;
        private ICallCentarService callCentarService;

        #endregion


        #region MinItemCounter
        private int _MinItemCounter;

        public int MinItemCounter
        {
            get { return _MinItemCounter; }
            set
            {
                if (_MinItemCounter != value)
                {
                    _MinItemCounter = value;
                    NotifyPropertyChanged("MinItemCounter");
                }
            }
        }
        #endregion

        #region MaxItemCounter
        private int _MaxItemCounter;

        public int MaxItemCounter
        {
            get { return _MaxItemCounter; }
            set
            {
                if (_MaxItemCounter != value)
                {
                    _MaxItemCounter = value;
                    NotifyPropertyChanged("MaxItemCounter");
                }
            }
        }
        #endregion

        #region ItemValue
        private int _ItemValue;

        public int ItemValue
        {
            get { return _ItemValue; }
            set
            {
                if (_ItemValue != value)
                {
                    _ItemValue = value;
                    NotifyPropertyChanged("ItemValue");
                }
            }
        }
        #endregion

        #region ItemContent
        private string _ItemContent;

        public string ItemContent
        {
            get { return _ItemContent; }
            set
            {
                if (_ItemContent != value)
                {
                    _ItemContent = value;
                    NotifyPropertyChanged("ItemContent");
                }
            }
        }
        #endregion

        #endregion

        #region Constructor

        public Home()
        {
            businessPartnerService = DependencyResolver.Kernel.Get<IBusinessPartnerService>();
            businessPartnerTypeService = DependencyResolver.Kernel.Get<IBusinessPartnerTypeService>();
            businessPartnerBankService = DependencyResolver.Kernel.Get<IBusinessPartnerBankService>();
            businessPartnerDocumentService = DependencyResolver.Kernel.Get<IBusinessPartnerDocumentService>();
            businessPartnerInstitutionService = DependencyResolver.Kernel.Get<IBusinessPartnerInstitutionService>();
            businessPartnerLocationService = DependencyResolver.Kernel.Get<IBusinessPartnerLocationService>();
            businessPartnerNoteService = DependencyResolver.Kernel.Get<IBusinessPartnerNoteService>();
            //businessPartnerOrganizationUnitService = DependencyResolver.Kernel.Get<IBusinessPartnerOrganizationUnitService>();
            businessPartnerPhoneService = DependencyResolver.Kernel.Get<IBusinessPartnerPhoneService>();


            employeeService = DependencyResolver.Kernel.Get<IEmployeeService>(); 
            employeeProfessionService = DependencyResolver.Kernel.Get<IEmployeeProfessionService>();
            employeeNoteService = DependencyResolver.Kernel.Get<IEmployeeNoteService>();
            employeeLicenceService = DependencyResolver.Kernel.Get<IEmployeeLicenceService>();
            employeeItemService = DependencyResolver.Kernel.Get<IEmployeeItemService>();
            employeeDocumentService = DependencyResolver.Kernel.Get<IEmployeeDocumentService>();
            employeeCardService = DependencyResolver.Kernel.Get<IEmployeeCardService>();

            licenceTypeService = DependencyResolver.Kernel.Get<ILicenceTypeService>(); 
            familyMemberService = DependencyResolver.Kernel.Get<IFamilyMemberService>(); 

            physicalPersonService = DependencyResolver.Kernel.Get<IPhysicalPersonService>(); 
            physicalPersonProfessionService = DependencyResolver.Kernel.Get<IPhysicalPersonProfessionService>();
            physicalPersonNoteService = DependencyResolver.Kernel.Get<IPhysicalPersonNoteService>();
            physicalPersonLicenceService = DependencyResolver.Kernel.Get<IPhysicalPersonLicenceService>();
            physicalPersonItemService = DependencyResolver.Kernel.Get<IPhysicalPersonItemService>();
            physicalPersonDocumentService = DependencyResolver.Kernel.Get<IPhysicalPersonDocumentService>();
            physicalPersonCardService = DependencyResolver.Kernel.Get<IPhysicalPersonCardService>();
            

            outputInvoiceService = DependencyResolver.Kernel.Get<IOutputInvoiceService>(); 
            inputInvoiceService = DependencyResolver.Kernel.Get<IInputInvoiceService>(); 

            countryService = DependencyResolver.Kernel.Get<ICountryService>(); 
            regionService = DependencyResolver.Kernel.Get<IRegionService>(); 
            municipalityService = DependencyResolver.Kernel.Get<IMunicipalityService>(); 
            cityService = DependencyResolver.Kernel.Get<ICityService>(); 

            sectorService = DependencyResolver.Kernel.Get<ISectorService>(); 
            professionService = DependencyResolver.Kernel.Get<IProfessionService>(); 
            bankService = DependencyResolver.Kernel.Get<IBankService>(); 
            agencyService = DependencyResolver.Kernel.Get<IAgencyService>(); 

            constructionSiteService = DependencyResolver.Kernel.Get<IConstructionSiteService>(); 
            constructionSiteDocumentService = DependencyResolver.Kernel.Get<IConstructionSiteDocumentService>();
            constructionSiteCalculationService = DependencyResolver.Kernel.Get<IConstructionSiteCalculationService>();
            constructionSiteNoteService = DependencyResolver.Kernel.Get<IConstructionSiteNoteService>();


            //employeeByBusinessPartnerService = DependencyResolver.Kernel.Get<IEmployeeByBusinessPartnerService>(); //radnici po firmama?
            //employeeByConstructionSiteService = DependencyResolver.Kernel.Get<IEmployeeByConstructionSiteService>(); // radnici po gradilistu
            //businessPartnerByConstructionSiteService = DependencyResolver.Kernel.Get<IBusinessPartnerByConstructionSiteService>(); //firme po gradilistu

            taxAdministrationService = DependencyResolver.Kernel.Get<ITaxAdministrationService>(); 

            limitationService = DependencyResolver.Kernel.Get<ILimitationService>(); 
            userService = DependencyResolver.Kernel.Get<IUserService>(); 
            vatService = DependencyResolver.Kernel.Get<IVatService>(); 
            discountService = DependencyResolver.Kernel.Get<IDiscountService>(); 
            serviceDeliveryService = DependencyResolver.Kernel.Get<IServiceDeliveryService>(); 
            statusService = DependencyResolver.Kernel.Get<IStatusService>(); 
            shipmentService = DependencyResolver.Kernel.Get<IShipmentService>(); 

            toDoService = DependencyResolver.Kernel.Get<IToDoService>();

            phonebookService = DependencyResolver.Kernel.Get<IPhonebookService>();
            callCentarService = DependencyResolver.Kernel.Get<ICallCentarService>();

            InitializeComponent();

            this.DataContext = this;
        }

        #endregion

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Thread th = new Thread(() => SyncData());
            th.IsBackground = true;
            th.Start();
        }

        #region Sync data

        private void SyncData()
        {
            int numOfTables = 48;
            int counter = 0;

            #region 1. Poslovni partner
            ItemContent = "Poslovni partner (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerSQLiteRepository().Sync(businessPartnerService, (synced, toSync) => 
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Poslovni partner (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 2. Tip poslovnog partnera
            ItemContent = "Tip poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerTypeSQLiteRepository().Sync(businessPartnerTypeService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Tip poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 3. Banke poslovnog partnera
            ItemContent = "Banke poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerBankSQLiteRepository().Sync(businessPartnerBankService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Banke poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 4. Dokumenti poslovnog partnera
            ItemContent = "Dokumenti poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerDocumentSQLiteRepository().Sync(businessPartnerDocumentService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Dokumenti poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 5. Institucije poslovnog partnera
            ItemContent = "Institucije poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerInstitutionSQLiteRepository().Sync(businessPartnerInstitutionService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Institucije poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 6. Institucije poslovnog partnera
            ItemContent = "Lokacije poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerLocationSQLiteRepository().Sync(businessPartnerLocationService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Lokacije poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 7. Napomene poslovnog partnera
            ItemContent = "Napomene poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerNoteSQLiteRepository().Sync(businessPartnerNoteService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Napomene poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 8. Organizacione jedinice poslovnog partnera
            //ItemContent = "Organizacione jedinice poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            //MinItemCounter = 0;
            //ItemValue = 0;
            //new BusinessPartnerOrganizationUnitSQLiteRepository().Sync(businessPartnerOrganizationUnitService, (synced, toSync) =>
            //{
            //    MaxItemCounter = toSync;
            //    ItemValue = synced;
            //    ItemContent = "Organizacione jedinice poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            //});
            #endregion

            #region 9. Telefoni poslovnog partnera
            ItemContent = "Telefoni poslovnog partnera (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BusinessPartnerPhoneSQLiteRepository().Sync(businessPartnerPhoneService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Telefoni poslovnog partnera (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 10. Radnici
            ItemContent = "Radnici (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeSQLiteRepository().Sync(employeeService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Radnici (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 11. Zanimanja radnika
            ItemContent = "Zanimanja radnika (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeProfessionItemSQLiteRepository().Sync(employeeProfessionService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Zanimanja radnika (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 12. Napomene radnika
            ItemContent = "Napomene radnika (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeNoteSQLiteRepository().Sync(employeeNoteService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Napomene radnika (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 13. Licence radnika
            ItemContent = "Licence radnika (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeLicenceItemSQLiteRepository().Sync(employeeLicenceService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Licence radnika (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 14. Radnici stavke
            ItemContent = "Radnici stavke (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeItemSQLiteRepository().Sync(employeeItemService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Radnici stavke (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 15. Dokumenti radnika
            ItemContent = "Dokumenti radnika (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeDocumentSQLiteRepository().Sync(employeeDocumentService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Dokumenti radnika (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 16. Kartice radnika
            ItemContent = "Kartice radnika (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new EmployeeCardSQLiteRepository().Sync(employeeCardService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Kartice radnika (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 17. Tip dozvole
            ItemContent = "Tip dozvole (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new LicenceTypeSQLiteRepository().Sync(licenceTypeService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Tip dozvole (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 18. Članovi porodice
            ItemContent = "Članovi porodice (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new FamilyMemberSQLiteRepository().Sync(familyMemberService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Članovi porodice (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 19. Fizička lica
            ItemContent = "Fizička lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonSQLiteRepository().Sync(physicalPersonService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Fizička lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 20. Zanimanja fizičkih lica
            ItemContent = "Zanimanja fizičkih lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonProfessionSQLiteRepository().Sync(physicalPersonProfessionService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Zanimanja fizičkih lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 21. Napomene fizička lica
            ItemContent = "Napomene fizička lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonNoteSQLiteRepository().Sync(physicalPersonNoteService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Napomene fizička lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 22. Licence fizička lica
            ItemContent = "Licence fizička lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonLicenceSQLiteRepository().Sync(physicalPersonLicenceService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Licence fizička lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 23. Fizička lica stavke
            ItemContent = "Fizička lica stavke (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonItemSQLiteRepository().Sync(physicalPersonItemService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Fizička lica stavke (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 24. Dokumenti fizička lica
            ItemContent = "Dokumenti fizička lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonDocumentSQLiteRepository().Sync(physicalPersonDocumentService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Dokumenti fizička lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 25. Kartice fizička lica
            ItemContent = "Kartice fizička lica (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhysicalPersonCardSQLiteRepository().Sync(physicalPersonCardService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Kartice fizička lica (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 26. Izlazni računi
            ItemContent = "Izlazni računi (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new OutputInvoiceSQLiteRepository().Sync(outputInvoiceService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Izlazni računi (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 27. Ulazni računi
            ItemContent = "Ulazni računi (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new InputInvoiceSQLiteRepository().Sync(inputInvoiceService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Ulazni računi (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 28. Države
            ItemContent = "Države (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new CountrySQLiteRepository().Sync(countryService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Države (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 29. Regioni
            ItemContent = "Regioni (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new RegionSQLiteRepository().Sync(regionService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Regioni (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 30. Opštine
            ItemContent = "Opštine (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new MunicipalitySQLiteRepository().Sync(municipalityService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Opštine (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 31. Gradovi
            ItemContent = "Gradovi (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new CitySQLiteRepository().Sync(cityService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Gradovi (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 32. Sektori
            ItemContent = "Sektori (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new SectorSQLiteRepository().Sync(sectorService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Sektori (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 33. Zanimanja
            ItemContent = "Zanimanja (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ProfessionSQLiteRepository().Sync(professionService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Zanimanja (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 34. Banke
            ItemContent = "Banke (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new BankSQLiteRepository().Sync(bankService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Banke (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 35. Agencije
            ItemContent = "Agencije (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new AgencySQLiteRepository().Sync(agencyService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Agencije (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 36. Gradilišta
            ItemContent = "Gradilišta (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ConstructionSiteSQLiteRepository().Sync(constructionSiteService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Gradilišta (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 37. Dokumenti gradilišta
            ItemContent = "Dokumenti gradilišta (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ConstructionSiteDocumentSQLiteRepository().Sync(constructionSiteDocumentService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Dokumenti gradilišta (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 38. Kalkulacije gradilišta
            ItemContent = "Kalkulacije gradilišta (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ConstructionSiteCalculationSQLiteRepository().Sync(constructionSiteCalculationService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Kalkulacije gradilišta (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 39. Napomene gradilišta
            ItemContent = "Napomene gradilišta (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ConstructionSiteNoteSQLiteRepository().Sync(constructionSiteNoteService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Napomene gradilišta (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 40. Poreska uprava
            ItemContent = "Poreska uprava (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new TaxAdministrationSQLiteRepository().Sync(taxAdministrationService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Poreska uprava (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 41. Ograničenja
            ItemContent = "Ograničenja (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new LimitationSQLiteRepository().Sync(limitationService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Ograničenja (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 42. Korisnici
            ItemContent = "Korisnici (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new UserSQLiteRepository().Sync(userService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Korisnici (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 43. PDV
            ItemContent = "PDV (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new VatSQLiteRepository().Sync(vatService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "PDV (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 44. Popusti
            ItemContent = "Popusti (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new DiscountSQLiteRepository().Sync(discountService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Popusti (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 45. Kurirska služba
            ItemContent = "Kurirska služba (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ServiceDeliverySQLiteRepository().Sync(serviceDeliveryService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Kurirska služba (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 46. Statusi
            ItemContent = "Statusi (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new StatusSQLiteRepository().Sync(statusService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Statusi (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 47. Pošiljke
            ItemContent = "Pošiljke (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ShipmentSQLiteRepository().Sync(shipmentService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Pošiljke (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 48. Podsetnik
            ItemContent = "Podsetnik (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new ToDoSQLiteRepository().Sync(toDoService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Podsetnik (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 49. Telefonski imenik
            ItemContent = "Telefonski imenik (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new PhonebookSQLiteRepository().Sync(phonebookService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Telefonski imenik (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            #region 50. Call centar
            ItemContent = "Call centar (" + ++counter + "/" + numOfTables + "): ";
            MinItemCounter = 0;
            ItemValue = 0;
            new CallCentarSQLiteRepository().Sync(callCentarService, (synced, toSync) =>
            {
                MaxItemCounter = toSync;
                ItemValue = synced;
                ItemContent = "Call centar (" + counter + "/" + numOfTables + "): " + synced + "/" + toSync;
            });
            #endregion

            ItemContent = "Sinhronizacija je uspešno izvršena!";
        }

        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

       
    }
}
