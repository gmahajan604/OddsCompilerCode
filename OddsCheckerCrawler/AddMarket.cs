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

namespace OddsCheckerCrawler
{
    public partial class AddMarket : Form
    {
        public int countthread = 0;

        public AddMarket()
        {
            InitializeComponent();
        }

        public AddMarket(long matchid,string couponid)
        {
            InitializeComponent();
            LoadTreeView(matchid,couponid);
            lblcouponname.Text = "Coupon Name: "+Helper.CouponName(couponid);
            lblcouponname.Name = couponid.ToString();
            //lbl
        }

        
        private void LoadTreeView(long matchid,string couponid)
        {
            GenerateCoupon coupon = new GenerateCoupon();
            DataTable dt = coupon.GetAvailableMarkets(matchid, couponid).Tables[0];
            TreeNode tnmarket = new TreeNode("Available Markets");
            foreach (DataRow row in dt.Rows)
            {
                TreeNode tn_market = new TreeNode(row["bettingmarket"].ToString());
                tn_market.Name = row["id"].ToString() + "," + row["matchid"].ToString() + "," + row["bettinglink"].ToString(); // +"," + row["couponid"].ToString(); ;
                tnmarket.Nodes.Add(tn_market);
            }

            treeView1.Nodes.Add(tnmarket);
        }

        public void FillData(string url, long id, long matchid, string bookies, int count)
        {
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;

            try
            {
                CrawlEachMarket crawl = new CrawlEachMarket();
                // string msg = crawl.DeleteMarketOdds(id);
                //  crawl.CrawlMarkets(url, id, matchid);

                DataGridView dataGridView1 = new DataGridView();
                DataTable dt = crawl.CrawlMarkets(url, id, matchid, bookies).Tables[0];
                DataRow newrow = dt.NewRow();
                dt.Rows.InsertAt(newrow, dt.Rows.Count);
                dataGridView1.DataSource = dt;
                //dataGridView1.DataSource = crawl.GetMarketOdds(id, bookies);
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                //dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.RowHeadersVisible = false;

                dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
                dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
                dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
                dataGridView1.Width = flowLayoutPanel1.Width - 30;
                dataGridView1.Name = Convert.ToString(id) + "," + Convert.ToString(matchid);
                //dataGridView1.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        flowLayoutPanel1.Controls.Add(dataGridView1);
                        countthread++;
                        if (countthread.Equals(count))
                        {

                            crawlcheck.SetProgress(false);
                            crawlcheck.IsCouponProcessRunning = false;

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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            int sum = 0;
            foreach (DataGridViewColumn column in grid.Columns)
            {

                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            }

            grid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sum = dgvHeight(grid);
            grid.ClientSize = new Size(flowLayoutPanel1.Width - 30, sum); 
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

        public void AddMarkets(string couponid)
        {
            List<Coupon> couponlist = new List<Coupon>();
            String msg = String.Empty;
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                GenerateCoupon coupon = new GenerateCoupon();
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    if (ctrl.GetType().Equals(typeof(DataGridView)))
                    {
                        //string uniqueid = Guid.NewGuid().ToString().Substring(0, 5);
                        DataGridView grid = (ctrl as DataGridView);
                        long matchid = Convert.ToInt64(grid.Name.Split(',')[1]);
                        string teamsname = Helper.GetMatchName(matchid);
                        //string identifier = "MH.GaaFootballAEid" + Guid.NewGuid().ToString().Substring(0, 5) + "." + teamsname;
                        for (int i = 0; i < grid.Rows.Count - 1; i++)
                        {
                            //if (string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
                            //{
                            //    MessageBox.Show("Please select some value for Toals column in row" + (i + 1).ToString());
                            //    return;
                            //}
                            //else
                            //{
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
                // GenerateCoupon coupon = new GenerateCoupon();
                msg = coupon.AddMarket(couponlist, couponid);
                MessageBox.Show(msg);
            }
        }

        private int dgvHeight(DataGridView grid)
        {
            int sum = 0;
            sum += grid.ColumnHeadersHeight;

            foreach (DataGridViewRow row in grid.Rows)
                sum += row.Height + 1;

            return sum;
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

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Level == 1)
                {
                    OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
                    if (!parent.IsCouponProcessRunning)
                    {
                        //if (!string.IsNullOrEmpty(parent.SelectedBookies()))
                        //{
                        //parent.SetProgress(true);
                        //parent.IsProcessRunning = true;
                        string info = e.Node.Name;
                        string[] info1 = info.Split(',');
                        countthread = 0;
                        parent.IsCouponProcessRunning = true;
                        parent.SetProgress(true);
                        flowLayoutPanel1.Controls.Clear();
                        Task taskA = Task.Factory.StartNew(() =>
                        {
                            FillData(info1[2],Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]),parent.SelectedBookies(),1);
                        }, TaskCreationOptions.LongRunning);
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

        public void ClearFlowLayoutPanel()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        public List<string> SelectedMarkets()
        {
            List<string> marketlist = new List<string>();
            if (treeView1.SelectedNodes.Count > 0)
            {
                foreach (TreeNode node in treeView1.SelectedNodes)
                {
                   if (node.Level == 0)
                    {
                        foreach (TreeNode marketnode in node.Nodes)
                        {
                            marketlist.Add(marketnode.Name);
                        }
                    }

                    else if (node.Level == 1)
                    {
                        marketlist.Add(node.Name);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Market Selected");
                
            }
            return marketlist;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            AddMarkets(lblcouponname.Name);
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}