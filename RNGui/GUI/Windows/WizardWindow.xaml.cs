using CSX.Wpf.Y2022.RNGui.Support;
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

namespace CSX.Wpf.Y2022.RNGui.GUI.Windows
{
    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    public partial class WizardWindow : Window
    {
        private readonly WizardData _data = new();
        internal WizardData Data => _data;

        public WizardWindow()
        {
            InitializeComponent();

            DataContext = Data;
        }

        private void WizTab_Click(object sender, RoutedEventArgs e)
            => WizPager.SelectedIndex = WizTabPanel.Children.IndexOf(sender as RadioButton);
    }
}
