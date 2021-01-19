using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LinkfireTechChallenge.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class SpotifyController : Controller
    {
        SpotifyAuthentication sAuth = new SpotifyAuthentication();

        [HttpGet]
        public ContentResult Get()
        {
            var qb = new QueryBuilder();
            qb.Add("response_type", "code");
            qb.Add("client_id", sAuth.clientID);
            qb.Add("scope", "user-read-private user-read-email playlist-modify-public playlist-modify-private");
            qb.Add("redirect_uri", sAuth.redirectURL);

            return new ContentResult
            {
                ContentType = "text/html",
                Content = @"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <meta charset=""utf-8"">
                            <title>Spotify Auth Example</title>
                        </head>
                        <body>
                            <a href=""https://accounts.spotify.com/authorize/" + qb.ToQueryString().ToString() + @"""><button>Authenticate at Spotify</button></a>
                        </body>
                    </html>
                "
            };
        }

        [HttpGet("/callback")]
        public ContentResult Get(string code)
        {
            string responseString = "";

            if (code.Length > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    Console.WriteLine(Environment.NewLine + "Your basic bearer: " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(sAuth.clientID + ":" + sAuth.clientSecret)));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(sAuth.clientID + ":" + sAuth.clientSecret)));

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("redirect_uri", sAuth.redirectURL),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    });

                    var response = client.PostAsync("https://accounts.spotify.com/api/token", formContent).Result;

                    var responseContent = response.Content;
                    responseString = responseContent.ReadAsStringAsync().Result;
                }
            }

            return new ContentResult
            {
                ContentType = "application/json",
                Content = responseString
            };
        }
    }

    class SpotifyAuthentication
    {
        public string clientID = "";
        public string clientSecret = "";
        public string redirectURL = "http://localhost:61297/callback";
    }
}
