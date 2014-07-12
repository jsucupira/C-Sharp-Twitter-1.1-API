using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwitterFeed.Test
{
    [TestClass]
    public class TwitterTest
    {
        private readonly string _consumerKey = ConfigurationManager.AppSettings["customer_key"];
        private readonly string _consumerSecret = ConfigurationManager.AppSettings["customer_secret"];
        private readonly string _token = ConfigurationManager.AppSettings["access_token"];
        private readonly string _tokenSecret = ConfigurationManager.AppSettings["access_token_secret"];


        [TestMethod]
        public void test_twitter_home_timeline()
        {
            TwitterUtil result = new TwitterUtil(_consumerKey, _consumerSecret, _token, _tokenSecret);
            IEnumerable<TwitterObject> twittes = result.GetHomeTimeLine("count=20&exclude_replies=true");
            Assert.IsTrue(twittes.Any());
        }

        [TestMethod]
        public void test_twitter_mention_timeline()
        {
            TwitterUtil result = new TwitterUtil(_consumerKey, _consumerSecret, _token, _tokenSecret);
            IEnumerable<TwitterObject> twittes = result.GetMetionsUrl("count=20&since_id=14927799");
            Assert.IsTrue(twittes.Any());
        }

        [TestMethod]
        public void test_twitter_retweent_timeline()
        {
            TwitterUtil result = new TwitterUtil(_consumerKey, _consumerSecret, _token, _tokenSecret);
            IEnumerable<TwitterObject> twittes = result.RetweetsOfMeUrl("count=20");
            Assert.IsTrue(twittes.Any());
        }

        [TestMethod]
        public void test_twitter_user_timeline()
        {
            TwitterUtil result = new TwitterUtil(_consumerKey, _consumerSecret, _token, _tokenSecret);
            IEnumerable<TwitterObject> twitterMessage = result.GetUserTimeLine("screen_name=jsucupira&count=20");
            Assert.IsTrue(twitterMessage.Any());
        }
    }
}