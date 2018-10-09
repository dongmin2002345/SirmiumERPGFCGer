using ServiceInterfaces.ViewModels.Common.BusinessPartners;
using SirmiumERPGFC.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.BusinessPartners
{
    public partial class BusinessPartnerEmployee_List : UserControl, INotifyPropertyChanged
    {
        #region BusinessPartnersFromDB
        private ObservableCollection<BusinessPartnerViewModel> _BusinessPartnersFromDB;

        public ObservableCollection<BusinessPartnerViewModel> BusinessPartnersFromDB
        {
            get { return _BusinessPartnersFromDB; }
            set
            {
                if (_BusinessPartnersFromDB != value)
                {
                    _BusinessPartnersFromDB = value;
                    NotifyPropertyChanged("BusinessPartnersFromDB");
                }
            }
        }
        #endregion

        public BusinessPartnerEmployee_List()
        {
            InitializeComponent();

            this.DataContext = this;

            BusinessPartnersFromDB = new ObservableCollection<BusinessPartnerViewModel>()
            {
                new BusinessPartnerViewModel()
                {
                    Code = "123",
                    Name = "SirmiumERP d.o.o."
                }
            };
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFirstPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLastPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.OpenFlyout(this, "Unos zaposlenih", 95, new BusinessPartnerEmployee_List_AddEdit());
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;


        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged(String propertyName) // [CallerMemberName] 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
