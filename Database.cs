using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace DatabaseCS
{

    public static class Database
    {
        private static string DB_PATH;
        private static MySqlConnection sql_con;
        private static MySqlDataReader rdr;
        public static bool IsConnected
        {
            get
            {
                bool toreturn = false;
                if (sql_con != null)
                {
                    toreturn = true;
                }
                return toreturn;
            }
        }
        public static void Connect(string Host, string DatabaseName, string User, string Password)
        {
            try
            {

                Init(Host, DatabaseName, User, Password);
                sql_con = new MySqlConnection(DB_PATH);
                sql_con.Open();
                rdr = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static bool Connect(string Host, string DatabaseName, string User)
        {
            try
            {
                Init(Host, DatabaseName, User, "");
                sql_con = new MySqlConnection(DB_PATH);
                sql_con.Open();
                rdr = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        private static void Init(string Host, string DatabaseName, string User, string Password)
        {
            DB_PATH = $"server = {Host}; user={User};password={Password};database={DatabaseName};SslMode=none";
        }

        /// <summary>
        /// Function that allows you to execute a SELECT sql request
        ///  Return a list of dictionary containing the datas returned by your sql server
        /// </summary>
        /// <param name="strRequete">Sql request</param>
        /// <returns>Tableau 2 dimensions contenant le resultat du SELECT</returns>
        /// <example>
        /// <code>
        /// string[,] tabAllUsers = db.Select("SELECT prenom, nom FROM tbl_users");
        /// </code>
        /// </example>
        public static List<Dictionary<string, string>> Select(this string strRequete)
        {

            if (!IsConnected)
            {
                throw new Exception("Cannot execute a request if the database is not connected ! ");
            }
            // recuperation de la requete passé en paramètre et éxecutions 
            var cmd = new MySqlCommand(strRequete, sql_con);
            // execute la requete
            List<Dictionary<string, string>> lstdata = new List<Dictionary<string, string>>();
            cmd = new MySqlCommand(strRequete, sql_con);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                // Switch permettant de recuperer la valeur en fonction de leur type
                Dictionary<string, string> data = new Dictionary<string, string>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    string strDataType = rdr.GetDataTypeName(i).Split('(')[0];
                    switch (strDataType)
                    {
                        case "TINYINT":
                        case "INT":
                        case "int":
                        case "INTEGER":
                        case "integer":
                            //tab[i, iCpt] = Convert.ToString(rdr.GetInt32(i));
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetInt32(i)));
                            break;
                        case "VARCHAR":
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetString(i)));
                            break;
                        case "DATETIME":
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetDateTime(i)));
                            break;
                        case "main":
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetValue(i)));
                            break;
                        case "BOOL":
                        case "BOOLEAN":
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetBoolean(i)));
                            break;
                        default:
                            data.Add(rdr.GetName(i), Convert.ToString(rdr.GetValue(i)));
                            break;
                    }
                }
                lstdata.Add(data);
            }
            rdr.Close();
            return lstdata;
        }
        /// <summary>
        /// Execute une requête SQL de type écritures, example, INSERT INTO, UPDATE etc...
        /// </summary>
        /// <param name="strRequete"></param>
        public static void Write(this string strRequete)
        {
            if (!IsConnected)
            {
                throw new Exception("Cannot execute a request if the database is not connected ! ");
            }
            var cmd = new MySqlCommand(strRequete, sql_con);
            cmd.ExecuteNonQuery();

        }
        public static void Close()
        {
            if (IsConnected)
                sql_con.Close();
        }

    }


}
