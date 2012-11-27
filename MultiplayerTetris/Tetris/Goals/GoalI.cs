using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris.Goals
{
    interface GoalI
    {
        /*
         *  x lines
         *  x min
         *  x tetris
         *  x combo
         *  x totalAPM at y min
         *  x lines under y min
         *  TOTAL BULLSHIT
         */
        bool isCompleted(int lines, int totalAPM, int tetris, int min, int combo);
    }
}
