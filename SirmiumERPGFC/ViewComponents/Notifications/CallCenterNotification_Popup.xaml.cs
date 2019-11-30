using ServiceInterfaces.ViewModels.Common.CallCentars;
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

namespace SirmiumERPGFC.ViewComponents.Notifications
{
    /// <summary>
    /// Interaction logic for CallCenterNotification_Popup.xaml
    /// </summary>
    public partial class CallCenterNotification_Popup : Window
    {
        #region Attributes


        #region CurrentCallCentar
        private CallCentarViewModel _CurrentCallCentar = new CallCentarViewModel();

        public CallCentarViewModel CurrentCallCentar
        {
            get { return _CurrentCallCentar; }
            set
            {
                if (_CurrentCallCentar != value)
                {
                    _CurrentCallCentar = value;
                    NotifyPropertyChanged("CurrentCallCentar");
                }
            }
        }
        #endregion


        #region IsPopup
        private bool _IsPopup;

        public bool IsPopup
        {
            get { return _IsPopup; }
            set
            {
                if (_IsPopup != value)
                {
                    _IsPopup = value;
                    NotifyPropertyChanged("IsPopup");
                }
            }
        }
        #endregion

      
        #endregion

        #region Constructor

        public CallCenterNotification_Popup(CallCentarViewModel CallCentarViewModel)
        {
            InitializeComponent();

            this.DataContext = this;

            CurrentCallCentar = CallCentarViewModel;
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

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
