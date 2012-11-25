﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerTetris.Tetris
{
    class Board
    {
        private int[,] board;
        private int rows;
        private int cols;

        public Board(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.board = new int[rows,cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    board[row, col] = 0;
        }

        private Boolean intersectsOnNext(Piece p)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getX() + j;
                    int row = p.getY() - i - 1;//In t+1
                    bool filled = p.getPiece()[i,j]==1?true:false;
                    if(filled)
                        if (row < rows && row >= 0 && col < cols && col >= 0)
                            if (board[row, col] != 0)
                                return true;
                }
            }
            return false;
        }

        public int getPosition(int row,int col)
        {
            if (row >= 0 && row < rows && col < cols && col >= 0)
                return this.board[row, col];
            else
                throw new System.IndexOutOfRangeException("Board.getPosition("+row.ToString()+","+col.ToString()+") index out of bounds");
        }
    }
}
