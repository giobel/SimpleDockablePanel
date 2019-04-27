using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MySql.Data.MySqlClient;
using SimpleDockablePanel.Properties;

namespace SimpleDockablePanel
{
    class Helpers
    {
        public static void ElementsCount(Document doc)
        {
            
            FilteredElementCollector fec = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            TaskDialog.Show("Result", fec.Count().ToString());
            
        }


        public static string ConnectDB(string tableName)
        {
            string server = "remotemysql.com";
            string database = "r7BFoOjCty";
            string uid = "r7BFoOjCty";
            string password = "1vU3s1bj6T";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            // string table = "filesize";
            //string table = "CQT";

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmdRead = new MySqlCommand("", connection);
            cmdRead.CommandText = "SELECT * FROM " + tableName;

            string report = "";

            try
            {

                connection.Open();
                MySqlDataReader reader = cmdRead.ExecuteReader();

                while (reader.Read())
                {

                    
                    
                    report += reader.GetInt16("id") + "\n";
                    report += reader.GetDateTime("date") + "\n";
                    report += reader.GetString("user") + "\n";
                    report += reader.GetInt32("rvtFileSize") + "\n";
                    report += reader.GetInt32("elementsCount") + "\n";
                    report += reader.GetInt32("typesCount") + "\n";
                    report += reader.GetInt32("sheetsCount") + "\n";
                    report += reader.GetInt32("viewsCount") + "\n";
                    report += reader.GetInt32("viewportsCount") + "\n";
                    try
                    {
                        report += reader.GetInt32("warningsCount");
                    }
                    catch
                    {
                        
                    }
                    

                }
                reader.Close();

                MessageBox.Show(string.Format("Connection with DB {0} established", tableName));
                return report;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }//close method

        public static string GetTime()
        {
            DateTime timeSync = DateTime.Now;
            return String.Format("{0:HH:mm:ss}", timeSync);
        }

        public static void CreateTab(UIControlledApplication a)
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
        }

        private static BitmapSource GetImage(IntPtr bm)
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
