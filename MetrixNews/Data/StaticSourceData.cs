using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class StaticSourceData
    {
        private static string TABLE_NAME = "StaticSources";

        private static List<string> COLUMNS = new List<string>()
        {
            "ID",
            "Biasness"
        };

        public int ID { get; set; }

        public string Biasness { get; set; }

        public StaticSourceData()
        {
            this.SetDefaults();
        }

        private void SetDefaults()
        {
            this.ID = 0;
            this.Biasness = string.Empty;
        }

        public static StaticSourceData GetByID(MySqlConnection conn,
                                         int id)
        {
            return Get(conn,
                       id).FirstOrDefault();
        }

        public static List<StaticSourceData> GetAll(MySqlConnection conn)
        {
            return Get(conn);
        }

        private static List<StaticSourceData> Get(MySqlConnection conn,
                                            int? id = null)
        {
            List<StaticSourceData> sources = new List<StaticSourceData>();

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
                while (reader.Read())
                {
                    StaticSourceData source = new StaticSourceData()
                    {
                        ID = reader.GetInt32(0),
                        Biasness = !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty
                    };

                    if (source.Biasness == "Hyper-Partisan Liber")
                    {
                        source.Biasness = "Hyper-Partisan Liberal";
                    }

                    sources.Add(source);
                }
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
            return this.ID + ", " + this.Biasness;
        }
    }
}
