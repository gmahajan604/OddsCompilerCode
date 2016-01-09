using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddsData;
using OddsProperties;
using System.Xml;
using HtmlAgilityPack;
using System.Reflection;

namespace OddsBusiness
{
    public class CrawlEachMarket
    {
        public void CrawlMarketLinks()
        {
            CrawlAllMarketsData crawl = new CrawlAllMarketsData();
            CrawlEachMarketData crawldata = new CrawlEachMarketData();
            DataTable dt = crawl.GetBettingMarkets();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                XmlDocument xmldoc = new XmlDocument();
                string link = dt.Rows[i]["bettinglink"].ToString();
                long bettingmarketid = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                long matchid = Convert.ToInt32(dt.Rows[i]["matchid"].ToString());
                List<Market> market = CrawlMarkets(link); //, id, matchid);
                xmldoc = GenerateXml(market);
                crawldata.InsertEachMarket(xmldoc,bettingmarketid,matchid);
            }
        }

        public void CrawlMarketLinks(long matchid)
        {
            CrawlAllMarketsData crawl = new CrawlAllMarketsData();
            DataTable dt = crawl.GetBettingMarkets(matchid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string link = dt.Rows[i]["bettinglink"].ToString();
                CrawlMarkets(link);
            }
        }

        

        public List<Market> CrawlMarkets(string bettinglink)  //, long id, long matchid)
        {
            //oddsTableContainer
            string html = Helper.GetWebSiteContent(bettinglink);
            HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
            List<string> bookielist = new List<string>();
            List<Market> marketlist = new List<Market>();
            XmlDocument xmldoc = new XmlDocument();
            CrawlEachMarketData crawldata = new CrawlEachMarketData();
            //string links = "";

            var rows = doc.DocumentNode.SelectSingleNode("//div[@id='oddsTableContainer']");
            var row1 = rows.SelectNodes("//table//tr[@class='eventTableHeader']//td");
            if (row1 != null)
            {
                for (int i = 3; i < row1.Count; i++)
                {
                    //string li = rows[i].InnerText;
                    var linkrows = row1[i].SelectNodes("./a");
                    if (linkrows != null)
                    {
                        string link = linkrows[0].Attributes["title"].Value;
                        bookielist.Add(link);
                        //links += link;
                    }
                    else
                    {
                        string val = row1[i].InnerText;
                        bookielist.Add(val);
                    }
                }
            }


            var row2 = rows.SelectNodes("//table//tbody[@id='t1']//tr//td");
            if (row2 != null)
            {
                for (int i = 3; i < row2.Count; i++)
                {
                    string best = "";
                    string beton = "";
                    string bookiename = "";
                    string bet = "";
                    if (i < bookielist.Count + 3)
                    {
                        best = row2[0].InnerText;
                        beton = row2[1].InnerText;
                        bookiename = bookielist[i - 3].ToString();
                        bet = row2[i].InnerText;
                        marketlist.Add(new Market() { bestbet = best.Trim(), beton = beton, bookiename = bookiename, bet = bet.Trim() }); //, bettingmarketid = id, matchid = matchid });
                        //links += best + " " + beton + " " + bookiename + " " + bet;
                    }

                    if (i >= bookielist.Count + 3 && i < (2 * bookielist.Count)+3)
                    {
                        best = row2[bookielist.Count + 3].InnerText;
                        beton = row2[bookielist.Count + 4].InnerText;
                        bookiename = bookielist[i - (bookielist.Count + 3)].ToString();
                        bet = row2[i+3].InnerText;
                        marketlist.Add(new Market() { bestbet = best.Trim(), beton = beton, bookiename = bookiename, bet = bet.Trim() }); //, bettingmarketid = id, matchid = matchid });
                        //links += best + " " + beton + " " + bookiename + " " + bet;
                    }

                    if (row2.Count > (3 * bookielist.Count))
                    {
                        if (i >= (2 * bookielist.Count) + 3 && i < (3 * bookielist.Count) + 3)
                        {
                            best = row2[(2 * bookielist.Count) + 6].InnerText;
                            beton = row2[(2 * bookielist.Count) + 7].InnerText;
                            bookiename = bookielist[i - ((2 * bookielist.Count) + 3)].ToString();
                            bet = row2[i + 6].InnerText;
                            marketlist.Add(new Market() { bestbet = best.Trim(), beton = beton, bookiename = bookiename, bet = bet.Trim()}); //, bettingmarketid = id, matchid = matchid });
                           // links += best + " " + beton + " " + bookiename + " " + bet;
                        }
                    }
                    //var betonrows = row2[i].SelectNodes("./a");
                    //if (betonrows != null)
                    //{
                    //    string beton = betonrows[1].InnerText;
                    //    links += beton;
                    //}
                    //else
                    //{
                    //    //var betrows = row2[i].SelectNodes("//td");
                    //            string bet = row2[i].InnerText;
                    //            links += bet.ToString();
                    //    }
                    //        //}
                    //    }
                    //}
                }
            }
            return marketlist;
            //xmldoc = GenerateXml(marketlist);
            //crawldata.InsertEachMarket(xmldoc);
            //return links;
        }

        public List<Market> GetMarketList(string bettinglink, long id, long matchid)
        {
            List<Market> marketlist = new List<Market>();
            try
            {
                string html = Helper.GetWebSiteContent(bettinglink);
                HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(html);
                List<string> bookielist = new List<string>();
                
                XmlDocument xmldoc = new XmlDocument();
                CrawlEachMarketData crawldata = new CrawlEachMarketData();
                //string links = "";

                var rows = doc.DocumentNode.SelectSingleNode("//div[@id='oddsTableContainer']");
                var row1 = rows.SelectNodes("//table//tr[@class='eventTableHeader']//td");
                if (row1 != null)
                {
                    for (int i = 3; i < row1.Count; i++)
                    {
                        //string li = rows[i].InnerText;
                        var linkrows = row1[i].SelectNodes("./a");
                        if (linkrows != null)
                        {

                            try
                            {
                                string link = linkrows[0].Attributes["title"].Value;
                                bookielist.Add(link);
                            }
                            catch (NullReferenceException ex)
                            {
                            }
                            //links += link;
                        }
                        else
                        {
                            string val = row1[i].InnerText;
                            bookielist.Add(val);
                        }

                    }
                }


                var row2 = rows.SelectNodes("//table//tbody[@id='t1']//tr");
                if (row2 != null)
                {
                    for (int i = 0; i < row2.Count; i++)
                    {
                        string best = "";
                        string beton = "";
                        string bookiename = "";
                        string bet = "";
                        var oddrows = row2[i].SelectNodes("./td");
                        if (oddrows != null)
                            for (int j = 3; j < oddrows.Count; j++)
                            {
                                try
                                {
                                    //best = oddrows[0].InnerText;
                                    beton = oddrows[1].InnerText;
                                    bookiename = bookielist[j - 3];
                                    bet = oddrows[j].InnerHtml.Replace("<br>", " ").Replace("<br />", " ");
                                    marketlist.Add(new Market() { beton = beton, bookiename = bookiename, bet = bet.Trim(), bettingmarketid = id, matchid = matchid });
                                    //links += link;
                                }
                                catch (System.ArgumentOutOfRangeException ex)
                                {
                                }
                            }



                    }


                }
               return marketlist;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
                
        }
        public void CrawlMarkets(string bettinglink, long id, long matchid)
        {
            try
            {
                CrawlEachMarketData crawldata = new CrawlEachMarketData();
                XmlDocument xmldoc = new XmlDocument();
                List<Market> marketlist = new List<Market>();
                marketlist = GetMarketList(bettinglink, id, matchid);
                //return marketlist;
                xmldoc = GenerateXml2(marketlist);
                //crawldata.GetMarket(xmldoc,id,matchid,
                crawldata.InsertEachMarket(xmldoc);
                //return links;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private XmlDocument GenerateXml(List<Market> marketlist)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Bookies");

                foreach (Market market in marketlist)
                {
                    writer.WriteStartElement("Bookie");

                    writer.WriteElementString("BookieName", market.bookiename);
                    writer.WriteElementString("BestBet", market.bestbet.ToString());
                    writer.WriteElementString("BetOn", market.beton);
                    writer.WriteElementString("Bet", market.bet.ToString());
                   // writer.WriteElementString("BettingMarketId", market.bettingmarketid.ToString());
                   // writer.WriteElementString("MatchId", market.matchid.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


            }

            return doc;

        }

        public DataTable GetMarketOdds(long marketid)
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            return crawl.GetMarketOdds(marketid);
        }

        public string DeleteMarketOdds(long marketid)
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            return crawl.DeleteMarketOdds(marketid);
        }

        public XmlDocument GenerateXml2(List<Market> marketlist)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Bookies");

                foreach (Market market in marketlist)
                {
                    writer.WriteStartElement("Bookie");

                    writer.WriteElementString("BookieName", market.bookiename);
                    //writer.WriteElementString("BestBet", market.bestbet.ToString());
                    writer.WriteElementString("BetOn", market.beton);
                    writer.WriteElementString("Bet", market.bet.ToString());
                    writer.WriteElementString("BettingMarketId", market.bettingmarketid.ToString());
                    writer.WriteElementString("MatchId", market.matchid.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


            }

            return doc;

        }

        public DataTable GetBookieNames()
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            return crawl.GetBookieNames();
        }

        public void UpdateBookiesMenu(bool Checked,string Bookiename)
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            crawl.UpdateBookiesMenu(Checked, Bookiename);
        }

        public DataTable GetMarketOdds(long marketid,string bookies)
        {
            CrawlEachMarketData crawl = new CrawlEachMarketData();
            return crawl.GetMarketOdds(marketid,bookies);
        }

        public DataSet CrawlMarkets(string bettinglink, long id, long matchid,string bookies)
        {
            DataSet ds = new DataSet();
            try
            {
                CrawlEachMarketData crawldata = new CrawlEachMarketData();
                XmlDocument xmldoc = new XmlDocument();
                List<Market> marketlist = new List<Market>();
           
               marketlist = GetMarketList(bettinglink, id, matchid);
               xmldoc = GenerateXml2(marketlist);
               ds = crawldata.GetMarket(xmldoc, id, matchid, bookies);

              // DataTable dt = new DataTable();
              //  dt = ToDataTable(marketlist);
              //   dt = PivotTable.GetInversedDataTable(dt, dt.Columns[3].ColumnName,dt.Columns[2].ColumnName,dt.Columns[4].ColumnName.ToString(),"",false);
              
                //DataColumn c=new DataColumn();
                //c.ColumnName="Odds";
                //c.DataType=System.Type.GetType("System.String");
                //dt.Columns.Add(c);
                //c.SetOrdinal(0);
              //  ds.Tables.Add(dt);
                return ds; 
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

public static DataTable ToDataTable<T>(List<T> items)
{
        DataTable dataTable = new DataTable(typeof(T).Name);

        //Get all the properties
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name);
        }
        foreach (T item in items)
        {
           var values = new object[Props.Length];
           for (int i = 0; i < Props.Length; i++)
           {
                //inserting property values to datatable rows
                values[i] = Props[i].GetValue(item, null);
           }
           dataTable.Rows.Add(values);
      }
      //put a breakpoint here and check datatable
      return dataTable;
}


      
    }
}

