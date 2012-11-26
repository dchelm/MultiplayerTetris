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
        private int highest = 30;//records tallest row

        public Board(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.board = new int[rows,cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                    board[row, col] = -1;
        }

        public Boolean intersects(Piece p,int dispCol, int dispRow)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getCol() + j + dispCol ;
                    int row = p.getRow() + i + dispRow ;
                    bool filled = p.getPiece()[i,j]==1?true:false;
                    if (filled)
                        if (row < rows && row >= 0 && col < cols && col >= 0)
                        {
                            if (board[row, col] != -1)
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
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int col = p.getCol() + j;
                    int row = p.getRow() + i;
                    bool filled = p.getPiece()[i, j] == 1 ? true : false;
                    if (filled)
                        if (row < rows && row >= 0 && col < cols && col >= 0)
                            if (board[row, col] != -1)
                                return false;
                            else
                            {
                                board[row, col] = p.type;
                                if (row < highest)
                                    highest = row;
                            }
                }
            }
            return true;
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
            //para mejorar el rendimiento podemos partir desde el row mas chico... por como esta construido lines la mas chica esta al final
            int displacement = 1;
            for (int i = lines[lines.Count-1]-1; i >= highest-1; i--)
            {
                if (lines.Contains(i))
                    displacement++;
                else
                    for (int col = 0; col < cols; col++)
                        board[i + displacement, col] = board[i, col];
            }
            highest += lines.Count;
        }

        public Piece projection(Piece p)
        {
            for (int i = p.getRow()+1; i <rows ; i++)
            {
                if (this.intersects(p, 0, i-p.getRow()))
                {
                    Piece aux = new Piece(p.type,i - 1, p.getCol());
                    aux.setRot(p.getRot());
                    return aux;
                }
            }
            return new Piece(p.type, 3, p.getRow());
        }

    }
}
