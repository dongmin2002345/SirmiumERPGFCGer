using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SirmiumERPGFC.Common
{
    public static class FlyoutHelper
    {
        public static void CloseFlyout(this DependencyObject depObject)
        {
            Application.Current.Dispatcher.BeginInvoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() =>
                {
                    var mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() as Window;
                    if (mainWindow == null)
                    {
                        mainWindow = System.Windows.Window.GetWindow(depObject);

                    }
                    object obj = mainWindow.FindName("mainFlyout");
                    Flyout flyout = (Flyout)obj;
                    flyout.IsOpen = false;
                }));
        }

        public static void OpenFlyout(this DependencyObject depObject, string header, double width, object content, bool isModal = true)
        {
            var mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() as Window;
            if (mainWindow == null)
            {
                mainWindow = System.Windows.Window.GetWindow(depObject);

            }
            object obj = mainWindow.FindName("mainFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Header = header;
            var widthNew = (mainWindow.ActualWidth * (width / 100));
            flyout.Width = widthNew;
            flyout.Content = content;
            flyout.IsModal = isModal;
            flyout.IsOpen = true;
        }


        public static void OpenFlyoutPopup(this DependencyObject depObject, string header, double width, object content)
        {
            var mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() as Window;
            if (mainWindow == null)
            {
                mainWindow = System.Windows.Window.GetWindow(depObject);

            }
            object obj = mainWindow.FindName("popupFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Header = header;
            var widthNew = (mainWindow.ActualWidth * (width / 100));
            flyout.Width = widthNew;
            flyout.Content = content;
            flyout.IsOpen = true;
        }

        public static void CloseFlyoutForList(this DependencyObject depObject)
        {

            var mainWindow = System.Windows.Window.GetWindow(depObject);


            object obj = mainWindow.FindName("mainFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.IsOpen = false;
        }

        public static void OpenFlyoutForList(this DependencyObject depObject, string header, double width, object content)
        {

            var mainWindow = System.Windows.Window.GetWindow(depObject);

            object obj = mainWindow.FindName("mainFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Header = header;
            var widthNew = (mainWindow.ActualWidth * (width / 100));
            flyout.Width = widthNew;
            flyout.Content = content;
            flyout.IsOpen = true;
        }

        public static void CloseFlyoutPopup(this DependencyObject depObject)
        {
            var mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() as Window;
            if (mainWindow == null)
            {
                mainWindow = System.Windows.Window.GetWindow(depObject);
            }
            object obj = mainWindow.FindName("popupFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.IsOpen = false;
        }

    }
}

