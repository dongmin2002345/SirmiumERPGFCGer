using ApiExtension.Sender;
using MahApps.Metro.Controls;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.Companies;
using ServiceInterfaces.ViewModels.Common.Identity;
using ServiceInterfaces.ViewModels.Common.ToDos;
using SirmiumERPGFC.Helpers;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.ViewComponents.Notifications;
using SirmiumERPGFC.Views.Administrations;
using SirmiumERPGFC.Views.Banks;
using SirmiumERPGFC.Views.BusinessPartners;
using SirmiumERPGFC.Views.ConstructionSites;
using SirmiumERPGFC.Views.Documents;
using SirmiumERPGFC.Views.Employees;
using SirmiumERPGFC.Views.Home;
using SirmiumERPGFC.Views.InputInvoices;
using SirmiumERPGFC.Views.Invoices;
using SirmiumERPGFC.Views.Locations;
using SirmiumERPGFC.Views.OutputInvoices;
using SirmiumERPGFC.Views.Prices;
using SirmiumERPGFC.Views.Profession;
using SirmiumERPGFC.Views.Sectors;
using SirmiumERPGFC.Views.Shipments;
using SirmiumERPGFC.Views.Statuses;
using SirmiumERPGFC.Views.TaxAdministrations;
using SirmiumERPGFC.Views.Users;
using SirmiumERPGFC.Views.Vats;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SirmiumERPGFC
{
    public partial class MainWindow : MetroWindow
    {
        #region Global attributes

        #region CurrentCompanyId
        private static int _CurrentCompanyId;

        public static int CurrentCompanyId
        {
            get { return _CurrentCompanyId; }
            set
            {
                if (_CurrentCompanyId != value)
                {
                    _CurrentCompanyId = value;
                }
            }
        }
        #endregion

        #region CurrentCompany
        private static CompanyViewModel _CurrentCompany;

        public static CompanyViewModel CurrentCompany
        {
            get { return _CurrentCompany; }
            set
            {
                if (_CurrentCompany != value)
                {
                    _CurrentCompany = value;
                }
            }
        }
        #endregion


        #region CurrentUserId
        private static int _CurrentUserId;

        public static int CurrentUserId
        {
            get { return _CurrentUserId; }
            set
            {
                if (_CurrentUserId != value)
                {
                    _CurrentUserId = value;
                }
            }
        }
        #endregion

        #region CurrentUser
        private static UserViewModel _CurrentUser;

        public static UserViewModel CurrentUser
        {
            get { return _CurrentUser; }
            set
            {
                if (_CurrentUser != value)
                {
                    _CurrentUser = value;
                }
            }
        }
        #endregion


        #region ConfirmationMessage
        private static string _ConfirmationMessage;

        public static string ConfirmationMessage
        {
            get { return _ConfirmationMessage; }
            set
            {
                if (_ConfirmationMessage != value)
                {
                    _ConfirmationMessage = value;
                }
            }
        }
        #endregion


        #region SuccessMessage
        private static String _SuccessMessage;

        public static String SuccessMessage
        {
            get { return _SuccessMessage; }
            set
            {
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    Notifier notifier = new Notifier(cfg =>
                    {
                        cfg.PositionProvider = new WindowPositionProvider(
                            parentWindow: Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(),
                            corner: Corner.TopRight,
                            offsetX: 10,
                            offsetY: 10);

                        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                            notificationLifetime: TimeSpan.FromSeconds(3),
                            maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                        cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
                    });

                    notifier.ShowSuccess(value);

                    _SuccessMessage = value;
                }));
            }
        }
        #endregion

        #region WarningMessage
        private static String _WarningMessage;

        public static String WarningMessage
        {
            get { return _WarningMessage; }
            set
            {
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    Notifier notifier = new Notifier(cfg =>
                    {
                        cfg.PositionProvider = new WindowPositionProvider(
                            parentWindow: Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(),
                            corner: Corner.TopRight,
                            offsetX: 10,
                            offsetY: 10);

                        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                            notificationLifetime: TimeSpan.FromSeconds(3),
                            maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                        cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
                    });

                    notifier.ShowWarning(value);

                    _WarningMessage = value;
                }));
            }
        }
        #endregion

        #region ErrorMessage
        private static String _ErrorMessage;

        public static String ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                {
                    try
                    {
                        Notifier notifier = new Notifier(cfg =>
                        {
                            cfg.PositionProvider = new WindowPositionProvider(
                                parentWindow: Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(),
                                corner: Corner.TopRight,
                                offsetX: 10,
                                offsetY: 10);

                            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                                notificationLifetime: TimeSpan.FromSeconds(3),
                                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                            cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
                        });

                        notifier.ShowError(value);
                    }
                    catch(Exception ex) { }
                    _ErrorMessage = value;
                }));
            }
        }
        #endregion

        #endregion


        private Notifier notifier;
        public bool UserIsSlaughter = false;
        public bool UserIsLivestockReception = false;
        public bool UserIsQuartering = false;
        public bool UserIsExpedition = false;
        public bool AppIsClosing = false;


        public static string OutlookDefinedPath = "";

        private HubConnection hubConnection;

        void LoadConfiguration()
        {
            try
            {
                var appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                OutlookDefinedPath = appConfig.AppSettings.Settings["OutlookDefinedPath"].Value;


                //ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                //map.ExeConfigFilename = Assembly.GetEntryAssembly().Location + ".config";
                //Configuration libConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                //AppSettingsSection section = (libConfig.GetSection("appSettings") as AppSettingsSection);
                //string tmpBaseAddress = section.Settings["BaseApiUrl"]?.Value;
            } catch(Exception ex)
            {

            }
        }

        public MainWindow()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            CustomPrincipal customPrincipal = Thread.CurrentPrincipal as CustomPrincipal;

            CurrentCompanyId = customPrincipal.Identity.CompanyId;
            CurrentCompany = new CompanyViewModel()
            {
                Id = customPrincipal.Identity.CompanyId,
                Identifier = customPrincipal.Identity.CompanyIdentifier,
                CompanyName = customPrincipal.Identity.CompanyName
            };
            CurrentUserId = customPrincipal.Identity.Id;
            CurrentUser = new UserViewModel()
            {
                Id = customPrincipal.Identity.Id,
                Identifier = customPrincipal.Identity.UserIdentifier,
                FirstName = customPrincipal.Identity.Name,
                LastName = customPrincipal.Identity.LastName
            };

            // First page to display is Home page
            cntCtrl.Content = new Home();

            LoadConfiguration();

            //OpenTab("Početna", new Home());

            this.Closing += MainWindow_Closing;

            Thread td = new Thread(() => {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(WpfApiHandler.GetPublicUrl() + "/notifications")
                    .Build();

                hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(15);
                hubConnection.On<CallCentarViewModel>("SendMessage", (msg) => {
                    Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                    {
                        CallCenterNotification_Popup popup = new CallCenterNotification_Popup(msg);
                        popup.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        popup.Owner = this;
                        popup.ShowDialog();
                    }));

                });
                hubConnection.On<ToDoViewModel>("SendToDo", (msg) => {
                    Application.Current.Dispatcher.BeginInvoke((System.Action)(() =>
                    {
                        //MainWindow.WarningMessage = $"TO-DO:\n{msg.Name}\n{msg.Description}";
                        ToDoNotification_Popup popup = new ToDoNotification_Popup(msg);
                        popup.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        popup.Owner = this;
                        popup.ShowDialog();
                    }));

                });
                hubConnection.Closed += HubConnection_Closed;
                hubConnection.ServerTimeout = TimeSpan.FromHours(1);
                hubConnection.On<CallCentarViewModel>("GroupMessage", (msg) => {
                });

                hubConnection.On<string>("AcceptedToNotificationChannel", (msg) => {
                    //Console.WriteLine("Welcome, " + msg);
                });

                hubConnection.StartAsync();

                hubConnection.SendAsync("JoinNotificationGroup", CurrentUser.Id);
            });
            td.IsBackground = true;
            td.Start();
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            if(!AppIsClosing)
            {
                await hubConnection.StartAsync();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppIsClosing = true;
            if (hubConnection != null)
            {
                try
                {
                    hubConnection.StopAsync();
                } catch(Exception ex)
                {
                    // Ignorisemo jer moze da se desi neocekivan prekid mreze
                }
            }
        }

        //private void OpenTab(string header, UserControl userControl)
        //{
        //    bool itemInList = false;
        //    CloseableTabItem selectedTabItem = null;
        //    foreach (CloseableTabItem item in contentTabControl.Items)
        //    {
        //        if (item.Uid == userControl.GetType().ToString())
        //        {
        //            itemInList = true;
        //            selectedTabItem = item;
        //        }
        //    }

        //    if (itemInList)
        //    {
        //        ((UIElement)selectedTabItem.Content).Visibility = Visibility.Visible; // show its contents
        //        selectedTabItem.Visibility = Visibility.Visible; // show the tab itself
        //        selectedTabItem.IsSelected = true; // select it
        //    }
        //    else
        //    {
        //        CloseableTabItem tabItem = new CloseableTabItem();
        //        tabItem.Header = header;
        //        tabItem.Uid = userControl.GetType().ToString();
        //        tabItem.Content = userControl;
        //        contentTabControl.Items.Insert(contentTabControl.Items.Count, tabItem);

        //        ((UIElement)tabItem.Content).Visibility = Visibility.Visible; // show its contents
        //        tabItem.Visibility = Visibility.Visible; // show the tab itself
        //        tabItem.IsSelected = true; // select it
        //    }
        //}

        //private void TabClosed(object source, RoutedEventArgs args)
        //{
        //    CloseableTabItem tabItem = (CloseableTabItem)args.Source; // get chosen tab
        //    ((UIElement)tabItem.Content).Visibility = Visibility.Collapsed; // collapse tab contents
        //    tabItem.Visibility = Visibility.Collapsed; // collapse tab
        //    tabItem.CIsVisible = false; // WPF bug workaround: set the binded property to false, as to change the IsChecked status

        //    if (contentTabControl.SelectedItem == tabItem)
        //    { // if no visible tab is selected
        //      // first, try to find a visible tab after the just-collapsed-tab
        //        for (int newIndex = contentTabControl.Items.IndexOf(tabItem) + 1;
        //                newIndex < contentTabControl.Items.Count; newIndex++)
        //        {
        //            TabItem newSelectedItem = (TabItem)contentTabControl.Items.GetItemAt(newIndex);
        //            if (newSelectedItem.Visibility == Visibility.Visible)
        //            {
        //                newSelectedItem.IsSelected = true;
        //                return; // a visible tab has been selected
        //            }
        //        }
        //        // second, try to find a visible tab before it
        //        for (int newIndex = contentTabControl.Items.IndexOf(tabItem) - 1;
        //                newIndex >= 0; newIndex--)
        //        {
        //            TabItem newSelectedItem = (TabItem)contentTabControl.Items.GetItemAt(newIndex);
        //            if (newSelectedItem.Visibility == Visibility.Visible)
        //            {
        //                newSelectedItem.IsSelected = true;
        //                return; // a visible tab has been selected
        //            }
        //        }
        //    }

        //    contentTabControl.Items.Remove(tabItem);
        //    tabItem.Content = null;
        //    tabItem = null;

        //}

        private void mniBusinessPartners_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new BusinessPartner_List();
            //throw new Exception();
            //OpenTab("Poslovni partneri", new BusinessPartnerList());
        }

        private void mniIndividuals_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Employee_List();
            //OpenTab("Radnici", new IndividualList());
        }

		private void mniPhysicalPersons_Click(object sender, RoutedEventArgs e)
		{
			cntCtrl.Content = new PhysicalPerson_List();
			//OpenTab("Radnici", new IndividualList());
		}

		private void mniOutputInvoices_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new OutputInvoiceList();
            //OpenTab("Izlazni računi", new OutputInvoiceList());
        }

		private void mniInputInvoices_Click(object sender, RoutedEventArgs e)
		{
			cntCtrl.Content = new InputInvoiceList();
			//OpenTab("Ulazni računi", new InputInvoiceList());
		}

		private void mniCities_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new CityList();
            //OpenTab("Gradovi", new CityList());
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Home();
            //OpenTab("Početna", new Home());
        }

        private void mniCountries_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new CountryList();
        }

        private void mniRegions_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new RegionList();
        }
    

		private void mniSector_Click(object sender, RoutedEventArgs e)
		{
			cntCtrl.Content = new Sector_List();
		}

        private void mniMunicipalities_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new MunicipalityList();
        }
        
        private void mniProfessions_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ProfessionList();
            //OpenTab("Poslovni partneri", new ProfessionList());
        }

        private void mniBanks_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Bank_List();
        }

        private void mniWorkActivity_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new AgencyList();
        }

        private void mniBusinessPartnerEmployees_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new BusinessPartnerEmployee_List();
        }

        private void mniConstructionSite_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ConstructionSite_List();
        }

        private void mniConstructionSiteEmployees_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ConstructionSiteEmployee_List();
        }

        private void mniFamilyMembers_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new FamilyMember_List();
        }

        private void mniConstructionSiteBusinessPartners_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ConstructionSiteBusinessPartner_List();
        }

        private void mniExcelImport_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new DataSync_List_Excel();
        }    

		private void mniLicenceType_Click(object sender, RoutedEventArgs e)
		{
			cntCtrl.Content = new LicenceType_List();
		}

        private void mniBusinessPartnerType_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new BusinessPartner_Type_List();
        }

        private void mniTaxAdministrations_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new TaxAdministrationList();
        }

        private void mniLimitations_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Limitations();
        }

        private void mniUsers_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new UserList();
        }

        private void MniVat_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Vat_List();
        }
        private void MniRabat_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Discount_List();
        }
        private void MniServiceDelivery_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ServiceDelivery_List();
        }

        private void MniStatus_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Status_List();
        }

        private void MniShipment_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Shipment_List();
        }

        private void mniInvoices_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new InvoiceList();
        }

        private void mniDocuments_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new Documents_Mailing_List();
        }

        private void MniToDoStatus_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new ToDoStatus_List();
        }

        private void mniDocumentList_Click(object sender, RoutedEventArgs e)
        {
            cntCtrl.Content = new DocumentTreeView();
        }
    }
}

