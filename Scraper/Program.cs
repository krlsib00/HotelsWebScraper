using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                var html = client.GetStringAsync("https://en.wikipedia.org/wiki/List_of_hotels_in_the_United_States").Result;

                var parser = new HtmlParser();

                var document = parser.Parse(html);

                var losAngeles = document.QuerySelector("#Los_Angeles_County");
                var divToSearch = losAngeles.ParentElement.NextElementSibling;

                var tableRows =
                    divToSearch.QuerySelectorAll("li");

                List<HotelInfo> results = new List<HotelInfo>();

                foreach (var li in tableRows)
                {
                    var hotelInfo = new HotelInfo();


                    var aElement = li.QuerySelector("a");
                    hotelInfo.HotelName = aElement.TextContent;

                    var secondElement = aElement.NextSibling;

                    if (secondElement != null)
                    {
                        hotelInfo.HotelStatus = secondElement.TextContent;
                    }

                    results.Add(hotelInfo);
                }

                Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));
            }
        }
    }
}
