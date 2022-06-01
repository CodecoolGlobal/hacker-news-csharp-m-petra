using Microsoft.AspNetCore.Mvc.Routing;

namespace Codecool.HackerNewsClient.Models
{
    public class News
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string TimeAgo { get; set; }
        public string Url { get; set; }
    }
}
