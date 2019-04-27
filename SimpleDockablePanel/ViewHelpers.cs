using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockablePanel
{
    class ViewHelpers
    {
        public static void OpenView(UIApplication uiapp, View myView)
        {
            uiapp.ActiveUIDocument.RequestViewChange(myView);
        }

    }
}
