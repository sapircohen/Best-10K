using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromWiki
{
    class PageInfo
    {
    
        public long PageId { get ; set ; }
        public string Title { get ; set ; }
        public List<string> LinksTitles { get ; set ; }

        public PageInfo(long pageId, string title, List<string> linksTitles)
        {
            this.PageId = pageId;
            this.Title = title;
            this.LinksTitles = linksTitles;
        }
        public PageInfo()
        {

        }

        public void PrintPageDetails()
        {
            Console.WriteLine($"Page ID: {this.PageId}, Page Title: {this.Title}\n\nPage links:\n");
            foreach (string title in this.LinksTitles)
            {
                Console.WriteLine($"Page links: {title}");
            }
        }
    }
}
