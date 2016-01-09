using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsProperties
{
   public class MatchResult
    {
        private string totalScore;
        private string halfSocre;
        private string firstGoalPlayer;
        private string allGoalPlayers;
        private string firstGoalTime;
        private string lastGoalTime;

        public string TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }
        public string HalfScore
        {
            get { return halfSocre; }
            set { halfSocre = value; }
        }
        public string FirstGoalPlayer
        {
            get { return firstGoalPlayer; }
            set { firstGoalPlayer = value; }
        }
        public string AllGoalPlayers
        {
            get { return allGoalPlayers; }
            set { allGoalPlayers = value; }
        }
        public string FirstGoalTime
        {
            get { return firstGoalTime; }
            set { firstGoalTime = value; }
        }
        public string LastGoalTime
        {
            get { return lastGoalTime; }
            set { lastGoalTime = value; }
        }
    }
}
