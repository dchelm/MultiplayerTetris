using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    class GoalApm : GoalI
    {
        private int totalAPM;
        private int min;

        public GoalApm(int totalAPM, int min)
        {
            this.totalAPM = totalAPM;
            this.min = min;
        }
        public bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo)
        {
            if (this.totalAPM <= totalAPM && this.min <= min)
                return true;
            return false;                
        }

        public override string ToString()
        {
            return "Get " + this.totalAPM + " totalAPM past the " + min + "th minute";
        }
    }
}
