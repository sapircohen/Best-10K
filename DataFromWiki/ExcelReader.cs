using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace DataFromWiki
{
    class ExcelReader
    {
        string path = "";
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;
        

        public ExcelReader(string path, int sheet)
        {
            this.path = path;
            wb = excel.Workbooks.Open(path);
            ws = excel.Worksheets[sheet];
            
        }

        public List<string> ReadCell(int row,int col)
        {
            //col would be 1 
            List<string> articles = new List<string>();
            //excel reads from 1 not from 0
            //check if cell is not empty 
            for (int i = 1; i <= row; i++)
            {
                if (ws.Cells[i, col].Value2 != null)
                {
                    articles.Add(ws.Cells[i, col].Value2.ToString());
                }
            }

            return articles;
        }

        //write in sheet 3
        public int WriteToExcel(int row, string pageTitle, List<string> pageLinks)
        {
            //1. original page title would be in column 1 
            int indexPageTitle = 1;
            //2. other page title would be in column 2
            int indexPageEdged = 2;
            //i is rows
            for (int i = 0; i < pageLinks.Count; row++, i++)
            {
                ws.Cells[row, indexPageTitle].Value2 = pageTitle;
                ws.Cells[row, indexPageEdged].Value2 = pageLinks[i];
            }
            wb.Save();
            return row;
        }
        public void Close()
        {
            wb.Close();
        }
        
    }
}
