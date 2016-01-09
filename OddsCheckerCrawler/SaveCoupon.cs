using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OddsCheckerCrawler
{
    public partial class SaveCoupon : Form
    {
        public SaveCoupon()
        {
            InitializeComponent();
        }

        private void btnname_Click(object sender, EventArgs e)
        {
            OddsCrawler crawl = ActiveMdiChild as OddsCrawler;
            crawl.SaveCoupon(txtcouponname.Text);
            this.Close();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
