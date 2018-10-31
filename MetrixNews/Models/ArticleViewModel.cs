using MetrixNews.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Models
{
    public class ArticleViewModel
    {
        public List<ArticleData> Articles { get; set; }

        public static ArticleViewModel GetModel(MySqlConnection conn)
        {
            List<ArticleData> articles = ArticleData.GetByCategory(conn, "law, govt and politics");

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Articles = articles
            };

            return viewModel;
        }
    }
}
