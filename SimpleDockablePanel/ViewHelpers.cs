using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleDockablePanel
{
    class ViewHelpers
    {
        public static void OpenView(UIApplication uiapp, View myView)
        {
            uiapp.ActiveUIDocument.RequestViewChange(myView);
        }

        public static void OpenView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewHelpers.OpenView(ShowDockableWindow._cachedUiApp, ShowDockableWindow.viewToOpen);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

    }
}
