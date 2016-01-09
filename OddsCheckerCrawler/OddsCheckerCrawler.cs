using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using OddsBusiness;
using System.Threading;
using BackCrawler;
using System.Data.SqlClient;

//using  OddsCheckerService;

namespace OddsCheckerCrawler
{
    public partial class OddsCheckerCrawler : Form
    {
        System.Threading.Timer time;
        private int childFormNumber = 0;

        private List<string> markets;
        private List<string> addmarkets;
        private List<string> updatemarkets;
        public bool IsProcessRunning = false;
        public bool IsCouponProcessRunning = false;
        public bool IsCrawlProcessRunning = false;



        public OddsCheckerCrawler()
        {
            InitializeComponent();
            if (isDataBaseConnected())
            {
                CreateBookiesMenu();
                CreateSportMenu();
            }
             else
            {
                MessageBox.Show("DataBase not connected, Please connect to database.");
            }
            // loadData();
        }
        private bool isDataBaseConnected()
        {
            try
            {
                using (SqlConnection objConn = new SqlConnection(OddsConnection2.GetConnectionString()))
                {
                    if (objConn.State == ConnectionState.Closed)
                    {
                        objConn.Open();
                    }
                    if (objConn.State == ConnectionState.Open)
                    {
                        objConn.Close();
                        return true;
                    }
                        else return false;
                }
            }
            catch(SqlException ex)
            {
                return false;
            }
        }
        private void loadData()
        {
            string url = "http://www.oddschecker.com/football-betting";
            //string html = Helper.GetWebSiteContent("http://www.oddschecker.com/gaelic-games/gaelic-football/"); //(ConfigurationSettings.AppSettings["URL"]);
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            CrawlFirstPage crawlpage = new CrawlFirstPage();
            CrawlEachMarket crawlmarket = new CrawlEachMarket();
            //string msg = crawlpage.DeleteFirstPageRecords();
            //Task taskA = Task.Factory.StartNew(() =>
            //{
            //    string msg1 = crawlpage.CrawlEachSport();
            //}, TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness);
            string msg2 = crawl.CrawlBettingLinks();
            //crawlpage.CrawlWorldMarkets();
            //crawlpage.CrawlLeagues(url);
        }

        private void CreateBookiesMenu()
        {
            CrawlEachMarket crawl = new CrawlEachMarket();
            DataTable dt = crawl.GetBookieNames();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ToolStripMenuItem market = new ToolStripMenuItem();
                    market.Text = row[0].ToString();
                    market.Checked = Convert.ToBoolean(row[1]);
                    market.CheckOnClick = true;
                    //market.CheckState = System.Windows.Forms.CheckState.Checked;
                    market.Click += new System.EventHandler(this.marketToolStripMenuItem_Click);
                    this.selectToolStripMenuItem.DropDownItems.Add(market);
                }
                ToolStripMenuItem selectall = new ToolStripMenuItem();
                selectall.Text = "Select All";
                selectall.Checked = CheckForSelectAll();
                selectall.CheckOnClick = true;
                //selectall.CheckState = System.Windows.Forms.CheckState.Checked;
                this.selectToolStripMenuItem.DropDownItems.Add(selectall);
                selectall.Click += new System.EventHandler(this.selectallToolStripMenuItem_Click);
                // this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            }
        }
        private void ShowNewForm(object sender, EventArgs e)
        {
            //Form childForm = new Form();
            //childForm.MdiParent = this;
            //childForm.Text = "Window " + childFormNumber++;
            //childForm.Show();
        }

        private void selectallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrawlEachMarket crawl = new CrawlEachMarket();
            foreach (ToolStripMenuItem item in selectToolStripMenuItem.DropDownItems)
            {
                item.Checked = (sender as ToolStripMenuItem).Checked;
                crawl.UpdateBookiesMenu(item.Checked, item.Text);
            }
        }

        private void marketToolStripMenuItem_Click(object sender, EventArgs e)
        {

            (selectToolStripMenuItem.DropDownItems[selectToolStripMenuItem.DropDownItems.Count - 1] as ToolStripMenuItem).Checked = CheckForSelectAll();
            CrawlEachMarket crawl = new CrawlEachMarket();
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            crawl.UpdateBookiesMenu(item.Checked, item.Text);

        }

        private bool CheckForSelectAll()
        {
            for (int i = 0; i < selectToolStripMenuItem.DropDownItems.Count; i++)
            {
                if (!(selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Checked)
                {
                    if (i == selectToolStripMenuItem.DropDownItems.Count - 1)
                    {
                        return true;
                        //(selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Checked = true;
                    }
                    else
                    {
                        return false;
                        //(selectToolStripMenuItem.DropDownItems[selectToolStripMenuItem.DropDownItems.Count - 1] as ToolStripMenuItem).Checked = false;
                        //break;
                    }
                }
            }
            return true;
        }


        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form childForm = new MatchesSchedule();
            childForm.MdiParent = this;
            //childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }
        private void ShowSchedule()
        {
            try
            {

                if (InvokeRequired)
                {

                    Action a = () =>
                    {
                        SetProgress(true);

                    };
                }
            }
            catch (Exception ex)
            {
            }
        }


        public string SelectedBookies()
        {
            string bookies = String.Empty;
            int count = this.selectToolStripMenuItem.DropDownItems.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if ((selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Checked)
                    {
                        if (!(selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Text.Equals("Select All"))
                        {
                            if (bookies == String.Empty)
                            {
                                bookies += "[" + (selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Text + "]";
                            }
                            else
                            {
                                bookies += ",[" + (selectToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem).Text + "]";
                            }
                        }
                    }
                }
            }
            return bookies;
        }
        private void MyThread()
        {
            try
            {
                OddsCrawler crawl = ActiveMdiChild as OddsCrawler;

                string bookies = SelectedBookies();

                foreach (string market in markets)
                {
                    string[] info = market.Split(',');
                    if (info.Length > 1)
                    {

                        Task.Factory.StartNew(() =>
                        {
                            crawl.FillData(info[2], Convert.ToInt64(info[0]), Convert.ToInt64(info[1]), bookies, markets.Count);


                        }, TaskCreationOptions.LongRunning);

                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void AddMarketThread()
        {
            try
            {
                AddMarket avail_market = ActiveMdiChild as AddMarket;

                string bookies = SelectedBookies();

                foreach (string market in addmarkets)
                {
                    string[] info1 = market.Split(',');
                    if (info1.Length > 1)
                    {

                        Task.Factory.StartNew(() =>
                        {
                            avail_market.FillData(info1[2], Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]), bookies, addmarkets.Count);
                        }, TaskCreationOptions.LongRunning);
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void UpdateMarketThread()
        {
            try
            {
                UpdatePrice priced_market = ActiveMdiChild as UpdatePrice;

                string bookies = SelectedBookies();

                foreach (string market in updatemarkets)
                {
                    string[] info1 = market.Split(',');
                    if (info1.Length > 1)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            priced_market.FillData(info1[2], Convert.ToInt64(info1[0]), Convert.ToInt64(info1[1]), updatemarkets.Count, info1[3]);
                        }, TaskCreationOptions.LongRunning);
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedBookies()))
                {
                    if (ActiveMdiChild is OddsCrawler)
                    {


                        if (IsProcessRunning)
                        {
                            MessageBox.Show("A process is already running");
                            return;
                        }

                        OddsCrawler crawl = ActiveMdiChild as OddsCrawler;

                        markets = crawl.SelectedMarkets();

                        if (markets.Count > 0)
                        {
                            crawl.ClearFlowLayoutPanel();
                            SetProgress(true);
                            IsProcessRunning = true;
                            crawl.countthread = 0;
                            string[] sportid = markets[0].Split(',');
                            if (sportid.Length > 1)
                                crawl.SetLabelSportIdText(sportid[3]);
                            //string bookies = SelectedBookies();
                            //foreach (string market in markets)
                            //{
                            //string[] info = market.Split(',');

                            //Task taskB = Task.Factory.StartNew(() =>
                            //{
                            //     crawl.FillData(info[2], Convert.ToInt64(info[0]), Convert.ToInt64(info[1]), bookies, markets.Count);
                            Thread thread = new Thread(MyThread);
                            thread.Start();
                            //});
                        }

                        //}



                    }

                    else
                        if (ActiveMdiChild is AddMarket)
                        {
                            if (IsProcessRunning)
                            {
                                MessageBox.Show("A process is already running");
                                return;
                            }

                            AddMarket crawl1 = ActiveMdiChild as AddMarket;
                            addmarkets = crawl1.SelectedMarkets();
                            if (addmarkets.Count > 0)
                            {
                                //List<string> markets = crawl.SelectedMarkets();
                                crawl1.ClearFlowLayoutPanel();
                                SetProgress(true);
                                IsCouponProcessRunning = true;
                                crawl1.countthread = 0;

                                Thread thread = new Thread(AddMarketThread);
                                thread.Start();
                            }
                        }

                        else
                            if (ActiveMdiChild is Coupons)
                            {
                                if (IsCouponProcessRunning)
                                {
                                    MessageBox.Show("A process is already running");
                                    return;
                                }
                                Coupons coupon = ActiveMdiChild as Coupons;
                                if (coupon.SelectedMarkets().Count > 0)
                                {
                                    List<string> markets = coupon.SelectedMarkets();
                                    coupon.ClearFlowLayoutPanel();
                                    SetProgress(true);
                                    IsCouponProcessRunning = true;
                                    coupon.countthread = 0;
                                    Task taskB = Task.Factory.StartNew(() =>
                                   {
                                       foreach (string market in markets)
                                       {
                                           string[] info = market.Split(',');
                                           coupon.FillData(info[2], Convert.ToInt64(info[0]), Convert.ToInt64(info[1]), info[3], markets.Count);
                                       }
                                   });
                                }
                                else
                                {
                                    MessageBox.Show("Please select a coupon");
                                }
                            }

                            else
                                if (ActiveMdiChild is UpdatePrice)
                                {
                                    if (IsCouponProcessRunning)
                                    {
                                        MessageBox.Show("A process is already running");
                                        return;
                                    }

                                    UpdatePrice update = ActiveMdiChild as UpdatePrice;
                                    updatemarkets = update.SelectedMarkets();
                                    if (updatemarkets.Count > 0)
                                    {
                                        //List<string> markets = crawl.SelectedMarkets();
                                        update.ClearFlowLayoutPanel();
                                        SetProgress(true);
                                        IsCouponProcessRunning = true;
                                        update.countthread = 0;

                                        Thread thread = new Thread(UpdateMarketThread);
                                        thread.Start();
                                        //foreach (string market in markets)
                                        //{
                                        //    string[] info = market.Split(',');

                                        //    Task taskB = Task.Factory.StartNew(() =>
                                        //    {
                                        //        crawl.FillData(info[2], Convert.ToInt64(info[0]), Convert.ToInt64(info[1]), SelectedBookies(), markets.Count);
                                        //    });


                                        //}

                                    }

                                }

                                else
                                {
                                    MessageBox.Show("Please select atleast one market");
                                }

                }
                else
                {
                    MessageBox.Show("Please select atleast one bookie from Select Bookies menu");
                }

            }
            catch (Exception ex)
            {
                IsProcessRunning = false;
                SetProgress(false);
                IsCouponProcessRunning = false;
            }
        }

        public void SetProgress(bool isIndeterminate)
        {
            try
            {
                if (isIndeterminate)
                {
                    toolStripStatusLabel.Visible = false;
                    toolStripProgressBar1.Visible = true;
                    btnstop.Visible = true;
                    toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    toolStripStatusLabel.Visible = true;
                    toolStripProgressBar1.Visible = false;
                    btnstop.Visible = false;
                    toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                }

            }
            catch (Exception ex)
            {
                IsProcessRunning = false;
                IsCouponProcessRunning = false;
                toolStripStatusLabel.Visible = true;
                toolStripProgressBar1.Visible = false;
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            }
        }



        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            bool IsArchived = true;
            Form childForm = new Coupons(!IsArchived);
            childForm.MdiParent = this;

            //childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bool IsArchived = true;
            Form childForm = new Coupons(!IsArchived);
            childForm.MdiParent = this;

            //childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is Coupons)
            {
                Coupons coupon = ActiveMdiChild as Coupons;
                coupon.CreateXMLFile();
            }
            else
            {
                MessageBox.Show("Please select a coupon to generate xml file");
            }
        }



        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            IsProcessRunning = false;
            IsCouponProcessRunning = false;
            SetProgress(false);

        }

        public void UncheckMenuItem(string name)
        {
            foreach (ToolStripMenuItem item in MatchesStripMenuItem.DropDownItems)
            {

                if (item.Name.Equals(name))
                {
                    item.Checked = false;
                }
            }
        }

        private void soccerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem sel_item = sender as ToolStripMenuItem;
            if (sel_item.Checked)
            {
                //foreach (ToolStripMenuItem item in selectToolStripMenuItem.DropDownItems)
                //{
                //    if (!item.Text.Equals(sel_item.Text))
                //        item.Checked = false;

                //    else
                //        item.Checked = true;

                //}

                if (IsProcessRunning)
                {
                    MessageBox.Show("A process is already running");
                    sel_item.Checked = false;
                    return;
                }
                else
                {
                    if (sel_item.Name == "Golf")
                    {
                        Form childForm = new GolfCrawler(sel_item.Name);
                        childForm.Name = sel_item.Name;
                        childForm.MdiParent = this;
                        childForm.Show();
                    }
                    else
                    {
                        Form childForm = new OddsCrawler(sel_item.Name);
                        childForm.Name = sel_item.Name;
                        childForm.MdiParent = this;
                        childForm.Show();
                    }
                }
            }
            else
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name.Equals(sel_item.Name))
                    {
                        form.Close();
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool IsArchived = true;
            Form childForm = new Coupons(IsArchived);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void OddsCheckerCrawler_FormClosing(object sender, FormClosingEventArgs e)
        {

            DialogResult result = MessageBox.Show("Do you really want to exit?", "Exit Odds Crawler", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (!result.Equals(DialogResult.OK))
            {
                e.Cancel = true;
            }
        }

        private void championLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form champion = new ChampLeagueResult();
            champion.MdiParent = this;
            champion.Show();
            //champion.IsMdiChild = this;
            //childForm.Text = "Window " + childFormNumber++;
        }

        private void startCrawlingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // OddsCheckerService.OddsCheckerService d = new OddsCheckerService.OddsCheckerService();
        }

        private void CreateSportMenu()
        {
            CrawlFirstPage crawl = new CrawlFirstPage();
            DataTable dt = crawl.GetSports();
            if (dt.Rows.Count > 0)
            {
                ToolStripMenuItem market;
                market = new ToolStripMenuItem();
                market.Text = "League";
                market.Name = "League";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);

                market = new ToolStripMenuItem();
                market.Text = "WorldMarket";
                market.Name = "WorldMarket";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);

                foreach (DataRow row in dt.Rows)
                {
                    market = new ToolStripMenuItem();
                    market.Text = row["SportName"].ToString();
                    market.Name = row["SportID"].ToString();
                    //market.CheckState = System.Windows.Forms.CheckState.Checked;
                    market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                    this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);
                  
                    market = new ToolStripMenuItem();
                    market.Text = row["SportName"].ToString();
                    market.Name = row["SportID"].ToString();
                    market.CheckOnClick = true;
                    //market.CheckState = System.Windows.Forms.CheckState.Checked;
                    market.Click += new System.EventHandler(this.sportviewToolStripMenuItem_Click);
                    this.sportsToolStripMenuItem.DropDownItems.Add(market);
                }
                market = new ToolStripMenuItem();
                market.Text = "GolfMarketName";
                market.Name = "GolfMarketName";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);

                market = new ToolStripMenuItem();
                market.Text = "GolfMarkets";
                market.Name = "GolfMarkets";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);

                market = new ToolStripMenuItem();
                market.Text = "BettingMarket";
                market.Name = "BettingMarket";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);

                market = new ToolStripMenuItem();
                market.Text = "MarketResult";
                market.Name = "MarketResult";
                market.Click += new System.EventHandler(this.crawlerToolStripMenuItem_Click);
                this.startCrawlingToolStripMenuItem.DropDownItems.Add(market);
            }
        }

        private void crawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Start Crawling.
            //  time = new System.Threading.Timer(startcrawling());
            //BackCrawler.GetCrawler d = new GetCrawler();
            //d.startCrawling();

            //      foreach (ToolStripMenuItem item in startCrawlingToolStripMenuItem.DropDown.Items)
            //   {
            // item.Text = (sender as ToolStripMenuItem).Text;
            if (IsCrawlProcessRunning == true)
            {
                SetProgress(false);
                IsCrawlProcessRunning = false;
            }
            SetProgress(true);
            IsCrawlProcessRunning = true;
            CrawlFirstPage crawldata = new CrawlFirstPage();

            if ((sender as ToolStripMenuItem).Text == "League")
            {
                DataTable dt = crawldata.GetSports("2");
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                if (sportid == 2)
                    crawldata.CrawlLeagues(link);
            }
            else if ((sender as ToolStripMenuItem).Text == "WorldMarket")
            {
                DataTable dt = crawldata.GetSports("2");
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                if (sportid == 2)
                    crawldata.CrawlWorldMarkets();
            }
            else if ((sender as ToolStripMenuItem).Text == "Soccer")
            {
                DataTable dt = crawldata.GetSports((sender as ToolStripMenuItem).Name.ToString());
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                // soocer
                //   crawldata.CrawlLeagues(link);
                //   crawldata.CrawlWorldMarkets();
                DataSet ds = crawldata.GetLeague();
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    string matchlink = Convert.ToString(ds.Tables[0].Rows[j]["link"]);
                    int sport_id = Convert.ToInt32(ds.Tables[0].Rows[j]["sportid"]);
                    long leagueid = Convert.ToInt32(ds.Tables[0].Rows[j]["leagueid"]);
                    ThreadParameters t = new ThreadParameters();
                    t.URL = matchlink;
                    t.SportID = sport_id;
                    t.LeagueID = leagueid;
                    crawldata.CrawlMyPage(t);
                }
            }
            else if ((sender as ToolStripMenuItem).Text == "GaaFootball")
            {
                DataTable dt = crawldata.GetSports((sender as ToolStripMenuItem).Name.ToString());
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                // GAA Football
                ThreadParameters tp = new ThreadParameters();
                tp.URL = link;
                tp.SportID = sportid;
                if (sportid == 12)
                {
                    crawldata.CrawlMyPage(tp);
                }
            }
            else if ((sender as ToolStripMenuItem).Text == "GaaHurling")
            {
                DataTable dt = crawldata.GetSports((sender as ToolStripMenuItem).Name.ToString());
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                ThreadParameters tp = new ThreadParameters();
                tp.URL = link;
                tp.SportID = sportid;
                // GAA Hurling.
                if (sportid == 28)
                {
                    crawldata.CrawlMyPage(tp);
                }
            }
            else if ((sender as ToolStripMenuItem).Text == "Golf")
            {
                DataTable dt = crawldata.GetSports((sender as ToolStripMenuItem).Name.ToString());
                string link = dt.Rows[0]["link"].ToString();
                int sportid = Convert.ToInt32(dt.Rows[0]["sportid"].ToString());
                // Golf
                if (sportid == 29)
                {
                    crawldata.CrawlGolfTurnament(link, sportid.ToString());
                }
            }
            else if ((sender as ToolStripMenuItem).Text == "GolfMarketName")
            {
                crawldata.CrawlGolfBettingMarketName();
            }
            else if ((sender as ToolStripMenuItem).Text == "GolfMarkets")
            {
                crawldata.CrawlGolfBettingMarket();
            }
            else if ((sender as ToolStripMenuItem).Text == "BettingMarket")
            {
                CrawlAllMarkets crawl = new CrawlAllMarkets();
                crawl.CrawlBettingLinks();
            }
            else if ((sender as ToolStripMenuItem).Text == "MarketResult")
            {
                crawldata.CrawlEachMatchResult();
            }
            //  Thread d = new Thread();
            SetProgress(false);
            IsCrawlProcessRunning = false;
            //   }
        }
        private void sportviewToolStripMenuItem_Click(object sender,EventArgs e)
        {
           ToolStripMenuItem sel_item = sender as ToolStripMenuItem;
            if (sel_item.Checked)
            {

                if (IsProcessRunning)
                {
                    MessageBox.Show("A process is already running");
                    sel_item.Checked = false;
                    return;
                }
                else
                {
                  
                        Form childForm = new CouponManage(sel_item.Name,sel_item.Text);
                        childForm.Name = sel_item.Name;
                        childForm.MdiParent = this;
                        childForm.Show();
                }
            }
            else
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name.Equals(sel_item.Name))
                    {
                        form.Close();
                    }
                }
            }
        }
        void startcrawling()
        {
        }
        private void stopCrawlingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void golfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem sel_item = sender as ToolStripMenuItem;
            if (sel_item.Checked)
            {
                //foreach (ToolStripMenuItem item in selectToolStripMenuItem.DropDownItems)
                //{
                //    if (!item.Text.Equals(sel_item.Text))
                //        item.Checked = false;

                //    else
                //        item.Checked = true;

                //}

                if (IsProcessRunning)
                {
                    MessageBox.Show("A process is already running");
                    sel_item.Checked = false;
                    return;
                }
                else
                {
                    if (sel_item.Name == "Golf")
                    {
                        Form childForm = new GolfCrawler(sel_item.Name);
                        childForm.Name = sel_item.Name;
                        childForm.MdiParent = this;
                        childForm.Show();
                    }
                    else
                    {
                        Form childForm = new OddsCrawler(sel_item.Name);
                        childForm.Name = sel_item.Name;
                        childForm.MdiParent = this;
                        childForm.Show();
                    }
                }
            }
            else
            {
                foreach (Form form in this.MdiChildren)
                {
                    if (form.Name.Equals(sel_item.Name))
                    {
                        form.Close();
                    }
                }
            }
        }

      

        private void archivedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            crawl.CrawlBettingLinksByCoupon(true);
        }

        private void notarchivedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            crawl.CrawlBettingLinksByCoupon(false);
        }

        private void toolStripCrawlingServiceStop_Click(object sender, EventArgs e)
        {
          //  MyService.StopService("OddsCheckerService", 6000);
        }

        private void toolStripCrawlingServiceStart_Click(object sender, EventArgs e)
        {
         //  MyService.StartService("OddsCheckerService", 6000);
        }

        private void reStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
      // MyService.RestartService("OddsCheckerService", 6000);
        }
    }
}
