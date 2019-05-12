using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockablePanel.Models
{

        class RevitBridgeCommand : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Application app = uiapp.Application;
                Document doc = uidoc.Document;

                try
                {
                    // Get all the wall types in the current project and convert them in a Dictionary.
                    FilteredElementCollector felc = new FilteredElementCollector(doc).OfClass(typeof(WallType));
                    Dictionary<string, int> dicwtypes = felc.ToDictionary(x => x.Name, y => y.Id.IntegerValue);
                    felc.Dispose();

                    // Create a view model that will be associated to the DataContext of the view.
                    ViewModels.ViewModelRevitBridge vmod = new ViewModels.ViewModelRevitBridge();
                    vmod.DicWallType = dicwtypes;
                    vmod.SelectedWallType = dicwtypes.First().Value;

                    // Create a new Revit model and assign it to the Revit model variable in the view model.
                    vmod.RevitModel = new Models.ModelRevitBridge(uiapp);

                    System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();

                    // Load the WPF window viewRevitbridge.
                    using (Views.RevitBridge view = new Views.RevitBridge())
                    {
                        System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(view);
                        helper.Owner = proc.MainWindowHandle;

                        // Assign the view model to the DataContext of the view.
                        view.DataContext = vmod;

                        if (view.ShowDialog() != true)
                        {
                            return Result.Cancelled;
                        }
                    }

                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }
            }
        }
    
}
