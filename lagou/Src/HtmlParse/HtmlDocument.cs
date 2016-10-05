using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace lagou
{
    class HtmlDocument : HtmlElement
    {
        public static HtmlDocument FormHtml(string html)
        {
            return new HtmlDocument(html);
        }
        public static async Task<HtmlDocument> FormUrlAsync(string url)
        {
            var web = new HttpClient();
            var html = await web.GetStringAsync(new Uri(url));
            return new HtmlDocument(html);
        }
        protected HtmlDocument(string html) : base(html) {  }
    }

}
