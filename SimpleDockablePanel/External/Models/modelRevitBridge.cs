using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinWPF.Models
{
    class modelRevitBridge
    {
        private UIApplication UIAPP = null;
        private Application APP = null;
        private UIDocument UIDOC = null;
        private Document DOC = null;

        //constructor
        public modelRevitBridge(UIApplication uiapp)
        {
            UIAPP = uiapp;
            APP = uiapp.Application;
            UIDOC = uiapp.ActiveUIDocument;
            DOC = UIDOC.Document;
        }

        //public function called from ViewModel
        public List<string> GenerateParametersAndValues(int idIntegerValue)
        {
            List<string> resstr = new List<string>();

            Element el = DOC.GetElement(new ElementId(idIntegerValue));
            if (el != null)
            {
                foreach (Parameter prm in el.Parameters)
                {
                    string str = prm.Definition.Name;
                    str += " : ";
                    str += prm.AsValueString();

                    resstr.Add(str);
                }
            }

            return resstr.OrderBy(x => x).ToList();
        }
    }
}
