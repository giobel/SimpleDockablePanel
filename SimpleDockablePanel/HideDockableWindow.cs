using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace SimpleDockablePanel
{
    /// <summary>
    /// Hide dockable dialog
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class HideDockableWindow : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            DockablePaneId dpid = new DockablePaneId(
              new Guid("{D7C963CE-B7CA-426A-8D51-6E8254D21157}"));

            DockablePane dp = commandData.Application
              .GetDockablePane(dpid);

            dp.Hide();
            return Result.Succeeded;
        }
    }
}
