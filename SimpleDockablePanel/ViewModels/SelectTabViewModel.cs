using Autodesk.Revit.UI;
using SimpleDockablePanel.Models;

namespace SimpleDockablePanel.ViewModels
{
    public class SelectTabViewModel
    {
        private SelectHelpers sh = new SelectHelpers();

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

        public void SelectAllInView()
        {
            _selectAllInViewHandler = new SelectAllInView();
            _selectAllInViewEvent = ExternalEvent.Create(_selectAllInViewHandler);
        }

        public void SelectAllInViewRaise(object obj)
        {
            _selectAllInViewEvent.Raise();
        }


        public SelectTabViewModel()
        {

            SelectAllInView(); //create handle and event
            sh.SelectBeams();
            sh.SelectWalls();
            sh.SelectFloors();

            SelectAllInViewClick = new RelayCommand(SelectAllInViewRaise, SelectExec);
            SelectBeamsClick = new RelayCommand(sh.SelectBeamsRaise, SelectExec);
            SelectWallsClick = new RelayCommand(sh.SelectWallsRaise, SelectExec);
            SelectFloorsClick = new RelayCommand(sh.SelectFloorsRaise, SelectExec);
        }

       



    }
}
