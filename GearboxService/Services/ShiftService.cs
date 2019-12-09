using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using GearboxService.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace GearboxService.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IConfiguration _config;
        private HttpClientHandler _clientHandler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true,
            AllowAutoRedirect = false
        };
        public ShiftService(IConfiguration config)
        {
            _config = config;
        }

        public List<RedeemResponse> SubmitCodes(List<string> codes)
        {
            _clientHandler = new HttpClientHandler()
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true,
                AllowAutoRedirect = false
            };
            var authenticityToken = GetAuthenticityToken("https://shift.gearboxsoftware.com/home");
            GetSessionCookie(authenticityToken);
            var responses = new List<RedeemResponse>();
            using (HttpClient client = new HttpClient(_clientHandler, false))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-CSRF-Token", authenticityToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://shift.gearboxsoftware.com/rewards");
                foreach (var code in codes)
                {
                    responses.Add(SendCode(code, client).Result);
                }
            }

            return responses;
        }

        private async Task<RedeemResponse> SendCode(string code, HttpClient client)
        {
            var shiftAddress = "https://shift.gearboxsoftware.com/entitlement_offer_codes?code={0}";
            var response = client.GetAsync(string.Format(shiftAddress, code)).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                await Task.Delay(TimeSpan.FromSeconds(60));
                return await SendCode(code, client);
            }
            await Task.Delay(TimeSpan.FromSeconds(3));
            return new RedeemResponse()
            {
                ResponseCode = response.StatusCode.ToString(),
                Content = content,
                Code = code
            };
        }

        private string GetAuthenticityToken(string url)
        {
            using (HttpClient client = new HttpClient(_clientHandler, false))
            {
                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var document = new HtmlDocument();
                document.LoadHtml(content);
                var token = document.DocumentNode.SelectSingleNode("//head//meta[@name='csrf-token']").Attributes
                    .First(x => x.Name == "content").Value;
                return token;
            }
        }

        private void GetSessionCookie(string authenticityToken)
        {
            using (HttpClient client = new HttpClient(_clientHandler, false))
            {
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("utf8", "✓"),
                    new KeyValuePair<string, string>("authenticity_token", authenticityToken),
                    new KeyValuePair<string, string>("user[email]", _config["Email"]),
                    new KeyValuePair<string, string>("user[password]", _config["Password"]),
                    new KeyValuePair<string, string>("commit", "SIGN+IN")
                });
                var response = client.PostAsync(new Uri("https://shift.gearboxsoftware.com/sessions/"), formData).Result;
            }
        }
    }
}