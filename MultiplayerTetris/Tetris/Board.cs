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
        private int rows;
        private int cols;
        private int highest;//records tallest row

        public Board(int rows, int cols)
        {
            this.highest = rows;
            this.rows = rows;
            this.cols = cols;
            this.board = new int[rows,cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    board[row, col] = -1;
        }

        public Boolean intersects(Piece p, int dispCol, int dispRow)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getCol() + j + dispCol;
                    int row = p.getRow() + i + dispRow;
                    bool filled = p.getPiece()[i, j] == 1 ? true : false;
                    if (filled)
                        if (row < rows && col < cols && col >= 0)
                        {
                            if (row >= 0 && board[row, col] != -1)
                            {
                                return true;
                            }
                        }
                }
            }
            return false;
        }

        public Boolean intersectsAndOutOfBounds(Piece p,int dispCol, int dispRow)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getCol() + j + dispCol ;
                    int row = p.getRow() + i + dispRow ;
                    bool filled = p.getPiece()[i,j]==1?true:false;
                    if (filled)
                        if (row < rows && col < cols && col >= 0)
                        {
                            if (row >= 0 &&  board[row, col] != -1)
                            {
                                return true;
                            }
                        }
                        else
                            return true;
                }
            }
            return false;
        }

        public bool addPiece(Piece p)
        {
            bool outOfBounds = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getCol() + j;
                    int row = p.getRow() + i;
                    bool filled = p.getPiece()[i, j] == 1 ? true : false;
                    if (filled)
                        if (row < rows && col < cols && col >= 0)
                            if (row < 0)
                                outOfBounds = true;
                            else if (board[row, col] == -1)
                            {
                                board[row, col] = p.type;
                                if (row < highest)
                                    highest = row;
                            }
                            
                }
            }
            return outOfBounds;
        }

        public int getPosition(int row,int col)
        {
            if (row >= 0 && row < rows && col < cols && col >= 0)
                return this.board[row, col];
            else
                throw new System.IndexOutOfRangeException("Board.getPosition("+row.ToString()+","+col.ToString()+") index out of bounds");
        }

        public void lines(List<int> lines)
        {
            int displacement = 1;
            for (int i = lines[lines.Count-1]-1; i >= this.highest; i--)
            {
                if (lines.Contains(i))
                    displacement++;
                else
                    for (int col = 0; col < cols; col++)
                    {
                        board[i + displacement, col] = board[i, col];
                        board[i, col] = -1;
                    }
            }
            highest += lines.Count;
        }

        public Piece projection(Piece p)
        {
            for (int i = p.getRow()+1; i <rows ; i++)
            {
                if (this.intersectsAndOutOfBounds(p, 0, i - p.getRow()))
                {
                    Piece aux = new Piece(p.type,i - 1, p.getCol());
                    aux.setRot(p.getRot());
                    return aux;
                }
            }
            return new Piece(p.type, 3, p.getRow());
        }

        public int getHighest()
        {
            return this.highest;
        }

    }
}
