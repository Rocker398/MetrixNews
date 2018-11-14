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
        public Dictionary<string, List<StaticArticleData>> Articles { get; set; }

        public Dictionary<int, StaticSourceData> Sources { get; set; }

        public static ArticleViewModel GetModel(MySqlConnection conn)
        {
            List<StaticArticleData> articles = StaticArticleData.GetAll(conn);
            List<StaticSourceData> sourceData = StaticSourceData.GetAll(conn);
            Dictionary<int, StaticSourceData> sources = sourceData.ToDictionary(x => x.ID, x => x);

            articles = articles.OrderByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "balanced")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews conservative")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan conservative")
                               .ToList();

            Dictionary<string, List<StaticArticleData>> articlesByCategory = new Dictionary<string, List<StaticArticleData>>();

            List<StaticArticleData> articleList = null;

            foreach (StaticArticleData currArticle in articles)
            {
                if (!string.IsNullOrWhiteSpace(currArticle.Category))
                {
                    if (articlesByCategory.ContainsKey(currArticle.Category))
                    {
                        articleList = articlesByCategory[currArticle.Category];
                        articleList.Add(currArticle);
                        articlesByCategory[currArticle.Category] = articleList;
                    }
                    else
                    {
                        articleList = new List<StaticArticleData>();
                        articleList.Add(currArticle);
                        articlesByCategory.Add(currArticle.Category, articleList);
                    }
                }
            }

            ArticleViewModel viewModel = new ArticleViewModel()
            {
                Articles = articlesByCategory,
                Sources = sources
            };

            return viewModel;
        }
    }
}
