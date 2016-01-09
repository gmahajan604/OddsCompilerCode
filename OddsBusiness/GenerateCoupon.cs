using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddsProperties;
using OddsData;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OddsBusiness
{
    public class GenerateCoupon
    {

        public string InsertCoupon(List<Coupon> couponlist, string couponname,string couponid)
        {
            try
            {
                string msg;
                XmlDocument doc = GenerateXml(couponlist);
                GenerateCouponData coupondata = new GenerateCouponData();
                msg = coupondata.InsertCoupon(doc, couponname,couponid);
                return msg;
            }
            catch (Exception ex)
            {
                return "An internal error occured while saving this coupon";
            }
        }

        public string AddCouponMatches(List<Coupon> couponlist, string couponid)
        {
            try
            {
                string msg;
                XmlDocument doc = GenerateXml(couponlist);
                GenerateCouponData coupondata = new GenerateCouponData();
                msg = coupondata.AddCouponMatches(doc, couponid);
                return msg;
            }
            catch (Exception ex)
            {
                return "An internal error occured while saving this coupon";
            }
        }

        public string AddMarket(List<Coupon> couponlist, string couponid)
        {
            try
            {
                string msg;
                XmlDocument doc = GenerateXml(couponlist);
                GenerateCouponData coupondata = new GenerateCouponData();
                msg = coupondata.AddMArket(doc, couponid);
                return msg;
            }
            catch (Exception ex)
            {
                return "An internal error occured while saving this coupon";
            }
        }

        public string UpdateOdds(List<Market> updatedOdds,string couponid)
        {
            try
            {
                string msg;
                XmlDocument doc = GenerateXmlForUpdatedOdds(updatedOdds);
                GenerateCouponData coupondata = new GenerateCouponData();
                msg = coupondata.UpdateOdds(doc,couponid);
                return msg;
            }
            catch (Exception ex)
            {
                return "An internal error occured while saving this coupon";
            }
        }

        private XmlDocument GenerateXml(List<Coupon> couponlist)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Coupons");

                foreach (Coupon coupon in couponlist)
                {
                    writer.WriteStartElement("Coupon");

                    //writer.WriteElementString("CouponId", Convert.ToString(coupon.Couponid));
                    writer.WriteElementString("BettingMarketId", Convert.ToString(coupon.Bettingmarketid));
                    writer.WriteElementString("Selection", coupon.Selection);
                    writer.WriteElementString("Toals", coupon.Toals);
                    writer.WriteElementString("Identifier", coupon.Identifier);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();


            }

            return doc;

        }

        private XmlDocument GenerateXmlForUpdatedOdds(List<Market> updateOdds)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("MktSelections");

                foreach (Market selection in updateOdds)
                {
                    writer.WriteStartElement("MktSelection");
                    //writer.WriteElementString("CouponId", Convert.ToString(coupon.Couponid));
                    writer.WriteElementString("Id", Convert.ToString(selection.id));
                    writer.WriteElementString("Selection", selection.beton);
                    writer.WriteElementString("Toals", selection.bestbet);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            return doc;

        }

        public DataSet GetCoupons(bool IsArchived)
        {
            GenerateCouponData coupon = new GenerateCouponData();
            return coupon.GetCoupons(IsArchived);
        }

        public DataSet GetCoupons(string couponid)
        {
            GenerateCouponData coupon = new GenerateCouponData();
            return coupon.GetCoupons(couponid);
        }

        public DataSet GetUpdatedCouponInfo(string couponid)
        {
            GenerateCouponData coupon = new GenerateCouponData();
            return coupon.GetUpdatedCouponInfo(couponid);
        }

        public DataTable GetCouponInfo(string couponid)
        {
            GenerateCouponData coupon = new GenerateCouponData();
            return coupon.GetCoupons(couponid).Tables[0];
        }

        public DataSet GetCouponMarket(string bettinglink, long id, long matchid,string bookies,string couponid)
        {
            CrawlEachMarket crawl = new CrawlEachMarket();
            GenerateCouponData coupondata = new GenerateCouponData();
            XmlDocument xmldoc = new XmlDocument();
            List<Market> marketlist = new List<Market>();
            marketlist = crawl.GetMarketList(bettinglink, id, matchid);
            xmldoc = crawl.GenerateXml2(marketlist);
            xmldoc = crawl.GenerateXml2(marketlist);
            return coupondata.GetCouponMarket(xmldoc, id,matchid, bookies,couponid);
            //return links;
        }

        public DataSet GetCouponMarket(long marketid, long matchid, string couponid)
        {
            GenerateCouponData coupondata = new GenerateCouponData();
            return coupondata.GetCouponMarket(marketid, matchid, couponid);
        }


        public DataSet GetAvailableMarkets(long matchid, string couponid)
        {
            GenerateCouponData coupondata = new GenerateCouponData();
            return coupondata.GetAvailableMarkets(matchid, couponid);
        }

        public DataSet GetLatestCoupons(int sportid)
        {
            GenerateCouponData coupondata = new GenerateCouponData();
            return coupondata.GetLatestCoupons(sportid);
        }

        public DataSet GetMatchesByCouponId(string couponid)
        {
            GenerateCouponData coupondata = new GenerateCouponData();
            return coupondata.GetMatchesByCouponId(couponid);
        }

      
        public bool IsCouponExist(string couponname)
        {
            try
            {
                bool check=false;
                GenerateCouponData coupondata = new GenerateCouponData();
                check = coupondata.IsCouponExist(couponname);
                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string UpdateCoupon(long selectionid,string result)
        {
            try
            {
                GenerateCouponData coupondata = new GenerateCouponData();
                string msg = coupondata.UpdateCoupon(selectionid, result);
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating coupon");
            }
        }

        public string AddToArchive(string couponid)
        {
            try
            {
                GenerateCouponData coupondata = new GenerateCouponData();
                string msg = coupondata.AddToArchive(couponid);
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating coupon");
            }
        }

        public string UpdateCoupon(long selectionid, string toal,string selection)
        {
            try
            {
                GenerateCouponData coupondata = new GenerateCouponData();
                string msg = coupondata.UpdateCoupon(selectionid,toal,selection);
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating coupon");
            }
        }
        public bool IsCouponIdExist(string couponid)
        {
            try
            {
                bool check = false;
                GenerateCouponData coupondata = new GenerateCouponData();
                check = coupondata.IsCouponIdExist(couponid);
                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// Manage Coupon
        /// 
        public void InsertCoupon(Coupon2 coupon)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CouponManage", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Couponname", coupon.CouponName);
                cmd.Parameters.AddWithValue("@Mode", "Insert");
                cmd.Parameters.AddWithValue("@CouponID", coupon.CouponID);
                cmd.Parameters.AddWithValue("@IsArchived", coupon.IsArchived);
                cmd.Parameters.AddWithValue("@matchid", coupon.MatchID);
                objConn.Open();
                cmd.ExecuteNonQuery();
                objConn.Close();
            }
        }
        public void UpdateCoupon(Coupon2 coupon)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CouponManage", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Couponname", coupon.CouponName);
                cmd.Parameters.AddWithValue("@Mode", "Update");
                cmd.Parameters.AddWithValue("@CID", coupon.CID);
                cmd.Parameters.AddWithValue("@IsArchived", coupon.IsArchived);
                cmd.Parameters.AddWithValue("@matchid", coupon.MatchID);
                objConn.Open();
                cmd.ExecuteNonQuery();
                objConn.Close();
            }
        }
        public void DeleteCoupon(Coupon2 coupon)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CouponManage", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@couponid", coupon.CouponID);
                cmd.Parameters.AddWithValue("@Mode", "Delete");
                objConn.Open();
                cmd.ExecuteNonQuery();
                objConn.Close();
            }
        }
        public DataTable SelectAllCoupon(int sportid)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CouponManage", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "SelectAll");
                cmd.Parameters.AddWithValue("@SportiD", sportid);
                objConn.Open();
                cmd.ExecuteNonQuery();
                objConn.Close();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable at = new DataTable();
                ad.Fill(at);
                return at;
            }
        }

        public Coupon2 SelectCoupon(int cid)
        {
            using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CouponManage", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "SelectCoupon");
                cmd.Parameters.AddWithValue("@CID", cid);
                objConn.Open();
                SqlDataReader raader;
                raader = cmd.ExecuteReader();
                objConn.Close();
                Coupon2 cup = new Coupon2();
                if (raader.Read())
                {
                    cup.CouponName = raader["CouponName"].ToString();
                    cup.CouponID = raader["CouponID"].ToString();
                    cup.CID = Convert.ToInt32(raader["cid"]);
                    cup.IsArchived = Convert.ToBoolean(raader["IsArchived"].ToString());
                    cup.MatchID = Convert.ToInt32(raader["matchid"].ToString());
                }
                return cup;
            }
        }
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() { return Text; }
    }
}