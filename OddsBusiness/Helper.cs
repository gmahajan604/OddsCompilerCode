using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using System.Data;
using OddsData;


namespace OddsBusiness
{
    public class Helper
    {

        public static string GetWebSiteContent(string url)
        {
            try
            {
                //CrawlingData cData = new CrawlingData();
                string _requestUrl = url.Trim();
                string _encodingType = "utf-8";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_requestUrl);
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 3;
                request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Arya.NET Robot)";
                request.KeepAlive = true; request.Timeout = 15 * 1000;
                System.Net.HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //showExtractConetent.InnerHtml = ((HttpWebResponse)response).StatusDescription;
                //showExtractConetent.InnerHtml += "<br/>";
                _encodingType = GetEncodingType(response);
                System.IO.StreamReader reader = new System.IO.StreamReader
                                               (response.GetResponseStream(), Encoding.GetEncoding(_encodingType));
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                return responseFromServer;
            }
            catch (WebException ex)
            {
                string response = GetWebSiteContent(url);
                return response;
            }
        }

        public static System.IO.StreamReader GetWebSiteContentToStream(string url)
        {
            System.IO.StreamReader reader;
            try
            {
                //CrawlingData cData = new CrawlingData();
                string _requestUrl = url.Trim();
                string _encodingType = "utf-8";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_requestUrl);
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 3;
                request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Arya.NET Robot)";
                request.KeepAlive = true; request.Timeout = 15 * 1000;
                System.Net.HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //showExtractConetent.InnerHtml = ((HttpWebResponse)response).StatusDescription;
                //showExtractConetent.InnerHtml += "<br/>";
                _encodingType = GetEncodingType(response);
                reader = new System.IO.StreamReader
                                               (response.GetResponseStream(), Encoding.GetEncoding(_encodingType));
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                return reader;
            }
            catch (WebException ex)
            {
                reader = GetWebSiteContentToStream(url);
                return reader;
            }
        }
        public static string GetWebSiteContent(string url,string type)
        {
            try
            {
                //CrawlingData cData = new CrawlingData();
                string _requestUrl = url.Trim();
                string _encodingType = "utf-8";
                FileWebRequest request = (FileWebRequest)WebRequest.Create(_requestUrl);
                //request..AllowAutoRedirect = true;
                //request.MaximumAutomaticRedirections = 3;
                //request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Arya.NET Robot)";
                //request. = true; request.Timeout = 15 * 1000;
                System.Net.FileWebResponse response = (FileWebResponse)request.GetResponse();
                //showExtractConetent.InnerHtml = ((HttpWebResponse)response).StatusDescription;
                //showExtractConetent.InnerHtml += "<br/>";
                //_encodingType = GetEncodingType(response);
                System.IO.StreamReader reader = new System.IO.StreamReader
                                               (response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                return responseFromServer;
            }
            catch (WebException ex)
            {
                string response = GetWebSiteContent(url);
                return response;
            }
        }

        private static string GetEncodingType(HttpWebResponse response)
        {
            string pageCharset = string.Empty;
            if (response.ContentType != string.Empty)
            {
                pageCharset = response.ContentEncoding;
            }

            return !string.IsNullOrEmpty(pageCharset) ? pageCharset : "utf-8";
        }

        public static HtmlDocument LoadHtml(string htmlcontent)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionFixNestedTags = true;
        
            // by dev----
            doc.OptionOutputAsXml = true;
            doc.OptionAutoCloseOnEnd = true;

            doc.LoadHtml(htmlcontent);
            return doc;
        }

        public static double FractionToDouble(string fraction)
        {
            double result;

            if (double.TryParse(fraction, out result))
            {
                return result;
            }

            string[] split = fraction.Split(new char[] { ' ', '/' });

            if (split.Length == 2 || split.Length == 3)
            {
                int a, b;

                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
                {
                    if (split.Length == 2)
                    {
                        return (double)a / b;
                    }

                    int c;

                    if (int.TryParse(split[2], out c))
                    {
                        return a + (double)b / c;
                    }
                }
            }
            return 0;
            //throw new FormatException("Not a valid fraction.");
        }


        public static string CouponName(string couponid)
        {
            GenerateCoupon coupon = new GenerateCoupon();
            return Convert.ToString(coupon.GetCouponInfo(couponid).Rows[0]["couponname"]);
        }

        public static void GenerateXMLforOdds(string file, DataSet ds,string sportname, string sportid)
        {

            XmlDocument doc = new XmlDocument();

            //DataSet ds = coupon.GetCoupons();
            ds.Relations.Add("Coupon_Match", ds.Tables[0].Columns["couponid"], ds.Tables[1].Columns["couponid"]);
            ds.Relations.Add("Match_Market", ds.Tables[1].Columns["matchid"], ds.Tables[2].Columns["matchid"]);
            ds.Relations.Add("Market_Sel", ds.Tables[2].Columns["bettingmarketid"], ds.Tables[3].Columns["bettingmarketid"]);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string couponname = Convert.ToString(dr["couponname"]);
                string createdon = Convert.ToString(dr["createdon"]);
                string couponid = Convert.ToString(dr["couponid"]);
                XmlNode ArkleCouponNode = doc.CreateElement("ArkleCoupon");
                XmlAttribute ArkleSportIdAttribute = doc.CreateAttribute("SportsId");
                ArkleSportIdAttribute.Value = sportid;
                ArkleCouponNode.Attributes.Append(ArkleSportIdAttribute);
                XmlAttribute ArkleSportAttribute = doc.CreateAttribute("_Sport");
                ArkleSportAttribute.Value = sportname;
                ArkleCouponNode.Attributes.Append(ArkleSportAttribute);
                XmlAttribute ArkleCouponIdentifier = doc.CreateAttribute("CouponIdentifier");
                ArkleCouponIdentifier.Value = couponname;
                ArkleCouponNode.Attributes.Append(ArkleCouponIdentifier);
                XmlAttribute ArkleCouponName = doc.CreateAttribute("CouponName");
                ArkleCouponName.Value = couponname;
                ArkleCouponNode.Attributes.Append(ArkleCouponName);
                XmlAttribute ArkleTimeCreated = doc.CreateAttribute("TimeCreated");
                ArkleTimeCreated.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleTimeCreated);
                XmlAttribute ArkleLastUpdated = doc.CreateAttribute("LastUpdated");
                ArkleLastUpdated.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleLastUpdated);
                XmlAttribute ArkleLastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
                ArkleLastPriceChangeIssued.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleLastPriceChangeIssued);

                XmlAttribute ArkleMarketBettingTypeID = doc.CreateAttribute("MarketBettingTypeID");
                ArkleMarketBettingTypeID.Value = "Normal";
                ArkleCouponNode.Attributes.Append(ArkleMarketBettingTypeID);

                XmlAttribute ArkleDeSerializeStatusID = doc.CreateAttribute("DeSerializeStatusID");
                ArkleDeSerializeStatusID.Value = "SerializationInProgress";
                ArkleCouponNode.Attributes.Append(ArkleDeSerializeStatusID);

                XmlAttribute ArkleXsd = doc.CreateAttribute("xmlns", "xsd", "http://www.w3.org/2000/xmlns/");
                ArkleXsd.Value = "http://www.w3.org/2001/XMLSchema";
                ArkleCouponNode.Attributes.Append(ArkleXsd);

                XmlAttribute ArkleXsi = doc.CreateAttribute("xmlns", "xsi", "http://www.w3.org/2000/xmlns/");
                ArkleXsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
                ArkleCouponNode.Attributes.Append(ArkleXsi);

                doc.AppendChild(ArkleCouponNode);

                XmlNode comments = doc.CreateElement("Comments");
                comments.AppendChild(doc.CreateTextNode("Produced By M Halaburda"));
                ArkleCouponNode.AppendChild(comments);

                XmlNode filename = doc.CreateElement("FileName");
                filename.AppendChild(doc.CreateTextNode(file));
                ArkleCouponNode.AppendChild(filename);


                XmlNode EventsCollection = doc.CreateElement("EventsCollection");
                ArkleCouponNode.AppendChild(EventsCollection);

                foreach (DataRow dr1 in dr.GetChildRows("Coupon_Match"))
                {
                    string identifier = "MH."+sportname+"AEid" + couponid + "." + dr1["Home"].ToString() + dr1["Away"].ToString();
                    string matchcreatedon = dr1["createdon"].ToString();
                    string matchname = dr1["Name"].ToString();
                    string[] matchdate = dr1["MatchDate"].ToString().Split(' ');
                    string time = dr1["Time"].ToString().Substring(0, 8);
                    string enddatetime = Convert.ToString(dr1["EndDateTime"]);
                    string offdate = Convert.ToString(dr1["OffDate"]);
                    //string date = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd") + "T" + time;
                    XmlNode ArkleEvent = doc.CreateElement("ArkleEvent");

                    XmlAttribute EventSportsId = doc.CreateAttribute("SportsId");
                    EventSportsId.Value = sportid;
                    ArkleEvent.Attributes.Append(EventSportsId);

                    XmlAttribute EventSport = doc.CreateAttribute("_Sport");
                    EventSport.Value = sportname;
                    ArkleEvent.Attributes.Append(EventSport);

                    XmlAttribute EventTimeCreated = doc.CreateAttribute("TimeCreated");
                    EventTimeCreated.Value = matchcreatedon;
                    ArkleEvent.Attributes.Append(EventTimeCreated);

                    XmlAttribute QuadpotDividend = doc.CreateAttribute("QuadpotDividend");
                    QuadpotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(QuadpotDividend);

                    XmlAttribute PlacepotDividend = doc.CreateAttribute("PlacepotDividend");
                    PlacepotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(PlacepotDividend);

                    XmlAttribute JackpotDividend = doc.CreateAttribute("JackpotDividend");
                    JackpotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(JackpotDividend);

                    XmlAttribute EventExpiryDate = doc.CreateAttribute("ExpiryDate");
                    EventExpiryDate.Value = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd") + "T" + "23:59:59";
                    ArkleEvent.Attributes.Append(EventExpiryDate);

                    XmlAttribute DisplayEndDateTime = doc.CreateAttribute("DisplayEndDateTime");
                    DisplayEndDateTime.Value = enddatetime;
                    ArkleEvent.Attributes.Append(DisplayEndDateTime);

                    XmlAttribute OrderPriority = doc.CreateAttribute("OrderPriority");
                    OrderPriority.Value = "0";
                    ArkleEvent.Attributes.Append(OrderPriority);

                    XmlAttribute Marketclass = doc.CreateAttribute("Marketclass");
                    Marketclass.Value = "";
                    ArkleEvent.Attributes.Append(OrderPriority);

                    XmlAttribute EventIdentifier = doc.CreateAttribute("EventIdentifier");
                    EventIdentifier.Value = identifier;
                    ArkleEvent.Attributes.Append(EventIdentifier);

                    XmlAttribute Name = doc.CreateAttribute("Name");
                    Name.Value = matchname;
                    ArkleEvent.Attributes.Append(Name);

                    XmlAttribute EventSettleStatusType = doc.CreateAttribute("EventSettleStatusType");
                    EventSettleStatusType.Value = "Unknown";
                    ArkleEvent.Attributes.Append(EventSettleStatusType);

                    XmlAttribute SourceURL = doc.CreateAttribute("SourceURL");
                    SourceURL.Value = "";
                    ArkleEvent.Attributes.Append(SourceURL);

                    EventsCollection.AppendChild(ArkleEvent);

                    XmlNode handicap = doc.CreateElement("HandicapMatchBettingMarkets");
                    ArkleEvent.AppendChild(handicap);

                    XmlNode MatchForPrintOut = doc.CreateElement("MatchBettingMarketsForPrintout");
                    ArkleEvent.AppendChild(MatchForPrintOut);

                    XmlNode LastUpdated = doc.CreateElement("LastUpdated");
                    LastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                    ArkleEvent.AppendChild(LastUpdated);

                    XmlNode MarketsCollection = doc.CreateElement("MarketsCollection");
                    ArkleEvent.AppendChild(MarketsCollection);


                    //tn_coupon.Nodes.Add(tn_date);
                    foreach (DataRow drChild in dr1.GetChildRows("Match_Market"))
                    {
                        //TreeNode tn_match = new TreeNode(drChild["Home"].ToString() + " V " + drChild["Away"].ToString());

                        string market = drChild["bettingmarket"].ToString();

                        XmlNode ArkleMarket = doc.CreateElement("ArkleMarket");

                        XmlAttribute MarketName = doc.CreateAttribute("Name");
                        MarketName.Value = market;
                        ArkleMarket.Attributes.Append(MarketName);

                        XmlAttribute ContainsAmendedResult = doc.CreateAttribute("ContainsAmendedResult");
                        ContainsAmendedResult.Value = "false";
                        ArkleMarket.Attributes.Append(ContainsAmendedResult);

                        XmlAttribute MarkSenseMarketPriority = doc.CreateAttribute("MarkSenseMarketPriority");
                        MarkSenseMarketPriority.Value = "0";
                        ArkleMarket.Attributes.Append(MarkSenseMarketPriority);

                        XmlAttribute WordDisplayPriority = doc.CreateAttribute("_WordDisplayPriority");
                        WordDisplayPriority.Value = "0";
                        ArkleMarket.Attributes.Append(WordDisplayPriority);

                        XmlAttribute ScreenDisplayPriority = doc.CreateAttribute("_ScreenDisplayPriority");
                        ScreenDisplayPriority.Value = "1";
                        ArkleMarket.Attributes.Append(ScreenDisplayPriority);

                        XmlAttribute EachWayNumberPlaces = doc.CreateAttribute("EachWayNumberPlaces");
                        EachWayNumberPlaces.Value = "1";
                        ArkleMarket.Attributes.Append(EachWayNumberPlaces);

                        XmlAttribute EachWayDenominator = doc.CreateAttribute("EachWayDenominator");
                        EachWayDenominator.Value = "1";
                        ArkleMarket.Attributes.Append(EachWayDenominator);

                        XmlAttribute Archived = doc.CreateAttribute("Archived");
                        Archived.Value = "false";
                        ArkleMarket.Attributes.Append(Archived);

                        XmlAttribute EachWayAllowed = doc.CreateAttribute("EachWayAllowed");
                        EachWayAllowed.Value = "false";
                        ArkleMarket.Attributes.Append(EachWayAllowed);

                        XmlAttribute RaceCardNumber = doc.CreateAttribute("RaceCardNumber");
                        RaceCardNumber.Value = "0";
                        ArkleMarket.Attributes.Append(RaceCardNumber);

                        XmlAttribute ExpectedOffDateExpired = doc.CreateAttribute("ExpectedOffDateExpired");
                        ExpectedOffDateExpired.Value = "false";
                        ArkleMarket.Attributes.Append(ExpectedOffDateExpired);

                        XmlAttribute ExpectedOffDateDisplay = doc.CreateAttribute("ExpectedOffDateDisplay");
                        ExpectedOffDateDisplay.Value = offdate;
                        ArkleMarket.Attributes.Append(ExpectedOffDateDisplay);

                        XmlAttribute ExpectedOffDate = doc.CreateAttribute("ExpectedOffDate");
                        ExpectedOffDate.Value = enddatetime;
                        ArkleMarket.Attributes.Append(ExpectedOffDate);

                        XmlAttribute ResultReceivedDate = doc.CreateAttribute("ResultReceivedDate");
                        ResultReceivedDate.Value = "0001-01-01T00:00:00";
                        ArkleMarket.Attributes.Append(ResultReceivedDate);

                        XmlAttribute MarketDisplayClass = doc.CreateAttribute("MarketDisplayClass");
                        MarketDisplayClass.Value = market;
                        ArkleMarket.Attributes.Append(MarketDisplayClass);

                        XmlAttribute URL = doc.CreateAttribute("URL");
                        URL.Value = "";
                        ArkleMarket.Attributes.Append(URL);

                        XmlAttribute NumbersGameTypeID = doc.CreateAttribute("NumbersGameTypeID");
                        NumbersGameTypeID.Value = "0";
                        ArkleMarket.Attributes.Append(NumbersGameTypeID);

                        XmlAttribute FavouritePriceDecimal = doc.CreateAttribute("FavouritePriceDecimal");
                        FavouritePriceDecimal.Value = "0";
                        ArkleMarket.Attributes.Append(FavouritePriceDecimal);

                        XmlAttribute SectionNumber = doc.CreateAttribute("SectionNumber");
                        SectionNumber.Value = "0";
                        ArkleMarket.Attributes.Append(SectionNumber);

                        XmlAttribute MarketIdentifier = doc.CreateAttribute("MarketIdentifier");
                        MarketIdentifier.Value = identifier + market.Substring(0, 1) + "id";
                        ArkleMarket.Attributes.Append(MarketIdentifier);

                        XmlAttribute MaxiumLiability = doc.CreateAttribute("MaxiumLiability");
                        MaxiumLiability.Value = "0";
                        ArkleMarket.Attributes.Append(MaxiumLiability);

                        MarketsCollection.AppendChild(ArkleMarket);


                        XmlNode ewRulesList = doc.CreateElement("ewRulesList");
                        ArkleMarket.AppendChild(ewRulesList);

                        XmlNode Rule4sList = doc.CreateElement("Rule4sList");
                        ArkleMarket.AppendChild(Rule4sList);

                        XmlNode ForecastList = doc.CreateElement("ForecastList");
                        ArkleMarket.AppendChild(ForecastList);

                        XmlNode MarketStatusCollection = doc.CreateElement("MarketStatusCollection");
                        ArkleMarket.AppendChild(MarketStatusCollection);

                        XmlNode SelectionsCollections = doc.CreateElement("SelectionsCollections");
                        ArkleMarket.AppendChild(SelectionsCollections);

                        int i = 1;
                        foreach (DataRow drprice in drChild.GetChildRows("Market_Sel"))
                        {
                            string pricecreated = drprice["createddate"].ToString();
                            string pricefixed = drprice["toals"].ToString();
                            string selection = drprice["selection"].ToString();
                            double price = Helper.FractionToDouble(pricefixed);
                            string pricedec = Convert.ToString(Math.Round(price, 2) + 1);
                            string sel_identifier = drprice["identifier"].ToString();
                            XmlNode ArkleSelection = doc.CreateElement("ArkleSelection");


                            XmlAttribute TimeCreated = doc.CreateAttribute("TimeCreated");
                            TimeCreated.Value = pricecreated;
                            ArkleSelection.Attributes.Append(TimeCreated);

                            XmlAttribute LastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
                            LastPriceChangeIssued.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(LastPriceChangeIssued);

                            XmlAttribute SelectionName = doc.CreateAttribute("Name");
                            SelectionName.Value = selection;
                            ArkleSelection.Attributes.Append(SelectionName);

                            XmlAttribute NumberInDeadHeat = doc.CreateAttribute("NumberInDeadHeat");
                            NumberInDeadHeat.Value = "0";
                            ArkleSelection.Attributes.Append(NumberInDeadHeat);

                            XmlAttribute CoFavNumber = doc.CreateAttribute("CoFavNumber");
                            CoFavNumber.Value = "0";
                            ArkleSelection.Attributes.Append(CoFavNumber);

                            XmlAttribute DisplayPlayerInScoreCastGridExcel = doc.CreateAttribute("_DisplayPlayerInScoreCastGridExcel");
                            DisplayPlayerInScoreCastGridExcel.Value = "false";
                            ArkleSelection.Attributes.Append(DisplayPlayerInScoreCastGridExcel);

                            XmlAttribute ExistedPreviously = doc.CreateAttribute("_ExistedPreviously");
                            ExistedPreviously.Value = "false";
                            ArkleSelection.Attributes.Append(ExistedPreviously);

                            XmlAttribute TimeSetToNonRunner = doc.CreateAttribute("TimeSetToNonRunner");
                            TimeSetToNonRunner.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(TimeSetToNonRunner);

                            XmlAttribute LastPriceCheck = doc.CreateAttribute("LastPriceCheck");
                            LastPriceCheck.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(LastPriceCheck);

                            XmlAttribute IncludeInExcel = doc.CreateAttribute("_IncludeInExcel");
                            IncludeInExcel.Value = "true";
                            ArkleSelection.Attributes.Append(IncludeInExcel);

                            XmlAttribute TeamName = doc.CreateAttribute("TeamName");
                            TeamName.Value = "";
                            ArkleSelection.Attributes.Append(TeamName);

                            XmlAttribute hda = doc.CreateAttribute("_hda");
                            hda.Value = "";
                            ArkleSelection.Attributes.Append(hda);

                            XmlAttribute PriceFixed = doc.CreateAttribute("PriceFixed");
                            PriceFixed.Value = pricefixed;
                            ArkleSelection.Attributes.Append(PriceFixed);

                            XmlAttribute DefaultPriceType = doc.CreateAttribute("DefaultPriceType");
                            DefaultPriceType.Value = "F";
                            ArkleSelection.Attributes.Append(DefaultPriceType);

                            XmlAttribute SelectionNumber = doc.CreateAttribute("SelectionNumber");
                            SelectionNumber.Value = "0";
                            ArkleSelection.Attributes.Append(SelectionNumber);

                            XmlAttribute GuaranteeOdds = doc.CreateAttribute("GuaranteeOdds");
                            GuaranteeOdds.Value = "0";
                            ArkleSelection.Attributes.Append(GuaranteeOdds);

                            XmlAttribute SelectionIdentifier = doc.CreateAttribute("SelectionIdentifier");
                            SelectionIdentifier.Value = identifier + sel_identifier;
                            ArkleSelection.Attributes.Append(SelectionIdentifier);

                            XmlAttribute SelectionPriceBackgroundColor = doc.CreateAttribute("SelectionPriceBackgroundColor");
                            SelectionPriceBackgroundColor.Value = "blueBackground";
                            ArkleSelection.Attributes.Append(SelectionPriceBackgroundColor);

                            XmlAttribute SettleStatusID = doc.CreateAttribute("SettleStatusID");
                            SettleStatusID.Value = "0";
                            ArkleSelection.Attributes.Append(SettleStatusID);

                            XmlAttribute SettleStatus = doc.CreateAttribute("_SettleStatus");
                            SettleStatus.Value = "NotSettled";
                            ArkleSelection.Attributes.Append(SettleStatus);

                            XmlAttribute FinishingPosition = doc.CreateAttribute("FinishingPosition");
                            FinishingPosition.Value = "0";
                            ArkleSelection.Attributes.Append(FinishingPosition);

                            XmlAttribute _FinishingPosition = doc.CreateAttribute("_FinishingPosition");
                            _FinishingPosition.Value = "NotSettled";
                            ArkleSelection.Attributes.Append(_FinishingPosition);

                            SelectionsCollections.AppendChild(ArkleSelection);

                            XmlNode SelectionPriceCollection = doc.CreateElement("SelectionPriceCollection");
                            ArkleSelection.AppendChild(SelectionPriceCollection);

                            ////////

                            XmlNode ArkleSelectionPrice = doc.CreateElement("ArkleSelectionPrice");

                            XmlAttribute PriceDec = doc.CreateAttribute("PriceDec");
                            PriceDec.Value = pricedec;
                            ArkleSelectionPrice.Attributes.Append(PriceDec);

                            XmlAttribute PriceFrac = doc.CreateAttribute("PriceFrac");
                            PriceFrac.Value = pricefixed;
                            ArkleSelectionPrice.Attributes.Append(PriceFrac);

                            XmlAttribute MarketType = doc.CreateAttribute("MarketType");
                            MarketType.Value = "F";
                            ArkleSelectionPrice.Attributes.Append(MarketType);

                            XmlAttribute TimePriceChangeCreated = doc.CreateAttribute("TimePriceChangeCreated");
                            TimePriceChangeCreated.Value = pricecreated;
                            ArkleSelectionPrice.Attributes.Append(TimePriceChangeCreated);

                            XmlAttribute TimePriceIssued = doc.CreateAttribute("TimePriceIssued");
                            TimePriceIssued.Value = pricecreated;
                            ArkleSelectionPrice.Attributes.Append(TimePriceIssued);

                            XmlAttribute SourceType = doc.CreateAttribute("SourceType");
                            SourceType.Value = "0";
                            ArkleSelectionPrice.Attributes.Append(SourceType);

                            XmlAttribute _SourceType = doc.CreateAttribute("_SourceType");
                            _SourceType.Value = "Unknown";
                            ArkleSelectionPrice.Attributes.Append(_SourceType);

                            SelectionPriceCollection.AppendChild(ArkleSelectionPrice);

                            XmlNode BaseTime = doc.CreateElement("BaseTime");
                            BaseTime.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                            SelectionPriceCollection.AppendChild(BaseTime);

                            i++;
                            //<RaceHeaderRotationIndex>0</RaceHeaderRotationIndex> <MarketRequiresResttle>false</MarketRequiresResttle> <DateLastUpdated>0001-01-01T00:00:00</DateLastUpdated>
                        }

                        XmlNode RaceHeaderRotationIndex = doc.CreateElement("RaceHeaderRotationIndex");
                        RaceHeaderRotationIndex.AppendChild(doc.CreateTextNode("0"));
                        SelectionsCollections.AppendChild(RaceHeaderRotationIndex);

                        XmlNode MarketRequiresResttle = doc.CreateElement("MarketRequiresResttle");
                        MarketRequiresResttle.AppendChild(doc.CreateTextNode("false"));
                        SelectionsCollections.AppendChild(MarketRequiresResttle);

                        XmlNode DateLastUpdated = doc.CreateElement("DateLastUpdated");
                        DateLastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                        SelectionsCollections.AppendChild(DateLastUpdated);


                    }
                }

            }
            doc.Save(file);

        }

        public static string GetMarketName(long marketid)
        {
            CrawlAllMarketsData crawldata = new CrawlAllMarketsData();
            return crawldata.GetMarketName(marketid);
        }

        public static string GetMatchName(long matchid)
        {
            CrawlFirstPageData crawl = new CrawlFirstPageData();
            return crawl.GetMatchName(matchid);
        }

    }
}
