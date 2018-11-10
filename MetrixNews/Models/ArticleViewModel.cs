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

            articles = articles.OrderByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "balanced")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews conservative")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan conservative")
                               .ToList();

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Articles = articles,
                Sources = sources
            };

            return viewModel;
        }
    }
}
