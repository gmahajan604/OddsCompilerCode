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
    public partial class MatchesSchedule : Form
    {
        public MatchesSchedule()
        {
            InitializeComponent();
            BindGrid();
        }

        private void MatchesSchedule_Load(object sender, EventArgs e)
        {
            
        }

        protected void BindGrid()
        {
            BindingSource bindingSource1 = new BindingSource();
            dataGridMatches.AutoGenerateColumns = false;
            dataGridMatches.ColumnCount = 7;

            dataGridMatches.Columns[0].Name = "Match Date";
            dataGridMatches.Columns[0].DataPropertyName = "MatchDate";
            dataGridMatches.Columns[1].Name = "Time";
            dataGridMatches.Columns[1].DataPropertyName = "Time";
            dataGridMatches.Columns[2].Name = "Home";
            dataGridMatches.Columns[2].DataPropertyName = "Home";
            dataGridMatches.Columns[3].Name = "Draw";
            dataGridMatches.Columns[3].DataPropertyName = "Draw";
            dataGridMatches.Columns[4].Name = "Away";
            dataGridMatches.Columns[4].DataPropertyName = "Away";
            dataGridMatches.Columns[5].Name = "ID";
            dataGridMatches.Columns[5].DataPropertyName = "id";
            dataGridMatches.Columns[5].Visible = false;
            var buttonCol = new DataGridViewButtonColumn();
            buttonCol.UseColumnTextForButtonValue = true;
            buttonCol.Name = "Markets";
            buttonCol.Text = "View Markets";
            
            dataGridMatches.Columns.Add(buttonCol);
             //DataPropertyName = "BettingLink";
            //dataGridMatches.Columns[4].DataPropertyName = "Away";
            CrawlFirstPage crawl = new CrawlFirstPage();
            
            bindingSource1.DataSource = crawl.GetMatchInfo();
            dataGridMatches.DataSource = bindingSource1;
           
            
            //dataGridMatches.
            
        }

        private void dataGridMatches_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridMatches.Columns[e.ColumnIndex].Name == "Markets")
                {
                    if (!e.RowIndex.ToString().Equals("-1"))
                    {
                        string id = Convert.ToString(dataGridMatches.Rows[e.RowIndex].Cells[5].Value);
                        string team1 = Convert.ToString(dataGridMatches.Rows[e.RowIndex].Cells[2].Value);
                        team1 = team1.Substring(0, team1.IndexOf('('));
                        string team2 = Convert.ToString(dataGridMatches.Rows[e.RowIndex].Cells[4].Value);
                        team2 = team2.Substring(0, team2.IndexOf('('));
                        string matchday = Convert.ToString(dataGridMatches.Rows[e.RowIndex].Cells[0].Value);
                        string time = Convert.ToString(dataGridMatches.Rows[e.RowIndex].Cells[1].Value);
                        Form childForm = new BettingMarkets(Convert.ToInt64(id), team1 + " VS " + team2 + "\n" + matchday + " " + time);
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
