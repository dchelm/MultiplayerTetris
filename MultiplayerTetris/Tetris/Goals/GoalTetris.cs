using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    class GoalTetris : GoalI
    {
        private int tetris;

        public GoalTetris(int tetris)
        {
            this.tetris = tetris;
        }
        public bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo)
        {
            if (this.tetris <= tetris)
                return true;
            return false;                
        }

        public override string ToString()
        {
            return "Make " + this.tetris + " tetris clears";
        }
    }
}
