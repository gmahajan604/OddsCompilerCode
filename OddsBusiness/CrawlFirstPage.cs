using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data;
using OddsProperties;
using System.Web;
using System.Xml;
using OddsData;
using System.Threading;

namespace OddsBusiness
{
    public class CrawlFirstPage:CrawlGolf
    {
        #region Old_CrawlEachSport
        //public string CrawlEachSport()
        //{
        //    try
        //    {
        //        CrawlFirstPageData crawldata = new CrawlFirstPageData();
        //        DataTable dt = crawldata.GetSports();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            string link = dt.Rows[i]["link"].ToString();
        //            int sportid = Convert.ToInt32(dt.Rows[i]["sportid"].ToString());
        //            if (sportid.Equals(2))
        //            {
        //                CrawlLeagues(link);
        //                CrawlWorldMarkets();
        //                DataSet ds = crawldata.GetLeagues();
        //                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //                {
        //                    string matchlink = Convert.ToString(ds.Tables[0].Rows[j]["link"]);
        //                    int sport_id = Convert.ToInt32(ds.Tables[0].Rows[j]["sportid"]);
        //                    long leagueid = Convert.ToInt32(ds.Tables[0].Rows[j]["leagueid"]);
        //                    CrawlMyPage(matchlink, sportid, leagueid);
        //                }
        //            }
        //            else if (sportid.Equals(29))
        //            {
        //                CrawlGolf golf = new CrawlGolf();
        //                golf.CrawlGolfTurnament(link, sportid.ToString());
        //            }
        //            else
        //                CrawlMyPage(link, sportid, 0);
        //        }
        //        return "Command completed successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        #endregion
        public string CrawlEachSport()
        {
            try
            {
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                DataTable dt = crawldata.GetSports();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string link = dt.Rows[i]["link"].ToString();
                    int sportid = Convert.ToInt32(dt.Rows[i]["sportid"].ToString());
                    if (sportid.Equals(2))
                    {
                        ThreadParameters tp = new ThreadParameters();
                        tp.URL = link;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlLeagues), tp);
                      // CrawlLeagues(link);
                       CrawlWorldMarkets();
                        DataSet ds = crawldata.GetLeagues();
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            string matchlink = Convert.ToString(ds.Tables[0].Rows[j]["link"]);
                            int sport_id = Convert.ToInt32(ds.Tables[0].Rows[j]["sportid"]);
                            long leagueid = Convert.ToInt32(ds.Tables[0].Rows[j]["leagueid"]);

                            ThreadParameters t = new ThreadParameters();
                            t.URL = matchlink;
                            t.SportID = sport_id;
                            t.LeagueID = leagueid;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlMyPage), t);

                          //  CrawlMyPage(matchlink, sportid, leagueid);
                        }
                    }
                    else if(sportid.Equals(29))
                    {
                        CrawlGolf golf = new CrawlGolf();
              //        golf.CrawlGolfTurnament(link,sportid.ToString());
                    
                        ThreadParameters t = new ThreadParameters();
                      t.URL = link;
                      t.SportID = sportid;
                      t.LeagueID = 0;
                      ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlGolfTurnament), t);
                    }
                    else
                    {
                        ThreadParameters t = new ThreadParameters();
                        t.URL = link;
                        t.SportID = sportid;
                        t.LeagueID = 0;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CrawlMyPage), t);

                       // CrawlMyPage(link, sportid, 0);
                    }
                }
                return "Command completed successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CrawlMyPage(string html)
        {
            try
            {
            DataSet ds = new DataSet();
            HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
            CrawlFirstPageData crawldata = new CrawlFirstPageData();
            Matches match = new Matches();
            XmlDocument xmldoc = new XmlDocument();
            List<Matches> matchlist = new List<Matches>();
            var row11 = doc.DocumentNode.SelectSingleNode("//div[@id='fixtures']");
            
            string title = row11.SelectNodes(".//h2")[0].InnerText;
            var rows = row11.SelectNodes("//table//tr");                         
            if (rows != null)
            {
                string enddatetime = String.Empty;
                for (int ii = 1; ii < rows.Count; ii = ii + 1)
                {
                    var dr = rows[ii].InnerText.Trim();
                    var cols = rows[ii].SelectNodes("./td[@class='day']");

                    if (cols != null)
                    {
                        string t = cols[0].InnerText.Trim();
                        match.date = t;
                        string[] matchdate = t.Split(' ');
                        enddatetime = matchdate[1].Substring(0, 2) + " " + matchdate[2].Substring(0, 3) + " " + matchdate[3];
                        enddatetime = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                    var colnew = rows[ii].SelectNodes("./td");
                       
                     if (colnew != null && colnew.Count>4)
                     {
                            match.time = colnew[0].InnerText.Trim();
                            var Dlink = colnew[4].InnerText.Trim();
                            var spn = colnew[1].SelectNodes(".//span[@class='fixtures-bet-name']");
                            string Place = spn[0].InnerText.Trim();       
                            var odds = colnew[1].SelectNodes(".//span[@class='odds']");
                            string Od = odds[0].InnerText.Trim();
                            match.home = Place +" "+ Od;
                            match.draw = colnew[2].InnerText.Trim();             
                            match.away = colnew[3].InnerText.Trim();
                            var link = colnew[4].SelectNodes("./a[contains(@href, '/gaelic-games/gaelic-football/')]");
                            match.bettinglink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "/betting-markets");
                            match.Displayenddatetime = DateTime.Parse(enddatetime +"T"+ match.time);
                            match.resultlink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "");
                           //match.bettinglink = "http://www.oddschecker.com/"+link[0].Attributes["href"].Value;
                            matchlist.Add(new Matches() { date = match.date,time=match.time, home = match.home.Substring(0,match.home.IndexOf("(")).Trim(), draw = match.draw.Substring(0,match.draw.IndexOf("(")).Trim(), away = match.away.Substring(0,match.away.IndexOf("(")).Trim(), bettinglink = match.bettinglink, Displayenddatetime = match.Displayenddatetime,resultlink="" });
                        }
                    }

                 }

                xmldoc = GenerateXml(matchlist);
                crawldata.InsertMatchInfo(xmldoc);
                
            }
            //ds = crawldata.NewRecords(xmldoc);
            //return ds;
            return "Command completed successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void CrawlMyPage(Object tp)
        {
            ThreadParameters dev = new ThreadParameters();
            dev = tp as ThreadParameters;
            string url = dev.URL;
            int sportid = dev.SportID;
            long leagueid = dev.LeagueID;
            try
            {
                TraceService("Crawling Started: League ID:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");

                System.IO.StreamReader rader;
                string shtml = Helper.GetWebSiteContent(url);
                //  shtml = shtml.Replace("<!doctype HTML>", "");
                DataSet ds = new DataSet();
                //  HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                Matches match = new Matches();
                HtmlDocument doc = Helper.LoadHtml(shtml);
                TextReader tr = new StringReader(shtml);
                //xmldoc.Load(tr);
                XmlDocument xmldoc = new XmlDocument();
                List<Matches> matchlist = new List<Matches>();
                var row11 = doc.DocumentNode.SelectSingleNode("//div[@id='fixtures']");

                string title = row11.SelectNodes(".//h2")[0].InnerText;
                var rows = row11.SelectNodes("//table//tr");
                if (rows != null)
                {
                    string enddatetime = String.Empty;
                    for (int ii = 1; ii < rows.Count; ii = ii + 1)
                    {
                        var dr = rows[ii].InnerText.Trim();
                        var cols = rows[ii].SelectNodes("./td[@class='day']");

                        if (cols != null)
                        {
                            string t = cols[0].InnerText.Trim();
                            match.date = t;
                            string[] matchdate = t.Split(' ');
                            enddatetime = matchdate[1].Substring(0, (matchdate[1].Length - 2)) + " " + matchdate[2].Substring(0, 3) + " " + matchdate[3];
                            enddatetime = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            var colnew = rows[ii].SelectNodes("./td");

                            if (colnew != null && colnew.Count > 4)
                            {
                                match.time = colnew[0].InnerText.Trim();
                                var Dlink = colnew[4].InnerText.Trim();
                                var home = colnew[1].SelectNodes(".//span[@class='fixtures-bet-name']");
                                var draw = colnew[2].SelectNodes(".//span[@class='fixtures-bet-name']");
                                var away = colnew[3].SelectNodes(".//span[@class='fixtures-bet-name']");
                                match.home = home[0].InnerText.Trim();
                                match.draw = draw[0].InnerText.Trim();
                                match.away = away[0].InnerText.Trim();
                                match.createddate = DateTime.Now;
                                //var link = colnew[4].SelectNodes("./a[contains(@href, '/gaelic-games/gaelic-football/')]");
                                var link = colnew[4].SelectNodes("./a");
                                match.bettinglink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "/betting-markets");
                                match.resultlink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "/winner");
                                match.Displayenddatetime = DateTime.Parse(enddatetime + " " + match.time);
                                match.league = leagueid.ToString();

                                //match.bettinglink = "http://www.oddschecker.com/"+link[0].Attributes["href"].Value;
                                matchlist.Add(new Matches() { date = match.date, time = match.time, home = match.home, draw = match.draw, away = match.away, bettinglink = match.bettinglink, Displayenddatetime = match.Displayenddatetime, resultlink = match.resultlink });
                                crawldata.InsertMatchinfoDev(match, sportid);
                            }
                        }
                    }

                    xmldoc = GenerateXml(matchlist);
                    // crawldata.InsertMatchInfo(xmldoc,sportid,leagueid);
                    TraceService("Data Inserted: League ID:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");
                }
                //ds = crawldata.NewRecords(xmldoc);
                //return ds;
              //  return "Command completed successfully";
            }
            catch (Exception ex)
            {
                TraceService("Error:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");
               // return ex.Message;
            }
        }
        public void CrawlGolfTurnament(object tp)
        {
            ThreadParameters t = new ThreadParameters();
            t = tp as ThreadParameters;
            string url = t.URL;
            int sportid = t.SportID;
            long leagueid = t.LeagueID;

            try
            {
                TraceService("Crawling Started: Golf:0 ,SportID:" + sportid + " , URL:" + url + "\n");

                string html = Helper.GetWebSiteContent(url);
                DataSet ds = new DataSet();
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                Matches match = new Matches();
                XmlDocument xmldoc = new XmlDocument();
                List<GolfTurnament> turnamentlist = new List<GolfTurnament>();
                var row11 = doc.DocumentNode.SelectSingleNode("//div[@class='containerHeight']");

                var row = row11.SelectNodes(".//a[@class='stats_link']");
                var rows = row11.SelectNodes("//div[@class='stats']");
                if (rows != null)
                {
                    string enddatetime = String.Empty;
                    for (int ii = 0; ii < rows.Count; ii = ii + 0)
                    {
                        GolfTurnament golf = new GolfTurnament();
                        int i = ii;
                        try
                        {
                            var n = rows[ii].ParentNode;
                            if (n.Name == "a")
                            {
                                if (n == null)
                                    golf.Link = "";
                                else golf.Link = "http://wikiform.com.au/oddschecker/" + n.Attributes["href"].Value;
                            }
                            else golf.Link = "";

                            if (rows[ii].Attributes["style"].Value == "clear:both;width:100px;")
                            {
                                golf.Duration = rows[ii].InnerText.Trim();
                                ii += 1;
                            }
                            else golf.Duration = "";
                            if (rows[ii].Attributes["style"].Value == "width:317px;text-decoration:underline;" || rows[ii].Attributes["style"].Value == "width:317px;")
                            {
                                golf.Turnament = rows[ii].InnerText.Trim();
                                ii += 1;
                            }
                            else golf.Turnament = "";
                            if (rows[ii].Attributes["style"].Value == "width:242px;")
                            {
                                golf.Course = rows[ii].InnerText.Trim();
                                ii += 1;
                            }
                            else golf.Course = "";
                            if (rows[ii].Attributes["style"].Value == "width:122px;border-right:0;")
                            {
                                golf.Champion = rows[ii].InnerText.Trim();
                                ii += 1;
                            }
                            else golf.Champion = "";
                            InsertGoldTurnament(golf);
                        }
                        catch (Exception ex)
                        {
                        }
                        if (i == ii)
                            ii += 1;
                        turnamentlist.Add(golf);
                    }

                  //  xmldoc = GenerateXmlGolfTurnament(turnamentlist);
                    //  InsertGoldTurnament(xmldoc, sportid);
                    TraceService("Data Inserted: Golf:0  ,SportID:" + sportid + " , URL:" + url + "\n");
                }
                //ds = crawldata.NewRecords(xmldoc);
                //return ds;
            }
            catch (Exception ex)
            {
                TraceService("Error:0 ,SportID:" + sportid + " , URL:" + url + "\n");
            }
        }
        //public string CrawlMyPage(string url,int sportid,long leagueid)
        //{
        //    try
        //    {
        //        TraceService("Crawling Started: League ID:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");

        //        System.IO.StreamReader rader;
        //        string shtml = Helper.GetWebSiteContent(url);
        //    //  shtml = shtml.Replace("<!doctype HTML>", "");
        //        DataSet ds = new DataSet();
        //    //  HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
        //        CrawlFirstPageData crawldata = new CrawlFirstPageData();
        //        Matches match = new Matches();
        //        HtmlDocument doc = Helper.LoadHtml(shtml);
        //        TextReader tr = new StringReader(shtml);
        //      //xmldoc.Load(tr);
        //        XmlDocument xmldoc = new XmlDocument();
        //        List<Matches> matchlist = new List<Matches>();
        //        var row11 =doc.DocumentNode.SelectSingleNode("//div[@id='fixtures']");

        //        string title = row11.SelectNodes(".//h2")[0].InnerText;
        //        var rows = row11.SelectNodes("//table//tr");
        //        if (rows != null)
        //        {
        //            string enddatetime = String.Empty;
        //            for (int ii = 1; ii < rows.Count; ii = ii + 1)
        //            {
        //                var dr = rows[ii].InnerText.Trim();
        //                var cols = rows[ii].SelectNodes("./td[@class='day']");

        //                if (cols != null)
        //                {
        //                    string t = cols[0].InnerText.Trim();
        //                    match.date = t;
        //                    string[] matchdate = t.Split(' ');
        //                    enddatetime = matchdate[1].Substring(0, (matchdate[1].Length - 2)) + " " + matchdate[2].Substring(0, 3) + " " + matchdate[3];
        //                    enddatetime = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd");
        //                }
        //                else
        //                {
        //                    var colnew = rows[ii].SelectNodes("./td");

        //                    if (colnew != null && colnew.Count > 4)
        //                    {
        //                        match.time = colnew[0].InnerText.Trim();
        //                        var Dlink = colnew[4].InnerText.Trim();
        //                        var home = colnew[1].SelectNodes(".//span[@class='fixtures-bet-name']");
        //                        var draw = colnew[2].SelectNodes(".//span[@class='fixtures-bet-name']");
        //                        var away = colnew[3].SelectNodes(".//span[@class='fixtures-bet-name']");
        //                        match.home = home[0].InnerText.Trim();
        //                        match.draw = draw[0].InnerText.Trim();
        //                        match.away = away[0].InnerText.Trim();
        //                        match.createddate = DateTime.Now;
        //                        //var link = colnew[4].SelectNodes("./a[contains(@href, '/gaelic-games/gaelic-football/')]");
        //                        var link = colnew[4].SelectNodes("./a");
        //                        match.bettinglink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "/betting-markets");
        //                        match.resultlink = "http://www.oddschecker.com/" + link[0].Attributes["href"].Value.Replace("/winner", "/winner");
        //                        match.Displayenddatetime = DateTime.Parse(enddatetime + " " + match.time);
        //                        match.league = leagueid.ToString();

        //                        //match.bettinglink = "http://www.oddschecker.com/"+link[0].Attributes["href"].Value;
        //                        matchlist.Add(new Matches() { date = match.date, time = match.time, home = match.home, draw = match.draw, away = match.away, bettinglink = match.bettinglink, Displayenddatetime = match.Displayenddatetime,resultlink=match.resultlink });
        //                        crawldata.InsertMatchinfoDev(match,sportid);
        //                    }
        //                }
        //            }

        //            xmldoc = GenerateXml(matchlist);
        //            // crawldata.InsertMatchInfo(xmldoc,sportid,leagueid);
        //            TraceService("Data Inserted: League ID:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");
        //        }
        //        //ds = crawldata.NewRecords(xmldoc);
        //        //return ds;
        //        return "Command completed successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceService("Error:" + leagueid + " ,SportID:" + sportid + " , URL:" + url + "\n");
        //        return ex.Message;
        //    }
        //}

        public void CrawlLeagues(object threadparam)
        {
            ThreadParameters p = threadparam as ThreadParameters;
            string url = p.URL;
            try
            {
                TraceService("Crawling Started: --------Lague--------SportID: Footbal , URL:" + url + "\n");
               
                string html = Helper.GetWebSiteContent(url);
                DataSet ds = new DataSet();
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                Matches match = new Matches();
                XmlDocument xmldoc = new XmlDocument();
                List<Leagues> leaguelist = new List<Leagues>();
                var list = doc.DocumentNode.SelectSingleNode("//ul[@id='sport-nav']//ul");
                int check = 0;
                
                var nodes = list.SelectNodes("./li");
                if (nodes != null)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        //if (check > 0)
                        //{
                        //    var node = nodes[i].SelectNodes("./a");
                        //    if (node != null)
                        //    {
                        //            string league = node[0].InnerText;
                        //            string link = "http://www.oddschecker.com" + node[0].Attributes["href"].Value;
                        //            leaguelist.Add(new Leagues() { League = league, Link = link });
                        //    } 
                        //}
                        //if (nodes[i].InnerText.Trim().Equals("All Events"))
                        //{
                        //    //i=nodes.GetNodeIndex(nodes[i]);
                        //    check++;
                        //}
                        if (nodes[i].InnerText.Trim().Contains("English"))
                        {
                            if (nodes[i].InnerText.Trim().Substring(0, 7).Equals("English"))
                            {
                                var node = nodes[i].SelectNodes(".//a");
                                if (node != null)
                                {
                                    for (int j = 0; j < node.Count; j++)
                                    {
                                        string league = node[j].InnerText;
                                        string link = "http://www.oddschecker.com" + node[j].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = "English" });
                                    }
                                }
                            }
                        }

                        if (nodes[i].InnerText.Trim().Contains("Scottish"))
                        {
                            if (nodes[i].InnerText.Trim().Substring(0, 8).Equals("Scottish"))
                            {
                                var node = nodes[i].SelectNodes(".//a");
                                if (node != null)
                                {
                                    for (int j = 0; j < node.Count; j++)
                                    {
                                        string league = node[j].InnerText;
                                        string link = "http://www.oddschecker.com" + node[j].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = "Scottish" });
                                    }
                                }


                            }
                        }

                        if (nodes[i].InnerText.Trim().Contains("European"))
                        {
                            if (nodes[i].InnerText.Trim().Substring(0, 8).Equals("European"))
                            {
                                var node = nodes[i].SelectNodes(".//a");
                                if (node != null)
                                {
                                    for (int j = 0; j < node.Count; j++)
                                    {
                                        string league = node[j].InnerText;
                                        string link = "http://www.oddschecker.com" + node[j].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = "European" });
                                    }
                                }

                            }
                        }

                        if (nodes[i].InnerText.Trim().Contains("Champions League"))
                        {
                            if (nodes[i].InnerText.Trim().Substring(0, 16).Equals("Champions League"))
                            {
                                var node = nodes[i].SelectNodes(".//a");
                                if (node != null)
                                {
                                    for (int j = 0; j < node.Count; j++)
                                    {
                                        string league = node[j].InnerText;
                                        string link = "http://www.oddschecker.com" + node[j].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = "Champions League" });
                                    }
                                }
                            }

                        }

                        if (nodes[i].InnerText.Trim().Contains("Europa League"))
                        {
                            if (nodes[i].InnerText.Trim().Substring(0, 13).Equals("Europa League"))
                            {
                                var node = nodes[i].SelectNodes(".//a");
                                if (node != null)
                                {
                                    for (int j = 0; j < node.Count; j++)
                                    {
                                        string league = node[j].InnerText;
                                        string link = "http://www.oddschecker.com" + node[j].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = "Europa League" });
                                    }
                                }

                            }
                        }
                    }
                    xmldoc = GenerateXmlForLeagues(leaguelist);
                    crawldata.InsertLeagues(xmldoc);
                    
                    TraceService("Data Inserted : --------Lague--------SportID: Footbal , URL:" + url + "\n");

                }

              
            }
            catch (Exception ex)
            {
                TraceService("Error  : --------Lague--------SportID: Footbal , URL:" + url + "\n");
                //return ex.Message;
            }
        }

        private XmlDocument GenerateXmlForLeagues(List<Leagues> leaguelist)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Leagues");

                foreach (Leagues league in leaguelist)
                {
                    writer.WriteStartElement("League");

                    writer.WriteElementString("Name", league.League);
                    writer.WriteElementString("Link", league.Link);
                    writer.WriteElementString("Section", league.Section);
                    
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


            }
            
            return doc;

        }
       
        private XmlDocument GenerateXml(List<Matches> matchlist)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
           {
                writer.WriteStartDocument();
                writer.WriteStartElement("Matches");

                foreach (Matches match in matchlist)
                {
                    writer.WriteStartElement("Match");

                    writer.WriteElementString("Date", match.date);
                    writer.WriteElementString("Time", match.time);
                    writer.WriteElementString("Home", match.home);
                    writer.WriteElementString("Draw", match.draw);
                    writer.WriteElementString("Away", match.away);
                    writer.WriteElementString("BettingLink", match.bettinglink);
                    writer.WriteElementString("DisplayEndDateTime", Convert.ToString(match.Displayenddatetime));
                    writer.WriteElementString("ResultLink", match.resultlink);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
              

           }

            return doc;
            
        }
        #region CrawlFirstPageDataService
        public string DeleteFirstPageRecords()
        {
            try
            {
                CrawlFirstPageData crawl = new CrawlFirstPageData();
                string msg = crawl.DeleteFirstPageRecords();
                return msg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public DataTable GetMatchInfo()
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            DataTable dt = crawl.GetMatchInfo();
            return dt;
        }
       
        /// <summary>
        /// Get Match Info
        /// Dev Nagar
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public DataSet GetMatchResult(int matchid)
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            DataSet dt = crawl.GetMatchResult(matchid);
            return dt;
        }

        public DataTable GetMatchInfo(long matchid)
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            DataTable dt = crawl.GetMatchInfo(matchid);
            return dt;
        }

        public string GetMatchName(long matchid)
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            return crawl.GetMatchName(matchid);
        }

        public DataTable GetMatchListByTime()
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            return crawl.GetMatchListByTime();
        }

        public string UpdateMatch(long id, string matchdate, string displayenddatetime)
        {
            try
            {
                CrawlFirstPageData crawl = new CrawlFirstPageData();
                string msg = crawl.UpdateMatch(id, matchdate, displayenddatetime);
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating match date");
            }
        }
        public DataTable GetSports()
        {
           
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                DataTable dt = crawldata.GetSports();
                return dt;
        }
        public DataTable GetSports(string sportname)
        {

            CrawlFirstPageData crawldata = new CrawlFirstPageData();
            DataTable dt = crawldata.GetSports(sportname);
            return dt;
        }

        /// <summary>
        /// Get All Match List and Crawl data result
        /// </summary>
        /// <returns></returns>
        public string CrawlEachMatchResult()
        {
            try
            {
                CrawlFirstPageData crawldata = new CrawlFirstPageData();

                DataTable ds = crawldata.GetMatchResultLinkForCrawl();
                for (int j = 0; j < ds.Rows.Count; j++)
                {
                    string matchlink = Convert.ToString(ds.Rows[j]["BettingLink"]).Replace("/betting-markets", "/winner");
                    string matchid = ds.Rows[j]["id"].ToString();
                    CrawlChampionLeauge(matchid.ToString(), matchlink, "");
                }

                return "Command completed successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string CrawlMatchResultByTime()
        {

            try
            {
                CrawlFirstPageData crawldata = new CrawlFirstPageData();

                DataTable ds = crawldata.GetMatchListByTime();
                for (int j = 0; j < ds.Rows.Count; j++)
                {
                    string matchlink = Convert.ToString(ds.Rows[j]["BettingLink"]).Replace("/betting-markets", "/winner");
                    string matchid = ds.Rows[j]["id"].ToString();
                    CrawlChampionLeauge(matchid.ToString(), matchlink, "");
                }

                return "Command completed successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataSet GetLeague()
        {
            CrawlFirstPageData crawldata = new CrawlFirstPageData();
            DataSet ds = crawldata.GetLeagues();
            return ds;
        }
        #endregion

        public void CrawlWorldMarkets()
        {
            try
            {
                TraceService("Crawling Started:-------------- World Market:--------------------------");

                string htmlcontent = Helper.GetWebSiteContent("http://www.oddschecker.com/football/world");
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(htmlcontent);
                List<BettingMarket> bettinglist = new List<BettingMarket>();
                List<Leagues> leaguelist = new List<Leagues>();
                HashSet<string> sections = new HashSet<string>();
                XmlDocument xmldoc = new XmlDocument();
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                string links = "";
                var row11 = doc.DocumentNode.SelectSingleNode("//div[@id='mc']");
                var rows2 = row11.SelectNodes(".//div[@class='module']");

                for (int j = 0; j < rows2.Count; j++)
                {
                    TraceService("Crawling Started For Section:-------------- World Market:--------------------------");

                    var heading = rows2[j].SelectNodes(".//h2");


                    var rows = rows2[j].SelectNodes(".//ul//li[@class='fourth-level-li']");
                    if (rows != null)
                    {
                        Task[] tasks = new Task[rows.Count];
                        for (int i = 0; i < rows.Count; i++)
                        {
                            //tasks[i] = Task.Factory.StartNew(() =>
                            //{
                            string section = rows[i].InnerText;
                            var linkrow = rows[i].SelectNodes("./a");
                            string link = "http://www.oddschecker.com/" + linkrow[0].Attributes["href"].Value;

                            TraceService("Crawling Started For Section:"+section+"  URL"+link+"-------------- World Market:--------------------------");

                            //links+=li+" "+link+" ";
                            if (heading != null)
                            {
                                if (heading[0].InnerText.Equals("World Leagues"))
                                {
                                    if (sections.Add(section))
                                    {
                                        tasks[i] = Task.Factory.StartNew(() =>
                                        {
                                            CrawlLeagues(section, link, leaguelist);
                                        }, TaskCreationOptions.LongRunning);
                                    }
                                }
                                else
                                {
                                    //if(sections.Add(heading[0].InnerText))
                                    tasks[i] = Task.Factory.StartNew(() =>
                                        {

                                            leaguelist.Add(new Leagues() { League = section, Link = link, Section = heading[0].InnerText });
                                        });
                                }
                            }
                            else
                            {
                                if (sections.Add(section))
                                {
                                    tasks[i] = Task.Factory.StartNew(() =>
                                   {
                                       CrawlLeagues(section, link, leaguelist);
                                   }, TaskCreationOptions.LongRunning);
                                }
                            }
                        }
                        Task.WaitAll(tasks);
                    }
                    //var rows = doc.DocumentNode.SelectNodes("//ul//li[@class='fourth-level-li']");
                    //if (rows != null)
                    //{
                    //    for (int i = 0; i < rows.Count; i++)
                    //    {
                    //        string section = rows[i].InnerText;
                    //        var linkrow = rows[i].SelectNodes("./a");
                    //        string link = "http://www.oddschecker.com" + linkrow[0].Attributes["href"].Value;
                    //        //links+=li+" "+link+" ";
                    //        if (sections.Add(section))
                    //            {
                    //                CrawlLeagues(section, link, leaguelist);
                    //            }
                    //    }
                    //}

                    xmldoc = GenerateXmlForLeagues(leaguelist);
                    crawldata.InsertLeagues(xmldoc);
                    TraceService("Data Inserted:-------------- World Market:--------------------------");
                }
            }
            catch (Exception ex)
            {
                TraceService("Error  : --------League--------SportID: Footbal , Error:" + ex.ToString() + "\n");
            }
        }

        public void CrawlLeagues(string section,string url,List<Leagues> leaguelist)
        {
            TraceService("Crawling Started:----League------ Section:"+section+" URL: "+url+" -");
            try
            {
                string html = Helper.GetWebSiteContent(url);
                DataSet ds = new DataSet();
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                Matches match = new Matches();
                XmlDocument xmldoc = new XmlDocument();

                var list = doc.DocumentNode.SelectSingleNode("//ul[@id='sport-nav']//ul");
                int check = 0;

                var sectionnodes = list.SelectNodes(".//li[@class='level-4 nav-content-item more-li first']");
                if (sectionnodes != null)
                {
                    for (int x = 0; x < sectionnodes.Count; x++)
                    {
                        if (sectionnodes[x].InnerText.Trim().Contains(section))
                        {
                            if (sectionnodes[x].InnerText.Trim().Substring(0, section.Length).Equals(section))
                            {
                                var nodes = sectionnodes[x].SelectNodes(".//a");
                                if (nodes != null)
                                {
                                    for (int i = 0; i < nodes.Count; i++)
                                    {
                                        string league = nodes[i].InnerText;
                                        string link = "http://www.oddschecker.com" + nodes[i].Attributes["href"].Value;
                                        leaguelist.Add(new Leagues() { League = league, Link = link, Section = section });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService("Error:----League------ Section:" + section + " URL: " + url + " - "+ex.ToString());
            }
        }

    
        public void CrawlChampionLeauge(string id,string link, string time)
        {
            try
            {
                // added file parameter for ftp(file) (GET,POST), remove it for http (GET,POST)
                string html = Helper.GetWebSiteContent(link);
                DataSet ds = new DataSet();
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                CrawlFirstPageData crawldata = new CrawlFirstPageData();
                Matches match = new Matches();
                MatchResult mr = new MatchResult();
                XmlDocument xmldoc = new XmlDocument();
                string[] result = new string[17];
                var list = doc.DocumentNode.SelectSingleNode("//div[@id='betting_stats']//div");
                result[0] = id;
                var nodes = list.SelectNodes("//tr");
                result[2] = list.SelectSingleNode("//div[@id='ht_score']").InnerText.Trim();
                result[1] = list.SelectSingleNode("//span[@class='score']").InnerText.Trim();

                    result[3] = list.SelectSingleNode("//div[@id='first_gs']").InnerText.Trim();
                    result[4] = list.SelectSingleNode("//div[@id='last_gs']").InnerText.Trim();
                    result[5] = result[3].Substring(result[3].IndexOf("mins")-3,2)+"";
                    result[6] = result[4].Substring(result[4].IndexOf("mins") - 3, 2) + "";
                    result[7] = "";
                    result[8] = list.SelectSingleNode("//div[@id='first_hgs']").InnerText.Trim();
                    result[9] = list.SelectSingleNode("//div[@id='last_hgs']").InnerText.Trim();
                    result[10] = list.SelectSingleNode("//div[@id='first_ags']").InnerText.Trim();
                    result[11] = list.SelectSingleNode("//div[@id='last_ags']").InnerText.Trim();
                    result[12] = list.SelectSingleNode("//div[@id='first_booking']").InnerText.Trim();
                    result[13] = list.SelectSingleNode("//div[@id='last_booking']").InnerText.Trim();
                    result[14] = list.SelectSingleNode("//div[@id='total_yellow']").InnerText.Trim();
                    result[15] = list.SelectSingleNode("//div[@id='total_red']").InnerText.Trim();
                    result[16] = list.SelectSingleNode("//div[@id='bookings_index']").InnerText.Trim();

                    // Get Update Market(Match) Result
                    MatchResult(result,id);
              
                    crawldata.InsertChampionResult(result);
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }

        }
        private void MatchResult(string[] result, string matchid)
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            int sportid = crawl.GetSportID(Convert.ToInt32(matchid));
             CrawlAllMarkets crawls=new CrawlAllMarkets();
            DataTable dt = new DataTable();
            dt = crawls.GetMatchResult(Convert.ToInt32(matchid),sportid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (dt.Rows.Count > 0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    DataRow row=dt.Rows[i];
                    if (row[0].ToString() == "Winner")
                    {
                        string[] score = result[1].Split('-');
                        if (Convert.ToInt32(score[0]) > Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Home";

                        else if (Convert.ToInt32(score[0]) < Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Away";
                        else dt.Rows[i][1] = "Draw";
                    }
                    else if (row[0].ToString() == "HalfTime")
                        dt.Rows[i][1] =((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1)));
                    else if (row[0].ToString() == "BothTeamToScore")
                    {
                        string[] scorefull = result[1].Split('-'); string full;
                        if (Convert.ToInt32(scorefull[0]) > 0 & Convert.ToInt32(scorefull[1]) > 0)
                            dt.Rows[i][1] = "YES";
                        else dt.Rows[i][1] = "NO";
                    }
                    else if (row[0].ToString() == "HalfTimeFullTimeWinner")
                    {
                        string[] scorefull = result[1].Split('-'); string full;
                        string[] scorehalf = ((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1))).Split('-'); string half = "";
                        if (Convert.ToInt32(scorehalf[0]) > Convert.ToInt32(scorehalf[1]))
                            half = "Home";
                        else if (Convert.ToInt32(scorehalf[0]) < Convert.ToInt32(scorehalf[1]))
                            half = "Away";
                        else half = "Draw";

                        if (Convert.ToInt32(scorefull[0]) > Convert.ToInt32(scorefull[1]))
                            full = "Home";
                        else if (Convert.ToInt32(scorefull[0]) < Convert.ToInt32(scorefull[1]))
                            full = "Away";
                        else full = "Draw";
                       
                        dt.Rows[i][1]= half + "/" + full;
                    }
                    else if (row[0].ToString() == "CorrectScore")
                    {
                      dt.Rows[i][1]=   result[2].Substring(0, result[2].IndexOf('-') - 1) + " " + result[1] + " " + result[2].Substring(result[2].IndexOf('-') + 2, result[2].Length - result[2].Substring(0, result[2].IndexOf('-') + 1).Length);
                    }
                    else if (row[0].ToString() == "HalfTimeScore")
                    {
                        dt.Rows[i][1] =((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1)));
                    }
                    else if (row[0].ToString() == "Handicaps")
                    {
                        dt.Rows[i][1] = "";
                    }
                    else if (row[0].ToString() == "AlternativeHandicaps")
                    {
                        dt.Rows[i][1] = "";
                    }
                    else if (row[0].ToString() == "MatchResultAndBoth")
                    {
                        string[] scorefull = result[1].Split('-'); string full;
                        if (Convert.ToInt32(scorefull[0]) > 0 & Convert.ToInt32(scorefull[1]) > 0)
                            dt.Rows[i][1] = "YES" +" "+result[1];
                        else dt.Rows[i][1] = "NO" + " " + result[1];
                    }
                    else if (row[0].ToString() == "DoubleChanceWinner")
                    {
                        string[] scorefull = result[1].Split('-'); string full;
                        if (Convert.ToInt32(scorefull[0]) ==Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Home or Draw, Away or Draw";
                        else if(Convert.ToInt32(scorefull[0]) >Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Home or Away, Home or Draw";
                        else if (Convert.ToInt32(scorefull[0]) < Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Home or Away , Away or Draw";
                    }
                    else if (row[0].ToString() == "DoubleChanceBeaten")
                    {
                        string[] scorefull = result[1].Split('-'); string full;
                        if (Convert.ToInt32(scorefull[0]) == Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Home or Away";
                        else if (Convert.ToInt32(scorefull[0]) > Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Away or Draw";
                        else if (Convert.ToInt32(scorefull[0]) < Convert.ToInt32(scorefull[1]))
                            dt.Rows[i][1] = "Home or Draw";
                    }
                    else if (row[0].ToString() == "TeamToScoreFirst")
                    {
                        string hgt = result[8].Substring(result[8].IndexOf("mins") - 3, 2) + "";
                        string agt = result[10].Substring(result[10].IndexOf("mins") - 3, 2) + "";
                        if (Convert.ToInt32(hgt) < Convert.ToInt32(agt))
                            dt.Rows[i][1] = "Home";
                        else if (Convert.ToInt32(hgt) < Convert.ToInt32(agt))
                            dt.Rows[i][1] = "Away";
                        else dt.Rows[i][1] = "Both";
                    }
                    else if (row[0].ToString() == "ToWinBothHalves")
                    {
                        string[] hscore =((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1))).Split('-');
                        string[] score = result[1].Split('-');
                        if (Convert.ToInt32(score[0]) > Convert.ToInt32(score[1]) && Convert.ToInt32(hscore[0]) > Convert.ToInt32(hscore[1]))
                            dt.Rows[i][1] = "Home";
                        else if (Convert.ToInt32(score[0]) < Convert.ToInt32(score[1]) && Convert.ToInt32(hscore[0]) < Convert.ToInt32(hscore[1]))
                            dt.Rows[i][1] = "Away";
                        else dt.Rows[i][1] = "Nither";
                    }
                    else if (row[0].ToString() == "ToWinEitherHalf")
                    {
                        string[] hscore =((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1))).Split('-');
                        string[] score = result[1].Split('-');
                        if (Convert.ToInt32(hscore[0]) > Convert.ToInt32(hscore[1]))
                            dt.Rows[i][1] = "Home";
                        else if (Convert.ToInt32(hscore[0]) > Convert.ToInt32(hscore[1])) dt.Rows[i][1] = "Away";
                        else dt.Rows[i][1] = "Draw";

                         if (Convert.ToInt32(score[0]) > Convert.ToInt32(score[1]))
                            dt.Rows[i][1] += "Home";
                         else if (Convert.ToInt32(score[0]) < Convert.ToInt32(score[1])) dt.Rows[i][1] += "Away";
                        else dt.Rows[i][1] += "Draw";
                    }
                    else if (row[0].ToString() == "ToWinFormBehind")
                    {
                        string[] hscore =((result[2].Substring(result[2].IndexOf('-') - 1, 1)) + "-" + (result[2].Substring(result[2].IndexOf('-') + 1, 1))).Split('-');
                        string[] score = result[1].Split('-');
                        if (Convert.ToInt32(hscore[0]) < Convert.ToInt32(hscore[1]) && Convert.ToInt32(score[0]) > Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Home";
                        else if(Convert.ToInt32(hscore[0]) > Convert.ToInt32(hscore[1]) && Convert.ToInt32(score[0]) < Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Away";
                        else dt.Rows[i][1] = "Nither";
                    }
                    else if (row[0].ToString() == "TimeOfFirstGoal")
                    {
                        dt.Rows[i][1] = result[6];
                    }
                    else if (row[0].ToString() == "TimeofLastGoal")
                    {
                        dt.Rows[i][1] = result[7];
                    }
                    else if (row[0].ToString() == "ExactTotalGoal")
                    {
                        dt.Rows[i][1] = "";
                    }
                    else if (row[0].ToString() == "ToScoreFirstAndNotWin")
                    {
                        string[] score = result[1].Split('-');
                        string hgt = result[8].Substring(result[8].IndexOf("mins") - 3, 2) + "";
                        string agt = result[10].Substring(result[10].IndexOf("mins") - 3, 2) + "";
                        if (Convert.ToInt32(hgt) < Convert.ToInt32(agt))
                        {
                            if(Convert.ToInt32(score[0])<Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Home";
                        }
                        else if (Convert.ToInt32(hgt) < Convert.ToInt32(agt))
                        {
                            if (Convert.ToInt32(score[0]) > Convert.ToInt32(score[1]))
                            dt.Rows[i][1] = "Away";
                        }
                        if (dt.Rows[i][1].ToString() == "") dt.Rows[i][1] = "No One";
                    }
                    else if (row[0].ToString() == "FirstGoalScorer")
                    {
                        dt.Rows[i][1] = result[3];
                    }
                    else if (row[0].ToString() == "LastGoalScorer")
                    {
                        dt.Rows[i][1] = result[4];
                    }
                    else if (row[0].ToString() == "AnyTimeGoalScorer")
                    {
                        dt.Rows[i][1] = ""; 
                    }
                    else if (row[0].ToString() == "ToScoreToOrMoreGoales")
                    {
                        dt.Rows[i][1] = "";
                    }
                    else if (row[0].ToString() == "ToScoreHatTrick")
                    {
                        dt.Rows[i][1] = "";
                    }
                    else if (row[0].ToString() == "TotalScore")
                    {
                        dt.Rows[i][1] = result[1];
                    }
                    else if (row[0].ToString() == "OverUnder0.5")
                    {
                        string[] score = result[1].ToString().Trim(' ').Split('-');
                        int total = Convert.ToInt32(score[0]) + Convert.ToInt32(score[1]);
                        if (total >= 1)
                            dt.Rows[i][1] = "Winner";
                        else dt.Rows[i][1] = "Beaten";
                    }
                    else if (row[0].ToString() == "OverUnder1.5")
                    {
                        string[] score = result[1].Trim(' ').Split('-');
                        int total = Convert.ToInt32(score[0]) + Convert.ToInt32(score[1]);
                        if (total >= 2)
                            dt.Rows[i][1] = "Winner";
                        else dt.Rows[i][1] = "Beaten";
                    }
                    else if (row[0].ToString() == "OverUnder2.5")
                    {
                        string[] score = result[1].Trim(' ').Split('-');
                        int total = Convert.ToInt32(score[0]) + Convert.ToInt32(score[1]);
                        if (total >= 3)
                            dt.Rows[i][1] = "Winner";
                        else dt.Rows[i][1] = "Beaten";
                    }
                    else if (row[0].ToString() == "OverUnder3.5")
                    {
                        string[] score = result[1].Trim(' ').Split('-');
                        int total = Convert.ToInt32(score[0]) + Convert.ToInt32(score[1]);
                        if (total >= 4)
                            dt.Rows[i][1] = "Winner";
                        else dt.Rows[i][1] = "Beaten";
                    }
                    else if (row[0].ToString() == "OverUnder4.5")
                    {
                        string[] score = result[1].Trim(' ').Split('-');
                        int total = Convert.ToInt32(score[0]) + Convert.ToInt32(score[1]);
                        if (total >= 5)
                            dt.Rows[i][1] = "Winner";
                        else dt.Rows[i][1] = "Beaten";
                    }
                    else if (row[0].ToString() == "")
                    {
                    }
                }

                // Update to database
                CrawlAllMarketsData marketdata = new CrawlAllMarketsData();
                marketdata.UpdateMarketResults(dt,Convert.ToInt32(matchid), sportid);
            }
        }
        private void TraceService(string content)
        {
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
    }
}
