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
    public partial class MarketOdds : Form
    {
        public MarketOdds()
        {
            InitializeComponent();
        }

        public MarketOdds(string link, long id, long matchid, string title)
        {
            InitializeComponent();
            groupBox1.Text = title;
            FillData(link, id, matchid);
        }

        protected void FillData(string link, long id, long matchid)
        {
            CrawlEachMarket crawl = new CrawlEachMarket();
            string msg = crawl.DeleteMarketOdds(id);
            crawl.CrawlMarkets(link, id, matchid);
            dataGridView1.DataSource = crawl.GetMarketOdds(id);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = String.Empty;
            }
            //int totalRowHeight = dataGridView1.ColumnHeadersHeight;

            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //    totalRowHeight += row.Height+5;

            //dataGridView1.Height = totalRowHeight;
            //this.Height = dataGridView1.Height + 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                MessageBox.Show(FileName+" saved successfully");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //if (dataGridView1.Columns[e.ColumnIndex].Name == "Odds")
                //{
                    if (!e.RowIndex.ToString().Equals("-1") && !e.ColumnIndex.ToString().Equals("1"))
                    {
                        string odd = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dataGridView1.Rows[e.RowIndex].Cells[0].Value = odd;
                        //long matchid = Convert.ToInt64(dataGridMarkets.Rows[e.RowIndex].Cells[1].Value);
                        //string match = groupBox1.Text;
                        //string html = Helper.GetWebSiteContent(Convert.ToString(dataGridMarkets.Rows[e.RowIndex].Cells[3].Value));

                        //Form childForm = new MarketOdds(html, id, matchid, match);
                        //childForm.MdiParent = this.ParentForm;
                        ////childForm.Text = "Window " + childFormNumber++;
                        //childForm.Show();
                        ////MessageBox.Show(id);
                    }
               // }
            }
            catch (Exception ex)
            {
            }
        }
    }
}