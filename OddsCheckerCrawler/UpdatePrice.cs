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
using System.Threading;
using OddsProperties;

namespace OddsCheckerCrawler
{
    public partial class UpdatePrice : Form
    {
        //private List<string> updatemarkets = new List<string>();
        public int countthread = 0;
        public UpdatePrice()
        {
            InitializeComponent();
        }

        public UpdatePrice(string couponid,long matchid)
        {
            InitializeComponent();
            LoadTree(couponid,matchid);            
            //lblcouponname.Text = Helper.CouponName(couponid);
            //lblmatch.Text = matchname;
            //lbldate.Text = matchdate;
        }

        private void LoadTree(string couponid,long matchid)
        {
            bool IsArchived = false;
            GenerateCoupon coupon = new GenerateCoupon();
            DataSet ds = coupon.GetCoupons(IsArchived);
            TreeNode tnupdate = new TreeNode("Markets Priced");
            string sql2 = "(couponid='" + couponid + "') AND (matchid=" + matchid + ")";
            foreach (DataRow drMkt in ds.Tables[4].Select(sql2))
            {

                TreeNode tn_market = new TreeNode(drMkt["bettingmarket"].ToString());
                tn_market.Name = drMkt["bettingmarketid"].ToString() + "," + drMkt["matchid"].ToString() + "," + drMkt["bettinglink"].ToString() + "," + drMkt["couponid"].ToString(); 
                tnupdate.Nodes.Add(tn_market);

            }
            treeView1.Nodes.Add(tnupdate);
        }
        //private void NewThread()
        //{
        //    OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
        //    try
        //    {
                
        //        crawlcheck.SetProgress(true);
        //        crawlcheck.IsCouponProcessRunning = true;
        //        flowLayoutPanel1.Controls.Clear();
        //        foreach (string market in updatemarkets)
        //        {
        //            string[] info = market.Split(',');


        //            Task taskB = Task.Factory.StartNew(() =>
        //            {
        //                FillData(info[0], Convert.ToInt64(info[1]), Convert.ToInt64(info[2]), updatemarkets.Count, Convert.ToInt64(info[3]));
        //            }, TaskCreationOptions.LongRunning);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        crawlcheck.SetProgress(false);
        //        crawlcheck.IsCouponProcessRunning = false;
        //        MessageBox.Show("An error occured while processing this request!");
        //    }
        //}

        private List<Market> selections = new List<Market>();
      
         public void FillData(string url, long id, long matchid, int count, string couponid)
       
         {
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
            string bookies = crawlcheck.SelectedBookies();
            GenerateCoupon coupon = new GenerateCoupon();
            //string msg = crawl.DeleteMarketOdds(id);
            //crawl.CrawlMarkets(url, id, matchid);
            DataGridView dataGridView1 = new DataGridView();
            DataSet ds = coupon.GetCouponMarket(url, id, matchid, bookies, couponid);
            DataTable dt = ds.Tables[0];
            DataRow newrow = dt.NewRow();
            dt.Rows.InsertAt(newrow, dt.Rows.Count);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.Width = flowLayoutPanel1.Width - 30;
            dataGridView1.Name = Convert.ToString(id);


            if (InvokeRequired)
            {
                Action a = () =>
                {
                    
                    flowLayoutPanel1.Controls.Add(dataGridView1);
                    dataGridView1.Columns[dataGridView1.Columns.Count-1].Visible = false;
                    lblmatch.Text = "Match Name: "+Convert.ToString(ds.Tables[1].Rows[0]["matchname"]);
                    lblmatch.Name = couponid;
                    lbldate.Text = "Match Date: "+ds.Tables[1].Rows[0]["MatchDate"].ToString();
                    lbldate.Name = Convert.ToString(matchid);
                    lblcouponname.Text = "Coupon Name: "+Helper.CouponName(couponid);
                    PricePercent(dataGridView1);

                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        selections.Add(new Market() { bestbet = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value), beton = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value) });
                    }
                    // Ensure that all UI updates are done on the main thread
                    //lblmatch.Text = ds.Tables[1].Rows[0]["matchname"].ToString();
                    //txtmatchdate.Text = ds.Tables[1].Rows[0]["MatchDate"].ToString();
                    //lblmktpriced.Text = "Markets Priced: " + ds.Tables[1].Rows[0]["MarketPriced"].ToString();
                    //lblmktavail.Text = "Markets Available: " + ds.Tables[1].Rows[0]["MarketAvail"].ToString();
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {

                        column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                    }
                   
                    countthread++;
                    if (countthread.Equals(count))
                    {
                        //dataGridView1.Rows[1].EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                        crawlcheck.SetProgress(false);
                        crawlcheck.IsCouponProcessRunning = false;
                    }
                };
                BeginInvoke(a);
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

         public void PricePercent(DataGridView grid)
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
             //grid.Rows.Add();
             grid.Rows[grid.Rows.Count - 1].Cells[0].Value = sum.ToString() + "%";
         }

        private int dgvHeight(DataGridView grid)
        {
            int sum = grid.ColumnHeadersHeight;

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

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
            try
            {

                if (e.Node.Level == 1)
                {
                    
                    if (!parent.IsCouponProcessRunning)
                    {
                        string info = e.Node.Name;
                        string[] info1 = info.Split(',');
                        countthread = 0;
                        parent.IsCouponProcessRunning = true;
                        parent.SetProgress(true);
                        flowLayoutPanel1.Controls.Clear();
                        Task taskA = Task.Factory.StartNew(() =>
                        {
                            FillData(info1[2], Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]), 1, info1[3]);

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
                parent.IsCouponProcessRunning = false;
                parent.SetProgress(false);
                MessageBox.Show("An internal error occured while processing the request");
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            GenerateCoupon coupon = new GenerateCoupon();
            List<Market> updatedOdds = new List<Market>();
            //CrawlFirstPage crawl = new CrawlFirstPage();
            try
            {
                string text = "";
                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    //if (!string.IsNullOrEmpty(txtmatchdate.Text))
                    //{
                    //    string enddatetime = String.Empty;
                    //    string[] matchdate = txtmatchdate.Text.Split(' ');
                    //    enddatetime = matchdate[1].Substring(0, 2) + " " + matchdate[2].Substring(0, 3) + " " + matchdate[3];
                    //    enddatetime = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd");
                    //    crawl.UpdateMatch(Convert.ToInt64(txtmatchdate.Name),txtmatchdate.Text,enddatetime);
                    //}
                    foreach (Control ctrl in flowLayoutPanel1.Controls)
                    {
                        if (ctrl.GetType().Equals(typeof(DataGridView)))
                        {
                            DataGridView grid = ctrl as DataGridView;
                            for (int i = 0; i < grid.Rows.Count - 1; i++)
                            {
                                if (!selections[i].bestbet.Equals(grid.Rows[i].Cells[0].Value) || !selections[i].beton.Equals(grid.Rows[i].Cells[1].Value))
                                {
                                    long id = Convert.ToInt64(grid.Rows[i].Cells[grid.Columns.Count - 1].Value);
                                    string toal = Convert.ToString(grid.Rows[i].Cells[0].Value);
                                    string selection = Convert.ToString(grid.Rows[i].Cells[1].Value);
                                    //coupon.UpdateCoupon(id, toal, selection);
                                    updatedOdds.Add(new Market() { id = id, bestbet = toal, beton = selection });
                                }
                            }
                        }
                    }
                    string couponid = lblmatch.Name;
                    coupon.UpdateOdds(updatedOdds,couponid);
                    
                    MessageBox.Show("Coupon updated successfully");
                    flowLayoutPanel1.Refresh();
                }
                else
                {
                    MessageBox.Show("Data not available!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while saving changes");
            }
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

        public void ClearFlowLayoutPanel()
        {
            flowLayoutPanel1.Controls.Clear();
        }
    }
}

  
      