using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;


namespace SimpleDockablePanel
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DeleteViewHandler : IExternalEventHandler
    {

        public static int executed = 0;

        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;

            using (Transaction t = new Transaction(doc, "Delete View"))
            {
                try
                {
                    t.Start();
                    doc.Delete(Ribbon.viewToOpen.Id);
                    t.Commit();

                    executed ++;
                }

                #region catch and finally
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", "Can't delete the view.\n" + Environment.NewLine + ex.Message);
                    
                }
                finally
                {

                }
                #endregion

            }//close using
        }

        public string GetName()
        {
            return "External Event Example";
        }
    }
}
