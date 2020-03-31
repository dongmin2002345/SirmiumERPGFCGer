using MahApps.Metro.Controls;
using SirmiumERPGFC.Common;
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

namespace SirmiumERPGFC.ViewComponents.Dialogs
{
    /// <summary>
    /// Interaction logic for DocumentSelectDialog.xaml
    /// </summary>
    public partial class DocumentSelectDialog : MetroWindow, INotifyPropertyChanged
    {
        #region SelectedDocument
        private DirectoryTreeItemViewModel _SelectedDocument;

        public DirectoryTreeItemViewModel SelectedDocument
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



        public DocumentSelectDialog()
        {
            InitializeComponent();

            this.DataContext = this;
        }


        private void btnConfirmFolder_Click(object sender, RoutedEventArgs e)
        {
            var selectedDoc = documentExplorer?.SelectedDocument;
            if (selectedDoc == null)
            {
                MainWindow.ErrorMessage = "Morate odabrati folder da biste potvrdili putanju!";
                return;
            }

            SelectedDocument = selectedDoc;
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
