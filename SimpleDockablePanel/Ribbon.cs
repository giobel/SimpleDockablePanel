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
        public static UserControl1 m_MyDock = null;

        public static DockablePaneId dpid = new DockablePaneId(new Guid("{D7C963CE-B7CA-426A-8D51-6E8254D21157}"));

        public Result OnStartup(UIControlledApplication a)
        {
            a.CreateRibbonTab("Changes Tracker");

            RibbonPanel AECPanelDebug = a.CreateRibbonPanel("Changes Tracker", "AEC LABS");

            string path = Assembly.GetExecutingAssembly().Location;

            #region DockableWindow

            PushButtonData pushButtonShowDockableWindow = new PushButtonData("Show DockableWindow", "Start", path, "SimpleDockablePanel.ShowDockableWindow");
            pushButtonShowDockableWindow.LargeImage = GetImage(Resources.green.GetHbitmap());

            PushButtonData pushButtonHideDockableWindow = new PushButtonData("Hide DockableWindow", "Hide DockableWindow", path, "SimpleDockablePanel.HideDockableWindow");
            pushButtonHideDockableWindow.LargeImage = GetImage(Resources.red.GetHbitmap());

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
            
            a.RegisterDockablePane(dpid, "Changes Tracker", MainDockableWindow as IDockablePaneProvider);
            
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


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }



    }

}
