using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class BettingMarket
    {
        private long _id;
        private long _matchid;
        private string _bettingmarket;
        private string _bettinglink;
        private DateTime _createddate;

        public long id { get { return _id; } set { _id = value; } }
        public long matchid { get { return _matchid; } set { _matchid = value; } }
        public string bettingmarket { get { return _bettingmarket; } set { _bettingmarket = value; } }
        public string bettinglink { get { return _bettinglink; } set { _bettinglink = value; } }
        public DateTime createddate { get { return _createddate; } set { _createddate = value; } }
    }
}
