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

namespace OddsCheckerCrawler
{
    public partial class BettingMarkets : Form
    {
        public BettingMarkets()
        {
            InitializeComponent();
        }

        public BettingMarkets(long matchid,string match)
        {
            InitializeComponent();
            BindGrid(matchid);
            groupBox1.Text = match;
        }

        protected void BindGrid(long matchid)
        {
            BindingSource bindingSource1 = new BindingSource();
            dataGridMarkets.AutoGenerateColumns = false;
            dataGridMarkets.ColumnCount = 4;

            dataGridMarkets.Columns[0].Name = "ID";
            dataGridMarkets.Columns[0].DataPropertyName = "id";
            dataGridMarkets.Columns[0].Visible = false;
            dataGridMarkets.Columns[1].Name = "MatchID";
            dataGridMarkets.Columns[1].DataPropertyName = "matchid";
            dataGridMarkets.Columns[1].Visible = false;
            dataGridMarkets.Columns[2].Name = "Markets";
            dataGridMarkets.Columns[2].DataPropertyName = "bettingmarket";
            dataGridMarkets.Columns[3].Name = "BettingLink";
            dataGridMarkets.Columns[3].DataPropertyName = "bettinglink";
            dataGridMarkets.Columns[3].Visible = false;
            var buttonCol = new DataGridViewButtonColumn();
            buttonCol.UseColumnTextForButtonValue = true;
            buttonCol.Name = "Odds";
            buttonCol.Text = "View Odds";

            dataGridMarkets.Columns.Add(buttonCol);
            CrawlAllMarkets crawl = new CrawlAllMarkets();
            dataGridMarkets.DataSource = crawl.GetBettingMarkets(matchid);

            //foreach (DataGridViewColumn column in dataGridMarkets.Columns)
            //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //this.Width = dataGridMarkets.Width + 100;

            //int totalRowHeight = dataGridMarkets.ColumnHeadersHeight;

            //foreach (DataGridViewRow row in dataGridMarkets.Rows)
            //    totalRowHeight += row.Height;

            //dataGridMarkets.Height = totalRowHeight+10;
            //this.Height = dataGridMarkets.Height + 100;
        }

        private void dataGridMarkets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridMarkets.Columns[e.ColumnIndex].Name == "Odds")
                {
                    if (!e.RowIndex.ToString().Equals("-1"))
                    {
                        long id = Convert.ToInt64(dataGridMarkets.Rows[e.RowIndex].Cells[0].Value);
                        long matchid = Convert.ToInt64(dataGridMarkets.Rows[e.RowIndex].Cells[1].Value);
                        string match = groupBox1.Text;
                        string link = Convert.ToString(dataGridMarkets.Rows[e.RowIndex].Cells[3].Value);

                        Form childForm = new MarketOdds(link, id, matchid,match);
                        childForm.MdiParent = this.ParentForm;
                        //childForm.Text = "Window " + childFormNumber++;
                        childForm.Show();
                        //MessageBox.Show(id);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



    }
}
