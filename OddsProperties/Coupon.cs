using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class Coupon
    {
        private long _bettingmarketid;
        private string _selection;
        private string _toals;
        private long _couponid;
        private string _couponname;
        private DateTime _createdon;
        private string _identifier;
        /// <summary>
        /// _matchId added for match
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public string Couponname
        {
            get { return _couponname; }
            set { _couponname = value; }
        }
        

        public string Selection
        {
            get { return _selection; }
            set { _selection = value; }
        }

        public string Toals
        {
            get { return _toals; }
            set { _toals = value; }
        }

        public long Couponid
        {
            get { return _couponid; }
            set { _couponid = value; }
        }
        

        public DateTime Createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }

        public long Bettingmarketid
        {
            get { return _bettingmarketid; }
            set { _bettingmarketid = value; }
        }

    }
}
