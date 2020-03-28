using MahApps.Metro.Controls;
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

namespace SirmiumERPGFC.Views.Home
{
    /// <summary>
    /// Interaction logic for Scanner_Window.xaml
    /// </summary>
    public partial class Scanner_Window : MetroWindow
    {
        public Scanner_Window()
        {
            InitializeComponent();
            Scanner_List userControl = new Scanner_List();
            Wrapper.Children.Add(userControl);
        }
    }
}
