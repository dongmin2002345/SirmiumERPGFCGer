using Ninject;
using SirmiumERPGFC.Identity;
using SirmiumERPGFC.Identity.Views;
using SirmiumERPGFC.Infrastructure;
using SirmiumERPGFC.ViewComponents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using WpfAppCommonCode.Extensions;

namespace SirmiumERPGFC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //ISeedDataService seedData = DependencyResolver.Kernel.Get<ISeedDataService>();
            //seedData.PopulateData();

            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewKeyDownEvent, new KeyEventHandler(Grid_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(Texbox_GotFocusEvent));
            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.PreviewKeyDownEvent, new KeyEventHandler(Grid_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(DatePicker), DatePicker.PreviewKeyDownEvent, new KeyEventHandler(Grid_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(ComboBox), DatePicker.PreviewKeyDownEvent, new KeyEventHandler(Grid_PreviewKeyDown));

            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("\\Resources\\Languages\\StringResources-SRB.xaml", UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);

            base.OnStartup(e);

            //Show the login view
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void Texbox_GotFocusEvent(object sender, RoutedEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (uie != null)
            {
                if (uie.GetType() == typeof(TextBox))
                {
                    var textbox = (TextBox)uie;

                    textbox.SelectAll();
                }
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
                var ex = (Exception)e.Exception;
                string logMessage = ex.Message; // (Environment.StackTrace);

                string appPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                appPath += "\\DispatcherStackTrace.txt";

                try
                {
                    File.WriteAllText(appPath, logMessage);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.Message);
                }
            }
            Application.Current.Shutdown();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null) {
                var ex = (Exception)e.ExceptionObject;
                //string logMessage = ex.ToLogString(Environment.StackTrace);
                string logMessage = ex.Message;

                string appPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                appPath += "\\CurrentDomainStackTrace.txt";

                try
                {
                    File.WriteAllText(appPath, logMessage);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.Message);
                }
            }
            Application.Current.Shutdown();
        }

        private void Grid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

    }
}
