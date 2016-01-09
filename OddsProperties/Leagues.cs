using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class Leagues
    {
        private long _leagueid;

        public long Leagueid
        {
            get { return _leagueid; }
            set { _leagueid = value; }
        }
        private string _league;

        public string League
        {
            get { return _league; }
            set { _league = value; }
        }
        private string _link;

        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }
        private DateTime _createdon;

        public DateTime Createdon
        {
            get { return _createdon; }
            set { _createdon = value; }
        }
        private int _sportid;

        public int Sportid
        {
            get { return _sportid; }
            set { _sportid = value; }
        }

        private string _section;

        public string Section
        {
            get { return _section; }
            set { _section = value; }
        }
    }
}
