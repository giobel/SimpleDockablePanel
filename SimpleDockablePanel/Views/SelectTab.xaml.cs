using System.Windows;
using System.Windows.Controls;

namespace SimpleDockablePanel.Views
{

    public partial class SelectTab : UserControl
    {

        public SelectTab()
        {
            InitializeComponent();
            this.DataContext = new Commands.SelectTabViewModel();

            (DataContext as Commands.SelectTabViewModel).SelectAllInView();

            (DataContext as Commands.SelectTabViewModel).SelectBeams();

            (DataContext as Commands.SelectTabViewModel).SelectWalls();

            (DataContext as Commands.SelectTabViewModel).SelectFloors();


        }
        /*
        private void ButtonWalls_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as Commands.SelectTabViewModel).SelectWallsRaise();   
        }

        private void ButtonBeams_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as Commands.SelectTabViewModel).SelectBeamsRaise();
        }

        private void ButtonFloors_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as Commands.SelectTabViewModel).SelectFloorsRaise();
        }*/
    }
}
