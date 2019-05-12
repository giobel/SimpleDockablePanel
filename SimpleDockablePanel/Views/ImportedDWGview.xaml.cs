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

            this.DataContext = new ViewModels.ImportDWGViewModel();


        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            stPanel.Children.Clear();

            RevitAddinWPF.Views.viewRevitBridge view = new RevitAddinWPF.Views.viewRevitBridge();

            view.DataContext = RevitAddinWPF.Command.vmod;

            stPanel.Children.Add(view);
        }
    }
}
