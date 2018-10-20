using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class DBAccess
    {
        private const string CONNECTION_STRING = "server=Metrix.News;uid=MetrixWeb;pwd=DCd#z^4DWD;database=Metrix;SslMode=none;";

        public DBAccess()
        {
            // Constructor
        }

        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(CONNECTION_STRING);
                conn.Open();

                return conn;
            }
            catch (MySqlException ex)
            {
                return null;
            }
        }
    }
}
