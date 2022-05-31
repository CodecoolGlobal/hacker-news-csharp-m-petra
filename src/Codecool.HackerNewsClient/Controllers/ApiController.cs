using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Codecool.HackerNewsClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.HackerNewsClient.Controllers
{
    public class ApiController : Controller
    {
        [Route("/")]
        [Route("api/{site}/{page?}")]
        public async Task<IActionResult> Index(string site, string page = "1")
        {
            if (site == "top" || site == null)
            {
                site = "news";
            }
            string apiUrl = $"https://api.hnpwa.com/v0/{site}/{page}.json";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);

                    var list = new List<News>();
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        News news = new News
                        {
                            Title = table.Rows[i]["title"].ToString(),
                            Author = table.Rows[i]["user"].ToString(),
                            TimeAgo = table.Rows[i]["time_ago"].ToString(),
                            
                        };
                        list.Add(news);
                    }

                    var newsView = new NewsView
                    {
                        NewsList = list
                    };

                    return View(newsView);
                }

                return new StatusCodeResult(500);
            }
        }
    }
}
