using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace SirmiumERPGFC.Views.Common
{
    public static class SirmiumERPVisualEffects
    {
        public static void AddEffectOnDialogShow(UserControl control)
        {
            control.IsEnabled = false;
            control.Opacity = 0.5;
            control.Effect = new BlurEffect();
        }

        public static void RemoveEffectOnDialogShow(UserControl control)
        {
            control.IsEnabled = true;
            control.Opacity = 1;
            control.Effect = null;
        }

        public static void AddEffectOnDialogShow(Window control)
        {
            control.IsEnabled = false;
            control.Opacity = 0.5;
            control.Effect = new BlurEffect();
        }

        public static void RemoveEffectOnDialogShow(Window control)
        {
            control.IsEnabled = true;
            control.Opacity = 1;
            control.Effect = null;
        }
    }
}