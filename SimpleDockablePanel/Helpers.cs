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

            string user = "";

            try
            {

                connection.Open();
                MySqlDataReader reader = cmdRead.ExecuteReader();

                while (reader.Read())
                {

                    user = reader.GetInt32("rvtFileSize").ToString();
                    /*
                    pdt.id = reader.GetInt16("id");
                    pdt.date = reader.GetDateTime("date");
                    pdt.user = reader.GetString("user");
                    pdt.rvtFileSize = reader.GetInt32("rvtFileSize");
                    pdt.elementsCount = reader.GetInt32("elementsCount");
                    pdt.typesCount = reader.GetInt32("typesCount");
                    pdt.sheetsCount = reader.GetInt32("sheetsCount");
                    pdt.viewsCount = reader.GetInt32("viewsCount");
                    pdt.viewportsCount = reader.GetInt32("viewportsCount");
                    try
                    {
                        pdt.warningsCount = reader.GetInt32("warningsCount");
                    }
                    catch
                    {
                        pdt.warningsCount = 0;
                    }
                    */

                }
                reader.Close();

                MessageBox.Show(string.Format("Connection with DB {0} established", tableName));
                return user;
                //return rvtFileSize;

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
