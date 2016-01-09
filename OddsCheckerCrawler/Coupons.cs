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
using System.Xml;

namespace OddsCheckerCrawler
{
    public partial class Coupons : Form
    {
        public int countthread = 0;
        private bool Is_Archived;
        public CodersLab.Windows.Controls.TreeView coupontree
        {
            get
            {
                return treeView1;
            }
            set
            {
                treeView1 = value;
            }
        }

        public Coupons(bool IsArchived)
        {

            InitializeComponent();
            LoadTree(IsArchived);
            Is_Archived = IsArchived;
            if (IsArchived)
            {
                lblmktavail.Visible = false;
                lblmktpriced.Visible = false;
                btnsave.Visible = false;
                btnaddmarket.Visible = false;
                btnupdate.Visible = false;
                btnxmlall.Visible = false;
                btnxmlupdated.Visible = false;
                addToToolStripMenuItem.Text = "Restore Coupon";
            }
            
            //OddsCheckerCrawler parent = this.MdiParent as OddsCheckerCrawler;
            //parent.toolStripMenuItem2.Enabled = true;
        }

        public void LoadTree(bool IsArchived)
        {
            treeView1.Nodes.Clear();
            GenerateCoupon coupon = new GenerateCoupon();
            DataSet ds = coupon.GetCoupons(IsArchived);
            ds.Relations.Add("Sport_Coupon", ds.Tables[0].Columns["sportid"], ds.Tables[1].Columns["sportid"]);
            ds.Relations.Add("Coupon_Date", ds.Tables[1].Columns["couponid"], ds.Tables[2].Columns["couponid"]);
            //ds.Relations.Add("Date_Match", ds.Tables[1].Columns["couponid"], ds.Tables[2].Columns["couponid"]);
            //ds.Relations.Add("Match_Market", ds.Tables[2].Columns["couponid"], ds.Tables[3].Columns["couponid"]);
            //ds.Relations.Add("Match_Date", ds.Tables[0].Columns["MatchDate"], ds.Tables[1].Columns["MatchDate"]);
            
            foreach (DataRow drSport in ds.Tables[0].Rows)
            {
                TreeNode tn_group = new TreeNode(Convert.ToString(drSport["SportName"]));
                tn_group.Name = Convert.ToString(drSport["sportid"]);
                treeView1.Nodes.Add(tn_group);
                foreach (DataRow dr in drSport.GetChildRows("Sport_Coupon"))
                {
                    TreeNode tn_coupon = new TreeNode(dr["couponname"].ToString());
                    string couponid = dr["couponid"].ToString();
                    tn_coupon.Name = couponid;
                    tn_coupon.ContextMenuStrip = CouponMenuStrip;
                    tn_group.Nodes.Add(tn_coupon);
                    foreach (DataRow dr1 in dr.GetChildRows("Coupon_Date"))
                    {
                        string matchdate = dr1["MatchDate"].ToString();
                        TreeNode tn_date = new TreeNode(matchdate);
                        tn_coupon.Nodes.Add(tn_date);

                        string sql = "(couponid='" + couponid + "') AND (MatchDate='" + matchdate + "')";
                        foreach (DataRow drChild in ds.Tables[3].Select(sql))
                        {

                            string matchid = drChild["matchid"].ToString();
                            TreeNode tn_match = new TreeNode(drChild["Home"].ToString() + " V " + drChild["Away"].ToString());
                            tn_date.Nodes.Add(tn_match);
                            string sql2 = "(couponid='" + couponid + "') AND (matchid=" + matchid + ")";
                            foreach (DataRow drMkt in ds.Tables[4].Select(sql2))
                            {

                                TreeNode tn_market = new TreeNode(drMkt["bettingmarket"].ToString());
                                tn_market.Name = drMkt["bettingmarketid"].ToString() + "," + drMkt["matchid"].ToString() + "," + drMkt["bettinglink"].ToString() + "," + drMkt["couponid"].ToString();
                                tn_match.Nodes.Add(tn_market);

                            }

                        }


                    }

                }
            }
           // treeView1.Nodes.Add(tn_group);
        }

        private int dgvHeight(DataGridView grid)
        {
            int sum = grid.ColumnHeadersHeight;

            foreach (DataGridViewRow row in grid.Rows)
                sum += row.Height + 1; 

            return sum;
        }


        //public void FillData(string url, long id, long matchid, string bookies, int count,string couponid)
        //{
        //    OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
        //    //if (InvokeRequired)
        //    //{
        //    //    Action b = () =>
        //    //    {
        //    //        crawlcheck.IsProcessRunning = true;
        //    //        crawlcheck.SetProgress(true);

        //    //    };
        //    //    BeginInvoke(b);
        //    //}
        //    GenerateCoupon coupon = new GenerateCoupon();
        //    //string msg = crawl.DeleteMarketOdds(id);
        //    //crawl.CrawlMarkets(url, id, matchid);

        //    DataGridView dataGridView1 = new DataGridView();
        //    //DataSet ds = coupon.GetCouponMarket(url, id, matchid, bookies, couponid);
        //    DataSet ds = coupon.GetCouponMarket(id, matchid,couponid);
        //    dataGridView1.DataSource =ds.Tables[0];
        //    //dataGridView1.DataSource = crawl.GetMarketOdds(id, bookies);
        //    dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //    dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
        //    dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        //    //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        //    dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        //    dataGridView1.AllowUserToDeleteRows = false;

        //    dataGridView1.RowHeadersVisible = false;

        //    dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

        //    dataGridView1.Width = flowLayoutPanel1.Width - 30;
        //    dataGridView1.Name = Convert.ToString(id);


        //    if (InvokeRequired)
        //    {
        //        Action a = () =>
        //        {
        //            flowLayoutPanel1.Controls.Add(dataGridView1);
        //            // Ensure that all UI updates are done on the main thread
        //            lblmatchnotset.Text = ds.Tables[1].Rows[0]["matchname"].ToString();
        //            txtmatchdate.Text = ds.Tables[1].Rows[0]["MatchDate"].ToString();
        //            lblmktpriced.Text = "Markets Priced: "+ds.Tables[1].Rows[0]["MarketPriced"].ToString();
        //            lblmktavail.Text = "Markets Available: "+ds.Tables[1].Rows[0]["MarketAvail"].ToString();
        //            foreach (DataGridViewColumn column in dataGridView1.Columns)
        //            {

        //                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

        //            }
        //            int sum = dgvHeight(dataGridView1);
        //            dataGridView1.Height = sum;
        //            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //            countthread++;
        //            if (countthread.Equals(count))
        //            {
        //                //dataGridView1.Rows[1].EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        //                crawlcheck.SetProgress(false);
        //                crawlcheck.IsCouponProcessRunning = false;
        //            }
        //        };
        //        BeginInvoke(a);
        //    }
        //}

        public void FillData(string url,long id, long matchid, string couponid,int count)
        {
            OddsCheckerCrawler crawlcheck = MdiParent as OddsCheckerCrawler;
            try
            {
                GenerateCoupon coupon = new GenerateCoupon();
                //string msg = crawl.DeleteMarketOdds(id);
                //crawl.CrawlMarkets(url, id, matchid);

                DataGridView dataGridView1 = new DataGridView();

                //DataSet ds = coupon.GetCouponMarket(url, id, matchid, bookies, couponid);
                DataSet ds = coupon.GetCouponMarket(id, matchid, couponid);
                DataTable dt = ds.Tables[0];
                DataRow newrow = dt.NewRow();
                dt.Rows.InsertAt(newrow, dt.Rows.Count);
                dataGridView1.DataSource = dt;
                //dataGridView1.DataSource = crawl.GetMarketOdds(id, bookies);
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                //dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                //dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.BackgroundColor = SystemColors.ControlLightLight;
                dataGridView1.AllowUserToAddRows = false;
                //dataGridView1.ReadOnly = true;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
                //dataGridView1.CellClick += dataGridView1_CellClick;
                //dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

                dataGridView1.Width = flowLayoutPanel1.Width - 30;
                dataGridView1.Name = url + "," + Convert.ToString(id) + "," + Convert.ToString(matchid) + "," + Convert.ToString(couponid);

              
                if (InvokeRequired)
                {
                    Action a = () =>
                    {
                        flowLayoutPanel1.Controls.Add(dataGridView1);
                        // Ensure that all UI updates are done on the main thread
                        dataGridView1.Columns[2].Visible = false;
                        dataGridView1.Columns[3].Visible = false;

                        lblmatchnotset.Text = ds.Tables[1].Rows[0]["matchname"].ToString();
                        lblmatchnotset.Name = Convert.ToString(couponid);
                        txtmatchdate.Text = ds.Tables[1].Rows[0]["MatchDate"].ToString()+" "+ds.Tables[1].Rows[0]["Time"].ToString();;
                        txtmatchdate.Name = Convert.ToString(matchid);
                        lblmktpriced.Text = "Markets Priced: " + ds.Tables[1].Rows[0]["MarketPriced"].ToString();
                        lblmktavail.Text = "Markets Available: " + ds.Tables[1].Rows[0]["MarketAvail"].ToString();
                        DataGridViewComboBoxColumn column1 = new DataGridViewComboBoxColumn();
                        column1.HeaderText = "Results";
                        column1.FlatStyle = FlatStyle.Flat;
                        
                        column1.Items.Add("Not Settled");
                        column1.Items.Add("Winner");
                        column1.Items.Add("Beaten");
                       

                        dataGridView1.Columns.Add((column1));
                        
                        PricePercent(dataGridView1);

                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        }

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (i < dataGridView1.Rows.Count - 1)
                            {
                                DataGridViewComboBoxCell cell = dataGridView1.Rows[i].Cells[dataGridView1.Columns.Count - 1] as DataGridViewComboBoxCell;
                                cell.Value = dataGridView1.Rows[i].Cells[2].Value;
                            }
                            else
                                if (i == dataGridView1.Rows.Count - 1)
                                {
                                    DataGridViewCell newCell = new DataGridViewTextBoxCell();
                                    dataGridView1[dataGridView1.Columns.Count - 1, i] = newCell;
                                }
                        }
                        int sum = dgvHeight(dataGridView1);
                        dataGridView1.Height = sum;

                        //dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Action b = () =>
                    {
                        crawlcheck.SetProgress(false);
                        crawlcheck.IsCouponProcessRunning = false;
                    }; BeginInvoke(b);
                }
            }
        }

        void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
            if (e.ColumnIndex != 4)
            {
                e.Cancel = true;
            }
        }


        public void ClearFlowLayoutPanel()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (e.RowIndex == grid.Rows.Count - 1)
            {
                double sum = 0;
                for(int i=0;i<e.RowIndex;i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
                    {
                        sum += 100 / ((Helper.FractionToDouble(grid.Rows[i].Cells[0].Value.ToString())) + 1);
                    }
                }
                sum = Math.Round(sum, 2);
                grid.Rows[e.RowIndex].Cells[0].Value = sum.ToString()+"%";
            }
            //throw new NotImplementedException();
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

       
             
      private void treeView1_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            OddsCheckerCrawler parent = MdiParent as OddsCheckerCrawler;
            try
            {
                if (parent.IsCouponProcessRunning)
                {
                    MessageBox.Show("A process is already running");
                    return;
                }

                if(e.Node.Level >2 && SelectedMarkets().Count > 0)
                {
               
                    List<string> markets = SelectedMarkets();
                    ClearFlowLayoutPanel();
                    parent.SetProgress(true);
                    parent.IsCouponProcessRunning = true;
                    countthread = 0;
                    Task taskB = Task.Factory.StartNew(() =>
                    {
                        foreach (string market in markets)
                        {
                            string[] info = market.Split(',');


                            FillData(info[2], Convert.ToInt64(info[0]), Convert.ToInt64(info[1]), info[3], markets.Count);



                        }
                    }, TaskCreationOptions.LongRunning);
                }
             
            }
            catch (Exception ex)
            {
                parent.SetProgress(false);
                parent.IsCouponProcessRunning = false;
                MessageBox.Show("An internal error occured while processing the request");
            }
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

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            FlowLayoutPanel panel = sender as FlowLayoutPanel;
            if (panel.HasChildren)
            {
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    if (ctrl.GetType().Equals(typeof(DataGridView)))
                    {
                        ctrl.Width = flowLayoutPanel1.Width - 30;
                    }
                }
            }
        }

        //public void SaveCoupon(string couponname)
        //{
        //    List<Coupon> couponlist = new List<Coupon>();
        //    String msg = String.Empty;
        //    if (flowLayoutPanel1.Controls.Count > 0)
        //    {
        //        foreach (Control ctrl in flowLayoutPanel1.Controls)
        //        {
        //            if (ctrl.GetType().Equals(typeof(DataGridView)))
        //            {
        //                DataGridView grid = (ctrl as DataGridView);
        //                for (int i = 0; i < grid.Rows.Count - 1; i++)
        //                {
        //                    if (string.IsNullOrEmpty(Convert.ToString(grid.Rows[i].Cells[0].Value)))
        //                    {
        //                        MessageBox.Show("Please select some value for Toals column in row" + (i + 1).ToString());
        //                    }
        //                    else
        //                    {
        //                        couponlist.Add(new Coupon() { Bettingmarketid = Convert.ToInt64(grid.Name), Toals = Convert.ToString(grid.Rows[i].Cells[0].Value), Selection = Convert.ToString(grid.Rows[i].Cells[1].Value) });
        //                    }
        //                }
        //            }
        //        }
        //        GenerateCoupon coupon = new GenerateCoupon();
        //        msg = coupon.InsertCoupon(couponlist, couponname);
        //        MessageBox.Show(msg);
        //    }
        //}

        public static void GenerateXMLforOdds(string file, DataSet ds)
        {

            XmlDocument doc = new XmlDocument();

            //DataSet ds = coupon.GetCoupons();
            ds.Relations.Add("Coupon_Match", ds.Tables[0].Columns["couponid"], ds.Tables[1].Columns["couponid"]);
            ds.Relations.Add("Match_Market", ds.Tables[1].Columns["matchid"], ds.Tables[2].Columns["matchid"]);
            ds.Relations.Add("Market_Sel", ds.Tables[2].Columns["bettingmarketid"], ds.Tables[3].Columns["bettingmarketid"]);
            TreeNode tn_group = new TreeNode("GAA Football");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string couponname = dr["couponname"].ToString();
                string createdon = dr["createdon"].ToString();
                
                XmlNode ArkleCouponNode = doc.CreateElement("ArkleCoupon");
                XmlAttribute ArkleSportIdAttribute = doc.CreateAttribute("SportsId");
                ArkleSportIdAttribute.Value = "12";
                ArkleCouponNode.Attributes.Append(ArkleSportIdAttribute);
                XmlAttribute ArkleSportAttribute = doc.CreateAttribute("_Sport");
                ArkleSportAttribute.Value = "GaaFootball";
                ArkleCouponNode.Attributes.Append(ArkleSportAttribute);
                XmlAttribute ArkleCouponIdentifier = doc.CreateAttribute("CouponIdentifier");
                ArkleCouponIdentifier.Value = couponname;
                ArkleCouponNode.Attributes.Append(ArkleCouponIdentifier);
                XmlAttribute ArkleCouponName = doc.CreateAttribute("CouponName");
                ArkleCouponName.Value = couponname;
                ArkleCouponNode.Attributes.Append(ArkleCouponName);
                XmlAttribute ArkleTimeCreated = doc.CreateAttribute("TimeCreated");
                ArkleTimeCreated.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleTimeCreated);
                XmlAttribute ArkleLastUpdated = doc.CreateAttribute("LastUpdated");
                ArkleLastUpdated.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleLastUpdated);
                XmlAttribute ArkleLastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
                ArkleLastPriceChangeIssued.Value = createdon;
                ArkleCouponNode.Attributes.Append(ArkleLastPriceChangeIssued);

                XmlAttribute ArkleMarketBettingTypeID = doc.CreateAttribute("MarketBettingTypeID");
                ArkleMarketBettingTypeID.Value = "Normal";
                ArkleCouponNode.Attributes.Append(ArkleMarketBettingTypeID);

                XmlAttribute ArkleDeSerializeStatusID = doc.CreateAttribute("DeSerializeStatusID");
                ArkleDeSerializeStatusID.Value = "SerializationInProgress";
                ArkleCouponNode.Attributes.Append(ArkleDeSerializeStatusID);

                XmlAttribute ArkleXsd = doc.CreateAttribute("xmlns", "xsd", "http://www.w3.org/2000/xmlns/");
                ArkleXsd.Value = "http://www.w3.org/2001/XMLSchema";
                ArkleCouponNode.Attributes.Append(ArkleXsd);

                XmlAttribute ArkleXsi = doc.CreateAttribute("xmlns", "xsi", "http://www.w3.org/2000/xmlns/");
                ArkleXsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
                ArkleCouponNode.Attributes.Append(ArkleXsi);

                doc.AppendChild(ArkleCouponNode);

                XmlNode comments = doc.CreateElement("Comments");
                comments.AppendChild(doc.CreateTextNode("Produced By M Halaburda"));
                ArkleCouponNode.AppendChild(comments);

                XmlNode filename = doc.CreateElement("FileName");
                filename.AppendChild(doc.CreateTextNode(file));
                ArkleCouponNode.AppendChild(filename);


                XmlNode EventsCollection = doc.CreateElement("EventsCollection");
                ArkleCouponNode.AppendChild(EventsCollection);

                foreach (DataRow dr1 in dr.GetChildRows("Coupon_Match"))
                {
                    string identifier = "MH.GaaFootballAEid" + Guid.NewGuid().ToString().Substring(0,5)+"."+ dr1["Home"].ToString()+dr1["Away"].ToString();
                    string matchcreatedon = dr1["createdon"].ToString();
                    string matchname = dr1["Name"].ToString();
                    string[] matchdate = dr1["MatchDate"].ToString().Split(' ');
                    string time = dr1["Time"].ToString().Substring(0,8);
                    string enddatetime = matchdate[1].Substring(0, 2) + " " + matchdate[2].Substring(0, 3) + " " + matchdate[3];
                    string date = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd")+"T"+time;
                    XmlNode ArkleEvent = doc.CreateElement("ArkleEvent");

                    XmlAttribute EventSportsId = doc.CreateAttribute("SportsId");
                    EventSportsId.Value = "12";
                    ArkleEvent.Attributes.Append(EventSportsId);

                    XmlAttribute EventSport = doc.CreateAttribute("_Sport");
                    EventSport.Value = "GaaFootball";
                    ArkleEvent.Attributes.Append(EventSport);

                    XmlAttribute EventTimeCreated = doc.CreateAttribute("TimeCreated");
                    EventTimeCreated.Value = matchcreatedon;
                    ArkleEvent.Attributes.Append(EventTimeCreated);

                    XmlAttribute QuadpotDividend = doc.CreateAttribute("QuadpotDividend");
                    QuadpotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(QuadpotDividend);

                    XmlAttribute PlacepotDividend = doc.CreateAttribute("PlacepotDividend");
                    PlacepotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(PlacepotDividend);

                    XmlAttribute JackpotDividend = doc.CreateAttribute("JackpotDividend");
                    JackpotDividend.Value = "0";
                    ArkleEvent.Attributes.Append(JackpotDividend);

                    XmlAttribute EventExpiryDate = doc.CreateAttribute("ExpiryDate");
                    EventExpiryDate.Value = DateTime.Parse(enddatetime).ToString("yyyy-MM-dd")+"T"+"23:59:59";
                    ArkleEvent.Attributes.Append(EventExpiryDate);

                    XmlAttribute DisplayEndDateTime = doc.CreateAttribute("DisplayEndDateTime");
                    DisplayEndDateTime.Value = date;
                    ArkleEvent.Attributes.Append(DisplayEndDateTime);

                    XmlAttribute OrderPriority = doc.CreateAttribute("OrderPriority");
                    OrderPriority.Value = "0";
                    ArkleEvent.Attributes.Append(OrderPriority);

                    XmlAttribute Marketclass = doc.CreateAttribute("Marketclass");
                    Marketclass.Value = "";
                    ArkleEvent.Attributes.Append(OrderPriority);

                    XmlAttribute EventIdentifier = doc.CreateAttribute("EventIdentifier");
                    EventIdentifier.Value = identifier;
                    ArkleEvent.Attributes.Append(EventIdentifier);

                    XmlAttribute Name = doc.CreateAttribute("Name");
                    Name.Value = matchname;
                    ArkleEvent.Attributes.Append(Name);

                    XmlAttribute EventSettleStatusType = doc.CreateAttribute("EventSettleStatusType");
                    EventSettleStatusType.Value = "Unknown";
                    ArkleEvent.Attributes.Append(EventSettleStatusType);

                    XmlAttribute SourceURL = doc.CreateAttribute("SourceURL");
                    SourceURL.Value = "";
                    ArkleEvent.Attributes.Append(SourceURL);

                    EventsCollection.AppendChild(ArkleEvent);

                    XmlNode handicap = doc.CreateElement("HandicapMatchBettingMarkets");
                    ArkleEvent.AppendChild(handicap);

                    XmlNode MatchForPrintOut = doc.CreateElement("MatchBettingMarketsForPrintout");
                    ArkleEvent.AppendChild(MatchForPrintOut);

                    XmlNode LastUpdated = doc.CreateElement("LastUpdated");
                    LastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                    ArkleEvent.AppendChild(LastUpdated);

                    XmlNode MarketsCollection = doc.CreateElement("MarketsCollection");
                    ArkleEvent.AppendChild(MarketsCollection);


                    //tn_coupon.Nodes.Add(tn_date);
                    foreach (DataRow drChild in dr1.GetChildRows("Match_Market"))
                    {
                        //TreeNode tn_match = new TreeNode(drChild["Home"].ToString() + " V " + drChild["Away"].ToString());

                        string market = drChild["bettingmarket"].ToString();

                        XmlNode ArkleMarket = doc.CreateElement("ArkleMarket");

                        XmlAttribute MarketName = doc.CreateAttribute("Name");
                        MarketName.Value = market;
                        ArkleMarket.Attributes.Append(MarketName);

                        XmlAttribute ContainsAmendedResult = doc.CreateAttribute("ContainsAmendedResult");
                        ContainsAmendedResult.Value = "false";
                        ArkleMarket.Attributes.Append(ContainsAmendedResult);

                        XmlAttribute MarkSenseMarketPriority = doc.CreateAttribute("MarkSenseMarketPriority");
                        MarkSenseMarketPriority.Value = "0";
                        ArkleMarket.Attributes.Append(MarkSenseMarketPriority);

                        XmlAttribute WordDisplayPriority = doc.CreateAttribute("_WordDisplayPriority");
                        WordDisplayPriority.Value = "0";
                        ArkleMarket.Attributes.Append(WordDisplayPriority);

                        XmlAttribute ScreenDisplayPriority = doc.CreateAttribute("_ScreenDisplayPriority");
                        ScreenDisplayPriority.Value = "1";
                        ArkleMarket.Attributes.Append(ScreenDisplayPriority);

                        XmlAttribute EachWayNumberPlaces = doc.CreateAttribute("EachWayNumberPlaces");
                        EachWayNumberPlaces.Value = "1";
                        ArkleMarket.Attributes.Append(EachWayNumberPlaces);

                        XmlAttribute EachWayDenominator = doc.CreateAttribute("EachWayDenominator");
                        EachWayDenominator.Value = "1";
                        ArkleMarket.Attributes.Append(EachWayDenominator);

                        XmlAttribute Archived = doc.CreateAttribute("Archived");
                        Archived.Value = "false";
                        ArkleMarket.Attributes.Append(Archived);

                        XmlAttribute EachWayAllowed = doc.CreateAttribute("EachWayAllowed");
                        EachWayAllowed.Value = "false";
                        ArkleMarket.Attributes.Append(EachWayAllowed);

                        XmlAttribute RaceCardNumber = doc.CreateAttribute("RaceCardNumber");
                        RaceCardNumber.Value = "0";
                        ArkleMarket.Attributes.Append(RaceCardNumber);

                        XmlAttribute ExpectedOffDateExpired = doc.CreateAttribute("ExpectedOffDateExpired");
                        ExpectedOffDateExpired.Value = "false";
                        ArkleMarket.Attributes.Append(ExpectedOffDateExpired);

                        XmlAttribute ExpectedOffDateDisplay = doc.CreateAttribute("ExpectedOffDateDisplay");
                        ExpectedOffDateDisplay.Value = "Sat 08 Mar 17:00";
                        ArkleMarket.Attributes.Append(ExpectedOffDateDisplay);

                        XmlAttribute ExpectedOffDate = doc.CreateAttribute("ExpectedOffDate");
                        ExpectedOffDate.Value = "2014-03-08T17:00:00";
                        ArkleMarket.Attributes.Append(ExpectedOffDate);

                        XmlAttribute ResultReceivedDate = doc.CreateAttribute("ResultReceivedDate");
                        ResultReceivedDate.Value = "0001-01-01T00:00:00";
                        ArkleMarket.Attributes.Append(ResultReceivedDate);

                        XmlAttribute MarketDisplayClass = doc.CreateAttribute("MarketDisplayClass");
                        MarketDisplayClass.Value = market;
                        ArkleMarket.Attributes.Append(MarketDisplayClass);

                        XmlAttribute URL = doc.CreateAttribute("URL");
                        URL.Value = "";
                        ArkleMarket.Attributes.Append(URL);

                        XmlAttribute NumbersGameTypeID = doc.CreateAttribute("NumbersGameTypeID");
                        NumbersGameTypeID.Value = "0";
                        ArkleMarket.Attributes.Append(NumbersGameTypeID);

                        XmlAttribute FavouritePriceDecimal = doc.CreateAttribute("FavouritePriceDecimal");
                        FavouritePriceDecimal.Value = "0";
                        ArkleMarket.Attributes.Append(FavouritePriceDecimal);

                        XmlAttribute SectionNumber = doc.CreateAttribute("SectionNumber");
                        SectionNumber.Value = "0";
                        ArkleMarket.Attributes.Append(SectionNumber);

                        XmlAttribute MarketIdentifier = doc.CreateAttribute("MarketIdentifier");
                        MarketIdentifier.Value = identifier+market.Substring(0,1)+"id";
                        ArkleMarket.Attributes.Append(MarketIdentifier);

                        XmlAttribute MaxiumLiability = doc.CreateAttribute("MaxiumLiability");
                        MaxiumLiability.Value = "0";
                        ArkleMarket.Attributes.Append(MaxiumLiability);

                        MarketsCollection.AppendChild(ArkleMarket);


                        XmlNode ewRulesList = doc.CreateElement("ewRulesList");
                        ArkleMarket.AppendChild(ewRulesList);

                        XmlNode Rule4sList = doc.CreateElement("Rule4sList");
                        ArkleMarket.AppendChild(Rule4sList);

                        XmlNode ForecastList = doc.CreateElement("ForecastList");
                        ArkleMarket.AppendChild(ForecastList);

                        XmlNode MarketStatusCollection = doc.CreateElement("MarketStatusCollection");
                        ArkleMarket.AppendChild(MarketStatusCollection);

                        XmlNode SelectionsCollections = doc.CreateElement("SelectionsCollections");
                        ArkleMarket.AppendChild(SelectionsCollections);
                        
                        int i = 1;
                        foreach (DataRow drprice in drChild.GetChildRows("Market_Sel"))
                        {
                            string pricecreated = drprice["createddate"].ToString();
                            string pricefixed = drprice["toals"].ToString();
                            string selection = drprice["selection"].ToString();
                            double price = Helper.FractionToDouble(pricefixed);
                            string pricedec = Convert.ToString(Math.Round(price,2)+1);
                            XmlNode ArkleSelection = doc.CreateElement("ArkleSelection");


                            XmlAttribute TimeCreated = doc.CreateAttribute("TimeCreated");
                            TimeCreated.Value = pricecreated;
                            ArkleSelection.Attributes.Append(TimeCreated);

                            XmlAttribute LastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
                            LastPriceChangeIssued.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(LastPriceChangeIssued);

                            XmlAttribute SelectionName = doc.CreateAttribute("Name");
                            SelectionName.Value = selection;
                            ArkleSelection.Attributes.Append(SelectionName);

                            XmlAttribute NumberInDeadHeat = doc.CreateAttribute("NumberInDeadHeat");
                            NumberInDeadHeat.Value = "0";
                            ArkleSelection.Attributes.Append(NumberInDeadHeat);

                            XmlAttribute CoFavNumber = doc.CreateAttribute("CoFavNumber");
                            CoFavNumber.Value = "0";
                            ArkleSelection.Attributes.Append(CoFavNumber);

                            XmlAttribute DisplayPlayerInScoreCastGridExcel = doc.CreateAttribute("_DisplayPlayerInScoreCastGridExcel");
                            DisplayPlayerInScoreCastGridExcel.Value = "false";
                            ArkleSelection.Attributes.Append(DisplayPlayerInScoreCastGridExcel);

                            XmlAttribute ExistedPreviously = doc.CreateAttribute("_ExistedPreviously");
                            ExistedPreviously.Value = "false";
                            ArkleSelection.Attributes.Append(ExistedPreviously);

                            XmlAttribute TimeSetToNonRunner = doc.CreateAttribute("TimeSetToNonRunner");
                            TimeSetToNonRunner.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(TimeSetToNonRunner);

                            XmlAttribute LastPriceCheck = doc.CreateAttribute("LastPriceCheck");
                            LastPriceCheck.Value = "0001-01-01T00:00:00";
                            ArkleSelection.Attributes.Append(LastPriceCheck);

                            XmlAttribute IncludeInExcel = doc.CreateAttribute("_IncludeInExcel");
                            IncludeInExcel.Value = "true";
                            ArkleSelection.Attributes.Append(IncludeInExcel);

                            XmlAttribute TeamName = doc.CreateAttribute("TeamName");
                            TeamName.Value = "";
                            ArkleSelection.Attributes.Append(TeamName);

                            XmlAttribute hda = doc.CreateAttribute("_hda");
                            hda.Value = "";
                            ArkleSelection.Attributes.Append(hda);

                            XmlAttribute PriceFixed = doc.CreateAttribute("PriceFixed");
                            PriceFixed.Value = pricefixed;
                            ArkleSelection.Attributes.Append(PriceFixed);

                            XmlAttribute DefaultPriceType = doc.CreateAttribute("DefaultPriceType");
                            DefaultPriceType.Value = "F";
                            ArkleSelection.Attributes.Append(DefaultPriceType);

                            XmlAttribute SelectionNumber = doc.CreateAttribute("SelectionNumber");
                            SelectionNumber.Value = "0";
                            ArkleSelection.Attributes.Append(SelectionNumber);

                            XmlAttribute GuaranteeOdds = doc.CreateAttribute("GuaranteeOdds");
                            GuaranteeOdds.Value = "0";
                            ArkleSelection.Attributes.Append(GuaranteeOdds);

                            XmlAttribute SelectionIdentifier = doc.CreateAttribute("SelectionIdentifier");
                            SelectionIdentifier.Value = identifier+market.Substring(0,1)+"S"+i+"id";
                            ArkleSelection.Attributes.Append(SelectionIdentifier);

                            XmlAttribute SelectionPriceBackgroundColor = doc.CreateAttribute("SelectionPriceBackgroundColor");
                            SelectionPriceBackgroundColor.Value = "blueBackground";
                            ArkleSelection.Attributes.Append(SelectionPriceBackgroundColor);

                            XmlAttribute SettleStatusID = doc.CreateAttribute("SettleStatusID");
                            SettleStatusID.Value = "0";
                            ArkleSelection.Attributes.Append(SettleStatusID);

                            XmlAttribute SettleStatus = doc.CreateAttribute("_SettleStatus");
                            SettleStatus.Value = "NotSettled";
                            ArkleSelection.Attributes.Append(SettleStatus);

                            XmlAttribute FinishingPosition = doc.CreateAttribute("FinishingPosition");
                            FinishingPosition.Value = "0";
                            ArkleSelection.Attributes.Append(FinishingPosition);

                            XmlAttribute _FinishingPosition = doc.CreateAttribute("_FinishingPosition");
                            _FinishingPosition.Value = "NotSettled";
                            ArkleSelection.Attributes.Append(_FinishingPosition);

                            SelectionsCollections.AppendChild(ArkleSelection);

                            XmlNode SelectionPriceCollection = doc.CreateElement("SelectionPriceCollection");
                            ArkleSelection.AppendChild(SelectionPriceCollection);

////////

                            XmlNode ArkleSelectionPrice = doc.CreateElement("ArkleSelectionPrice");

                            XmlAttribute PriceDec = doc.CreateAttribute("PriceDec");
                            PriceDec.Value = pricedec;
                            ArkleSelectionPrice.Attributes.Append(PriceDec);

                            XmlAttribute PriceFrac = doc.CreateAttribute("PriceFrac");
                            PriceFrac.Value = pricefixed;
                            ArkleSelectionPrice.Attributes.Append(PriceFrac);

                            XmlAttribute MarketType = doc.CreateAttribute("MarketType");
                            MarketType.Value = "F";
                            ArkleSelectionPrice.Attributes.Append(MarketType);

                            XmlAttribute TimePriceChangeCreated = doc.CreateAttribute("TimePriceChangeCreated");
                            TimePriceChangeCreated.Value = pricecreated;
                            ArkleSelectionPrice.Attributes.Append(TimePriceChangeCreated);

                            XmlAttribute TimePriceIssued = doc.CreateAttribute("TimePriceIssued");
                            TimePriceIssued.Value = pricecreated;
                            ArkleSelectionPrice.Attributes.Append(TimePriceIssued);

                            XmlAttribute SourceType = doc.CreateAttribute("SourceType");
                            SourceType.Value = "0";
                            ArkleSelectionPrice.Attributes.Append(SourceType);

                            XmlAttribute _SourceType = doc.CreateAttribute("_SourceType");
                            _SourceType.Value = "Unknown";
                            ArkleSelectionPrice.Attributes.Append(_SourceType);

                            SelectionPriceCollection.AppendChild(ArkleSelectionPrice);

                            XmlNode BaseTime = doc.CreateElement("BaseTime");
                            BaseTime.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                            SelectionPriceCollection.AppendChild(BaseTime);

                            i++;
                            //<RaceHeaderRotationIndex>0</RaceHeaderRotationIndex> <MarketRequiresResttle>false</MarketRequiresResttle> <DateLastUpdated>0001-01-01T00:00:00</DateLastUpdated>
                        }

                            XmlNode RaceHeaderRotationIndex = doc.CreateElement("RaceHeaderRotationIndex");
                            RaceHeaderRotationIndex.AppendChild(doc.CreateTextNode("0"));
                            SelectionsCollections.AppendChild(RaceHeaderRotationIndex);

                            XmlNode MarketRequiresResttle = doc.CreateElement("MarketRequiresResttle");
                            MarketRequiresResttle.AppendChild(doc.CreateTextNode("false"));
                            SelectionsCollections.AppendChild(MarketRequiresResttle);

                            XmlNode DateLastUpdated = doc.CreateElement("DateLastUpdated");
                            DateLastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
                            SelectionsCollections.AppendChild(DateLastUpdated);


                        }
                    }

                }
            doc.Save(file);

            }

        private void btnxml_Click(object sender, EventArgs e)
        {
             if (treeView1.SelectedNodes.Count.Equals(1))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string FileName = saveFileDialog.FileName;
                    string couponid="";
                    foreach (TreeNode node in treeView1.SelectedNodes)
                    {
                        couponid = node.Name;
                    }
                    //string[] info = node.Split(',');
                    GenerateCoupon coupon = new GenerateCoupon();
                    DataSet ds = coupon.GetCoupons(couponid);
                    GenerateXMLforOdds(FileName,ds);
                    MessageBox.Show("File saved successfully");
                }
                // string node = treeView1.SelectedNode.Name;
                ////string[] info = node.Split(',');
                // GenerateCoupon coupon = new GenerateCoupon();
                // DataSet ds = coupon.GetCoupons(Convert.ToInt64(node));
                // GenerateXMLforOdds(ds);
            }



        }

        private void btnaddmarket_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                long matchid = Convert.ToInt64(txtmatchdate.Name);
                string couponid = lblmatchnotset.Name;
                GenerateCoupon coupon = new GenerateCoupon();
                DataSet ds = coupon.GetAvailableMarkets(matchid, couponid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Form childForm = new AddMarket(matchid, couponid);
                    childForm.MdiParent = this.MdiParent;
                    //childForm.Text = "Window " + childFormNumber++;
                    childForm.Show();
                }
                else
                {
                    MessageBox.Show("All markets are priced");
                }
            }
            else
            {
                MessageBox.Show("Data not available!");
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                //List<string> markets = new List<string>();
                //foreach (Control ctrl in flowLayoutPanel1.Controls)
                //{
                    
                //    if (ctrl.GetType().Equals(typeof(DataGridView)))
                //    {
                //        markets.Add(ctrl.Name);
                //    }
                //}

                Form childForm = new UpdatePrice(lblmatchnotset.Name,Convert.ToInt64(txtmatchdate.Name));
                childForm.MdiParent = this.MdiParent;
                //childForm.Text = "Window " + childFormNumber++;
                childForm.Show();
            }
            else
            {
                MessageBox.Show("Match not found!");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (treeView1.SelectedNodes.Count.Equals(1))
            {
                foreach (TreeNode node in treeView1.SelectedNodes)
                {
                    if (node.Level == 1)
                    {
                        TreeNode mynode = new TreeNode();
                        mynode = node;
                        while (mynode.Parent != null)
                        {
                            mynode = mynode.Parent;
                        }
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                        if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            string FileName = saveFileDialog.FileName;
                            string couponid = "";
                            string sportname = mynode.Text;
                            string sportid = mynode.Name;
                            couponid = node.Name;
                            DataSet ds;
                            //string[] info = node.Split(',');
                            GenerateCoupon coupon = new GenerateCoupon();
                            if(btn.Name.Equals("btnxmlall"))
                            {
                               ds = coupon.GetCoupons(couponid);
                            }
                            else
                            {
                                ds = coupon.GetUpdatedCouponInfo(couponid);
                            }
                            Helper.GenerateXMLforOdds(FileName, ds,sportname,sportid);
                            MessageBox.Show("File saved successfully");
                        }
                        // string node = treeView1.SelectedNode.Name;
                        ////string[] info = node.Split(',');
                        // GenerateCoupon coupon = new GenerateCoupon();
                        // DataSet ds = coupon.GetCoupons(Convert.ToInt64(node));
                        // GenerateXMLforOdds(ds);
                    }
                    else
                    {
                        MessageBox.Show("Please select a coupon to generate XML");
                    }
                }
            }
        }

        public void CreateXMLFile()
        {

            if (treeView1.SelectedNodes.Count.Equals(1))
            {
                foreach (TreeNode node in treeView1.SelectedNodes)
                {
                    if (node.Level == 1)
                    {

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                        if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        {
                            string FileName = saveFileDialog.FileName;
                            string couponid = "";

                            couponid = node.Name;

                            //string[] info = node.Split(',');
                            GenerateCoupon coupon = new GenerateCoupon();
                            DataSet ds = coupon.GetCoupons(couponid);
                            GenerateXMLforOdds(FileName, ds);
                            MessageBox.Show("File saved successfully");
                        }
                        // string node = treeView1.SelectedNode.Name;
                        ////string[] info = node.Split(',');
                        // GenerateCoupon coupon = new GenerateCoupon();
                        // DataSet ds = coupon.GetCoupons(Convert.ToInt64(node));
                        // GenerateXMLforOdds(ds);
                    }
                    else
                    {
                        MessageBox.Show("Please select a coupon to generate XML");
                    }
                }
            }

            else
            {
                MessageBox.Show("Please select one coupon to generate XML");
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            GenerateCoupon coupon = new GenerateCoupon();
            CrawlFirstPage crawl = new CrawlFirstPage();
            try
            {
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
                                long id = Convert.ToInt64(grid.Rows[i].Cells[3].Value);
                                DataGridViewComboBoxCell cell = grid.Rows[i].Cells[4] as DataGridViewComboBoxCell;
                                string result = Convert.ToString(cell.Value);
                                coupon.UpdateCoupon(id, result);
                            }
                        }
                    }
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

        private void addToToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
                GenerateCoupon coupon = new GenerateCoupon();
                string msg = coupon.AddToArchive(couponid);
                LoadTree(Is_Archived);
                MessageBox.Show(msg);
            
        }
        private string couponid;
       

        private void treeView1_MouseUp_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                
                if (node != null && node.Level == 1)
                {
                    couponid = node.Name;
                    
                }
            }
        }

        private void Coupons_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                OddsCheckerCrawler crawl = MdiParent as OddsCheckerCrawler;
                crawl.SetProgress(false);
                crawl.IsCouponProcessRunning = false;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
             List<string> markets = SelectedMarkets();
            string matchid="0";
            foreach (string market in markets)
            {
                string[] info = market.Split(',');
                matchid = info[1];
            }
            frmResultView f = new frmResultView(matchid);
            f.Show();
        }

            //XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //doc.AppendChild(docNode);

            //Main Section Starts

            //XmlNode ArkleCouponNode = doc.CreateElement("ArkleCoupon");
            //XmlAttribute ArkleSportIdAttribute = doc.CreateAttribute("SportsId");
            //ArkleSportIdAttribute.Value = "12";
            //ArkleCouponNode.Attributes.Append(ArkleSportIdAttribute);
            //XmlAttribute ArkleSportAttribute = doc.CreateAttribute("_Sport");
            //ArkleSportAttribute.Value = "GaaFootball";
            //ArkleCouponNode.Attributes.Append(ArkleSportAttribute);
            //XmlAttribute ArkleCouponIdentifier = doc.CreateAttribute("CouponIdentifier");
            //ArkleCouponIdentifier.Value = "Test";
            //ArkleCouponNode.Attributes.Append(ArkleCouponIdentifier);
            //XmlAttribute ArkleCouponName = doc.CreateAttribute("CouponName");
            //ArkleCouponName.Value = "test";
            //ArkleCouponNode.Attributes.Append(ArkleCouponName);
            //XmlAttribute ArkleTimeCreated = doc.CreateAttribute("TimeCreated");
            //ArkleTimeCreated.Value = "2014-03-05T21:15:47";
            //ArkleCouponNode.Attributes.Append(ArkleTimeCreated);
            //XmlAttribute ArkleLastUpdated = doc.CreateAttribute("LastUpdated");
            //ArkleTimeCreated.Value = "2014-03-05T21:15:47";
            //ArkleCouponNode.Attributes.Append(ArkleLastUpdated);
            //XmlAttribute ArkleLastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
            //ArkleTimeCreated.Value = "2014-03-05T21:15:47";
            //ArkleCouponNode.Attributes.Append(ArkleLastPriceChangeIssued);

            //XmlAttribute ArkleMarketBettingTypeID = doc.CreateAttribute("MarketBettingTypeID");
            //ArkleTimeCreated.Value = "Normal";
            //ArkleCouponNode.Attributes.Append(ArkleMarketBettingTypeID);

            //XmlAttribute ArkleDeSerializeStatusID = doc.CreateAttribute("DeSerializeStatusID");
            //ArkleDeSerializeStatusID.Value = "SerializationInProgress";
            //ArkleCouponNode.Attributes.Append(ArkleDeSerializeStatusID);

            ////XmlAttribute ArkleXsd = doc.CreateAttribute("xmlns", "xsd", " ");
            ////ArkleXsd.Value = "http://www.w3.org/2001/XMLSchema";
            ////ArkleCouponNode.Attributes.Append(ArkleXsd);

            ////XmlAttribute ArkleXsi = doc.CreateAttribute("xmlns", "xsi", " ");
            ////ArkleXsi.Value = "http://www.w3.org/2001/XMLSchema-instance";
            ////ArkleCouponNode.Attributes.Append(ArkleXsi);

            //doc.AppendChild(ArkleCouponNode);

            //XmlNode comments = doc.CreateElement("Comments");
            //comments.AppendChild(doc.CreateTextNode("Produced By M Halaburda"));
            //ArkleCouponNode.AppendChild(comments);

            //XmlNode filename = doc.CreateElement("FileName");
            //filename.AppendChild(doc.CreateTextNode("Test"));
            //ArkleCouponNode.AppendChild(filename);

            //Main SectionEnds

            //Event Collection Part Starts

            //XmlNode EventsCollection = doc.CreateElement("EventsCollection");
            //ArkleCouponNode.AppendChild(EventsCollection);
            //XmlNode ArkleEvent = doc.CreateElement("ArkleEvent");

            //XmlAttribute EventSportsId = doc.CreateAttribute("SportsId");
            //EventSportsId.Value = "12";
            //ArkleEvent.Attributes.Append(EventSportsId);

            //XmlAttribute EventSport = doc.CreateAttribute("_Sport");
            //EventSport.Value = "GaaFootball";
            //ArkleEvent.Attributes.Append(EventSport);

            //XmlAttribute EventTimeCreated = doc.CreateAttribute("TimeCreated");
            //EventTimeCreated.Value = "2014-03-05T21:15:47";
            //ArkleEvent.Attributes.Append(EventTimeCreated);

            //XmlAttribute QuadpotDividend = doc.CreateAttribute("QuadpotDividend");
            //QuadpotDividend.Value = "0";
            //ArkleEvent.Attributes.Append(QuadpotDividend);

            //XmlAttribute PlacepotDividend = doc.CreateAttribute("PlacepotDividend");
            //PlacepotDividend.Value = "0";
            //ArkleEvent.Attributes.Append(PlacepotDividend);

            //XmlAttribute JackpotDividend = doc.CreateAttribute("JackpotDividend");
            //JackpotDividend.Value = "0";
            //ArkleEvent.Attributes.Append(JackpotDividend);

            //XmlAttribute EventExpiryDate = doc.CreateAttribute("ExpiryDate");
            //EventExpiryDate.Value = "2014-03-08T23:59:59";
            //ArkleEvent.Attributes.Append(EventExpiryDate);

            //XmlAttribute DisplayEndDateTime = doc.CreateAttribute("DisplayEndDateTime");
            //DisplayEndDateTime.Value = "2014-03-08T17:00:00";
            //ArkleEvent.Attributes.Append(DisplayEndDateTime);

            //XmlAttribute OrderPriority = doc.CreateAttribute("OrderPriority");
            //OrderPriority.Value = "0";
            //ArkleEvent.Attributes.Append(OrderPriority);

            //XmlAttribute Marketclass = doc.CreateAttribute("Marketclass");
            //Marketclass.Value = "";
            //ArkleEvent.Attributes.Append(OrderPriority);

            //XmlAttribute EventIdentifier = doc.CreateAttribute("EventIdentifier");
            //EventIdentifier.Value = "MH.RugbyUnionAEid41706.7083333333ScotlandFrance";
            //ArkleEvent.Attributes.Append(EventIdentifier);

            //XmlAttribute Name = doc.CreateAttribute("Name");
            //Name.Value = "Scotland v France";
            //ArkleEvent.Attributes.Append(Name);

            //XmlAttribute EventSettleStatusType = doc.CreateAttribute("EventSettleStatusType");
            //EventSettleStatusType.Value = "Unknown";
            //ArkleEvent.Attributes.Append(EventSettleStatusType);

            //XmlAttribute SourceURL = doc.CreateAttribute("SourceURL");
            //SourceURL.Value = "";
            //ArkleEvent.Attributes.Append(SourceURL);

            //EventsCollection.AppendChild(ArkleEvent);

            //XmlNode handicap = doc.CreateElement("HandicapMatchBettingMarkets");
            //ArkleEvent.AppendChild(handicap);

            //XmlNode MatchForPrintOut = doc.CreateElement("MatchBettingMarketsForPrintout");
            //ArkleEvent.AppendChild(MatchForPrintOut);

            //XmlNode LastUpdated = doc.CreateElement("LastUpdated");
            //LastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
            //ArkleEvent.AppendChild(LastUpdated);

            ////MarketCollection


            ////+<ArkleMarket Name="Match" ContainsAmendedResult="false" MarkSenseMarketPriority="0" _WordDisplayPriority="0" 
            ////_ScreenDisplayPriority="1" EachWayNumberPlaces="1" EachWayDenominator="1" Archived="false" EachWayAllowed="false" RaceCardNumber="0"
            ////ExpectedOffDateExpired="false" ExpectedOffDateDisplay="Sat 08 Mar 17:00" ExpectedOffDate="2014-03-08T17:00:00" ResultReceivedDate="0001-01-01T00:00:00" 
            ////MarketDisplayClass="Match" URL="" NumbersGameTypeID="0" FavouritePriceDecimal="0" SectionNumber="0" MarketIdentifier="MH.RugbyUnion41706.7083333333ScotlandFranceMid" 
            ////MaxiumLiability="0">

            //XmlNode MarketsCollection = doc.CreateElement("MarketsCollection");
            //ArkleEvent.AppendChild(MarketsCollection);

            //XmlNode ArkleMarket = doc.CreateElement("ArkleMarket");

            //XmlAttribute MarketName = doc.CreateAttribute("Name");
            //MarketName.Value = "Match";
            //ArkleMarket.Attributes.Append(MarketName);

            //XmlAttribute ContainsAmendedResult = doc.CreateAttribute("ContainsAmendedResult");
            //ContainsAmendedResult.Value = "false";
            //ArkleMarket.Attributes.Append(ContainsAmendedResult);

            //XmlAttribute MarkSenseMarketPriority = doc.CreateAttribute("MarkSenseMarketPriority");
            //MarkSenseMarketPriority.Value = "0";
            //ArkleMarket.Attributes.Append(MarkSenseMarketPriority);

            //XmlAttribute WordDisplayPriority = doc.CreateAttribute("_WordDisplayPriority");
            //WordDisplayPriority.Value = "0";
            //ArkleMarket.Attributes.Append(WordDisplayPriority);

            //XmlAttribute ScreenDisplayPriority = doc.CreateAttribute("_ScreenDisplayPriority");
            //ScreenDisplayPriority.Value = "1";
            //ArkleMarket.Attributes.Append(ScreenDisplayPriority);

            //XmlAttribute EachWayNumberPlaces = doc.CreateAttribute("EachWayNumberPlaces");
            //EachWayNumberPlaces.Value = "1";
            //ArkleMarket.Attributes.Append(EachWayNumberPlaces);

            //XmlAttribute EachWayDenominator = doc.CreateAttribute("EachWayDenominator");
            //EachWayDenominator.Value = "1";
            //ArkleMarket.Attributes.Append(EachWayDenominator);

            //XmlAttribute Archived = doc.CreateAttribute("Archived");
            //Archived.Value = "false";
            //ArkleMarket.Attributes.Append(Archived);

            //XmlAttribute EachWayAllowed = doc.CreateAttribute("EachWayAllowed");
            //EachWayAllowed.Value = "false";
            //ArkleMarket.Attributes.Append(EachWayAllowed);

            //XmlAttribute RaceCardNumber = doc.CreateAttribute("RaceCardNumber");
            //RaceCardNumber.Value = "0";
            //ArkleMarket.Attributes.Append(RaceCardNumber);

            //XmlAttribute ExpectedOffDateExpired = doc.CreateAttribute("ExpectedOffDateExpired");
            //ExpectedOffDateExpired.Value = "false";
            //ArkleMarket.Attributes.Append(ExpectedOffDateExpired);

            //XmlAttribute ExpectedOffDateDisplay = doc.CreateAttribute("ExpectedOffDateDisplay");
            //ExpectedOffDateDisplay.Value = "Sat 08 Mar 17:00";
            //ArkleMarket.Attributes.Append(ExpectedOffDateDisplay);

            //XmlAttribute ExpectedOffDate = doc.CreateAttribute("ExpectedOffDate");
            //ExpectedOffDate.Value = "2014-03-08T17:00:00";
            //ArkleMarket.Attributes.Append(ExpectedOffDate);

            //XmlAttribute ResultReceivedDate = doc.CreateAttribute("ResultReceivedDate");
            //ResultReceivedDate.Value = "0001-01-01T00:00:00";
            //ArkleMarket.Attributes.Append(ResultReceivedDate);

            //XmlAttribute MarketDisplayClass = doc.CreateAttribute("MarketDisplayClass");
            //MarketDisplayClass.Value = "Match";
            //ArkleMarket.Attributes.Append(MarketDisplayClass);

            //XmlAttribute URL = doc.CreateAttribute("URL");
            //URL.Value = "";
            //ArkleMarket.Attributes.Append(URL);

            //XmlAttribute NumbersGameTypeID = doc.CreateAttribute("NumbersGameTypeID");
            //NumbersGameTypeID.Value = "0";
            //ArkleMarket.Attributes.Append(NumbersGameTypeID);

            //XmlAttribute FavouritePriceDecimal = doc.CreateAttribute("FavouritePriceDecimal");
            //FavouritePriceDecimal.Value = "0";
            //ArkleMarket.Attributes.Append(FavouritePriceDecimal);

            //XmlAttribute SectionNumber = doc.CreateAttribute("SectionNumber");
            //SectionNumber.Value = "0";
            //ArkleMarket.Attributes.Append(SectionNumber);

            //XmlAttribute MarketIdentifier = doc.CreateAttribute("MarketIdentifier");
            //MarketIdentifier.Value = "MH.RugbyUnion41706.7083333333ScotlandFranceMid";
            //ArkleMarket.Attributes.Append(MarketIdentifier);

            //XmlAttribute MaxiumLiability = doc.CreateAttribute("MaxiumLiability");
            //MaxiumLiability.Value = "0";
            //ArkleMarket.Attributes.Append(MaxiumLiability);

            //MarketsCollection.AppendChild(ArkleMarket);


            //XmlNode ewRulesList = doc.CreateElement("ewRulesList");
            //ArkleMarket.AppendChild(ewRulesList);

            //XmlNode Rule4sList = doc.CreateElement("Rule4sList");
            //ArkleMarket.AppendChild(Rule4sList);

            //XmlNode ForecastList = doc.CreateElement("ForecastList");
            //ArkleMarket.AppendChild(ForecastList);

            //XmlNode MarketStatusCollection = doc.CreateElement("MarketStatusCollection");
            //ArkleMarket.AppendChild(MarketStatusCollection);

            //XmlNode SelectionsCollections = doc.CreateElement("SelectionsCollections");
            //ArkleMarket.AppendChild(SelectionsCollections);


            ////Arkle Selection 

            //XmlNode ArkleSelection = doc.CreateElement("ArkleSelection");


            //XmlAttribute TimeCreated = doc.CreateAttribute("TimeCreated");
            //TimeCreated.Value = "2014-03-05T21:15:47";
            //ArkleSelection.Attributes.Append(TimeCreated);

            //XmlAttribute LastPriceChangeIssued = doc.CreateAttribute("LastPriceChangeIssued");
            //LastPriceChangeIssued.Value = "0001-01-01T00:00:00";
            //ArkleSelection.Attributes.Append(LastPriceChangeIssued);

            //XmlAttribute SelectionName = doc.CreateAttribute("Name");
            //SelectionName.Value = "Scotland";
            //ArkleSelection.Attributes.Append(SelectionName);

            //XmlAttribute NumberInDeadHeat = doc.CreateAttribute("NumberInDeadHeat");
            //NumberInDeadHeat.Value = "0";
            //ArkleSelection.Attributes.Append(NumberInDeadHeat);

            //XmlAttribute CoFavNumber = doc.CreateAttribute("CoFavNumber");
            //CoFavNumber.Value = "0";
            //ArkleSelection.Attributes.Append(CoFavNumber);

            //XmlAttribute DisplayPlayerInScoreCastGridExcel = doc.CreateAttribute("_DisplayPlayerInScoreCastGridExcel");
            //DisplayPlayerInScoreCastGridExcel.Value = "false";
            //ArkleSelection.Attributes.Append(DisplayPlayerInScoreCastGridExcel);

            //XmlAttribute ExistedPreviously = doc.CreateAttribute("_ExistedPreviously");
            //ExistedPreviously.Value = "false";
            //ArkleSelection.Attributes.Append(ExistedPreviously);

            //XmlAttribute TimeSetToNonRunner = doc.CreateAttribute("TimeSetToNonRunner");
            //TimeSetToNonRunner.Value = "0001-01-01T00:00:00";
            //ArkleSelection.Attributes.Append(TimeSetToNonRunner);

            //XmlAttribute LastPriceCheck = doc.CreateAttribute("LastPriceCheck");
            //LastPriceCheck.Value = "0001-01-01T00:00:00";
            //ArkleSelection.Attributes.Append(LastPriceCheck);

            //XmlAttribute IncludeInExcel = doc.CreateAttribute("_IncludeInExcel");
            //IncludeInExcel.Value = "true";
            //ArkleSelection.Attributes.Append(IncludeInExcel);

            //XmlAttribute TeamName = doc.CreateAttribute("TeamName");
            //TeamName.Value = "Scotland";
            //ArkleSelection.Attributes.Append(TeamName);

            //XmlAttribute hda = doc.CreateAttribute("_hda");
            //hda.Value = "home";
            //ArkleSelection.Attributes.Append(hda);

            //XmlAttribute PriceFixed = doc.CreateAttribute("PriceFixed");
            //PriceFixed.Value = "5/2";
            //ArkleSelection.Attributes.Append(PriceFixed);

            //XmlAttribute DefaultPriceType = doc.CreateAttribute("DefaultPriceType");
            //DefaultPriceType.Value = "F";
            //ArkleSelection.Attributes.Append(DefaultPriceType);

            //XmlAttribute SelectionNumber = doc.CreateAttribute("SelectionNumber");
            //SelectionNumber.Value = "0";
            //ArkleSelection.Attributes.Append(SelectionNumber);

            //XmlAttribute GuaranteeOdds = doc.CreateAttribute("GuaranteeOdds");
            //GuaranteeOdds.Value = "0";
            //ArkleSelection.Attributes.Append(GuaranteeOdds);

            //XmlAttribute SelectionIdentifier = doc.CreateAttribute("SelectionIdentifier");
            //SelectionIdentifier.Value = "MH.RugbyUnion41706.7083333333ScotlandFranceMS1id";
            //ArkleSelection.Attributes.Append(SelectionIdentifier);

            //XmlAttribute SelectionPriceBackgroundColor = doc.CreateAttribute("SelectionPriceBackgroundColor");
            //SelectionPriceBackgroundColor.Value = "blueBackground";
            //ArkleSelection.Attributes.Append(SelectionPriceBackgroundColor);

            //XmlAttribute SettleStatusID = doc.CreateAttribute("SettleStatusID");
            //SettleStatusID.Value = "0";
            //ArkleSelection.Attributes.Append(SettleStatusID);

            //XmlAttribute SettleStatus = doc.CreateAttribute("_SettleStatus");
            //SettleStatus.Value = "NotSettled";
            //ArkleSelection.Attributes.Append(SettleStatus);

            //XmlAttribute FinishingPosition = doc.CreateAttribute("FinishingPosition");
            //FinishingPosition.Value = "0";
            //ArkleSelection.Attributes.Append(FinishingPosition);

            //XmlAttribute _FinishingPosition = doc.CreateAttribute("_FinishingPosition");
            //_FinishingPosition.Value = "NotSettled";
            //ArkleSelection.Attributes.Append(_FinishingPosition);

            //SelectionsCollections.AppendChild(ArkleSelection);

            //XmlNode SelectionPriceCollection = doc.CreateElement("SelectionPriceCollection");
            //ArkleSelection.AppendChild(SelectionPriceCollection);

            ////SelectionPriceCollection


            //XmlNode ArkleSelectionPrice = doc.CreateElement("ArkleSelectionPrice");

            //XmlAttribute PriceDec = doc.CreateAttribute("PriceDec");
            //PriceDec.Value = "3.5";
            //ArkleSelectionPrice.Attributes.Append(PriceDec);

            //XmlAttribute PriceFrac = doc.CreateAttribute("PriceFrac");
            //PriceFrac.Value = "5/2";
            //ArkleSelectionPrice.Attributes.Append(PriceFrac);

            //XmlAttribute MarketType = doc.CreateAttribute("MarketType");
            //MarketType.Value = "F";
            //ArkleSelectionPrice.Attributes.Append(MarketType);

            //XmlAttribute TimePriceChangeCreated = doc.CreateAttribute("TimePriceChangeCreated");
            //TimePriceChangeCreated.Value = "2014-03-05T21:15:47";
            //ArkleSelectionPrice.Attributes.Append(TimePriceChangeCreated);

            //XmlAttribute TimePriceIssued = doc.CreateAttribute("TimePriceIssued");
            //TimePriceIssued.Value = "2014-03-05T21:15:47";
            //ArkleSelectionPrice.Attributes.Append(TimePriceIssued);

            //XmlAttribute SourceType = doc.CreateAttribute("SourceType");
            //SourceType.Value = "0";
            //ArkleSelectionPrice.Attributes.Append(SourceType);

            //XmlAttribute _SourceType = doc.CreateAttribute("_SourceType");
            //_SourceType.Value = "Unknown";
            //ArkleSelectionPrice.Attributes.Append(_SourceType);

            //SelectionPriceCollection.AppendChild(ArkleSelectionPrice);

            //XmlNode BaseTime = doc.CreateElement("BaseTime");
            //BaseTime.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
            //SelectionPriceCollection.AppendChild(BaseTime);


            ////<RaceHeaderRotationIndex>0</RaceHeaderRotationIndex> <MarketRequiresResttle>false</MarketRequiresResttle> <DateLastUpdated>0001-01-01T00:00:00</DateLastUpdated>


            //XmlNode RaceHeaderRotationIndex = doc.CreateElement("RaceHeaderRotationIndex");
            //RaceHeaderRotationIndex.AppendChild(doc.CreateTextNode("0"));
            //SelectionsCollections.AppendChild(RaceHeaderRotationIndex);

            //XmlNode MarketRequiresResttle = doc.CreateElement("MarketRequiresResttle");
            //MarketRequiresResttle.AppendChild(doc.CreateTextNode("false"));
            //SelectionsCollections.AppendChild(MarketRequiresResttle);

            //XmlNode DateLastUpdated = doc.CreateElement("DateLastUpdated");
            //DateLastUpdated.AppendChild(doc.CreateTextNode("0001-01-01T00:00:00"));
            //SelectionsCollections.AppendChild(DateLastUpdated);


           
       

       

      

      

    }
}
