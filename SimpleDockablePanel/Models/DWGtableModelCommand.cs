using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockablePanel.Models
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DWGtableModelCommand : IExternalEventHandler
    {
        private int countDWg { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                int number = Helpers.CountDWGs(uiapp);
                TaskDialog.Show("result", String.Format("Number of DWWGs = {0}", number));

                countDWg = number;

                //Views.DWGtableView dWGtableView = new Views.DWGtableView();
                //dWGtableView.dwgNumber = number;

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }

        }

        public string GetName()
        {
            return "External Event Count DWGs";
        }

        public void PassValue(ObservableCollection<int> localValue)
        {
            localValue.Add(countDWg);
        }

    }
}
