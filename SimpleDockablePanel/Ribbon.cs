#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
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

        DeleteViewHandler _deleteViewHandler;

        ExternalEvent deleteViewEvent;

        public static UIApplication _cachedUiApp;

        private Document mydoc = null;
        
        public static View viewToOpen = null;

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

            a.ControlledApplication.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementDeleted);

            a.ControlledApplication.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementCreated);

            a.ControlledApplication.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ViewCreated);

            a.ControlledApplication.DocumentSynchronizedWithCentral += new EventHandler<DocumentSynchronizedWithCentralEventArgs>(DocSync);

            a.ControlledApplication.DocumentSaved += new EventHandler<DocumentSavedEventArgs>(DocSaved);

            a.Idling += OnIdling;
            
            m_MyDock.button.Click += OpenView_Click;

            m_MyDock.btnDeleteView.Click += DeleteView_Click;

            m_MyDock.btnDBConnect.Click += Button_DBConnect_Click;

            _deleteViewHandler = new DeleteViewHandler();

            deleteViewEvent = ExternalEvent.Create(_deleteViewHandler);
            

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

        private void OpenView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewHelpers.OpenView(_cachedUiApp, viewToOpen);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        private void DocSaved(object sender, DocumentSavedEventArgs e)
        {
            string currentUser = _cachedUiApp.Application.Username;
            m_MyDock.txtBoxSyncTime.Text = String.Format("{0} {1}", Helpers.GetTime(), currentUser);
        }

        private void DocSync(object sender, DocumentSynchronizedWithCentralEventArgs e)
        {
            string currentUser = _cachedUiApp.Application.Username;
            m_MyDock.txtBoxSyncTime.Text = String.Format("{0} {1}",Helpers.GetTime(), currentUser);
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

        private void DeleteView_Click(object sender, RoutedEventArgs e)
        {
            deleteViewEvent.Raise();
            m_MyDock.labelView.Text = "Deleted";
            
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            UIApplication uiapp = sender as UIApplication;

            _cachedUiApp = uiapp;

            if (uiapp != null)
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                FilteredElementCollector fec = new FilteredElementCollector(doc).WhereElementIsNotElementType();
                m_MyDock.labelCount2.Text = fec.GetElementCount().ToString();
                m_MyDock.txtBoxActiveView.Text = String.Format("{0}",uiapp.ActiveUIDocument.ActiveView.Name);
            }
            
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

            m_MyDock.labelCount1.Text = elementIds.Count.ToString();
        }

        private void ElementDeleted(object sender, DocumentChangedEventArgs e)
        {
            string deletedElementsIds = "";

            ICollection<ElementId> deletedElements = e.GetDeletedElementIds();

            foreach (ElementId eid in deletedElements)
            {
                string value = "not in dictionary";

                dictionaryDB.TryGetValue(eid.IntegerValue, out value);

                dictionaryDB.Remove(eid.IntegerValue);

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
                    viewToOpen = ele as View;
                    m_MyDock.labelView.Text = String.Format("{0} Id: {1}", ele.Name, eid.ToString());
                }
                else
                {
                    m_MyDock.labelView.Text = "-";
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

            m_MyDock.labelCount2.Text = elementIds.Count.ToString();

        }


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }



    }

}
