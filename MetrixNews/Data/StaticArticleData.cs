using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Data
{
    public class StaticArticleData
    {
        private static string TABLE_NAME = "StaticArticles";

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
            "Joy", // 10
            "Anger",
            "Disgust",
            "Sadness",
            "Fear"
        };

        public int ID { get; set; }

        public int SourceID { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? PublishedDate { get; set; }

        public Decimal Sentiment { get; set; }

        public Decimal SentimentPercent
        {
            get
            {
                return this.Sentiment * 100;
            }
        }

        public Decimal Joy { get; set; }

        public Decimal Anger { get; set; }

        public Decimal Disgust { get; set; }

        public Decimal Sadness { get; set; }

        public Decimal Fear { get; set; }

        public Tuple<Decimal, string> ProminentEmotion
        {
            get
            {
                Decimal emotionVal = 0;
                string emotionStr = string.Empty;

                List<Decimal> values = new List<Decimal>() { this.Joy, this.Anger, this.Disgust, this.Sadness, this.Fear };

                emotionVal = values.Max();

                if (emotionVal == this.Joy)
                {
                    emotionStr = "Joy";
                }
                else if (emotionVal == this.Anger)
                {
                    emotionStr = "Anger";
                }
                else if (emotionVal == this.Disgust)
                {
                    emotionStr = "Disgust";
                }
                else if (emotionVal == this.Sadness)
                {
                    emotionStr = "Sadness";
                }
                else
                {
                    emotionStr = "Fear";
                }

                Tuple<Decimal, string> emotion = Tuple.Create(emotionVal, emotionStr);
                return emotion;
            }
        }

        public StaticArticleData()
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
            this.Joy = 0;
            this.Anger = 0;
            this.Disgust = 0;
            this.Sadness = 0;
            this.Fear = 0;
        }

        public static StaticArticleData GetByID(MySqlConnection conn, int id)
        {
            return Get(conn,
                       id).FirstOrDefault();
        }

        public static List<StaticArticleData> GetAll(MySqlConnection conn)
        {
            return Get(conn);
        }

        public static List<StaticArticleData> GetByCategory(MySqlConnection conn,
                                                      string category)
        {
            return Get(conn,
                       category: category);
        }

        private static List<StaticArticleData> Get(MySqlConnection conn,
                                             int? id = null,
                                             string category = null)
        {
            List<StaticArticleData> articles = new List<StaticArticleData>();

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
                                         : string.Empty);

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            if (!string.IsNullOrWhiteSpace(category))
            {
                cmd.Parameters.Add("@Category", MySqlDbType.VarChar).Value = category;
            }

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int currID = reader.GetInt32(0);

                    if (currID == 34)
                    {
                        continue;
                    }

                    StaticArticleData article = new StaticArticleData()
                    {
                        ID = reader.GetInt32(0),
                        SourceID = !reader.IsDBNull(1) ? reader.GetInt32(1) : 0,
                        Title = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty,
                        Author = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty,
                        Description = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty,
                        Category = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty,
                        Url = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty,
                        ImageUrl = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty,
                        PublishedDate = !reader.IsDBNull(8) ? reader.GetDateTime(8) : (DateTime?)null,
                        Sentiment = !reader.IsDBNull(9) ? reader.GetDecimal(9) : 0,
                        Joy = !reader.IsDBNull(10) ? reader.GetDecimal(10) : 0,
                        Anger = !reader.IsDBNull(11) ? reader.GetDecimal(11) : 0,
                        Disgust = !reader.IsDBNull(12) ? reader.GetDecimal(12) : 0,
                        Sadness = !reader.IsDBNull(13) ? reader.GetDecimal(13) : 0,
                        Fear = !reader.IsDBNull(14) ? reader.GetDecimal(14) : 0
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
            return this.ID + ", " + this.Author + ", " + this.Category + ", " + this.Description + ", " + this.ImageUrl + ", " + this.Sentiment + ", " + this.Joy + ", " + this.Anger + ", " + this.Disgust + ", " + this.Sadness + ", " + this.Fear + ", " + this.SourceID + ", " + this.Title + ", " + this.Url;
        }
    }
}
