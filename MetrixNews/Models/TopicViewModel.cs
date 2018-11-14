using MetrixNews.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetrixNews.Models
{
    public class TopicViewModel
    {
        public List<StaticArticleData> Articles { get; set; }

        public Dictionary<int, StaticSourceData> Sources { get; set; }

        public Dictionary<string, List<StaticArticleData>> ArticlesByEmotion { get; set;}

        public static TopicViewModel GetModel(MySqlConnection conn, string topic)
        {
            List<StaticArticleData> articles = StaticArticleData.GetByCategory(conn, topic);
            List<StaticSourceData> sourceData = StaticSourceData.GetAll(conn);
            Dictionary<int, StaticSourceData> sources = sourceData.ToDictionary(x => x.ID, x => x);

            articles = articles.OrderByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews liberal")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "balanced")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "skews conservative")
                               .ThenByDescending(x => sources.ContainsKey(x.SourceID) && sources[x.SourceID].Biasness.ToLower() == "hyper-partisan conservative")
                               .ToList();

            // Get articles by emotion rating
            Dictionary<string, List<StaticArticleData>> articlesByEmotion = GetArticlesByEmotion(articles);

            TopicViewModel viewModel = new TopicViewModel()
            {
                Articles = articles,
                Sources = sources,
                ArticlesByEmotion = articlesByEmotion
            };

            return viewModel;
        }

        private static Dictionary<string, List<StaticArticleData>> GetArticlesByEmotion(List<StaticArticleData> articles)
        {
            Dictionary<string, List<StaticArticleData>> articlesByEmotion = new Dictionary<string, List<StaticArticleData>>()
            {
                { "Joy", new List<StaticArticleData>() },
                { "Fear", new List<StaticArticleData>() },
                { "Anger", new List<StaticArticleData>() },
                { "Sadness", new List<StaticArticleData>() },
                { "Disgust", new List<StaticArticleData>() }
            };

            // Organize the articles into the emotions they are most prominent in
            foreach (StaticArticleData article in articles)
            {
                string emotion = article.ProminentEmotion.Item2;

                if (articlesByEmotion.ContainsKey(emotion))
                {
                    List<StaticArticleData> existing = articlesByEmotion[emotion];
                    existing.Add(article);
                    articlesByEmotion[emotion] = existing;
                }
            }

            return articlesByEmotion;
        }
    }
}
