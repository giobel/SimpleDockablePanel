using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SimpleDockablePanel
{
    class Helpers
    {
        public static void ElementsCount(Document doc)
        {
            
            FilteredElementCollector fec = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            TaskDialog.Show("Result", fec.Count().ToString());
            
        }

        public static void OpenView(UIApplication uiapp, View myView)
        {
            uiapp.ActiveUIDocument.RequestViewChange(myView);
        }


    }
}
