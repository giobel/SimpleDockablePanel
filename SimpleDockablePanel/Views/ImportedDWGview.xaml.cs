using Autodesk.Revit.UI;
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

            _DWGHandler = new RevitAddinWPF.Command();
            _DWGEvent = ExternalEvent.Create(_DWGHandler);

            _DWGEvent.Raise();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            _DWGEvent.Raise();

            stPanel.Children.Clear();

            RevitAddinWPF.Views.viewRevitBridge view = new RevitAddinWPF.Views.viewRevitBridge();

            view.DataContext = RevitAddinWPF.Command.vmod;

            stPanel.Children.Add(view);
        }



        private RevitAddinWPF.Command _DWGHandler;
        private ExternalEvent _DWGEvent;

        public void DWGeventHandler()
        {
            _DWGHandler = new RevitAddinWPF.Command();
            _DWGEvent = ExternalEvent.Create(_DWGHandler);

        }

        public void DWGRaise(object obj)
        {
            _DWGEvent.Raise();
        }
    }
}
