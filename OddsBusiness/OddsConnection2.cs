using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddsData;

namespace OddsBusiness
{
   public static class OddsConnection2
    {
       public static string GetConnectionString()
       {
           return OddsConnection.GetConnectionString();
       }
    }
}
