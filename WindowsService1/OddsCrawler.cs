using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using OddsBusiness;
using System.Timers;
using System.Configuration;

namespace WindowsService1
{
    public partial class OddsCrawler : ServiceBase
    {
        Timer timer = new Timer();
        
        public OddsCrawler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000 * Convert.ToDouble(ConfigurationSettings.AppSettings["TimeInterval"]);
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {

            string html = Helper.GetWebSiteContent("http://www.oddschecker.com/gaelic-games/gaelic-football/"); //(ConfigurationSettings.AppSettings["URL"]);
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            CrawlFirstPage crawlpage = new CrawlFirstPage();
            ////CrawlEachMarket crawlmarket = new CrawlEachMarket();
            string msg = crawlpage.DeleteFirstPageRecords();
            string msg1 = crawlpage.CrawlMyPage(html);
            string msg2 = crawl.CrawlBettingLinks();
            //TraceService("Reply from DeleteFirstPageRecords at " + DateTime.Now + ":" + msg + "\n Reply from CrawlMyPage at " + DateTime.Now + ":" + msg1 + "\n Reply from CrawlBettingLinks at " + DateTime.Now + ":" + msg2 + "\n");
            //string html = Helper.GetWebSiteContent(ConfigurationSettings.AppSettings["URL"]);
            //CrawlAllMarkets crawl = new CrawlAllMarkets();
            //CrawlFirstPage crawlpage = new CrawlFirstPage();
            //CrawlEachMarket crawlmarket = new CrawlEachMarket();
            
            //crawlpage.CrawlMyPage(html);
            //crawl.CrawlBettingLinks();
            //crawlmarket.CrawlMarketLinks();
        }
    }
}
