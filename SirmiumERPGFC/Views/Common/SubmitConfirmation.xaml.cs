using System;
using System.Collections.Generic;
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

namespace SirmiumERPGFC.Views.Common
{
    public partial class SubmitConfirmation : Window
    {
        public SubmitConfirmation()
        {
            InitializeComponent();
            tbSubmitMessage.Text = "Nakon proknjižavanja podatke nije moguće promeniti! Da li ste sigurni?";
            btnOK.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
