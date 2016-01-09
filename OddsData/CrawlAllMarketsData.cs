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
    public class CrawlAllMarketsData
    {
        public void InsertMarkets(XmlDocument doc)
        {
            try
            {
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@BettingMarket_XML", xmlreader), new SqlParameter("@mode", "Insert") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BettingMarket", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                    objConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorLog("Error In Insert Market Data: "+ex.ToString());
            }
        }
        
        public DataTable GetBettingMarkets()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BettingMarket", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SelectAll");
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

        public DataTable GetBettingMarkets(long matchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BettingMarket", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "Select");
                    cmd.Parameters.AddWithValue("@matchid", matchid);
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

        public string DeleteAllMarketsRecord()
        {
            try
            {
                //DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_BettingMarket", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "DeleteAll");
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }

                return "Records deleted successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
      
        public DataSet GetMatchesAndMarkets(int sportid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_MatchesAndMarkets", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SelectForGaelic");
                    cmd.Parameters.AddWithValue("@sportid", sportid);
                    cmd.CommandTimeout = 10000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        public DataSet GetMatchesAndMarketsForSoccer()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_MatchesAndMarkets", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SelectForSoccer");
                    cmd.Parameters.AddWithValue("@sportid", "2");
                    cmd.CommandTimeout = 10000;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        public string GetMarketName(long bettingmarketid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetMarketName", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@bettingmarketid", bettingmarketid);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    string match = Convert.ToString(cmd.ExecuteScalar());
                    objConn.Close();
                    return match;

                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// Added By Dev Nagar
        /// Get Info of Matches by League Id
        /// </summary>
        /// <param name="bettingmarketid"></param>
        /// <returns>DataTable</returns>
        public DataTable GetMatches(int leagueid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("GetMatchesByLeague", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LeagueID", leagueid);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// Get Match List by coupon (in result)
        /// </summary>
        /// <param name="couponname"></param>
        /// <returns></returns>
        public DataTable GetMatchesByCoupon()
        {
            try
            {
                DataTable ds = new DataTable();

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select * from Matchinfo where id in(select matchid from bettingmarket where id in(select distinct(bettingmarketid) from CouponMarkets ))", objConn);
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
        public string[] GetMatchinfobyid(int id)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select * from matchinfo where id='"+id+"'", objConn);
                  //  cmd.CommandType = CommandType.StoredProcedure;

                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    string[] info = new string[2];
                    if (reader.Read())
                    {
                        info[0] = reader["BettingLink"].ToString();
                        info[1] = reader["Time"].ToString();
                    }
                    reader.Close();
                    cmd.Dispose();
                    objConn.Close();
                    return info;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }
        }
        public DataSet GetMatchByDate(int sportid)
        {
            try
            {
                DataSet ds = new DataSet(); 

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Match", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "SelectMatch");
                    cmd.Parameters.AddWithValue("@SportID", sportid);
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
        public DataTable GetMatchResult(int mid,int sportid)
        {

            try
            {
                DataTable ds = new DataTable();

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetMarketResult", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MatchID", mid);
                    cmd.Parameters.AddWithValue("@SportID", sportid);
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

        public void UpdateMarketResults(DataTable dt, int matchid,int sportid)
        {
            try
            {
                DataTable ds = new DataTable();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    objConn.Open();
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("sp_UpdateMarketResult", objConn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MatchID", matchid);
                            cmd.Parameters.AddWithValue("@SportID", sportid);
                            cmd.Parameters.AddWithValue("@ResultName", row["ResultName"].ToString());
                            cmd.Parameters.AddWithValue("@Result",row["Result"].ToString());
                            cmd.CommandTimeout = 500;
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            TraceServices(dt.Rows[0].ToString() + " " + ex.ToString());
                        }
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());

            }

        }
        public DataTable GetMasterMarketName(int sportid)
        {
            try
            {
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    objConn.Open();
                    SqlCommand cmd = new SqlCommand("sp_MasterMarket", objConn);
                    cmd.Parameters.AddWithValue("@SportID",sportid);
                    cmd.Parameters.AddWithValue("@Mode", "Select");
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    objConn.Close();
                    cmd.Dispose();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }
        #region ErrorLog
        public void ErrorLog(string content)
        { //set up a filestream
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
        public void TraceServices(string content)
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
        #endregion
      
    }
}
