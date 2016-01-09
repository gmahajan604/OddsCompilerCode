using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
    public class Matches
    {
        private long _id;
        private string _league;
        private string _date;
        private string _time;
        private string _home;
        private string _away;
        private string _bettinglink;
        private string _bestbook;
        private string _draw;
        private DateTime _createddate;
        private DateTime _displayenddatetime;
        private string _resultlink;

        public DateTime Displayenddatetime
        {
            get { return _displayenddatetime; }
            set { _displayenddatetime = value; }
        }


        public long id { get{ return _id;}  set{ _id=value; } }
        public string league { get{ return _league;}  set{ _league=value; } }
        public string date { get{ return _date;}  set{ _date=value; } }
        public string time { get{ return _time;}  set{ _time=value; } }
        public string home { get{ return _home;}  set{ _home=value; } }
        public string away { get { return _away; } set { _away = value; } }
        public string bettinglink { get{ return _bettinglink;}  set{ _bettinglink=value; } }
        public string bestbook { get{ return _bestbook;}  set{ _bestbook=value; } }
        public string draw { get { return _draw; } set { _draw = value; } }
        public DateTime createddate { get { return _createddate; } set { _createddate = value; } }
        public string resultlink { get { return _resultlink; } set { _resultlink = value; } }


    }

}
