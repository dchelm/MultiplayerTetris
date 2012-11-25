using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris
{
    class Board
    {
        private int[,] board;
        private int rows = 20;
        private int cols = 10;

        private List<Piece> pieces;

        public Board()
        {
            this.board = new int[rows,cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    board[row, col] = 0;
        }

        private Boolean intersects_on_next(Piece p)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getX() + j;
                    int row = p.getY() + i + 1;//In t+1
                    bool filled = p.getPiece()[i,j]==1?true:false;
                    if(filled)
                    {
                        if (row < rows && row >= 0 && col < cols && col >= 0)
                        {
                            if (board[row, col] == 1)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
