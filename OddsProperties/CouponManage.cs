using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class Coupon2
    {
        private string couponid;
        private string couponname;
        private bool isarchived;
        private int matchid;
        private int cid;

        public string CouponID { get { return couponid; } set { couponid = value; } }
        public string CouponName { get { return couponname; } set { couponname = value; } }
        public bool IsArchived { get { return isarchived; } set { isarchived = value; } }
        public int MatchID { get { return matchid; } set { matchid = value; } }
        public int CID { get { return cid; } set { cid = value; } }
    }
}