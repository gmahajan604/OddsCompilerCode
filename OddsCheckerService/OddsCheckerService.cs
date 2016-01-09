using System;
using System.ServiceProcess;
using System.Timers;
using OddsBusiness;
using System.IO;

namespace OddsCheckerService
{
    public partial class OddsCheckerService : ServiceBase
    {
        Timer timer; public bool flag = true;

        public OddsCheckerService()
        {
            InitializeComponent();
            // timer.Start();
        }

        protected override void OnStart(string[] args)
        {
           // timer
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000 * 1;
            timer.Enabled = true;
            TraceService("Starting service");
            flag = false;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            TraceService("stopping service");
            timer.Dispose();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (flag == false)
            {
                flag = true;
                callback();
                timer.Interval = 60000 * 360;
                //crawlmarket.CrawlMarketLinks();
            }
            //crawlmarket.CrawlMarketLinks();
        }
        private void callback()
        {
            TraceService("Timer Started");

            CrawlAllMarkets crawl = new CrawlAllMarkets();
            CrawlFirstPage crawlpage = new CrawlFirstPage();
            ////CrawlEachMarket crawlmarket = new CrawlEachMarket();
            //string msg = crawlpage.DeleteFirstPageRecords();
            string msg1 = "", msg2 = "", msg3 = "";
            try
            {
                msg1 = DateTime.Now + ":" + crawlpage.CrawlEachSport();
            }
            catch (Exception ex)
            {
            }
            finally { TraceService("Reply from CrawlEachSport at " + msg1 + "\n"); }

            try
            {
                msg2 = DateTime.Now + ":" + crawl.CrawlBettingLinks();
            }
            catch (Exception ex) { }
            finally { TraceService("Reply from CrawlEachSport at " + msg2 + "\n"); }
            try
            {
                msg3 = DateTime.Now + ":" + crawlpage.CrawlAllGolf();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                TraceService(" Reply from CrawlEachMatchResult at " + msg3 + "\n");
            }
            try
            {
                msg3 = DateTime.Now + ":" + crawlpage.CrawlEachMatchResult();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                TraceService(" Reply from CrawlEachMatchResult at " + msg3 + "\n");
            }
        }
        private void TraceService(string content)
        {
            try
            {
                //set up a filestream
                FileStream fs = new FileStream(@"d:\ScheduledService.txt", FileMode.OpenOrCreate, FileAccess.Write);

                //set up a streamwriter for adding text
                StreamWriter sw = new StreamWriter(fs);

                //find the end of the underlying filestream
                sw.BaseStream.Seek(0, SeekOrigin.End);

                //add the text
                sw.WriteLine(content + " DateTime: " + DateTime.Now.ToString());
                //add the text to the underlying filestream

                sw.Flush();
                //close the writer
                sw.Close();
                fs.Dispose();

            }
            catch (Exception ex)
            {

            }
        }
    }
}