using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    class GoalMin : GoalI
    {
        private int min;

        public GoalMin(int min)
        {
            this.min = min;
        }
        public bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo)
        {
            if (this.min <= min)
                return true;
            return false;                
        }

        public override string ToString()
        {
            return "Play for " + this.min+ " minutes";
        }
    }
}
