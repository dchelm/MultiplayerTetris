using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    class GoalCombo : GoalI
    {
        private int combo;

        public GoalCombo(int combo)
        {
            this.combo = combo;
        }
        public bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo)
        {
            if (this.combo <= combo)
                return true;
            return false;                
        }

        public override string ToString()
        {
            return "Get a x" + this.combo + " COMBO";
        }
    }
}
