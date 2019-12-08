using System.Buffers.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using GearboxService.Models;
using Microsoft.Extensions.Configuration;

namespace GearboxService.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IConfiguration _config;

        public TwitterService(IConfiguration config)
        {
            _config = config;
        }

        public List<string> GetCodes()
        {
            var bearerToken = GetBearerToken();
            var tweets = GetTweets(bearerToken);
            var codes = ParseTweets(tweets);
            return codes;
        }
        
        private string GetBearerToken()
        {
            string tokenUrl = "https://api.twitter.com/oauth2/token";
            string bearerToken;
            using (HttpClient client = new HttpClient())
            {
                string twitterAuthentication = _config["TwitterAuthentication"];
                var authorizationBytes = Encoding.ASCII.GetBytes(twitterAuthentication);
                client.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authorizationBytes));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
                var formEncoded = new Dictionary<string, string>();
                formEncoded.Add("grant_type", "client_credentials");
                HttpContent requestContent = new FormUrlEncodedContent(formEncoded);
                var response = client.PostAsync(tokenUrl, requestContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var token = JsonSerializer.Deserialize<BearerToken>(content).AccessToken;
                return token;
            }
        }

        private List<Tweet> GetTweets(string bearerToken)
        {
            string twitterAccount = _config["TwitterAccount"];
            string apiUrl = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={twitterAccount}&exclude_replies=true&count=200&tweet_mode=extended";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var response = client.GetAsync(apiUrl).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return JsonSerializer.Deserialize<List<Tweet>>(content);
            }
        }

        private List<string> ParseTweets(List<Tweet> tweets)
        {
            List<string> codes = new List<string>();
            var regex = new Regex(@"([\w\d]{5}-){4}[\w\d]{5}");
            foreach (var tweet in tweets)
            {
                var matches = regex.Match(tweet.FullText);
                if (matches.Length > 0)
                {
                    foreach (var match in matches.Captures)
                    {
                        codes.Add(match.ToString());
                    }
                }
            }

            return codes;
        }
    }
}