using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace TwitterFeed
{
    public class TwitterUtil : OAuthBase
    {
        public TwitterUtil(string oauthConsumerKey, string oauthConsumerSecret, string oauthToken, string oauthTokenSecret)
        {
            _oauthConsumerKey = oauthConsumerKey;
            _oauthConsumerSecret = oauthConsumerSecret;
            _oauthToken = oauthToken;
            _oauthTokenSecret = oauthTokenSecret;
        }

        private const string HOME_TIMELINE_URL = "https://api.twitter.com/1.1/statuses/home_timeline.json";
        private const string MENTIONS_URL = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";
        private const string RETWEETS_OF_ME_URL = "https://api.twitter.com/1.1/statuses/retweets_of_me.json";
        private const string USER_TIME_LINE_URL = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        private readonly string _oauthConsumerKey;
        private readonly string _oauthConsumerSecret;
        private readonly string _oauthNonce = GenerateNonce();
        private readonly string _oauthTimestamp = GenerateTimeStamp();
        private readonly string _oauthToken;
        private readonly string _oauthTokenSecret;

        /// <summary>
        ///     https://dev.twitter.com/docs/api/1.1/get/statuses/home_timeline
        ///     i.e. count=200
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IEnumerable<TwitterObject> GetHomeTimeLine(string queryString)
        {
            string url = HOME_TIMELINE_URL + "?" + CleanupQueryString(queryString);

            string oauthSignature = OauthSignature(url);
            // create the request header
            string authHeader = AuthHeader(oauthSignature);
            // make the request

            ServicePointManager.Expect100Continue = false;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
                string result = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<List<TwitterObject>>(result);
            }
        }

        /// <summary>
        ///     https://dev.twitter.com/docs/api/1.1/get/statuses/mentions_timeline
        ///     i.e. count=20&since_id=14927799
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IEnumerable<TwitterObject> GetMetionsUrl(string queryString)
        {
            string url = MENTIONS_URL + "?" + CleanupQueryString(queryString);

            string oauthSignature = OauthSignature(url);
            // create the request header
            string authHeader = AuthHeader(oauthSignature);
            // make the request

            ServicePointManager.Expect100Continue = false;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
                string result = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<List<TwitterObject>>(result);
            }
        }

        /// <summary>
        ///     https://dev.twitter.com/docs/api/1.1/get/statuses/user_timeline
        ///     i.e. screen_name=twitterapi&count=2
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IEnumerable<TwitterObject> GetUserTimeLine(string queryString)
        {
            string url = USER_TIME_LINE_URL + "?" + CleanupQueryString(queryString);
            string oauthSignature = OauthSignature(url);
            // create the request header
            string authHeader = AuthHeader(oauthSignature);
            // make the request

            ServicePointManager.Expect100Continue = false;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
                string result = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<List<TwitterObject>>(result);
            }
        }

        /// <summary>
        ///     https://dev.twitter.com/docs/api/1.1/get/statuses/retweets_of_me
        ///     i.e. count=50&since_id=259320959964680190&max_id=259320959964680500
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IEnumerable<TwitterObject> RetweetsOfMeUrl(string queryString)
        {
            string url = RETWEETS_OF_ME_URL + "?" + CleanupQueryString(queryString);

            string oauthSignature = OauthSignature(url);
            // create the request header
            string authHeader = AuthHeader(oauthSignature);
            // make the request

            ServicePointManager.Expect100Continue = false;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
                string result = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<List<TwitterObject>>(result);
            }
        }

        #region private methods

        private string AuthHeader(string oauthSignature)
        {
            const string headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                        "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                        "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                        "oauth_version=\"{6}\"";

            string authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(_oauthNonce),
                Uri.EscapeDataString(Hmacsha1SignatureType),
                Uri.EscapeDataString(_oauthTimestamp),
                Uri.EscapeDataString(_oauthConsumerKey),
                Uri.EscapeDataString(_oauthToken),
                Uri.EscapeDataString(oauthSignature),
                Uri.EscapeDataString(OAuthVersion));
            return authHeader;
        }

        private string OauthSignature(string url)
        {
            string normalizeUrl;
            string normalizedString;
            string oauthSignature = GenerateSignature(new Uri(url), _oauthConsumerKey, _oauthConsumerSecret, _oauthToken, _oauthTokenSecret, "GET", _oauthTimestamp, _oauthNonce, out normalizeUrl, out normalizedString);
            return oauthSignature;
        }

        
        private static string CleanupQueryString(string querystring)
        {
            if (!string.IsNullOrEmpty(querystring))
            {
                if (querystring.IndexOf('&') == 0)
                    querystring = querystring.Remove(0, 1);
            }
            return querystring;
        }

        #endregion
    }
}