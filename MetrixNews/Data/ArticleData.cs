using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class ArticleData
    {
        private static string TABLE_NAME = "Articles";

        private static List<string> COLUMNS = new List<string>()
        {
            "ID",
            "SourceID",
            "Title",
            "Author",
            "Description",
            "Category", // 5
            "URL",
            "ImageURL",
            "PublishedAt",
            "Sentiment",
            "SentimentFriendly" // 10
        };

        public int ID { get; set; }

        public int SourceID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedDate { get; set; }

        public Decimal Sentiment { get; set; }

        public Decimal SentimentPercent {
            get
            {
                return this.Sentiment * 100;
            }
        }

        public string SentimentFriendly { get; set; }

        public ArticleData()
        {
            this.SetDefaults();
        }

        private void SetDefaults()
        {
            this.ID = 0;
            this.SourceID = 0;
            this.Title = string.Empty;
            this.Author = string.Empty;
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Url = string.Empty;
            this.ImageUrl = string.Empty;
            this.PublishedDate = DateTime.UtcNow;
            this.Sentiment = 0;
            this.SentimentFriendly = string.Empty;
        }

        public static ArticleData GetByID(MySqlConnection conn, int id)
        {
            return Get(conn,
                       id).FirstOrDefault();
        }

        public static List<ArticleData> GetAll(MySqlConnection conn)
        {
            return Get(conn);
        }

        public static List<ArticleData> GetByCategory(MySqlConnection conn,
                                                      string category)
        {
            return Get(conn,
                       category: category);
        }

        private static List<ArticleData> Get(MySqlConnection conn,
                                             int? id = null,
                                             string category = null)
        {
            List<ArticleData> articles = new List<ArticleData>();

            if (id != null && id <= 0)
            {
                return articles;
            }

            string sql = GetDefaultSelectStatement() + " " +
                         "FROM " + TABLE_NAME + " " +
                         "WHERE 1 = 1 " +
                             (id != null ? "AND " + TABLE_NAME + ".ID = @ID "
                                         : string.Empty) +
                             (!string.IsNullOrWhiteSpace(category) ? "AND " + TABLE_NAME + ".Category = @Category "
                                         : string.Empty) +
                         "LIMIT 20 ";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            if (!string.IsNullOrWhiteSpace(category))
            {
                cmd.Parameters.Add("@Category", MySqlDbType.VarChar).Value = category;
            }

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ArticleData article = new ArticleData()
                    {
                        ID = reader.GetInt32(0),
                        SourceID = reader.GetInt32(1),
                        Title = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty,
                        Author = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty,
                        Description = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty,
                        Category = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty,
                        Url = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty,
                        ImageUrl = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty,
                        PublishedDate = reader.GetDateTime(8),
                        Sentiment = reader.GetDecimal(9),
                        SentimentFriendly = !reader.IsDBNull(10) ? reader.GetString(10) : string.Empty
                    };

                    articles.Add(article);
                }
            }

            return articles;
        }

        private static string GetDefaultSelectStatement()
        {
            string selectStr = "SELECT " + string.Join(", ", COLUMNS);

            return selectStr;
        }

        public override string ToString()
        {
            return this.ID + ", " + this.Author + ", " + this.Category + ", " + this.Description + ", " + this.ImageUrl + ", " + this.PublishedDate.ToShortDateString() + ", " + this.Sentiment + ", " + this.SentimentFriendly + ", " + this.SourceID + ", " + this.Title + ", " + this.Url;
        }
    }
}
