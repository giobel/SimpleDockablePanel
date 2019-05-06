using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MySql.Data.MySqlClient;
using SimpleDockablePanel.Properties;

namespace SimpleDockablePanel
{
   

    class Helpers
    {


        public static void ElementsCount(Document doc)
        {
            
            FilteredElementCollector fec = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            TaskDialog.Show("Result", fec.Count().ToString());
            
        }

        public static void SelectElementsFilter(UIApplication uiapp, string categoryName)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<ElementId> elementSet = new List<ElementId>();

            ISelectionFilter beamFilter = new ElementSelectionFilter(categoryName);

            IList<Reference> selectedIds = uidoc.Selection.PickObjects(ObjectType.Element, beamFilter, "Select Beams");

            foreach (Reference r in selectedIds)
            {
                Element elements = uidoc.Document.GetElement(r);
                elementSet.Add(r.ElementId);

            }
            uidoc.Selection.SetElementIds(elementSet);
        }

        public static void DocSaved(object sender, DocumentSavedEventArgs e)
        {
            
            string currentUser = ShowDockableWindow._cachedUiApp.Application.Username;
            Ribbon.m_MyDock.txtBoxSyncTime.Text = String.Format("{0} {1}", Helpers.GetTime(), currentUser);
        }

        public static Dictionary<int, string> elementsToDictionary(Document doc)
        {

            Dictionary<int, string> myDictionary = new Dictionary<int, string>();

            ICollection<ElementId> elementIds = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElementIds();

            foreach (var eid in elementIds)
            {
                Element ele = doc.GetElement(eid);

                if (ele.Category != null && ele.Category.Name != null)
                {
                    try
                    {
                        int elementIdKey = eid.IntegerValue;
                        string keyCategoryName = doc.GetElement(eid).Category.Name;

                        myDictionary.Add(elementIdKey, keyCategoryName);

                        //System.IO.File.AppendAllText(@"C:\Temp\RevitDB.txt", String.Format("{0} : {1}\n",elementIdKey, keyCategoryName));
                    }
                    catch
                    {
                        
                    }
                }
            }

            return myDictionary;
        }

        public static int CountElementIds(Document doc)
        {
            ICollection<ElementId> elementIds = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToElementIds();

            return elementIds.Count();

        }

        public static string GetTime()
        {
            DateTime timeSync = DateTime.Now;
            return String.Format("{0:HH:mm:ss}", timeSync);
        }

        public static void CreateTab(UIControlledApplication a)
        {
            a.CreateRibbonTab("Changes Tracker");

            RibbonPanel AECPanelDebug = a.CreateRibbonPanel("Changes Tracker", "AEC LABS");

            string path = Assembly.GetExecutingAssembly().Location;

            #region DockableWindow

            PushButtonData pushButtonShowDockableWindow = new PushButtonData("Show DockableWindow", "Show DockableWindow", path, "SimpleDockablePanel.ShowDockableWindow");
            pushButtonShowDockableWindow.LargeImage = GetImage(Resources.red.GetHbitmap());

            PushButtonData pushButtonHideDockableWindow = new PushButtonData("Hide DockableWindow", "Hide DockableWindow", path, "SimpleDockablePanel.HideDockableWindow");
            pushButtonHideDockableWindow.LargeImage = GetImage(Resources.orange.GetHbitmap());

            RibbonItem ri2 = AECPanelDebug.AddItem(pushButtonShowDockableWindow);
            RibbonItem ri3 = AECPanelDebug.AddItem(pushButtonHideDockableWindow);
            #endregion
        }

        private static BitmapSource GetImage(IntPtr bm)
        {
            BitmapSource bmSource
              = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bm,
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bmSource;
        }

    }

    public class ElementSelectionFilter : ISelectionFilter
    {

        public string catNameChosen { get; set; }

        public ElementSelectionFilter(string catName)
        {
            this.catNameChosen = catName;
        }

        public bool AllowElement(Element e)
        {

            if (e.Category.Name == catNameChosen)
            {
                return true;
            }
            return false;
        }
        public bool AllowReference(Reference refer, XYZ point)
        {
            return false;
        }

    }//close class
}
