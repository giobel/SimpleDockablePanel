using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDockablePanel.Commands
{
    public class SelectTabViewModel
    {

        private bool SelectExec(object obj)
        {
            return true;
        }

        public RelayCommand SelectAllInViewClick { get; set; }
        public RelayCommand SelectBeamsClick { get; set; }
        public RelayCommand SelectFloorsClick { get; set; }
        public RelayCommand SelectWallsClick { get; set; }

        private SelectAllInView _selectAllInViewHandler;
        private ExternalEvent _selectAllInViewEvent;

        private SelectBeams _selectBeamsHandler;
        private ExternalEvent _selectBeamsEvent;

        private SelectWalls _selectWallsHandler;
        private ExternalEvent _selectWallsEvent;

        private SelectFloors _selectFloorsHandler;
        private ExternalEvent _selectFloorsEvent;


        public SelectTabViewModel()
        {
            SelectAllInViewClick = new RelayCommand(SelectAllInViewRaise, SelectExec);
            SelectBeamsClick = new RelayCommand(SelectBeamsRaise, SelectExec);
            SelectWallsClick = new RelayCommand(SelectWallsRaise, SelectExec);
            SelectFloorsClick = new RelayCommand(SelectFloorsRaise, SelectExec);
        }

        public void SelectAllInView()
        {
            _selectAllInViewHandler = new SelectAllInView();
            _selectAllInViewEvent = ExternalEvent.Create(_selectAllInViewHandler);
        }

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


        public void SelectAllInViewRaise(object obj)
        {
            _selectAllInViewEvent.Raise();
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
}
