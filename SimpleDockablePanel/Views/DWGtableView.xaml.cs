using Autodesk.Revit.UI;
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

namespace SimpleDockablePanel.Views
{
    /// <summary>
    /// Interaction logic for DWGtableView.xaml
    /// </summary>
    public partial class DWGtableView : UserControl
    {
        public DWGtableView()
        {
            InitializeComponent();

            ViewModels.DWGtableViewModel vm = new ViewModels.DWGtableViewModel();

            this.DataContext = vm;
        }
    }
}
