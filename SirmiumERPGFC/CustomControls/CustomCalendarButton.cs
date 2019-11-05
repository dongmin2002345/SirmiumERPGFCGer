using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SirmiumERPGFC.CustomControls
{
    public class CustomCalendarButton : Button, INotifyPropertyChanged
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached(
          "IsSelected",
          typeof(bool),
          typeof(CustomCalendarButton),
          new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, (depobj, propchangedargs) => {

          })
        );

        #region IsSelected

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }
        #endregion

        public CustomCalendarButton() : base() { }

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
