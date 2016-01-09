using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class GolfMarket
    {
        private int golfMarketID;
        private int mastetMarketID;
        private string playername;
        private int golfid;

        // Properties
        public int GolfMarketID { get { return golfMarketID; } set { golfMarketID = value; } }
        public int MasterMarketID { get { return mastetMarketID; } set { mastetMarketID = value; } }
        public string PlayerName { get { return playername; } set { playername = value; } }
        public int GolfID { get { return golfid; } set { golfid = value; } }

    }
}
