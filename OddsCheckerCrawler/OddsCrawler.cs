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
using CodersLab.Windows.Controls;
using OddsProperties;
using System.Threading;

namespace OddsCheckerCrawler
{
    public partial class  OddsCrawler : Form
    {
        public int countthread = 0;
        private string sport;
        public OddsCrawler(string sportid)
        {
            InitializeComponent();
            sport = sportid;
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
            if(!sport.Equals("M2"))
                LoadTreeViewForGaelic(sport);
            else
                if(sport.Equals("M2"))
                    LoadTreeViewForSoccer();
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

       

        private void LoadComboBox()
        {
            GenerateCoupon coupon = new GenerateCoupon();
            int sportid = Convert.ToInt32(sport.Substring(1));
            comboCoupon.DataSource = coupon.GetLatestCoupons(sportid).Tables[0];
            
            comboCoupon.ValueMember = "couponid";
            comboCoupon.DisplayMember = "couponname";
            comboCoupon.SelectedIndex = -1;
            //comboCoupon.Refresh();
        }
        private void LoadTreeViewForGaelic(string sport)
        {
            int id = Convert.ToInt32(sport.Substring(1));
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
                        foreach (DataRow drChild in dr1.GetChildRows("Match_Mkt"))
                        {
                            TreeNode tn_market = new TreeNode(drChild["bettingmarket"].ToString());
                            tn_market.Name = drChild["id"].ToString() + "," + drChild["matchid"].ToString() + "," + drChild["bettinglink"].ToString() + "," + Convert.ToString(drSport["sportid"]);
                            tn_match.Nodes.Add(tn_market);
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
                                foreach (DataRow drmarket in drchildrow.GetChildRows("Match_Mkt"))
                                {
                                    TreeNode tn_market = new TreeNode(drmarket["bettingmarket"].ToString());
                                    tn_market.Name = drmarket["id"].ToString() + "," + drmarket["matchid"].ToString() + "," + drmarket["bettinglink"].ToString() + "," + Convert.ToString(drSport["sportid"]);

                                    tn_match.Nodes.Add(tn_market);
                                }
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

        #region LoadTreeByOneTAble
        //private void LoadTreeViewForGaelic(string sport)
        //{
        //    int id = Convert.ToInt32(sport.Substring(1));
        //    CrawlAllMarkets crawl = new CrawlAllMarkets();
        //    DataSet ds = crawl.GetMatchesAndMarkets(id);
        //    DataTable dtsport = ds.Tables[0].DefaultView.ToTable(true, "SportName");
        //    DataTable dtmatchdate = ds.Tables[0].DefaultView.ToTable(true, "MatchDate");

        //    foreach (DataRow drSport in dtsport.Rows)
        //    {
        //        TreeNode tn_group = new TreeNode(drSport["SportName"].ToString());
        //        string sportname = drSport["SportName"].ToString();
        //        foreach (DataRow dr in ds.Tables[0].DefaultView.ToTable(true, "sportid","SportName", "matchdate").Select("SportName='" + sportname + "'", "matchdate"))
        //        {
        //            string matchdate = dr["MatchDate"].ToString();
        //            string sportid = dr["sportid"].ToString();
        //            TreeNode tn_date = new TreeNode(matchdate);
        //            tn_group.Nodes.Add(tn_date);
        //            string sql1 = "(matchdate='" + matchdate + "') AND (sportid='" + sportid + "')";
        //            foreach (DataRow dr1 in ds.Tables[0].DefaultView.ToTable(true, "Home","Away","sportid", "SportName", "matchdate").Select(sql1))
        //            //foreach (DataRow dr1 in dr.GetChildRows("Match_Date"))
        //            {
        //                TreeNode tn_match = new TreeNode(dr1["Home"].ToString() + " V " + dr1["Away"].ToString());
        //                tn_date.Nodes.Add(tn_match);
        //                string sql2 = "(matchdate='" + matchdate + "') AND (Sportid='"+sportid+"')  AND (Home='" + dr1["Home"].ToString() + "') AND (Away='" + dr1["Away"].ToString() + "')";

        //                foreach (DataRow drChild in ds.Tables[0].DefaultView.ToTable(true, "id", "MarketBettingLink", "bettingmarket", "matchid", "Home", "Away","sportid", "matchdate").Select(sql2))
        //                {
        //                    TreeNode tn_market = new TreeNode(drChild["bettingmarket"].ToString());
        //                    tn_market.Name = drChild["id"].ToString() + "," + drChild["matchid"].ToString() + "," + drChild["MarketBettingLink"].ToString() + "," + Convert.ToString(sportid);
        //                    tn_match.Nodes.Add(tn_market);
        //                }
        //            }
        //        }
        //        if (InvokeRequired)
        //        {
        //            Action a = () =>
        //            {
        //                treeView1.Nodes.Add(tn_group);
        //            }; BeginInvoke(a);
        //        }
        //    }
        //    //treeView1.Nodes.Add(tn_group);
        //}
        //private void LoadTreeViewForSoccer()
        //{
        //    CrawlAllMarkets crawl = new CrawlAllMarkets();
        //    DataSet ds = crawl.GetMatchesAndMarketsFoSoccer();
        //  //  ds.Relations.Add("Sport_Match", ds.Tables[0].Columns["sportid"], ds.Tables[1].Columns["sportid"]);
        // //   ds.Relations.Add("Date_Section", ds.Tables[1].Columns["matchdate"], ds.Tables[2].Columns["matchdate"]);
        // // ds.Relations.Add("League_Match", ds.Tables[3].Columns["leagueid"], ds.Tables[4].Columns["leagueid"]);
        // //   ds.Relations.Add("Match_Mkt", ds.Tables[4].Columns["id"], ds.Tables[5].Columns["matchid"]);
        //    DataTable dtsport = ds.Tables[0].DefaultView.ToTable(true, "SportName");
        //    DataTable dtmatchdate = ds.Tables[0].DefaultView.ToTable(true, "MatchDate");

        //    DataTable dtt = ds.Tables[0].DefaultView.ToTable(true, "section", "matchdate");
        //    //TreeNode tn_group = new TreeNode("GAA Football");
        //    foreach (DataRow drSport in dtsport.Rows)
        //    {
        //        TreeNode tn_group = new TreeNode(drSport["SportName"].ToString());

        //        //treeView1.Nodes.Add(tn_group);

        //        foreach (DataRow dr in dtmatchdate.Rows)
        //        {
        //            TreeNode tn_date = new TreeNode(dr["MatchDate"].ToString());
        //            tn_group.Nodes.Add(tn_date);
        //            TreeNode worldsec = new TreeNode("World");
        //            tn_date.Nodes.Add(worldsec);
        //            string matchdate2=dr["MatchDate"].ToString();
        //            foreach (DataRow dr1 in ds.Tables[0].DefaultView.ToTable(true, "section","matchdate").Select("matchdate='" + matchdate2 + "'","matchdate"))
        //            {
        //                string matchdate = Convert.ToString(dr["matchdate"]);
        //                string section = Convert.ToString(dr1["section"]);

        //                TreeNode tn_section = new TreeNode(section);
        //                if (section.Equals("English") || section.Equals("Scottish") || section.Equals("European") || section.Equals("Champions League") || section.Equals("Europa League"))
        //                    tn_date.Nodes.Add(tn_section);
        //                else
        //                {
        //                    worldsec.Nodes.Add(tn_section);
        //                }
        //                string sql = "(matchdate='" + matchdate + "') AND (section='" + section + "')";
        //                foreach (DataRow drChild in ds.Tables[0].DefaultView.ToTable(true, "leagueid","league","matchdate","section").Select(sql))
        //                {
        //                    string leagueid = drChild["leagueid"].ToString();
        //                    TreeNode tn_league = new TreeNode(drChild["league"].ToString());
        //                    tn_section.Nodes.Add(tn_league);
        //                    string sql1 = "(matchdate='" + matchdate + "') AND (leagueid='" + leagueid + "')";
        //                    foreach (DataRow drchildrow in ds.Tables[0].DefaultView.ToTable(true, "Home", "Away","matchdate","leagueid").Select(sql1))
        //                    {

        //                        TreeNode tn_match = new TreeNode(drchildrow["Home"].ToString() + " V " + drchildrow["Away"].ToString());
        //                        tn_league.Nodes.Add(tn_match);
        //                        string sql2 = "(matchdate='" + matchdate + "') AND (leagueid='" + leagueid + "') AND (Home='" + drchildrow["Home"].ToString() + "') AND (Away='" + drchildrow["Away"].ToString() + "')";

        //                        foreach (DataRow drmarket in ds.Tables[0].DefaultView.ToTable(true, "bettingmarket", "matchid", "id", "MarketBettingLink", "Home", "Away", "matchdate", "leagueid").Select(sql2))
        //                        {
        //                            TreeNode tn_market = new TreeNode(drmarket["bettingmarket"].ToString());
        //                            tn_market.Name = drmarket["id"].ToString() + "," + drmarket["matchid"].ToString() + "," + drmarket["MarketBettingLink"].ToString() + "," + Convert.ToString("2");
        //                            tn_match.Nodes.Add(tn_market);
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //        if (InvokeRequired)
        //        {
        //            Action a = () =>
        //            {
        //                treeView1.Nodes.Add(tn_group);
        //            }; BeginInvoke(a);
        //        }
        //    }
        //    //treeView1.Nodes.Add(tn_group);
        //}
        #endregion

        public void ClearFlowLayoutPanel()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        public void setheight()
        {
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl.GetType().Equals(typeof(DataGridView)))
                {
                     dgvHeight(ctrl as DataGridView);
                }
            }
        }
        private int dgvHeight(DataGridView grid)
        {
            //var totalHeight = grid.Rows.GetRowsHeight(DataGridViewElementStates.None);
            //return totalHeight;
            int sum = 0;
            sum += grid.ColumnHeadersHeight;

            foreach (DataGridViewRow row in grid.Rows)
                sum += row.Height + 1; // I dont think the height property includes the cell border size, so + 1

            return sum;
            //grid.Height = sum;
        }

        
        public void FillData(string url, long id, long matchid,string bookies,int count)
        {
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
            
            try
            {
                CrawlEachMarket crawl = new CrawlEachMarket();
                //string msg = crawl.DeleteMarketOdds(id);
                //crawl.CrawlMarkets(url, id, matchid);

                DataGridView dataGridView1 = new DataGridView();
                DataTable dt = crawl.CrawlMarkets(url, id, matchid, bookies).Tables[0];
                DataRow newrow = dt.NewRow();
                dt.Rows.InsertAt(newrow, dt.Rows.Count);
                dataGridView1.DataSource = dt; 
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                //dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.RowHeadersVisible = false;
                //dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
                //dataGridView1.CellClick += dataGridView1_CellClick;
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                //dataGridView1.Width = flowLayoutPanel1.Width - 30;
                dataGridView1.Name = Convert.ToString(id) + "," + Convert.ToString(matchid);
               // dataGridView1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;

                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        //if (!matchid.Equals(match_id))
                        if (!flowLayoutPanel1.Controls.ContainsKey(matchid.ToString()))
                        {
                            CrawlFirstPage crawlpage = new CrawlFirstPage();
                            DataTable dt1 = crawlpage.GetMatchInfo(matchid);
                            GroupBox grpBox = new GroupBox();
                            string match = Convert.ToString(dt1.Rows[0]["Name"])+" ("+Convert.ToString(dt1.Rows[0]["MatchDate"])+")";
                            grpBox.Text = match;
                            grpBox.Name = matchid.ToString();
                            grpBox.AutoSize = true;
                            FlowLayoutPanel flowpanel = new FlowLayoutPanel();
                            flowpanel.FlowDirection = FlowDirection.TopDown;
                            flowpanel.Dock = DockStyle.Fill;
                            flowpanel.WrapContents = false;
                            flowpanel.AutoSize = true;
                            flowpanel.Name = matchid.ToString()+matchid.ToString();
                            grpBox.Controls.Add(flowpanel);
                            flowLayoutPanel1.Controls.Add(grpBox);
                            grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                            
                            //grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                        }

                        FlowLayoutPanel panel = flowLayoutPanel1.Controls.Find(matchid.ToString() + matchid.ToString(), true).FirstOrDefault() as FlowLayoutPanel;
                        panel.Controls.Add(dataGridView1);
                        countthread++;
                        if (countthread.Equals(count))
                        {
                            crawlcheck.SetProgress(false);
                            crawlcheck.IsProcessRunning = false;
                        }
                    };
                    BeginInvoke(a);
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Action b = () =>
                    {
                        crawlcheck.SetProgress(false);
                        crawlcheck.IsProcessRunning = false;
                    }; BeginInvoke(b);
                   
                }

            }
           
        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            int sum = 0;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            }
            grid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //grid.AllowUserToResizeColumns = true;
            
            sum = dgvHeight(grid);
            grid.ClientSize = new Size(flowLayoutPanel1.Width - 30, sum); //dataGridView1.Height = sum;
        }

        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (e.RowIndex < grid.Rows.Count - 1 && e.ColumnIndex==0)
            {
                double sum = 0;
                for (int i = 0; i < grid.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
                    {
                        sum += 100 / ((Helper.FractionToDouble(grid.Rows[i].Cells[0].Value.ToString())) + 1);
                    }
                }
                sum = Math.Round(sum, 2);
                grid.Rows[grid.Rows.Count - 1].Cells[0].Value = sum.ToString() + "%";
            }
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (e.RowIndex == grid.Rows.Count - 1 )
            {
                double sum = 0;
                for (int i = 0; i < grid.Rows.Count-1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
                    {
                        sum += 100 / ((Helper.FractionToDouble(grid.Rows[i].Cells[0].Value.ToString())) + 1);
                    }
                }
                sum = Math.Round(sum, 2);
                grid.Rows[grid.Rows.Count-1].Cells[0].Value = sum.ToString() + "%";
            }
        }

        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (e.ColumnIndex > 1)
            {
                double sum = 0;
                if (e.RowIndex != -1)
                {
                    string odd = Convert.ToString(grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (odd.Contains(" "))
                    {
                        string handi = odd.Substring(0, odd.IndexOf(" "));
                        string price = odd.Substring(odd.IndexOf(" "));
                        string selection = Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value);
                        grid.Rows[e.RowIndex].Cells[0].Value = price.Trim();
                        if (selection.Contains("("))
                        {
                            grid.Rows[e.RowIndex].Cells[1].Value = selection.Substring(0, selection.IndexOf("(")).Trim() + " (" + handi + ")";
                        }
                        else
                        {
                            grid.Rows[e.RowIndex].Cells[1].Value += " (" + handi + ")";
                        }
                    }
                    else
                    {
                        grid.Rows[e.RowIndex].Cells[0].Value = odd;
                        if (Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value).Contains("("))
                        {
                            grid.Rows[e.RowIndex].Cells[1].Value = Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value).Substring(0, Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value).IndexOf("(")).Trim();
                        }
                        //grid.Rows[e.RowIndex].Cells[1].Value =  Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value).Substring(0, Convert.ToString(grid.Rows[e.RowIndex].Cells[1].Value).IndexOf("("));
                    }
                }
                else
                {
                    for (int j = 0; j < grid.Rows.Count - 1; j++)
                    {
                        string odd = Convert.ToString(grid.Rows[j].Cells[e.ColumnIndex].Value);
                        if (odd.Contains(" "))
                        {
                            string handi = odd.Substring(0, odd.IndexOf(" "));
                            string price = odd.Substring(odd.IndexOf(" "));
                            string selection = Convert.ToString(grid.Rows[j].Cells[1].Value);
                            grid.Rows[j].Cells[0].Value = price.Trim();
                            
                            if (selection.Contains("("))
                            {
                                grid.Rows[j].Cells[1].Value = selection.Substring(0, selection.IndexOf("(")).Trim() + " (" + handi + ")";
                            }
                            else
                            {
                                grid.Rows[j].Cells[1].Value += " (" + handi + ")";
                               
                            }
                        }
                        else
                        {
                            grid.Rows[j].Cells[0].Value = odd;
                            if(Convert.ToString(grid.Rows[j].Cells[1].Value).Contains("("))
                            {
                                grid.Rows[j].Cells[1].Value = Convert.ToString(grid.Rows[j].Cells[1].Value).Substring(0, Convert.ToString(grid.Rows[j].Cells[1].Value).IndexOf("(")).Trim();
                            }
                            
                        }
                       
                    }

                }

                for (int i = 0; i < grid.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
                    {
                        sum += 100 / ((Helper.FractionToDouble(grid.Rows[i].Cells[0].Value.ToString())) + 1);
                    }
                }
                sum = Math.Round(sum, 2);
                grid.Rows[grid.Rows.Count - 1].Cells[0].Value = sum.ToString() + "%";
            }
        }


        public void SetLabelSportIdText(string text)
        {
            lblsportid.Text = text;
        }
        private void treeView1_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode parentnode = e.Node;
                while (parentnode.Parent != null)
                {
                    parentnode = parentnode.Parent;
                }
                
                if ((parentnode.Text.Equals("Soccer") && e.Node.Level >= 5) || (!parentnode.Text.Equals("Soccer") && e.Node.Level == 3))
                //if (e.Node.Level == 3 || e.Node.Level == 5)
                {
                    OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
                    if (!parent.IsProcessRunning)
                    {
                    //if (!string.IsNullOrEmpty(parent.SelectedBookies()))
                    //{
                            //parent.SetProgress(true);
                            //parent.IsProcessRunning = true;
                            string info = e.Node.Name;
                            string[] info1 = info.Split(',');
                            if (info1.Length > 1)
                            {
                                countthread = 0;
                                parent.IsProcessRunning = true;
                                parent.SetProgress(true);
                                flowLayoutPanel1.Controls.Clear();
                                lblsportid.Text = info1[3];
                                Task taskA = Task.Factory.StartNew(() =>
                                {
                                    FillData(info1[2], Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]), parent.SelectedBookies(), 1);
                                }, TaskCreationOptions.LongRunning);
                            }
                        //}
                //else
                //{
                //    MessageBox.Show("Please select atleast one bookie from Select Bookies menu");
                //}
                    }
                    else
                    {
                        MessageBox.Show("A process is already running");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An internal error occured while processing the request");
            }
        }

        public List<string> SelectedMarkets()
        {
            List<string> marketlist = new List<string>();
            if (treeView1.SelectedNodes.Count > 0)
            {
                // need to fetch mynode before loop
                foreach (TreeNode node in treeView1.SelectedNodes)
                {
                    TreeNode mynode = new TreeNode();
                    mynode = node;
                    while (mynode.Parent != null)
                    {
                        mynode = mynode.Parent;
                    }
                    if (!mynode.Text.Equals("Soccer"))
                    {
                        if (node.Level == 0)
                        {
                            foreach (TreeNode datenode in node.Nodes)
                            {
                                foreach (TreeNode matchnode in datenode.Nodes)
                                {
                                    foreach (TreeNode marketnode in matchnode.Nodes)
                                    {
                                        marketlist.Add(marketnode.Name);
                                    }
                                }

                            }
                        }

                        else if (node.Level == 1)
                        {
                            foreach (TreeNode matchnode in node.Nodes)
                            {
                                foreach (TreeNode marketnode in matchnode.Nodes)
                                {
                                    marketlist.Add(marketnode.Name);
                                }
                            }
                        }

                        else if (node.Level == 2)
                        {
                            foreach (TreeNode marketnode in node.Nodes)
                            {
                                marketlist.Add(marketnode.Name);
                            }
                        }

                        else if (node.Level == 3)
                        {
                            marketlist.Add(node.Name);
                        }
                    }

                    else
                    {
                        if (node.Level == 0)
                        {
                            foreach (TreeNode datenode in node.Nodes)
                            {
                                foreach (TreeNode sectionnode in datenode.Nodes)
                                {
                                    foreach (TreeNode leagues in sectionnode.Nodes)
                                    {
                                        foreach (TreeNode matches in leagues.Nodes)
                                        {
                                            foreach (TreeNode market in matches.Nodes)
                                            {
                                                if (market.Nodes.Count > 0)
                                                {
                                                    foreach (TreeNode market1 in market.Nodes)
                                                    {
                                                        marketlist.Add(market1.Name);
                                                    }
                                                }

                                                else
                                                    marketlist.Add(market.Name);
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        else if (node.Level == 1)
                        {
                            foreach (TreeNode sectionnode in node.Nodes)
                            {
                                foreach (TreeNode leagues in sectionnode.Nodes)
                                {
                                    foreach (TreeNode matches in leagues.Nodes)
                                    {
                                        foreach (TreeNode market in matches.Nodes)
                                        {
                                            if (market.Nodes.Count > 0)
                                            {
                                                foreach (TreeNode market1 in market.Nodes)
                                                {
                                                    marketlist.Add(market1.Name);
                                                }
                                            }

                                            else
                                                marketlist.Add(market.Name);
                                        }
                                    }
                                }
                            }
                        }

                        else if (node.Level == 2)
                        {
                            foreach (TreeNode leagues in node.Nodes)
                            {
                                foreach (TreeNode matches in leagues.Nodes)
                                {
                                    foreach (TreeNode market in matches.Nodes)
                                    {
                                        if (market.Nodes.Count > 0)
                                        {
                                            foreach (TreeNode market1 in market.Nodes)
                                            {
                                                marketlist.Add(market1.Name);
                                            }
                                        }

                                        else
                                            marketlist.Add(market.Name);
                                    }
                                }
                            }
                        }

                        else if (node.Level == 3)
                        {
                            foreach (TreeNode matches in node.Nodes)
                            {
                                foreach (TreeNode market in matches.Nodes)
                                {
                                    if (market.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode market1 in market.Nodes)
                                        {
                                            marketlist.Add(market1.Name);
                                        }
                                    }

                                    else
                                        marketlist.Add(market.Name);
                                }
                            }
                        }

                        else if (node.Level == 4)
                        {
                            
                                foreach (TreeNode market in node.Nodes)
                                {
                                    if (market.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode market1 in market.Nodes)
                                        {
                                            marketlist.Add(market1.Name);
                                        }
                                    }

                                    else
                                        marketlist.Add(market.Name);
                                }
                           
                        }

                        else if (node.Level >= 5)
                        {

                            if (node.Nodes.Count > 0)
                            {
                                foreach (TreeNode market1 in node.Nodes)
                                {
                                    marketlist.Add(market1.Name);
                                }
                            }

                            else
                                marketlist.Add(node.Name);
                            

                        }
                    }
                }
                
            }
            
            else
            {
                MessageBox.Show("No Market Selected");
                
            }
            return marketlist;
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            FlowLayoutPanel panel = sender as FlowLayoutPanel;
            if (panel.HasChildren)
            {
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    if (ctrl.GetType().Equals(typeof(GroupBox)))
                    {
                        
                        ctrl.Width = flowLayoutPanel1.Width - 30;
                    }
                }
            }
        }

        private int couponmarkets=0;
        public void SaveCoupon(string couponname)
        {
           
            OddsCheckerCrawler parent = MdiParent as OddsCheckerCrawler;
            try
            {
                    List<Coupon> couponlist = new List<Coupon>();
                    String msg = String.Empty;
                    if (flowLayoutPanel1.Controls.Count > 0)
                    {
                        int count = flowLayoutPanel1.Controls.Count;
                        Task[] tasks = new Task[count];
                        foreach (Control ctrl in flowLayoutPanel1.Controls)
                        {
                             tasks[ctrl.TabIndex] = Task.Factory.StartNew(() =>
                            {
                            if (ctrl.GetType().Equals(typeof(GroupBox)))
                            {
                                GroupBox grpBox = ctrl as GroupBox;

                                foreach (Control ctrl1 in grpBox.Controls)
                                {
                                    if (ctrl1.GetType().Equals(typeof(FlowLayoutPanel)))
                                    {
                                        FlowLayoutPanel panel = ctrl1 as FlowLayoutPanel;
                                        //DataTable dt = coupon.GetMatchesByCouponId(couponid).Tables[0];
                                        //string test = panel.Name;
                                        //DataRow[] rows = dt.Select("(id = " + panel.Name + ")");
                                        //if (ctrl.GetType().Equals(typeof(DataGridView)))
                                        //if (rows.Length <= 0)
                                        //{
                                        foreach (Control ctrl2 in panel.Controls)
                                        {

                                            if (ctrl2.GetType().Equals(typeof(DataGridView)))
                                            {
                                                //string uniqueid = Guid.NewGuid().ToString().Substring(0, 5);
                                                DataGridView grid = (ctrl2 as DataGridView);
                                                long matchid = Convert.ToInt64(grid.Name.Split(',')[1]);
                                                string teamsname = Helper.GetMatchName(matchid);
                                                //string identifier = "MH.GaaFootballAEid" + Guid.NewGuid().ToString().Substring(0, 5) + "." + teamsname;
                                                for (int i = 0; i < grid.Rows.Count - 1; i++)
                                                {
                                                    int x = i + 1;
                                                    string market = Helper.GetMarketName(Convert.ToInt64(grid.Name.Split(',')[0]));
                                                    string identifier = /*"MH.GaaFootballAEid" + uniqueid + "." + teamsname; */  market.Substring(0, 1) + "S" + x + "id";
                                                    //string market = coupon.GetMarketName(Convert.ToInt64(grid.Name.Split(',')[0]));
                                                    //identifier = identifier + market.Substring(0, 1) + "S" + i+1 + "id";
                                                    couponlist.Add(new Coupon() { Bettingmarketid = Convert.ToInt64(grid.Name.Split(',')[0]), Toals = Convert.ToString(grid.Rows[i].Cells[0].Value), Selection = Convert.ToString(grid.Rows[i].Cells[1].Value), Identifier = identifier });
                                                    couponmarkets++;
                                                }
                                            }
                                        }
                                    }
                                  }
                                }
                            }, TaskCreationOptions.LongRunning);
                        }
                        Task.WaitAll(tasks);
                       
                            if (InvokeRequired)
                            {
                                Action a = () =>
                                {
                                    parent.IsProcessRunning = false;
                                    parent.SetProgress(false);
                                    string couponid = Guid.NewGuid().ToString().Substring(0, 5);
                                    GenerateCoupon coupon = new GenerateCoupon();
                                    while (coupon.IsCouponIdExist(couponid))
                                    {
                                        couponid = Guid.NewGuid().ToString().Substring(0, 5);
                                    }
                                    msg = coupon.InsertCoupon(couponlist, couponname,couponid);
                                    MessageBox.Show(msg);
                                }; BeginInvoke(a);
                            }
                    }
                    else
                    {
                        Action c = () =>
                        {
                            parent.IsProcessRunning = false;
                            parent.SetProgress(false);
                        }; BeginInvoke(c);
                        MessageBox.Show("No available markets found!");
                    }
                }
              
            
            
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Action b = () =>
                    {
                        parent.IsProcessRunning = false;
                        parent.SetProgress(false);
                        MessageBox.Show("An error occured while saving this coupon");
                    }; BeginInvoke(b);
                }
            }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
            try
            {
                GenerateCoupon coupon = new GenerateCoupon();
                if (!string.IsNullOrEmpty(txtcouponname.Text))
                {
                    if (!coupon.IsCouponExist(txtcouponname.Text))
                    {
                        

                        if (parent.IsProcessRunning)
                        {
                            MessageBox.Show("Please wait while selected markets are being processed");
                            return;
                        }

                        parent.IsProcessRunning = true;
                        parent.SetProgress(true);
                        Task taskB = Task.Factory.StartNew(() =>
                               {
                                   SaveCoupon(txtcouponname.Text);
                                   if (InvokeRequired)
                                   {
                                       Action loadCombo = () =>
                                           {
                                               LoadComboBox();
                                           };
                                       BeginInvoke(loadCombo);
                                   }
                               });
                    }
                    else
                    {
                        MessageBox.Show("Coupon name already exist!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a coupon name");
                }
            }
            catch (Exception ex)
            {
                parent.IsProcessRunning = false;
                parent.SetProgress(false);
                MessageBox.Show("An error occured while saving this coupon");
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(comboCoupon.SelectedItem)))
                {
                    string couponid = Convert.ToString(comboCoupon.SelectedValue);
                    

                    if (parent.IsProcessRunning)
                    {
                        MessageBox.Show("Please wait while selected markets are being processed");
                        return;
                    }

                    parent.IsProcessRunning = true;
                    parent.SetProgress(true);
                    Task taskB = Task.Factory.StartNew(() =>
                              {
                                  AddToCoupon(couponid);
                              });
                }
                else
                {
                    MessageBox.Show("Please select a coupon to add matches");
                }
            }
            catch (Exception ex)
            {
                parent.IsProcessRunning = false;
                parent.SetProgress(false);
                MessageBox.Show("An error occured while saving this coupon");

            }
        }

        public void AddToCoupon(string couponid)
        {
            GenerateCoupon coupon = new GenerateCoupon();
            OddsCheckerCrawler parent = MdiParent as OddsCheckerCrawler;
            try
            {


                List<Coupon> couponlist = new List<Coupon>();
                String msg = String.Empty;
                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    DataTable dt = coupon.GetMatchesByCouponId(couponid).Tables[0];
                    if (lblsportid.Text.Equals(Convert.ToString(dt.Rows[0]["sportid"])))
                    {
                        foreach (Control ctrl in flowLayoutPanel1.Controls)
                        {
                            if (ctrl.GetType().Equals(typeof(GroupBox)))
                            {
                                GroupBox grpBox = ctrl as GroupBox;

                                //string test = grpBox.Name;

                                DataRow[] rows = dt.Select("(id = " + grpBox.Name + ")");
                                if (rows.Length <= 0)
                                {
                                    foreach (Control ctrl1 in grpBox.Controls)
                                    {
                                        if (ctrl1.GetType().Equals(typeof(FlowLayoutPanel)))
                                        {
                                            FlowLayoutPanel panel = ctrl1 as FlowLayoutPanel;
                                            //DataTable dt = coupon.GetMatchesByCouponId(couponid).Tables[0];
                                            //string test = panel.Name;
                                            //DataRow[] rows = dt.Select("(id = " + panel.Name + ")");
                                            //if (ctrl.GetType().Equals(typeof(DataGridView)))
                                            //if (rows.Length <= 0)
                                            //{
                                            foreach (Control ctrl2 in panel.Controls)
                                            {
                                                //string uniqueid = Guid.NewGuid().ToString().Substring(0, 5);
                                                DataGridView grid = (ctrl2 as DataGridView);
                                                long matchid = Convert.ToInt64(grid.Name.Split(',')[1]);
                                                string teamsname = Helper.GetMatchName(matchid);
                                                //string identifier = "MH.GaaFootballAEid" + Guid.NewGuid().ToString().Substring(0, 5) + "." + teamsname;
                                                for (int i = 0; i < grid.Rows.Count - 1; i++)
                                                {

                                                    int x = i + 1;
                                                    string market = Helper.GetMarketName(Convert.ToInt64(grid.Name.Split(',')[0]));
                                                    string identifier = /*"MH.GaaFootballAEid" + uniqueid + "." + teamsname; */  market.Substring(0, 1) + "S" + x + "id";
                                                    //string market = coupon.GetMarketName(Convert.ToInt64(grid.Name.Split(',')[0]));
                                                    //identifier = identifier + market.Substring(0, 1) + "S" + i+1 + "id";
                                                    couponlist.Add(new Coupon() { Bettingmarketid = Convert.ToInt64(grid.Name.Split(',')[0]), Toals = Convert.ToString(grid.Rows[i].Cells[0].Value), Selection = Convert.ToString(grid.Rows[i].Cells[1].Value), Identifier = identifier });
                                                    //}
                                                }
                                            }


                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Cannot add " + rows[0]["Name"].ToString() + ". It alredy exists in selected coupon.");
                                }
                            }
                        }

                        if (InvokeRequired)
                        {
                            Action a = () =>
                            {
                                parent.IsProcessRunning = false;
                                parent.SetProgress(false);
                                msg = coupon.AddCouponMatches(couponlist, couponid);
                                MessageBox.Show(msg);
                            }; BeginInvoke(a);
                        }
                    }

                    else
                    {
                        
                        if (InvokeRequired)
                        {
                            Action a = () =>
                            {
                                parent.IsProcessRunning = false;
                                parent.SetProgress(false);
                                MessageBox.Show("Sorry! this coupon belongs to a different sport");
                               
                            }; BeginInvoke(a);
                        }
                    }

                   
                }
                else
                {
                    MessageBox.Show("No available markets found!");
                }
            }

            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Action b = () =>
                    {
                        parent.IsProcessRunning = false;
                        parent.SetProgress(false);
                        MessageBox.Show("An error occured while updating this coupon");
                    }; BeginInvoke(b);
                }
            }
        }

        private void OddsCrawler_Load(object sender, EventArgs e)
        {
            OddsCheckerCrawler crawl = this.MdiParent as OddsCheckerCrawler;
            crawl.SetProgress(true);
            crawl.IsProcessRunning = true;
            LoadComboBox();
            
        }

        private void OddsCrawler_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                OddsCheckerCrawler crawl = MdiParent as OddsCheckerCrawler;
                crawl.UncheckMenuItem(this.Name);
                crawl.SetProgress(false);
                crawl.IsProcessRunning = false;
            }
            catch (System.Exception ex)
            {
            }
        }
    }
}
