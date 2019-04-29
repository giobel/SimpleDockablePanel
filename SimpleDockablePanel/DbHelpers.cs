using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace SimpleDockablePanel
{
    class DbHelpers
    {

        public static bool ConnectDB(string tableName)
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

            try
            {

            
            connection.Open();
            
                return true;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
        }//close method

        public static string dbGetTableValues(string tableName, string _server, string _database, string _user, string _password)
        {
            string server = _server;
            string database = _database;
            string uid = _user;
            string password = _password;
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

                //MessageBox.Show(string.Format("Connection with DB {0} established", tableName));
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

        public static bool dbGetTableNames(string _server, string _database, string _user, string _password)
        {

            /*
            string server = "remotemysql.com";
            string database = "r7BFoOjCty";
            string uid = "r7BFoOjCty";
            string password = "1vU3s1bj6T";
            */

            string server = _server;
            string database = _database;
            string uid = _user;
            string password = _password;

            /*
            string server = "127.0.0.1";
            string database = "_learning_burst_sql_example";
            string uid = "root";
            string password = "password";
            */

            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            // string table = "filesize";
            //string table = "CQT";

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                

                List<String> Tablenames = new List<String>();

                connection.Open();

                MySqlCommand cmdRead = new MySqlCommand("", connection);

                cmdRead.CommandText = String.Format("SELECT table_name FROM information_schema.tables WHERE table_schema = '{0}'", database);

                using (MySqlDataReader reader = cmdRead.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Tablenames.Add(reader.GetString(0));

                        Ribbon.m_MyDock.cboxDatabases.Items.Add(reader.GetString(0));

                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
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
