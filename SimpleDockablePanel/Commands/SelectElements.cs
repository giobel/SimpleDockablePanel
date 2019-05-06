using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.UI.Selection;

namespace SimpleDockablePanel
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SelectBeams : IExternalEventHandler
    {

        public void Execute(UIApplication uiapp)
        {
            try
            {
                Helpers.SelectElementsFilter(uiapp, "Structural Framing");
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            
        }

        public string GetName()
        {
            return "External Event Select Beams";
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SelectWalls : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            try
            {
                Helpers.SelectElementsFilter(uiapp, "Walls");
            }

            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }
        public string GetName()
        {
            return "External Event Select Walls";
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SelectFloors : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            try
            {
                Helpers.SelectElementsFilter(uiapp, "Floors");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public string GetName()
        {
            return "External Event Select Floors";
        }
    }

}
