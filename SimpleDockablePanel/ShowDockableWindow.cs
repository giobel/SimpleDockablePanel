using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SimpleDockablePanel
{


    /// <summary>
    /// Show dockable dialog
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class ShowDockableWindow : IExternalCommand
        
    {
        private UserControl1 m_MyDock = null;

        private Dictionary<int, string> dictionaryDB = new Dictionary<int, string>();

        DeleteViewHandler _deleteViewHandler;

        ExternalEvent deleteViewEvent;

        public static UIApplication _cachedUiApp;

        private Document mydoc = null;

        public static View viewToOpen = null;


        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            
            DockablePane dp = commandData.Application.GetDockablePane(Ribbon.dpid);

            mydoc = commandData.Application.ActiveUIDocument.Document;

            _cachedUiApp = commandData.Application;

            Autodesk.Revit.ApplicationServices.Application a = _cachedUiApp.Application;

            //a.DocumentOpening += new EventHandler<DocumentOpeningEventArgs>(Doc_opened);

            a.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementCreated_AddToDictionary);

            //a.DocumentChanged += new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(App_Changed);

            a.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementDeleted);

            a.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ElementCreated);

            a.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(ViewCreated);

            a.DocumentSynchronizedWithCentral += new EventHandler<DocumentSynchronizedWithCentralEventArgs>(DocSync);

            a.DocumentSaved += new EventHandler<DocumentSavedEventArgs>(Helpers.DocSaved);

            _cachedUiApp.Idling += OnIdling;

            m_MyDock = Ribbon.m_MyDock;

            m_MyDock.button.Click += ViewHelpers.OpenView_Click;

            m_MyDock.btnDeleteView.Click += DeleteView_Click;

            m_MyDock.btnDBConnect.Click += Button_DBConnect_Click;

            _deleteViewHandler = new DeleteViewHandler();

            deleteViewEvent = ExternalEvent.Create(_deleteViewHandler);

            dictionaryDB = Helpers.elementsToDictionary(mydoc); // elements type not included!

            m_MyDock.labelCount1.Text = Helpers.CountElementIds(mydoc).ToString();

            dp.Show();



            return Result.Succeeded;
        }//close Result



 



        private void DocSync(object sender, DocumentSynchronizedWithCentralEventArgs e)
        {
            string currentUser = _cachedUiApp.Application.Username;
            m_MyDock.txtBoxSyncTime.Text = String.Format("{0} {1}", Helpers.GetTime(), currentUser);
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
                m_MyDock.txtBoxActiveView.Text = String.Format("{0}", uiapp.ActiveUIDocument.ActiveView.Name);
            }

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
                    m_MyDock.labelView.Text = String.Format("{0} {1}", ele.Name, eid.ToString());
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

    }//close class
}//close namespace
