#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace RevitAddinWPF
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalEventHandler
    {
        public static ViewModels.viewmodelRevitBridge vmod { get; set; }

        public void Execute(UIApplication uiapp)
        {
            
            Application app = uiapp.Application;

            Document doc = uiapp.ActiveUIDocument.Document;

            try
            {
                // Get all the wall types in the current project and convert them in a Dictionary.
                FilteredElementCollector felc = new FilteredElementCollector(doc).OfClass(typeof(WallType));
                Dictionary<string, int> dicwtypes = felc.ToDictionary(x => x.Name, y => y.Id.IntegerValue);
                felc.Dispose();

                // Create a view model that will be associated to the DataContext of the view.

                //ViewModels.viewmodelRevitBridge 
                vmod = new ViewModels.viewmodelRevitBridge();
                vmod.DicWallType = dicwtypes;
                vmod.SelectedWallType = dicwtypes.First().Value;

                // Create a new Revit model and assign it to the Revit model variable in the view model.
                vmod.RevitModel = new Models.modelRevitBridge(uiapp);

                System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();

                
                /*
                
                // Load the WPF window viewRevitbridge.
                using (Views.viewRevitBridge view = new Views.viewRevitBridge())
                {
                    System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(view);
                    helper.Owner = proc.MainWindowHandle;

                    // Assign the view model to the DataContext of the view.
                    view.DataContext = vmod;
                    
                    

                    
                    if (view.ShowDialog() != true)
                    {
                        return;
                    }
                }
                */
                
            }
            catch
            {
                return;
            }

          



        }

        public string GetName()
        {
            return "External Event Bridge";
        }
    }
}
