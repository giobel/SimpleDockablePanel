#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using SimpleDockablePanel.Properties;

#endregion

namespace SimpleDockablePanel
{
    public class Ribbon : IExternalApplication
    {
        private UserControl1 m_MyDock = null;
        private Dictionary<int, string> dictionaryDB = new Dictionary<int, string>();

        public static UIApplication _cachedUiApp;

        private Document mydoc = null;
        
        private View viewToOpen = null;

        public Result OnStartup(UIControlledApplication a)
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

            DockablePaneProviderData data = new DockablePaneProviderData();

            UserControl1 MainDockableWindow = new UserControl1();

            m_MyDock = MainDockableWindow;

            data.FrameworkElement = MainDockableWindow as System.Windows.FrameworkElement;

            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,

                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            };

            DockablePaneId dpid = new DockablePaneId(new Guid("{D7C963CE-B7CA-426A-8D51-6E8254D21157}"));

            a.RegisterDockablePane(dpid, "Changes Tracker", MainDockableWindow as IDockablePaneProvider);

            a.ControlledApplication.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(Doc_opened);

            a.ControlledApplication.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementCreated_AddToDictionary);

            //a.ControlledApplication.DocumentChanged += new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(App_Changed);

            a.ControlledApplication.DocumentChanged += new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(ElementDeleted);

            a.ControlledApplication.DocumentChanged += new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(ElementCreated);

            a.ControlledApplication.DocumentChanged += new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(ViewCreated);

            a.Idling += OnIdling;

            m_MyDock.button.Click += Button_Click;

            m_MyDock.btnDBConnect.Click += Button_DBConnect_Click;
            
            return Result.Succeeded;
        }



        private void Button_DBConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_MyDock.txtBoxSize.Text = Helpers.ConnectDB("CQT");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.OpenView(_cachedUiApp, viewToOpen);
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            UIApplication uiapp = sender as UIApplication;

            _cachedUiApp = uiapp;

            if (uiapp != null)
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;
            }

            m_MyDock.txtBoxDebug.Text = uiapp.ActiveUIDocument.ActiveView.Name;
        }

        private void Doc_opened(object sender, DocumentOpenedEventArgs e)
        {
            Document doc = e.Document;

            mydoc = doc;

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

                        dictionaryDB.Add(elementIdKey, keyCategoryName);

                        //System.IO.File.AppendAllText(@"C:\Temp\RevitDB.txt", String.Format("{0} : {1}\n",elementIdKey, keyCategoryName));
                    }
                    catch
                    {
                    }
                }
            }

            m_MyDock.labelCount1.Content = elementIds.Count;
        }

        private void ElementDeleted(object sender, DocumentChangedEventArgs e)
        {
            string deletedElementsIds = "";

            ICollection<ElementId> deletedElements = e.GetDeletedElementIds();

            foreach (ElementId eid in deletedElements)
            {
                string value = "not in dictionary";

                dictionaryDB.TryGetValue(eid.IntegerValue, out value);

                deletedElementsIds += String.Format("{0} : {1}\n", eid.IntegerValue, value);

            }

            m_MyDock.txtBoxDeleted.Text = String.Format("{0} element(s) deleted: \n{1}", deletedElements.Count, deletedElementsIds);
        }

        private void ElementCreated_AddToDictionary(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            ICollection<ElementId> createdElements = e.GetAddedElementIds();

            foreach (ElementId eid in createdElements)
            {

                Element ele = doc.GetElement(eid);

                    try
                    {
                        int elementIdKey = eid.IntegerValue;

                        if (ele.Category.Name != null)
                        {
                            string keyCategoryName = ele.Category.Name;
                            dictionaryDB.Add(elementIdKey, keyCategoryName);
                        }

                        //System.IO.File.AppendAllText(@"C:\Temp\RevitDB.txt", String.Format("{0} : {1}\n",elementIdKey, keyCategoryName));
                    }
                    catch
                    {
                    }
            }
        }



        private void ElementCreated(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            string createdElementsIds = "";

            ICollection<ElementId> createdElements = e.GetAddedElementIds();

            foreach (ElementId eid in createdElements)
            {
                string value = "not in dictionary";

                dictionaryDB.TryGetValue(eid.IntegerValue, out value);

                createdElementsIds += String.Format("{0} : {1}\n", eid.IntegerValue, value);

            }

            m_MyDock.txtBoxCreated.Text = String.Format("{0} element(s) created: \n{1}", createdElements.Count, createdElementsIds);
        }

        private void ViewCreated(object sender, DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

            ICollection<ElementId> createdElements = e.GetAddedElementIds();

            foreach (ElementId eid in createdElements)
            {

                Element ele = doc.GetElement(eid);
                if (ele.Category.Name == "Views")
                {
                    m_MyDock.labelView.Content = eid;
                    viewToOpen = ele as View;
                }
                else
                {
                    m_MyDock.labelView.Content = "-";
                }

            }
        }

        private void App_Changed(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            Document doc = e.GetDocument();

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

                        dictionaryDB.Add(elementIdKey, keyCategoryName);

                        //System.IO.File.AppendAllText(@"C:\Temp\RevitDB.txt", String.Format("{0} : {1}\n",elementIdKey, keyCategoryName));
                    }
                    catch
                    {
                    }
                } 
            }

            m_MyDock.labelCount2.Content = elementIds.Count;

        }


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }


        private BitmapSource GetImage(IntPtr bm)
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

}
