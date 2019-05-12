using System;
using Autodesk.Revit.UI;

namespace SimpleDockablePanel.Models
{
    public class SelectHelpers
    {


        private SelectBeams _selectBeamsHandler;
        private ExternalEvent _selectBeamsEvent;

        private SelectWalls _selectWallsHandler;
        private ExternalEvent _selectWallsEvent;

        private SelectFloors _selectFloorsHandler;
        private ExternalEvent _selectFloorsEvent;



        public void SelectBeams()
        {
            _selectBeamsHandler = new SelectBeams();
            _selectBeamsEvent = ExternalEvent.Create(_selectBeamsHandler);
        }

        public void SelectWalls()
        {
            _selectWallsHandler = new SelectWalls();
            _selectWallsEvent = ExternalEvent.Create(_selectWallsHandler);

        }

        public void SelectFloors()
        {
            _selectFloorsHandler = new SelectFloors();
            _selectFloorsEvent = ExternalEvent.Create(_selectFloorsHandler);
        }



        public void SelectBeamsRaise(object obj)
        {
            _selectBeamsEvent.Raise();
        }

        public void SelectWallsRaise(object obj)
        {
            _selectWallsEvent.Raise();
        }

        public void SelectFloorsRaise(object obj)
        {
            _selectFloorsEvent.Raise();
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SelectAllInView : IExternalEventHandler
    {

        public void Execute(UIApplication uiapp)
        {
            try
            {
                Helpers.SelectAllInView(uiapp);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }

        }

        public string GetName()
        {
            return "External Event Select All";
        }
    }


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

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CountDWGItems : IExternalEventHandler
    {

        public void Execute(UIApplication uiapp)
        {
            try
            {
                int number = Helpers.CountDWGs(uiapp);
                TaskDialog.Show("result", String.Format("Number of DWWGs = {0}", number));
                DWGcontainer dw = new DWGcontainer();
                dw.countDWG= number;
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
    }

}
