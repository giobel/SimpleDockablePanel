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


        public SelectTabViewModel()
        {
            sh.SelectAllInView(); //create handle and event
            sh.SelectBeams();
            sh.SelectWalls();
            sh.SelectFloors();

            SelectAllInViewClick = new RelayCommand(sh.SelectAllInViewRaise, SelectExec);
            SelectBeamsClick = new RelayCommand(sh.SelectBeamsRaise, SelectExec);
            SelectWallsClick = new RelayCommand(sh.SelectWallsRaise, SelectExec);
            SelectFloorsClick = new RelayCommand(sh.SelectFloorsRaise, SelectExec);
        }

       



    }
}
