using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using LinqToWiki.Download;
using LinqToWiki;
using LinqToWiki.Generated;
namespace DataFromWiki
{
    class Program
    {
        static void Main(string[] args)
        {


            //DATA about popular pages taken from: https://en.wikipedia.org/wiki/Wikipedia:Multiyear_ranking_of_most_viewed_pages#Sources
            //and from: http://wikirank-2018.di.unimi.it/faq.html

            //USING LINQ-TO-WIKI
            //Downloader.LogDownloading = true;
            var wiki = new Wiki("LinqToWiki.Samples", "https://en.wikipedia.org", "/w/api.php");
            PageResultPageId(wiki);
            
            //1. Read from execl Articles titles from excel fourth sheet.
            ExcelReader er = new ExcelReader(@"C:\Users\ספיר כהן\Desktop\כיוונים לרשת בויקיפדיה\Best 10K\DataFromWiki\MoviesNetwork.xlsx", 1);
            List<string> articles = er.ReadCell(5108,1);
            er.Close();

            //Read from execl Articles titles from the second sheet.
            //ExcelReader er2 = new ExcelReader(@"C:\Users\ספיר כהן\Desktop\כיוונים לרשת בויקיפדיה\Best 10K\DataFromWiki\MusicNetwork.xlsx", 1);
            //List<string> allAricles = er2.ReadCell(10000, 1);
            //er2.Close();

            ////2. get all pages information
            List<PageInfo> vertexAndEdges = PageResultProps(wiki, articles);

            ////3. Intersect to find edges
            CheckEdges(vertexAndEdges, articles);

            //4. Write to excel (sheet 3)
            ExcelReader ew = new ExcelReader(@"C:\Users\ספיר כהן\Desktop\כיוונים לרשת בויקיפדיה\Best 10K\DataFromWiki\MoviesNetwork.xlsx", 2);
            int row = 1;
            int counter = 0;
            foreach (PageInfo item in vertexAndEdges)
            {
                counter++;
                row = ew.WriteToExcel(row, item.Title, item.LinksTitles);
            }

            ew.Close();
            //François Girard

        }
        //FOR LINQ-TO-WIKI (CONTROLLER FUNCTIONS)

        //get all pages props and links. 
        private static List<PageInfo> PageResultProps(Wiki wiki, List<string> pageTitles)
        {
            List<PageInfo> pi = new List<PageInfo>();
            //get info for all pages titles 
            foreach (var pageTitle in pageTitles)
            {
                var pageInfo = wiki.Query.allpages().
                Where(page => page.from == pageTitle.TrimEnd() && page.to==pageTitle.TrimEnd()).Pages.
                Select(p => PageResult.Create(
                        p.info,
                        p.links().Select(s => s.title).ToList())
                        );

                var pageInfo2 = pageInfo.ToEnumerable().ToArray();
                if (pageInfo2.Length >= 1)
                {
                    PageInfo article = new PageInfo((long)pageInfo2[0].Info.pageid, pageInfo2[0].Info.title, pageInfo2[0].Data.ToList());
                    //article.PrintPageDetails();
                    pi.Add(article);
                }
            }
            return pi;
        }
        //getting pages id for the networks
        private static List<PageInfo> PageResultPageId(Wiki wiki)
        {
            List<PageInfo> pi = new List<PageInfo>();
            string pageTitle = "François Girard";
            var pageInfo = wiki.Query.allpages().
            Where(page => page.from == pageTitle.TrimEnd() && page.to == pageTitle.TrimEnd()).Pages.
            Select(p => p.info);

            var pageInfo2 = pageInfo.ToEnumerable().ToArray();
            if (pageInfo2.Length >= 1)
            {
                //PageInfo article = new PageInfo((long)pageInfo2[0].Info.pageid, pageInfo2[0].Info.title, pageInfo2[0].Data.ToList());
                //pi.Add(article);
            }
            return pi;
        }

        private static void CheckEdges(List<PageInfo> pages, List<string> vertecies)
        {
            foreach (PageInfo page in pages)
            {
                page.LinksTitles = vertecies.Intersect(page.LinksTitles).ToList();
            }
        }
        private static void Write<T>(WikiQueryPageResult<PageResult<T>> source)
        {
            Write(source.ToEnumerable());
        }

        private static void Write<T>(IEnumerable<PageResult<T>> source)
        {
            foreach (var page in source.Take(10))
            {
                Console.WriteLine(page.Info.title);
                foreach (var item in page.Data.Take(10))
                    Console.WriteLine("  " + item);
            }
        }
    }
}
//next missions:
//1. foreach of the articles we need to create edges ONLY if they exsits in the article LIST (in the 10000 list!).
//2. save the edges to excel sheets. 
//3. after all excel sheets are prepard, try the algorithm.
//4. try to create a graph visualization.