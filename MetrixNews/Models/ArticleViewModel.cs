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

        public Dictionary<int, SourceData> Sources { get; set; }

        public static ArticleViewModel GetModel(MySqlConnection conn)
        {
            List<ArticleData> articles = ArticleData.GetByCategory(conn, "law, govt and politics");
            List<SourceData> sourceData = SourceData.GetAll(conn);
            Dictionary<int, SourceData> sources = sourceData.ToDictionary(x => x.ID, x => x);

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Articles = articles,
                Sources = sources
            };

            return viewModel;
        }
    }
}
