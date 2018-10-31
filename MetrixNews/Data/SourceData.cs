using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class SourceData
    {
        private static string TABLE_NAME = "Sources";

        private static List<string> COLUMNS = new List<string>()
        {
            "ID",
            "Name",
            "APIName",
            "URL",
            "Biasness"
        };

        public int ID { get; set; }

        public string Name { get; set; }

        public string APIName { get; set; }

        public string Url { get; set; }

        public string Biasness { get; set; }

        public SourceData()
        {
            this.SetDefaults();
        }

        private void SetDefaults()
        {
            this.ID = 0;
            this.Name = string.Empty;
            this.APIName = string.Empty;
            this.Url = string.Empty;
            this.Biasness = string.Empty;
        }

        public static SourceData GetByID(MySqlConnection conn,
                                         int id)
        {
            return Get(conn,
                       id).FirstOrDefault();
        }

        public static List<SourceData> GetAll(MySqlConnection conn)
        {
            return Get(conn);
        }

        private static List<SourceData> Get(MySqlConnection conn,
                                            int? id = null)
        {
            List<SourceData> sources = new List<SourceData>();

            if (id != null && id <= 0)
            {
                return sources;
            }

            string sql = GetDefaultSelectStatement() + " " +
                         "FROM " + TABLE_NAME + " " +
                         "WHERE 1 = 1 " +
                         (id != null ? "AND " + TABLE_NAME + ".ID = @ID "
                                     : string.Empty);

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                SourceData source = new SourceData()
                {
                    ID = reader.GetInt32(0),
                    Name = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty,
                    APIName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty,
                    Url = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty,
                    Biasness = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty
                };

                sources.Add(source);
            }

            return sources;
        }

        private static string GetDefaultSelectStatement()
        {
            string selectStr = "SELECT " + string.Join(", ", COLUMNS);

            return selectStr;
        }

        public override string ToString()
        {
            return this.ID + ", " + this.APIName + ", " + this.Biasness + ", " + this.Name + ", " + this.Url;
        }
    }
}
