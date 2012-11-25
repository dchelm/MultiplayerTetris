using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris
{
    class SPGameController
    {
        private int[] linesToPoints = { 100, 300, 500, 800 };
        private int level = 0;
        private int lines = 0;
        private int points = 0;
        private int rows = 20;
        private int cols = 10;
        private int multiplier = 0;
        private Board board;

        //cuando una piesa cae.. antes de chequear lineas hay que agregarla al mapa :).. 
        private void checkLines(int topRow)
        {
            board = new Board(rows,cols);
            List<int> lines = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int row = topRow - i;
                if (row >= 0 && row < rows)
                {
                    bool line = true;
                    for (int col = 0; col < cols; col++)
                        if (board.getPosition(row,col) == 0)
                            line = false;
                    if (line)
                        lines.Add(row);
                }
            }
            if (lines.Count > 0)
            {
                this.linesUpdate(lines.Count);
                board.lines(lines);
            }
        }

        private void linesUpdate(int rows)
        {
            this.points += linesToPoints[rows - 1] * (level + 1);
            this.lines += rows;
        }
    }
}
