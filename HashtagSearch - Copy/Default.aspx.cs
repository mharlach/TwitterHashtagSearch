using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterHashtagSearch
{
    public partial class _Default : Page
    {
        private DataTable TweetsDataSource;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Auth.Credentials == null)
            {
                searchTextbox.Enabled = false;
                searchButton.Enabled = false;
            }
            else
            {
                searchTextbox.Enabled = true;
                searchButton.Enabled = true;
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Bind();
        }

        protected void searchButton_click(object sender, EventArgs e)
        {
            var searchQuery = searchTextbox.Text;
            SearchTweets(searchQuery);
        }

        protected void LikeButton_Click(object sender, CommandEventArgs e)
        {
            long tweetId = long.Parse((string)e.CommandArgument);
            LikeTweet(tweetId);
        }

        private void SearchTweets(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                if (searchQuery.StartsWith("#") != true)
                {
                    searchQuery = $"#{searchTextbox.Text}";
                }

                var tweetParams = new SearchTweetsParameters(searchQuery)
                {
                    MaximumNumberOfResults = 100,
                    TweetSearchType = TweetSearchType.OriginalTweetsOnly,
                };

                try
                {
                    Session[SessionKeys.Tweets] = Search.SearchTweets(tweetParams).ToDictionary(x => x.Id);
                }
                catch (Exception ex)
                {
                    Session[SessionKeys.Tweets] = null;
                }

            }
        }

        private void LikeTweet(long tweetId)
        {
            try
            {
                bool result = Tweet.FavoriteTweet(tweetId);

                var tweet = Tweet.GetTweet(tweetId);
                var tweets = Session[SessionKeys.Tweets] as Dictionary<long, ITweet>;
                tweets[tweetId] = tweet;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to like tweet");
            }
        }

        private void Bind()
        {
            TweetsDataSource = new DataTable();
            TweetsDataSource.Columns.Add("Id", typeof(long));
            TweetsDataSource.Columns.Add("AuthorName", typeof(string));
            TweetsDataSource.Columns.Add("ScreenName", typeof(string));
            TweetsDataSource.Columns.Add("TweetText", typeof(string));
            TweetsDataSource.Columns.Add("Timestamp", typeof(DateTime));
            TweetsDataSource.Columns.Add("RetweetsCount", typeof(int));
            TweetsDataSource.Columns.Add("LikesCount", typeof(int));
            TweetsDataSource.Columns.Add("ImageUrl", typeof(string));
            TweetsDataSource.Columns.Add("LikeEnabled", typeof(bool));

            if (Session[SessionKeys.Tweets] != null)
            {
                var tweets = Session[SessionKeys.Tweets] as Dictionary<long, ITweet>;
                foreach (var t in tweets.Values)
                {
                    TweetsDataSource.Rows.Add(new object[]
                    {
                        t.Id,
                        t.CreatedBy.Name,
                        $"@{t.CreatedBy.ScreenName}",
                        t.Text,
                        t.CreatedAt,
                        t.RetweetCount,
                        t.FavoriteCount,
                        t.CreatedBy.ProfileImageUrl400x400 ,
                        !t.Favorited
                    });
                }
            }

            tweetsListView.DataSource = TweetsDataSource;
            tweetsListView.DataBind();
        }
    }
}