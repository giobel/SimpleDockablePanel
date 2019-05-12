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

namespace RevitAddinWPF.Views
{
    /// <summary>
    /// Interaction logic for viewRevitBridge.xaml
    /// </summary>
    public partial class viewRevitBridge : UserControl, IDisposable
    {
        public viewRevitBridge()
        {
            InitializeComponent();
            //Views.viewRevitBridge view = new Views.viewRevitBridge();
            //this.DataContext = Views.viewRevitBridge;
        }

        public void Dispose()
        {
            //this.Close();
        }

        private void bOk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
