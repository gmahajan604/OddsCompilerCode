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
using OddsData;

namespace OddsBusiness
{
   public class CrawlGolf
    {
       public int SportID = 29;
       public string CrawlAllGolf()
       {
           CrawlFirstPageData data = new OddsData.CrawlFirstPageData();

           CrawlGolfBettingMarketName();

           DataTable dt = data.GetSports(SportID.ToString());
           CrawlGolfTurnament(dt.Rows[0]["Link"].ToString(),SportID.ToString());

           CrawlGolfBettingMarket();
           return "Command Completed Successfully.";
       }
       public void CrawlGolfBettingMarketName()
       {
           string url = "";
           string htmlcontent = Helper.GetWebSiteContent(url);
           HtmlAgilityPack.HtmlDocument doc = Helper.LoadHtml(htmlcontent);
           XmlDocument xmldoc = new XmlDocument();
           CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
           var row11 = doc.DocumentNode.SelectSingleNode("//ul[@class='more-outrights 1']");
           var rows = row11.SelectNodes("./a");
           if (rows != null)
           {
               List<MarketMaster> lit = new List<MarketMaster>();
               for (int i = 0; i < rows.Count; i++)
               {
                   MarketMaster market = new MarketMaster();
                   string li = rows[i].InnerText;
                   var linkrow = rows[i].SelectNodes("./span");
                   market.MarketMarketName = linkrow[0].InnerText;
                   market.MarketResultLink = "http://www.oddschecker.com" + rows[0].Attributes["href"].Value;
                   market.MarketSportID = 29;
                   InsertMasterMarket(market);
                   lit.Add(market);
               }
               xmldoc = GenerateXmlGolfMarketMaster(lit);
           }
       }
     
     public string CrawlGolfTurnament(string url, string sportid)
       {
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

                   xmldoc = GenerateXmlGolfTurnament(turnamentlist);
                 //  InsertGoldTurnament(xmldoc, sportid);
                   TraceService("Data Inserted: Golf:0  ,SportID:" + sportid + " , URL:" + url + "\n");
               }
               //ds = crawldata.NewRecords(xmldoc);
               //return ds;
               return "Command completed successfully";
           }
           catch (Exception ex)
           {
               TraceService("Error:0 ,SportID:" + sportid + " , URL:" + url + "\n");
               return ex.Message;
           }
       }

       #region DatabaseMethod
         
       public void CrawlGolfBettingMarket()
       {

       }

       public void InsertGolfBettingMarket()
       {

       }
       public void InsertGoldTurnament(GolfTurnament golf)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_GolfSport", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Insert");
                    cmd.Parameters.AddWithValue("@Duration", golf.Duration.Replace("&nbsp;",""));
                    cmd.Parameters.AddWithValue("@Turnament", golf.Turnament);
                    cmd.Parameters.AddWithValue("@Course", golf.Course.Replace("&nbsp;", ""));
                    cmd.Parameters.AddWithValue("@Champion", golf.Champion);
                    cmd.Parameters.AddWithValue("@Link", golf.Link);
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }
                catch (Exception ex)
                {
                    objConn.Close();
                }
            }

        }
       public DataTable GetGoldMarkets(string golfid,string turnament,string url)
       {
           return null;
       }
        public DataTable GetGoldMarkets(string golfid)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                try
                {
                    if (golfid != "")
                    {
                        DataTable dt = new DataTable();
                        SqlCommand cmd = new SqlCommand("sp_GolfSport", objConn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GolfMarket");
                        cmd.Parameters.AddWithValue("@GolfID", Convert.ToInt32(golfid));
                        objConn.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        ad.Fill(dt);
                        objConn.Close();
                        return dt;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    objConn.Close();
                    return null;
                }
            }
        }

       public DataTable GetGolfTurnament()
       {
           using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
           {
               try
               {
                   DataTable dt = new DataTable();
                   SqlCommand cmd = new SqlCommand("sp_GolfSport", objConn);
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.AddWithValue("@Mode", "GolfTurnament");
                   objConn.Open();
                   cmd.ExecuteNonQuery();
                   SqlDataAdapter ad = new SqlDataAdapter(cmd);
                   ad.Fill(dt);
                   objConn.Close();
                   return dt;
               }
               catch (Exception ex)
               {
                   objConn.Close();
                   return null;
               }
           }

       }
             /// <summary>
        /// dev
        /// </summary>
        /// <param name="marketid"></param>
        /// <returns></returns>
        /// 
        public void InsertMasterMarket(MarketMaster market)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                objConn.Open();
                SqlCommand cmd = new SqlCommand("sp_MasterMarket", objConn);
                cmd.Parameters.AddWithValue("@SportID", market.MarketSportID);
                cmd.Parameters.AddWithValue("@ResultName", market.MarketMarketName);
                cmd.Parameters.AddWithValue("@ResultLink", market.MarketResultLink);
                cmd.Parameters.AddWithValue("@Mode", "Insert");
                cmd.ExecuteNonQuery();
                objConn.Close();
                cmd.Dispose();
            }
        }
       #endregion
       private XmlDocument GenerateXmlGolfMarketMaster(List<MarketMaster> market)
       {
           XmlDocument doc = new XmlDocument();
           using (XmlWriter writer = doc.CreateNavigator().AppendChild())
           {
               writer.WriteStartDocument();
               writer.WriteStartElement("MarketName");
               foreach (MarketMaster match in market)
               {
                   writer.WriteStartElement("Match");
                   writer.WriteElementString("Name", match.MarketMarketName);
                   writer.WriteElementString("SportID", match.MarketSportID.ToString());
                   writer.WriteElementString("Link", match.MarketResultLink);
                   writer.WriteEndElement();
               }
               writer.WriteEndElement();
               writer.WriteEndDocument();
               writer.Flush();
           }
           return doc;
       }
       private XmlDocument GenerateXmlGolfTurnament(List<GolfTurnament> market)
       {
           XmlDocument doc = new XmlDocument();
           using (XmlWriter writer = doc.CreateNavigator().AppendChild())
           {
               writer.WriteStartDocument();
               writer.WriteStartElement("GolfTurnament");
               foreach (GolfTurnament match in market)
               {
                   writer.WriteStartElement("Turnament");
                   writer.WriteElementString("Duration", match.Duration);
                   writer.WriteElementString("Name", match.Turnament.ToString());
                   writer.WriteElementString("Course", match.Course);
                   writer.WriteElementString("Champion", match.Champion);
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
