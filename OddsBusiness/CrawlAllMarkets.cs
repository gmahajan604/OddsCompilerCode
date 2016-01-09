using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using OddsData;
using OddsProperties;
using System.Xml;
using System.IO;
using System.Threading;

namespace OddsBusiness
{
    public class CrawlAllMarkets
    {
        public string CrawlBettingLinks()
        {
            TraceService("Crawl BettingLinks Started: ");
            try
            {
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                DataTable dt = crawldata.GetMatchInfo();
               //Task[] tasks = new Task[dt.Rows.Count];
               // for (int i = 0; i < dt.Rows.Count; i++)
               // {
               //     string url = dt.Rows[i]["BettingLink"].ToString();
               //     int id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
               //     tasks[i] = Task.Factory.StartNew(() =>
               //     {
               //         CrawlBettingMarkets(url, id);
               //     },TaskCreationOptions.LongRunning);
               // }
               // Task.WaitAll(tasks);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string url = dt.Rows[i]["BettingLink"].ToString();
                    int id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    ThreadParameters tp = new ThreadParameters();
                    tp.URL = url;
                    tp.MatchID = id;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlBettingMarkets), tp);
                }
                return "Command completed successfully";
            }
            catch (Exception ex)
            {
              ErrorLog("CrawlBettingMarket ---- Error:"+ex.ToString());
                return ex.Message;
            }
        }
        public string CrawlBettingLinksByCoupon(bool archive)
        {
            TraceService("Crawl BettingLinks Started: ");
            try
            {
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                DataTable dt = crawldata.GetMatchByCoupon(archive);
               // Task[] tasks = new Task[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string url = dt.Rows[i]["BettingLink"].ToString();
                    int id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    ThreadParameters tp = new ThreadParameters();
                    tp.URL = url;
                    tp.MatchID = id;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlBettingMarkets), tp);
                }
               // Task.WaitAll(tasks);
                return "Command completed successfully";
            }
            catch (Exception ex)
            {
                ErrorLog("CrawlBettingMarket ---- Error:" + ex.ToString());
                return ex.Message;
            }
        }
        public void CrawlBettingMarkets(object tp)
        {
            ThreadParameters t = tp as ThreadParameters;
            string url = t.URL;
            int matchid = t.MatchID;
            try
            {
                TraceService("Crawling Betting Market, MatchID:" + matchid + " and URL: " + url + " ----");
                string htmlcontent = Helper.GetWebSiteContent(url);
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(htmlcontent);
                List<BettingMarket> bettinglist = new List<BettingMarket>();
                XmlDocument xmldoc = new XmlDocument();
                CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
                var row11 = doc.DocumentNode.SelectSingleNode("//div[@id='mc']");
                var rows = row11.SelectNodes("//ul//li[@class='more-list-li']");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        string li = rows[i].InnerText;
                        var linkrow = rows[i].SelectNodes("./a");
                        string link = "http://www.oddschecker.com" + linkrow[0].Attributes["href"].Value;
                        //links+=li+" "+link+" ";
                        if (!bettinglist.Exists(bettingmarket => bettingmarket.bettingmarket==li))
                            bettinglist.Add(new BettingMarket() { matchid = matchid, bettingmarket = li, bettinglink = link });
                    }
                }

                xmldoc = GenerateXml(bettinglist);
                crawldata.InsertMarkets(xmldoc);
                TraceService("Betting Market Data Inserted MatchID:" + matchid + " and URL:" + url + "------");
            }
            catch (Exception ex)
            {
                ErrorLog("CrawlBettingMarket______MatchID: " + matchid + " and URL:" + url +" Error:"+ex.ToString());
            }
        }
      
        private XmlDocument GenerateXml(List<BettingMarket> bettingmarkets)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("BettingMarkets");

                foreach (BettingMarket market in bettingmarkets)
                {
                    writer.WriteStartElement("BettingMarket");
                    writer.WriteElementString("MatchId", market.matchid.ToString());
                    writer.WriteElementString("Market", market.bettingmarket);
                    writer.WriteElementString("Link", market.bettinglink);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            return doc;

        }
        private void TraceService(string content)
        {
            //set up a filestream
            try
            {
                //set up a filestream
                FileStream fs = new FileStream(@"c:\OddsService\ScheduledService.txt", FileMode.OpenOrCreate, FileAccess.Write);

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
        private void ErrorLog(string content)
        {
            try
            {
                //set up a filestream
                FileStream fs = new FileStream(@"c:\OddsService\OddsCompilerErrorLog.txt", FileMode.OpenOrCreate, FileAccess.Write);

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

        #region CrawlAllMarketDataServive
        public DataTable GetMasterMarketName(int sportid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMasterMarketName(sportid);
        }
        public DataTable GetBettingMarkets(long matchid)
        {
            
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetBettingMarkets(matchid);
        }

        public DataSet GetMatchesAndMarkets(int sportid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatchesAndMarkets(sportid);
        }

        public DataSet GetMatchesAndMarketsFoSoccer()
        {

            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatchesAndMarketsForSoccer();
        }

        public string GetMarketName(long marketid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMarketName(marketid);
        }


        /// <summary>
        /// Added my dev nagar
        /// for Info of Matches by League id
        /// </summary>
        /// <param name="leagueid"></param>
        /// <returns>datatable</returns>
        public DataTable GetMatchesByLeague(int leagueid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatches(leagueid);
        }
        public DataTable GetMatchesByCoupon()
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatchesByCoupon();
        }
        public string[] GetChampionMatchInfo(int matchid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatchinfobyid(matchid);
        }
        public DataTable GetMatchResult(int matchid,int sportid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
         return   crawldata.GetMatchResult(matchid,sportid);

        }
        public DataSet GetMatchByDate(int sportid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMatchByDate(sportid);
        }
        #endregion
    }
}