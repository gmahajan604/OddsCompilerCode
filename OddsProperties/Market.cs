using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class Market
    {
        private long _id;
        private string _bestbet;
        private string _beton;
        private string _bookiename;
        private string _bet;
        private DateTime _createddate;
        private long _bettingmarketid;
        private long _matchid;

        public long id { get { return _id; } set { _id = value; } }
        public string bestbet { get { return _bestbet; } set { _bestbet = value; } }
        public string beton { get { return _beton; } set { _beton = value; } }
        public string bookiename { get { return _bookiename; } set { _bookiename = value; } }
        public string bet { get { return _bet; } set { _bet = value; } }
        public DateTime createddate { get { return _createddate; } set { _createddate = value; } }
        public long bettingmarketid { get { return _bettingmarketid; } set { _bettingmarketid = value; } }
        public long matchid { get { return _matchid; } set { _matchid = value; } }
    }
}
