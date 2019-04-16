using System.Windows.Controls;
using System.Windows;
using Autodesk.Revit.UI;
using System;
using System.Windows.Navigation;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.DB;

namespace SimpleDockablePanel
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Page, Autodesk.Revit.UI.IDockablePaneProvider
    {

        #region Data
        private Guid m_targetGuid;
        private DockPosition m_position = DockPosition.Bottom;
        private int m_left = 1;
        private int m_right = 1;
        private int m_top = 1;
        private int m_bottom = 1;
        //const string _url_tbc = "http://thebuildingcoder.typepad.com";
        //const string _url_git = "https://github.com/jeremytammik/DockableDialog";
        #endregion

        

        public UserControl1()
        {   
            InitializeComponent();
        }
        
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState();
            data.InitialState.DockPosition = DockPosition.Tabbed;
            data.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
            

        }
        
        public void SetInitialDockingParameters(int left, int right, int top, int bottom, DockPosition position, Guid targetGuid)
        {
            m_position = position;
            m_left = left;
            m_right = right;
            m_top = top;
            m_bottom = bottom;
            m_targetGuid = targetGuid;
        }

        string doc = "";

        void Application_ViewActivated(object sender,  ViewActivatedEventArgs e)
        {
            doc = e.CurrentActiveView.Name;
            
        }

        private void PaneInfoButton_Click(object sender, RoutedEventArgs e)
        {
            //Helpers.ElementsCount(doc);

            TaskDialog.Show("result", doc);

        }

        private void DockableDialogs_Loaded(object sender, RoutedEventArgs e)
        {
            //web_browser.Navigated += new NavigatedEventHandler(WebBrowser_Navigated);


            
        }

        private void wpf_stats_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_getById_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_listTabs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
