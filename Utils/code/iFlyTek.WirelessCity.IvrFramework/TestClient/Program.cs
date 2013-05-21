using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            using (WebClient client = new WebClient())
            {
                DateTime dt = DateTime.Now;
                client.Encoding = Encoding.UTF8;
                client.QueryString.Add("request", "中文request");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection collection = new NameValueCollection();
                collection.Add("TextBox1", "中文TextBox1");
                collection.Add("TextBox2", "中文TextBox2");
                byte[] array;
                try
                {
                    array = client.UploadValues("http://localhost:4047/Default.aspx", "POST", collection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("检测WebClient默认的超时时间："+(DateTime.Now-dt));
                    return;
                }
               
                string str1 = Encoding.Default.GetString(array);
                string str2 = Encoding.UTF8.GetString(array);
                string str3 = Encoding.GetEncoding("gb2312").GetString(array);
                Console.WriteLine(str2);
            }
             * */
            /*
            string newLine  = System.Environment.NewLine;
            Console.WriteLine(newLine);
            string str = WebClientHelper.WebClientProxy.JustTest("http://localhost:4047/Default.aspx");
            //Console.WriteLine(str);
            Lexer lexr = new Lexer(str);
            Parser parser = new Parser(lexr);
            NodeFilter filter = new AndFilter(new TagNameFilter("input"), new HasAttributeFilter("id","TextBox1"));
            //NodeList htmlNodes = parser.Parse(null);
            NodeList htmlNodes = parser.Parse(filter);
            for(int i=0;i<htmlNodes.Count;i++)
            {
                if(htmlNodes[i] is InputTag)
                {
                    InputTag tag = (InputTag) htmlNodes[i];
                    Console.WriteLine(tag.GetText());
                }
                //Console.WriteLine(htmlNodes[i]);
            }
            Console.WriteLine(str);
             */
            string str = WebClientHelper.WebClientProxy.DownloadString("http://192.168.202.32:9898/QueryPostCode.aspx");
            Console.WriteLine(str);
        }
    }
}
