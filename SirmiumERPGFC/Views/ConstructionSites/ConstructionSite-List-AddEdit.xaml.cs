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

namespace SirmiumERPGFC.Views.ConstructionSites
{
    /// <summary>
    /// Interaction logic for ConstructionSite_List_AddEdit.xaml
    /// </summary>
    public partial class ConstructionSite_List_AddEdit : UserControl
    {
        public ConstructionSite_List_AddEdit()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FlyoutHelper.CloseFlyout(this);
        }
    }
}
