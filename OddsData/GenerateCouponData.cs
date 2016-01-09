using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OddsProperties;
 

namespace OddsData
{
    public class GenerateCouponData
    {
        public string InsertCoupon(XmlDocument doc, string couponname,string couponid)
        {
            try
            {
                
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@CouponInfo_XML", xmlreader), new SqlParameter("@couponname", couponname), new SqlParameter("@couponid", couponid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    int no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }
                return "Coupon "+couponname+" saved successfully";
            }
            catch (Exception ex)
            {
                return "An internal error ocurred while saving this coupon";
            }

        }

        public string AddCouponMatches(XmlDocument doc, string couponid)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@CouponInfo_XML", xmlreader), new SqlParameter("@couponid", couponid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_AddCouponMatches", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    int no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }
                return "Coupon updated successfully";
            }
            catch (Exception ex)
            {
                return "An internal error ocurred while saving this coupon";
            }

        }

        public string UpdateOdds(XmlDocument doc,string couponid)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@UpdateOdds_XML", xmlreader), new SqlParameter("@mode", "UpdateOdds"), new SqlParameter("@couponid", couponid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    int no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }
                return "Coupon updated successfully";
            }
            catch (Exception ex)
            {
                return "An internal error ocurred while saving this coupon";
            }

        }

        public string AddMArket(XmlDocument doc, string couponid)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@CouponInfo_XML", xmlreader), new SqlParameter("@coupon_id", couponid), new SqlParameter("@mode", "AddMarkets") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    int no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }
                return "Added successfully";
            }
            catch (Exception ex)
            {
                return "An internal error ocurred while saving this coupon";
            }

        }

        public DataSet GetCoupons(bool IsArchived)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IsArchived", IsArchived);
                    cmd.Parameters.AddWithValue("@mode", "SelectAll");
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet GetCoupons(string couponid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "AllInfo");
                    cmd.Parameters.AddWithValue("@coupon_id", couponid);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet GetUpdatedCouponInfo(string couponid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "UpdatedInfo");
                    cmd.Parameters.AddWithValue("@coupon_id", couponid);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public DataSet GetCouponMarket(XmlDocument doc, long marketid,long matchid, string bookies,string couponid)
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@CouponInfo_XML", xmlreader), new SqlParameter("@mode", "MarketAndToals"), new SqlParameter("@bookies", bookies), new SqlParameter("@bettingmarketid", marketid), new SqlParameter("@coupon_id", couponid), new SqlParameter("@matchid", matchid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //objConn.Open();
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet GetCouponMarket(long marketid, long matchid, string couponid)
        {
            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] arrParam = { new SqlParameter("@bettingmarketid", marketid), new SqlParameter("@coupon_id", couponid), new SqlParameter("@matchid", matchid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetCouponMarkets", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //objConn.Open();
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet GetAvailableMarkets(long matchid, string couponid)
        {
            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] arrParam = {new SqlParameter("@couponid", couponid), new SqlParameter("@matchid", matchid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAvailableMarkets", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //objConn.Open();
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet GetLatestCoupons(int sportid)
        {
            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] arrParam = { new SqlParameter("@mode", "SelectLatestCoupons"), new SqlParameter("@sportid", sportid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //objConn.Open();
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        

        public DataSet GetMatchesByCouponId(string couponid)
        {
            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] arrParam = { new SqlParameter("@mode", "SelectMatchesByCouponId"), new SqlParameter("@coupon_id", couponid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //objConn.Open();
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
      
        public bool IsCouponExist(string couponname)
        {
            try
            {
                SqlParameter[] arrParam = { new SqlParameter("@couponname", couponname), new SqlParameter("@mode", "IsCouponExist") };
                bool check = false;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Coupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    check = Convert.ToBoolean(cmd.ExecuteScalar());
                    objConn.Close();
                }
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
                SqlParameter[] arrParam = { new SqlParameter("@selectionid", selectionid), new SqlParameter("@mode", "UpdateCoupon"), new SqlParameter("@result", result) };
                int no = 0;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }

                if (no > 0)
                    return "Coupon updated successfully";
               
                else
                    return "An error occured while updating coupon";
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
                SqlParameter[] arrParam = { new SqlParameter("@couponid", couponid), new SqlParameter("@mode", "AddToArchive") };
                int no = 0;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }

                if (no > 0)
                    return "Coupon updated successfully";

                else
                    return "An error occured while adding coupon to archive";
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
                SqlParameter[] arrParam = { new SqlParameter("@selectionid", selectionid), new SqlParameter("@mode", "UpdateToals"), new SqlParameter("@toals", toal), new SqlParameter("@selection", selection) };
                int no = 0;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateCoupon", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }

                if (no > 0)
                    return "Coupon updated successfully";

                else
                    return "An error occured while updating coupon";
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
                SqlParameter[] arrParam = { new SqlParameter("@couponid", couponid)};
                bool check = false;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_IsCouponIdExist", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    check = Convert.ToBoolean(cmd.ExecuteScalar());
                    objConn.Close();
                }
                return check;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
    }
}