using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MySql.Data.MySqlClient;

namespace SimpleDockablePanel
{
    class Helpers
    {
        public static void ElementsCount(Document doc)
        {
            
            FilteredElementCollector fec = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            TaskDialog.Show("Result", fec.Count().ToString());
            
        }

        public static void OpenView(UIApplication uiapp, View myView)
        {
            uiapp.ActiveUIDocument.RequestViewChange(myView);
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

    }
}
