using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class GolfTurnament
    {
        private int golfID;
        private string duration;
        private string turnament;
        private string course;
        private string champion;
        private string link;

        public int GolfID { get { return golfID; } set { golfID = value; } }
        public string Duration { get { return duration; } set { duration = value; } }
        public string Turnament { get { return turnament; } set { turnament = value; } }
        public string Course{get{return course;} set {course=value;}}
        public string Champion { get { return champion; } set { champion = value; } }
        public string Link{get{return link;} set {link=value;}}

    }
}
