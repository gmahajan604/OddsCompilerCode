using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OddsBusiness;
using OddsProperties;
using System.Data.SqlClient;

namespace OddsCheckerCrawler
{
    public partial class frmResultView : Form
    {
        public frmResultView(string strMatchid)
        {
            InitializeComponent();
            lblMatchID.Text = strMatchid;
            lblsportid.Text = "0";
            GetMatchInfo(Convert.ToInt32(lblMatchID.Text));
        
            GetMatchResult(Convert.ToInt32(lblMatchID.Text), Convert.ToInt32(lblsportid.Text));
        }
        private void GetMatchResult(int matchid,int spportid)
        {
            CrawlAllMarkets crawl=new CrawlAllMarkets();
            DataTable dt = new DataTable();
            dt = crawl.GetMatchResult(matchid,spportid);
            dataGridView1.DataSource = dt;
        }
        private void GetMatchInfo(int matchid)
        {
            try
            {
                using (SqlConnection objConn = new SqlConnection(OddsConnection2.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("select sportid, (Home +' V ' +Away) as Matchname, MatchDate,Time from MatchInfo where id='"+matchid+"'", objConn);
                    cmd.CommandTimeout = 500;
                    objConn.Open(); SqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblMatchNmae.Text = reader["Matchname"].ToString();
                        lblsportid.Text = reader["sportid"].ToString();
                        lblDate.Text = reader["matchdate"].ToString();
                        lblTime.Text = reader["Time"].ToString();
                    }
                    reader.Close();
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
