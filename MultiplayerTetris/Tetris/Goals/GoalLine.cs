using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    class GoalLine : GoalI
    {
        private int lines;
        private int min;

        public GoalLine(int lines,int min)
        {
            this.min = min;
            this.lines = lines;
        }
        public bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo)
        {
            if (min == 0)
            {
                if (lines >= this.lines)
                    return true;
            }
            else
                if (lines >= this.lines && this.min < min)
                    return true;
            return false;                
        }

        public override string ToString()
        {
            if(min==0)
                return "Clear " + lines + " lines";
            else
                return "Clear " + lines + " lines before "+this.min+" minutes";
        }
    }
}
