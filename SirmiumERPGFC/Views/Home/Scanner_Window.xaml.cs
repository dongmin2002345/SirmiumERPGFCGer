﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.Home
{
    /// <summary>
    /// Interaction logic for Scanner_Window.xaml
    /// </summary>
    public partial class Scanner_Window : MetroWindow, INotifyPropertyChanged
    {
        #region SelectedDocument
        private string _SelectedDocument;

        public string SelectedDocument
        {
            get { return _SelectedDocument; }
            set
            {
                if (_SelectedDocument != value)
                {
                    _SelectedDocument = value;
                    NotifyPropertyChanged("SelectedDocument");
                }
            }
        }
        #endregion


        public Scanner_Window()
        {
            InitializeComponent();

            this.DataContext = this;

            if (scannerList != null)
                scannerList.DocumentSaved += new DocumentSavedToPdfHandler(DocumentSavedHandler);
        }

        private void DocumentSavedHandler(string docPath)
        {
            SelectedDocument = docPath;
        }

        private void btnConfirmFolder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDocument == null)
            {
                MainWindow.ErrorMessage = "Morate skenirati dokument i sačuvati ga u PDF formatu da biste nastavili dalje!";
                return;
            }

            this.DialogResult = true;
        }

        private void btnCancelFolder_Click(object sender, RoutedEventArgs e)
        {
            SelectedDocument = null;
            this.DialogResult = false;
            this.Close();
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            Dispatcher.BeginInvoke((Action)(() => {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
        #endregion
    }
}
