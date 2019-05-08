using Autodesk.Revit.UI;
using SimpleDockablePanel.ViewModels;
using System.Collections.Generic;
using SimpleDockablePanel.Models;

namespace SimpleDockablePanel.ViewModels
{
    public class ImportDWGViewModel
    {
        public List<ImportedDWG> DWGlist { get; set; }

        private bool SelectExec(object obj)
        {
            return true;
        }

        public RelayCommand RefreshImportedDWGClick { get; set; }

        private FindImportedDWG _importedDWGViewHandler;
        private ExternalEvent _importedDWGEvent;

        

        public void ImportedDWGRaise(object obj)
        {
            
            _importedDWGEvent.Raise();
        }

        public void FindImportedDWGCommand()
        {
            _importedDWGViewHandler = new Models.FindImportedDWG();
            _importedDWGEvent = ExternalEvent.Create(_importedDWGViewHandler);
        }

        public ImportDWGViewModel()
        {
            
            //FindImportedDWGCommand();
            
            RefreshImportedDWGClick = new RelayCommand(ImportedDWGRaise, SelectExec);

            
        }

        
            


        //public IList<ImportedDWG> pierino => DWGlist;
    }
}
