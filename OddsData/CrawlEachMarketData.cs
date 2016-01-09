using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OddsData
{
    public class CrawlEachMarketData:CrawlAllMarketsData
    {
        public void InsertEachMarket(XmlDocument doc,long bettingmarketid, long matchid)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@Market_XML", xmlreader), new SqlParameter("@mode", "Insert"), new SqlParameter("@bettingmarketid", bettingmarketid), new SqlParameter("@matchid", matchid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
               // throw new Exception(ex.Message);
            }

        }

        public void InsertEachMarket(XmlDocument doc)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@Market_XML", xmlreader), new SqlParameter("@mode", "Insert2") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
              //  throw new Exception(ex.Message);
            }

        }

        public DataTable GetMarketOdds(long marketid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "MatchOdds");
                    cmd.Parameters.AddWithValue("@bettingmarketid", marketid);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
               // throw new Exception(ex.Message);
            }

        }

        public string DeleteMarketOdds(long marketid)
        {
            try
            {
                int no = 0;
                //DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "DeleteByMarketId");
                    cmd.Parameters.AddWithValue("@bettingmarketid",marketid);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();

                }

                return no.ToString();
                //return "Records deleted successfully";
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }
        }

        public DataTable GetBookieNames()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BookiesMenu", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SelectBookies");
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        public void UpdateBookiesMenu(bool Checked, string Bookiename)
        {
            try
            {
                int no = 0;
                //DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BookiesMenu", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "UpdateBookiesMenu");
                    cmd.Parameters.AddWithValue("@Checked", Checked);
                    cmd.Parameters.AddWithValue("@Bookiename", Bookiename);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();

                }

                
                //return "Records deleted successfully";
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
            }
        }

        public DataTable GetMarketOdds(long marketid, string bookies)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "MatchOddsByBookies");
                    cmd.Parameters.AddWithValue("@bookies", bookies);
                    cmd.Parameters.AddWithValue("@bettingmarketid", marketid);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    objConn.Close();
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        public DataSet GetMarket(XmlDocument doc, long marketid, long matchid, string bookies)
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@Market_XML", xmlreader), new SqlParameter("@mode", "NewInsert"), new SqlParameter("@bookies", bookies), new SqlParameter("@bettingmarketid", marketid), new SqlParameter("@matchid", matchid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Market", objConn);
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
                ErrorLog(ex.ToString());
                return null;
            }
        }

        public int GetSportID(int matchid)
        {

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select sportid from matchinfo where id='"+matchid+"'", objConn);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    string sportid = Convert.ToString(cmd.ExecuteScalar());
                    objConn.Close();
                    return sportid != "" ? Convert.ToInt32(sportid) : 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return 0 ;
            }

        }
        
    }
}
