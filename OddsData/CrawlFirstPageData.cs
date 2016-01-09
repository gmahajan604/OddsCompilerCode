using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using OddsData;
using OddsProperties;

namespace OddsData
{
    public class CrawlFirstPageData:CrawlAllMarketsData
    {

        public void InsertMatchInfo(XmlDocument doc)
        {
            try
            {

                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@MatchInfo_XML", xmlreader), new SqlParameter("@mode", "Insert") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
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
                //throw new Exception(ex.Message);
            }

        }

        public void InsertMatchInfo(XmlDocument doc, int sportid, long leagueid)
        {
            try
            {
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@MatchInfo_XML", xmlreader), new SqlParameter("@mode", "Insert"), new SqlParameter("@sportid", sportid), new SqlParameter("@leagueid", leagueid) };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 10000;
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
            }

        }
        public void InsertMatchinfoDev(Matches match, int sportid)
        {
            try
            {
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlParameter[] arrParam = { new SqlParameter("@MatchDate", match.date), new SqlParameter("@Time", match.time), new SqlParameter("@Home", match.home)
                                          , new SqlParameter("@Draw",match.draw), new SqlParameter("@Away",match.away), new SqlParameter("@BettingLink",match.bettinglink)
                                          , new SqlParameter("@CreatedDate",match.createddate), new SqlParameter("@DisplayEndDateTime",match.Displayenddatetime),new SqlParameter("@sportid",sportid),
                                          new SqlParameter("@leagueid",Convert.ToInt32(match.league)),new SqlParameter("@ResultLink",match.resultlink)
                                          };

                    SqlCommand cmd = new SqlCommand("sp_InsertMatchinfoDev", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    cmd.ExecuteNonQuery();
                    objConn.Close();
                }
            }
            catch(Exception ex)
            {
                ErrorLog(ex.ToString());
            }
        }

        public void InsertLeagues(XmlDocument doc)
        {
            try
            {
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@League_XML", xmlreader), new SqlParameter("@mode", "Insert") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Leagues", objConn);
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
            }
        }
        /// <summary>
        ///  Store(Insert) 6 Result of Match.
        ///  Dev Nagar
        /// </summary>
        /// <param name="All Result array string"></param>
        public void InsertChampionResult(string[] result)
        {
            try
            {
                SqlParameter[] arrParam = { new SqlParameter("@MatchID",Convert.ToInt32(result[0]) ),
                                              new SqlParameter("@HTScore", result[2]),
                                              new SqlParameter("@FGScorer",result[3] ),
                                              new SqlParameter("@LGScorer",result[4] ),
                                              new SqlParameter("@TotalScore",result[1] ),
                                              new SqlParameter("@FGTime", result[5]),
                                              new SqlParameter("@LGTime", result[6]),
                                          new SqlParameter("@AllGPlayer",result[7]),
                                          new SqlParameter("@FHGPlayer",result[8]),
                                          new SqlParameter("@LHGPlayer",result[9]),
                                          new SqlParameter("@FAGPlayer",result[10]),
                                          new SqlParameter("@LAGPlayer",result[11]),
                                          new SqlParameter("@FirstBooking",result[12]),
                                          new SqlParameter("@LastBooking",result[13]),
                                          new SqlParameter("@TotalYellowCard",result[14]),
                                          new SqlParameter("@TotalRedCard",result[15]),
                                          new SqlParameter("@BookingIndexes",result[16])
                                          };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_insertMatchResult", objConn);
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
            }
        }

        public DataSet NewRecords(XmlDocument doc)
        {
            try
            {
                DataSet ds = new DataSet();
                StringReader str = new StringReader(doc.OuterXml);
                XmlTextReader xmlreader = new XmlTextReader(str);
                SqlParameter[] arrParam = { new SqlParameter("@MatchInfo_XML", xmlreader), new SqlParameter("@mode", "Sp") };

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //cmd.ExecuteNonQuery();
                    //objConn.Close();
                    return ds;

                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }

        }

        public string DeleteFirstPageRecords()
        {
            try
            {
                //DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
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
                ErrorLog(ex.ToString());
                return null;
            }
        }
        public DataTable GetMatchInfo()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
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

        public DataTable GetMatchByCoupon(bool archived)
        {
            try
            {
                DataTable ds = new DataTable();

                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Match", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(archived==false)
                    cmd.Parameters.AddWithValue("@Mode", "SelectNotArchivedMatch");
                    else
                    cmd.Parameters.AddWithValue("@Mode", "SelectArchivedMatch");
                    //  cmd.Parameters.AddWithValue("@SportID", sportid);
                    cmd.CommandTimeout = 500;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }
        }


        public DataTable GetSports()
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "GetSports");
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
        public DataTable GetSports(string sportname)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "GetSports");
                    cmd.Parameters.AddWithValue("@sportid", Convert.ToInt32(sportname));
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

        public string GetMatchName(long matchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetMatchName", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@matchid", matchid);
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

        public DataTable GetMatchInfo(long matchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "Select");
                    cmd.Parameters.AddWithValue("@id", matchid);
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

        public string UpdateMatch(long id, string matchdate, string displayenddatetime)
        {
            try
            {
                SqlParameter[] arrParam = { new SqlParameter("@id", id), new SqlParameter("@mode", "UpdateMatch"), new SqlParameter("@matchdate", matchdate), new SqlParameter("@displayenddatetime", displayenddatetime) };
                int no = 0;
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertMatchInfo", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(arrParam);
                    cmd.CommandTimeout = 500;
                    objConn.Open();
                    no = cmd.ExecuteNonQuery();
                    objConn.Close();
                }

                if (no > 0)
                    return "Match date updated successfully";

                else
                    return "An error occured while updating match date";
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }
        }

        public DataSet GetLeagues()
        {

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_Leagues", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "Select");
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

        /// <summary>
        /// Get Link and ID of MatchInfo, and Crawling.
        /// </summary>
        /// <returns></returns>
        public DataTable GetMatchResultLinkForCrawl()
        {
            try
            {
                DataTable ds = new DataTable();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select id,ResultLink,BettingLink from MatchInfo where id not in(select MatchId from Matchresult)", objConn);
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
        public DataTable GetMatchListByTime()
        {
            try
            {
                DataTable ds = new DataTable();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select id,ResultLink,BettingLink from MatchInfo where id not in(select MatchId from Matchresult) and Time<='" + DateTime.Now.AddHours(-4).AddMinutes(-30).TimeOfDay + "' and Day(DisplayEndDateTime)='" + DateTime.Now.Day + "' and Month(DisplayEndDateTime)='" + DateTime.Now.Month + "' and Year(DisplayEndDateTime)='" + DateTime.Now.Year + "'", objConn);
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

        /// <summary>
        /// select match result according to match id
        /// </summary>
        /// <parameter>MatchID</parameter>
        /// <returns>DataTable</returns>
        public DataSet GetMatchResult(int matchid)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection objConn = new SqlConnection(OddsConnection.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetMatchResult", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MatchID", matchid);
                    cmd.Parameters.AddWithValue("@Type", "First");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ds.Tables.Add(dt);

                    cmd = new SqlCommand("sp_GetMatchResult", objConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MatchID", matchid);
                    cmd.Parameters.AddWithValue("@Type", "Second");
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.ToString());
                return null;
            }
        }
     
    }
}
