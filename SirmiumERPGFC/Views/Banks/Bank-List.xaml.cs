﻿using SirmiumERPGFC.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SirmiumERPGFC.Views.Banks
{
    /// <summary>
    /// Interaction logic for Bank_List.xaml
    /// </summary>
    public partial class Bank_List : UserControl
    {
        public Bank_List()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.OpenFlyout(this, "Podaci o bankama", 95, new Bank_List_AddEdit());
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
