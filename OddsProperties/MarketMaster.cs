using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
   public class MarketMaster
    {
       private int mMasterID;
       private int msportid;
       private string mResultName;
       private string mResultLink;

       public int MarketMasterID { get { return mMasterID; } set { mMasterID = value; } }
       public int MarketSportID { get { return msportid; } set { msportid = value; } }
       public string MarketMarketName { get { return  mResultName; } set { mResultName = value; } }
       public string MarketResultLink { get { return mResultLink; } set { mResultLink = value; } }

    }
}
