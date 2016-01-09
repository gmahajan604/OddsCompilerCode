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
    public partial class GolfCrawler : Form
    {
        public int countthread = 0;
        private string sport;
        public GolfCrawler(string sportid)
        {
            InitializeComponent();
            sport = sportid;
            Shown += new EventHandler(Form1_Shown);
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
        }
        void Form1_Shown(object sender, EventArgs e)
        {
            // Start the background worker.
            backgroundWorker1.RunWorkerAsync();
        }
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadTreeViewForGolf();
            BindMasterMarket(Convert.ToInt32(sport));
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
        private void LoadTreeViewForGolf()
        {
            CrawlGolf golf = new CrawlGolf();
            DataTable dt = golf.GetGolfTurnament();
            TreeNode tn_group = new TreeNode("GolfTurnaments");
            foreach (DataRow drSport in dt.Rows)
            {
                TreeNode Duration = new TreeNode(drSport["Duration"].ToString());
                TreeNode Turnament=new TreeNode(drSport["Turnament"].ToString()+":"+drSport["GolfID"].ToString());
                Turnament.Nodes.Add(new TreeNode(drSport["Course"].ToString()));
                Turnament.Nodes.Add(new TreeNode(drSport["Champion"].ToString()));
                Duration.Nodes.Add(Turnament);
                tn_group.Nodes.Add(Duration);
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
                if (e.Node.Level == 2)
                {
                    OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
                    if (!parent.IsProcessRunning)
                    {
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
                                FillData(info1[1].ToString(),info1[0].ToString(),1);
                            }, TaskCreationOptions.LongRunning);
                        }
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
        private void BindMasterMarket(int sportid)
        {
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            cmbMasterMarketName.DataSource = crawl.GetMasterMarketName(sportid);
            cmbMasterMarketName.DisplayMember = "ResultName";
            cmbMasterMarketName.ValueMember = "ResultLink";
        }
        public void FillData2(string url, long id, long goflid, string bookies, int count)
        {
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;

            try
            {
                CrawlEachMarket crawl = new CrawlEachMarket();

                DataGridView dataGridView1 = new DataGridView();
                DataTable dt = crawl.CrawlMarkets(url, id, goflid,bookies).Tables[0];
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
                dataGridView1.Name = Convert.ToString(id) + "," + Convert.ToString(goflid);
                // dataGridView1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;

                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        //if (!matchid.Equals(match_id))
                        if (!flowLayoutPanel1.Controls.ContainsKey(goflid.ToString()))
                        {
                            CrawlFirstPage crawlpage = new CrawlFirstPage();
                            DataTable dt1 = crawlpage.GetMatchInfo(goflid);
                            GroupBox grpBox = new GroupBox();
                            string match = Convert.ToString(dt1.Rows[0]["Name"]) + " (" + Convert.ToString(dt1.Rows[0]["MatchDate"]) + ")";
                            grpBox.Text = match;
                            grpBox.Name = goflid.ToString();
                            grpBox.AutoSize = true;
                            FlowLayoutPanel flowpanel = new FlowLayoutPanel();
                            flowpanel.FlowDirection = FlowDirection.TopDown;
                            flowpanel.Dock = DockStyle.Fill;
                            flowpanel.WrapContents = false;
                            flowpanel.AutoSize = true;
                            flowpanel.Name = goflid.ToString() + goflid.ToString();
                            grpBox.Controls.Add(flowpanel);
                            flowLayoutPanel1.Controls.Add(grpBox);
                            grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                            //grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                        }

                        FlowLayoutPanel panel = flowLayoutPanel1.Controls.Find(goflid.ToString() + goflid.ToString(), true).FirstOrDefault() as FlowLayoutPanel;
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
            if (e.RowIndex < grid.Rows.Count - 1 && e.ColumnIndex == 0)
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
            if (e.RowIndex == grid.Rows.Count - 1)
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
                            if (Convert.ToString(grid.Rows[j].Cells[1].Value).Contains("("))
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


        private void FillData(string golfid,string url, int count)
        {
            //OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
            //try
            //{
            //    CrawlGolf golf = new CrawlGolf();
            //    DataGridView dataGridView1 = new DataGridView();
            //    DataTable dt = golf.GetGoldMarkets(golfid,turnament,url);
            //    DataRow newrow = dt.NewRow();
            //    dt.Rows.InsertAt(newrow, dt.Rows.Count);
            //    dataGridView1.DataSource = dt;
            //    dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            //    dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            //    dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //    //dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            //    //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            //    //dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //    dataGridView1.AllowUserToDeleteRows = false;
            //    dataGridView1.AllowUserToAddRows = false;
            //    dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
            //    dataGridView1.BorderStyle = BorderStyle.None;
            //    dataGridView1.RowHeadersVisible = false;
            //    dataGridView1.Name = Convert.ToString(turnament);
            //    // dataGridView1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
            //    if (InvokeRequired)
            //    {
            //        Action a = () =>
            //        {
            //            //if (!matchid.Equals(match_id))
            //            if (!flowLayoutPanel1.Controls.ContainsKey(golfid.ToString()))
            //            {
            //                CrawlFirstPage crawlpage = new CrawlFirstPage();
            //                GroupBox grpBox = new GroupBox();
            //                grpBox.Text = turnament;
            //                grpBox.Name =golfid.ToString();
            //                grpBox.AutoSize = true;
            //                FlowLayoutPanel flowpanel = new FlowLayoutPanel();
            //                flowpanel.FlowDirection = FlowDirection.TopDown;
            //                flowpanel.Dock = DockStyle.Fill;
            //                flowpanel.WrapContents = false;
            //                flowpanel.AutoSize = true;
            //                flowpanel.Name = golfid.ToString() + golfid.ToString();
            //                grpBox.Controls.Add(flowpanel);
            //                flowLayoutPanel1.Controls.Add(grpBox);
            //                grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //                //grpBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //            }
            //            FlowLayoutPanel panel = flowLayoutPanel1.Controls.Find(golfid.ToString() + golfid.ToString(), true).FirstOrDefault() as FlowLayoutPanel;
            //            panel.Controls.Add(dataGridView1);
            //            countthread++;
            //            if (countthread.Equals(count))
            //            {
            //                crawlcheck.SetProgress(false);
            //                crawlcheck.IsProcessRunning = false;
            //            }
            //        };
            //        BeginInvoke(a);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (InvokeRequired)
            //    {
            //        Action b = () =>
            //        {
            //            crawlcheck.SetProgress(false);
            //            crawlcheck.IsProcessRunning = false;
            //        }; BeginInvoke(b);

            //    }

            //}
           
        }

        private void btnViewMarket_Click(object sender, EventArgs e)
        {
            //try
            //{
               
            //        OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
            //        if (!parent.IsProcessRunning)
            //        {
            //            //if (!string.IsNullOrEmpty(parent.SelectedBookies()))
            //            //{
            //            //parent.SetProgress(true);
            //            //parent.IsProcessRunning = true;
            //            string[] info1 = info.Split(',');
            //            if (info1.Length > 1)
            //            {
            //                countthread = 0;
            //                parent.IsProcessRunning = true;
            //                parent.SetProgress(true);
            //                flowLayoutPanel1.Controls.Clear();
            //                lblsportid.Text = info1[3];
            //                Task taskA = Task.Factory.StartNew(() =>
            //                {
            //                    FillData2(cmbMasterMarketName.SelectedValue, Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]), parent.SelectedBookies(), 1);
            //                }, TaskCreationOptions.LongRunning);
            //            }
            //            //}
            //            //else
            //            //{
            //            //    MessageBox.Show("Please select atleast one bookie from Select Bookies menu");
            //            //}
            //        }
            //        else
            //        {
            //            MessageBox.Show("A process is already running");
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An internal error occured while processing the request");
            //}
        }
    }
}