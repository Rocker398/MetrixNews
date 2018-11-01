using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class MailingListData
    {
        private static string TABLE_NAME = "MailingList";

        private static List<string> COLUMNS = new List<string>()
        {
            "id",
            "EmailAddress"
        };

        public int ID { get; set; }

        public string EmailAddress { get; set; }

        public MailingListData()
        {
            this.SetDefaults();
        }

        private void SetDefaults()
        {
            this.ID = 0;
            this.EmailAddress = string.Empty;
        }

        public void Save(MySqlConnection conn)
        {
            string sql = "INSERT INTO " + TABLE_NAME + " (EmailAddress) " +
                         "VALUES (@EmailAddress)";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@EmailAddress", MySqlDbType.VarChar).Value = this.EmailAddress;

            cmd.ExecuteNonQuery();
        }

        public override string ToString()
        {
            return this.ID + ", " + this.EmailAddress;
        }
    }
}
