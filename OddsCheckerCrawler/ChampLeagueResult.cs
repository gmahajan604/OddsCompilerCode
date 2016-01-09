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
using System.Net;

namespace OddsCheckerCrawler
{
    public partial class ChampLeagueResult : Form
    {
        public int countthread = 0;
        public ChampLeagueResult()
        {
            InitializeComponent();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Your background task goes here
            LoadChampionsLeague();
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
        private void ChampLeagueResult_Load(object sender, EventArgs e)
        {

        }
        private void ChampLeagueResult_Shown(object sender, EventArgs e)
        {
            // Start the background worker
            backgroundWorker1.RunWorkerAsync();
        }

        private void LoadChampionsLeague()
        {
            DataTable dt = new DataTable();
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            // 358==Leagueid of Champions League
            dt = crawl.GetMatchesByCoupon();

            TreeNode tn_group = new TreeNode("Match Result");
            string matchdate = "";
            foreach (DataRow row in dt.Rows)
            {

                if (matchdate != row["MatchDate"].ToString())
                {
                    matchdate = row["MatchDate"].ToString();
                    TreeNode tn_date = new TreeNode(matchdate);
                    string sql1 = "(matchdate='" + matchdate + "')";
                    tn_group.Nodes.Add(tn_date);
                    foreach (DataRow dr1 in dt.Select(sql1))
                    {
                        TreeNode tn_match = new TreeNode(dr1["Home"].ToString() + " V " + dr1["Away"].ToString() + ":" + dr1["id"].ToString());
                        tn_date.Nodes.Add(tn_match);
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
      
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            try
            {
                TreeNode parentnode = e.Node;
                while (parentnode.Parent != null)
                {
                    parentnode = parentnode.Parent;
                }

                if (parentnode.Text.Equals("Match Result"))
                //if (e.Node.Level == 3 || e.Node.Level == 5)
                {
                    OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
                    if (!parent.IsProcessRunning)
                    {
                        //if (!string.IsNullOrEmpty(parent.SelectedBookies()))
                        //{

                        //parent.SetProgress(true);
                        //parent.IsProcessRunning = true;
                        string info = e.Node.Text;
                        string[] info1 = info.Split(':');
                        CrawlAllMarkets crawl = new CrawlAllMarkets();
                        string[] match = crawl.GetChampionMatchInfo(Convert.ToInt32(info1[1].ToString()));
                        //match[0]=link;match[1]=time;
                        if (match[0] != null)
                        {
                            parent.IsProcessRunning = true;
                            parent.SetProgress(true);
                            flowLayoutPanel1.Controls.Clear();

                            //    lblsportid.Text = info1[3];
                            countthread = 0;

                            Task taskA = Task.Factory.StartNew(() =>
                            {
                                FillMatchInfo(info1[1], match[0], match[1],1);

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

        private void FillMatchInfo(string id, string link, string time,int count)
        {
            CrawlFirstPage crawl = new CrawlFirstPage();
            //  crawl.CrawlChampionLeauge(id, link,time);
           // crawl.CrawlChampionLeauge(id, "file:///E:/OddsCompiler/z-rowdata/Results/Barcelona%20v%20Atletico%20Madrid/Barcelona%20v%20Atletico%20Madrid%20Winner%20Betting%20Odds%20%20%20Football%20Betting%20%20%20Oddschecker.htm", time);
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
            try
            {
                DataSet dt = crawl.GetMatchResult(Convert.ToInt32(id));
                DataGridView dataGridView1 = new DataGridView();
                dataGridView1.DataSource = dt.Tables[0];
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.Width = flowLayoutPanel1.Width - 5;
                dataGridView1.Name = Convert.ToString(id) ;

                // Second DataGridView
                DataGridView dataGridView2 = new DataGridView();
                dataGridView2.DataSource = dt.Tables[1];
                dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView2.AllowUserToDeleteRows = false;
                dataGridView2.BackgroundColor = SystemColors.ControlLightLight;
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.RowHeadersVisible = false;
                dataGridView2.Width = flowLayoutPanel1.Width - 5;
                dataGridView2.Name = Convert.ToString(id);
                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        flowLayoutPanel1.Controls.Add(dataGridView1);
                        flowLayoutPanel1.Controls.Add(dataGridView2);
             //           dataGridView1.DataSource = crawl.GetMatchResult(Convert.ToInt32(id)).DefaultView;
             //           dataGridView1.Refresh();
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        }
                        foreach (DataGridViewColumn column in dataGridView2.Columns)
                        {
                            column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        }
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

        private void btnsave_Click(object sender, EventArgs e)
        {
            CrawlAllMarkets c = new CrawlAllMarkets();
            
          //  c.CrawlBettingMarkets("http://www.oddschecker.com//football/world/morocco/gnf-1/ocs-olympique-de-safi-v-fus-rabat/betting-markets", 7624);
            CrawlFirstPage crawl = new CrawlFirstPage();
            Console.WriteLine("Crawling Started EachSport");
            crawl.GetMatchListByTime();
         //   crawl.CrawlChampionLeauge("3866", "file:///E:/OddsCompiler/z-rowdata/Results/Barcelona%20v%20Atletico%20Madrid/Barcelona%20v%20Atletico%20Madrid%20Winner%20Betting%20Odds%20%20%20Football%20Betting%20%20%20Oddschecker.htm", "34pm");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> markets = SelectedMarkets();
            string matchid = "0";
            foreach (string market in markets)
            {
                string[] info = market.Split(',');
                matchid = info[1];
            }
            frmResultView f = new frmResultView(matchid);
            f.Show();
        }
        public List<string> SelectedMarkets()
        {
            List<string> marketlist = new List<string>();
            if (treeView1.SelectedNodes.Count > 0)
            {
                foreach (TreeNode node in treeView1.SelectedNodes)
                {
                    if (node.Level == 1)
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

                    else if (node.Level == 2)
                    {

                        foreach (TreeNode matchnode in node.Nodes)
                        {
                            foreach (TreeNode marketnode in matchnode.Nodes)
                            {
                                marketlist.Add(marketnode.Name);
                            }
                        }


                    }

                    else if (node.Level == 3)
                    {

                        foreach (TreeNode marketnode in node.Nodes)
                        {
                            marketlist.Add(marketnode.Name);
                        }

                    }

                    else
                        if (node.Level == 4)
                        {
                            marketlist.Add(node.Name);
                        }
                }
            }
            //else
            //{
            //    MessageBox.Show("No Market Selected");
            //}
            return marketlist;
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  WebClient client = new WebClient();
          //  string downloadString = client.DownloadString("http://www.gooogle.com");

        }
    }
}