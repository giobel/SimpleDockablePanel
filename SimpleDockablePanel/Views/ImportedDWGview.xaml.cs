using SimpleDockablePanel.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SimpleDockablePanel.Views
{
    /// <summary>
    /// Interaction logic for ImportedDWGview.xaml
    /// </summary>
    public partial class ImportedDWGview : UserControl
    {
        public ImportedDWGview()
        {
            InitializeComponent();
            ViewModels.ImportDWGViewModel import = new ViewModels.ImportDWGViewModel();
            this.DataContext = import;

            //listBox1.ItemsSource = import.DWGlist;

        }

    }
}
