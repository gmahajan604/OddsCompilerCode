using OddsBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OddsProperties;

namespace OddsCheckerCrawler
{
    public partial class CouponManage : Form
    {
        public int countthread = 0;
        private string sport;
        GenerateCoupon clsCpn;
        public CouponManage(string sportid,string sportname)
        {
            InitializeComponent();
            sport = sportid;
            lblSportName.Text = sportname;

            Shown += new EventHandler(Form1_Shown);
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            // Start the background worker
            backgroundWorker1.RunWorkerAsync();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Your background task goes here
            //if (!sport.Equals("M2"))
            //    LoadTreeViewForGaelic(sport);
            //else
            //    if (sport.Equals("M2"))
            //        LoadTreeViewForSoccer();
            bindAllCoupon(Convert.ToInt32(sport));
            LoadTreeView(Convert.ToInt32(sport));
            OddsCheckerCrawler crawl = this.MdiParent as OddsCheckerCrawler;
            if (InvokeRequired)
            {
                Action b = () =>
                {
                    crawl.SetProgress(false);
                    crawl.IsProcessRunning = false;
                }; BeginInvoke(b);
            }
        }

        private void LoadTreeViewForGaelic(string sport)
        {
            int id = Convert.ToInt32(sport);
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            DataSet ds = crawl.GetMatchesAndMarkets(id);
            ds.Relations.Add("Sport_Match", ds.Tables[0].Columns["sportid"], ds.Tables[1].Columns["sportid"]);
            ds.Relations.Add("Match_Mkt", ds.Tables[2].Columns["id"], ds.Tables[3].Columns["matchid"]);
            //ds.Relations.Add("Match_Date", ds.Tables[1].Columns["MatchDate"], ds.Tables[2].Columns["MatchDate"]);
            //TreeNode tn_group = new TreeNode("GAA Football");
            foreach (DataRow drSport in ds.Tables[0].Rows)
            {
                TreeNode tn_group = new TreeNode(drSport["SportName"].ToString());
                foreach (DataRow dr in drSport.GetChildRows("Sport_Match"))
                {
                    string matchdate = dr["MatchDate"].ToString();
                    string sportid = dr["sportid"].ToString();
                    TreeNode tn_date = new TreeNode(matchdate);
                    tn_group.Nodes.Add(tn_date);
                    string sql1 = "(matchdate='" + matchdate + "') AND (sportid='" + sportid + "')";
                    foreach (DataRow dr1 in ds.Tables[2].Select(sql1))
                    //foreach (DataRow dr1 in dr.GetChildRows("Match_Date"))
                    {
                        TreeNode tn_match = new TreeNode(dr1["Home"].ToString() + " V " + dr1["Away"].ToString());
                        tn_date.Nodes.Add(tn_match);
                        //foreach (DataRow drChild in dr1.GetChildRows("Match_Mkt"))
                        //{
                        //    TreeNode tn_market = new TreeNode(drChild["bettingmarket"].ToString());
                        //    tn_market.Name = drChild["id"].ToString() + "," + drChild["matchid"].ToString() + "," + drChild["bettinglink"].ToString() + "," + Convert.ToString(drSport["sportid"]);
                        //    tn_match.Nodes.Add(tn_market);
                        //}
                    }
                }
                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        treeView1.Nodes.Add(tn_group);
                    }; BeginInvoke(a);
                }
            }
            //treeView1.Nodes.Add(tn_group);
        }

        private void LoadTreeViewForSoccer()
        {
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            DataSet ds = crawl.GetMatchesAndMarketsFoSoccer();
            ds.Relations.Add("Sport_Match", ds.Tables[0].Columns["sportid"], ds.Tables[1].Columns["sportid"]);
            ds.Relations.Add("Date_Section", ds.Tables[1].Columns["matchdate"], ds.Tables[2].Columns["matchdate"]);
            // ds.Relations.Add("League_Match", ds.Tables[3].Columns["leagueid"], ds.Tables[4].Columns["leagueid"]);
            ds.Relations.Add("Match_Mkt", ds.Tables[4].Columns["id"], ds.Tables[5].Columns["matchid"]);

            //TreeNode tn_group = new TreeNode("GAA Football");
            foreach (DataRow drSport in ds.Tables[0].Rows)
            {
                TreeNode tn_group = new TreeNode(drSport["SportName"].ToString());

                //treeView1.Nodes.Add(tn_group);

                foreach (DataRow dr in drSport.GetChildRows("Sport_Match"))
                {
                    TreeNode tn_date = new TreeNode(dr["MatchDate"].ToString());
                    tn_group.Nodes.Add(tn_date);
                    TreeNode worldsec = new TreeNode("World");
                    tn_date.Nodes.Add(worldsec);
                    foreach (DataRow dr1 in dr.GetChildRows("Date_Section"))
                    {
                        string matchdate = Convert.ToString(dr1["matchdate"]);
                        string section = Convert.ToString(dr1["section"]);

                        TreeNode tn_section = new TreeNode(section);
                        if (section.Equals("English") || section.Equals("Scottish") || section.Equals("European") || section.Equals("Champions League") || section.Equals("Europa League"))
                            tn_date.Nodes.Add(tn_section);
                        else
                        {
                            worldsec.Nodes.Add(tn_section);
                        }
                        string sql = "(matchdate='" + matchdate + "') AND (section='" + section + "')";
                        foreach (DataRow drChild in ds.Tables[3].Select(sql))
                        {
                            string leagueid = drChild["leagueid"].ToString();
                            TreeNode tn_league = new TreeNode(drChild["league"].ToString());
                            tn_section.Nodes.Add(tn_league);
                            string sql1 = "(matchdate='" + matchdate + "') AND (leagueid='" + leagueid + "')";
                            foreach (DataRow drchildrow in ds.Tables[4].Select(sql1))
                            {

                                TreeNode tn_match = new TreeNode(drchildrow["Home"].ToString() + " V " + drchildrow["Away"].ToString());
                                tn_league.Nodes.Add(tn_match);
                                //foreach (DataRow drmarket in drchildrow.GetChildRows("Match_Mkt"))
                                //{
                                //    TreeNode tn_market = new TreeNode(drmarket["bettingmarket"].ToString());
                                //    tn_market.Name = drmarket["id"].ToString() + "," + drmarket["matchid"].ToString() + "," + drmarket["bettinglink"].ToString() + "," + Convert.ToString(drSport["sportid"]);
                                //    tn_match.Nodes.Add(tn_market);
                                //}
                            }
                        }

                    }
                }

                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        treeView1.Nodes.Add(tn_group);

                    }; BeginInvoke(a);
                }

            }
            //treeView1.Nodes.Add(tn_group);
        }
        private void LoadTreeView(int sportid)
        {
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            DataSet ds = crawl.GetMatchByDate(sportid);
            ds.Relations.Add("Date_Section1", ds.Tables[0].Columns["matchdate"], ds.Tables[1].Columns["matchdate"]);
                TreeNode tn_group = new TreeNode(lblSportName.Text);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TreeNode tn_date = new TreeNode(dr["MatchDate"].ToString());
                    string matchdate = Convert.ToString(dr["matchdate"]);

                    string sql1 = "(matchdate='" + matchdate + "')";

                    foreach (DataRow dr1 in ds.Tables[1].Select(sql1))
                    {
                        TreeNode tn_match = new TreeNode(dr1["Home"].ToString() + " V " + dr1["Away"].ToString() + ":" + dr1["id"].ToString());
                        tn_date.Nodes.Add(tn_match);
                    }
                    tn_group.Nodes.Add(tn_date);
                }
                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        treeView1.Nodes.Add(tn_group);
                    }; BeginInvoke(a);
                }
        }
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Level==2)
            {
                lblMatchName.Text = e.Node.Text;
            }
            else
            {
                lblMatchName.Text = "";
                MessageBox.Show("Please Select Match.");
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (lblMatchName.Text == "")
            {
                MessageBox.Show("Please select match first.");
            }
            else
            {
                Coupon2 cup = new Coupon2();
                cup.CouponName = txtcouponname.Text;
                string[] match = lblMatchName.Text.ToString().Split(':');
                cup.MatchID = Convert.ToInt32(match[1].ToString());
                string couponid = Guid.NewGuid().ToString().Substring(0, 5);
                cup.CouponID = couponid;
                cup.IsArchived = false;
                clsCpn = new GenerateCoupon();
                clsCpn.InsertCoupon(cup);
                MessageBox.Show("Coupon Saved.");
                RefreshCoupon(Convert.ToInt32(sport));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboCoupon.SelectedText=="")
            {
                MessageBox.Show("Please select coupon first.");
            }
            else
            {
                Coupon2 cup = new Coupon2();
                cup.CID = Convert.ToInt32(comboCoupon.SelectedValue);
                clsCpn = new GenerateCoupon();
                clsCpn.DeleteCoupon(cup);
                MessageBox.Show("Coupon Saved.");
                RefreshCoupon(Convert.ToInt32(sport));
            }
        }
        private void RefreshCoupon(int sportid)
        {
            DataTable dt = new DataTable();
            clsCpn = new GenerateCoupon();
            dt = clsCpn.SelectAllCoupon(sportid);
           
                    if (dt.Rows.Count > 0)
                    {
                        comboCoupon.DataSource = dt;
                        comboCoupon.DisplayMember = "Couponname";
                        comboCoupon.ValueMember = "cid";
                    }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lblMatchName.Text == "")
            {
                MessageBox.Show("Please select match first.");
            }
            else
            {
                Coupon2 cup = new Coupon2();
                cup.CouponName = txtcouponname.Text;
                string[] match = lblMatchName.Text.ToString().Split(':');
                cup.MatchID = Convert.ToInt32(match[1].ToString());
                string couponid = Guid.NewGuid().ToString().Substring(0, 5);
                cup.CID = Convert.ToInt32(comboCoupon.SelectedValue);
                cup.CouponID = couponid;
                cup.IsArchived = false;
                clsCpn = new GenerateCoupon();
                clsCpn.UpdateCoupon(cup);
                MessageBox.Show("Coupon Updated successfully.");
                RefreshCoupon(Convert.ToInt32(sport));
            }
        }
        private void bindAllCoupon(int sportid)
        {
            DataTable dt = new DataTable();
            clsCpn = new GenerateCoupon();
            dt=  clsCpn.SelectAllCoupon(sportid);
            if (InvokeRequired)
            {
                Action a = () =>
                {
                    if (dt.Rows.Count > 0)
                    {
                        comboCoupon.DataSource = dt;
                        comboCoupon.DisplayMember = "Couponname";
                        comboCoupon.ValueMember = "cid";
                    }
                }; BeginInvoke(a);
            }
        }
       
        public void SelectCoupn()
        {
            try
            {
                Coupon2 cid = new Coupon2();

               // ComboboxItem slelecteditem = (ComboboxItem)comboCoupon.SelectedValue;
              //  cid = clsCpn.SelectCoupon(Convert.ToInt32(comboCoupon.SelectedValue));
                txtcouponname.Text = comboCoupon.SelectedText;
            }
            catch(Exception ex)
            { }
        }

     
        private void comboCoupon_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectCoupn();
        }
    }
}