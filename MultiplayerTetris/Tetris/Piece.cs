using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MultiplayerTetris
{
    class Piece
    {
        static enum PIECES {I=1,J,L,O,S,T,Z};
        static enum COLORS {cyan,blueviolet,magenta,lightgrey,green,yellow,red};
        private int rot = 0;
        private int[,,] piece; // rotation, piece
        private int x; //top left column
        private int y; //top left row

        public Piece(PIECES p)
        {
            piece = new int[4, 4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for(int k = 0;k<4;k++)
                    piece[k, i, j] = 0;
            switch (p)
            {
                case PIECES.I:
                    piece[0, 1, 0] = 1;    piece[1, 0, 2] = 1;    piece[2, 1, 0] = 1;    piece[3, 0, 2] = 1;
                    piece[0, 1, 1] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 1] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 2, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 2, 2] = 1;
                    piece[0, 1, 3] = 1;    piece[1, 3, 2] = 1;    piece[2, 1, 3] = 1;    piece[3, 3, 2] = 1;
                    break;
                case PIECES.J:
                    piece[0, 1, 1] = 1;    piece[1, 0, 2] = 1;    piece[2, 0, 1] = 1;    piece[3, 0, 3] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 1] = 1;    piece[3, 0, 2] = 1;
                    piece[0, 1, 3] = 1;    piece[1, 2, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 2, 3] = 1;    piece[1, 2, 1] = 1;    piece[2, 1, 3] = 1;    piece[3, 2, 2] = 1;
                    break;
                case PIECES.L:
                    piece[0, 1, 2] = 1;    piece[1, 0, 0] = 1;    piece[2, 0, 2] = 1;    piece[3, 0, 1] = 1;
                    piece[0, 1, 1] = 1;    piece[1, 0, 1] = 1;    piece[2, 1, 2] = 1;    piece[3, 1, 1] = 1;
                    piece[0, 1, 0] = 1;    piece[1, 1, 1] = 1;    piece[2, 1, 1] = 1;    piece[3, 2, 1] = 1;
                    piece[0, 2, 0] = 1;    piece[1, 2, 1] = 1;    piece[2, 1, 0] = 1;    piece[3, 2, 2] = 1;
                    break;                                                                   
                case PIECES.O:                                                               
                    piece[0, 0, 1] = 1;    piece[1, 0, 1] = 1;    piece[2, 0, 1] = 1;    piece[3, 0, 1] = 1;
                    piece[0, 0, 1] = 1;    piece[1, 0, 1] = 1;    piece[2, 0, 1] = 1;    piece[3, 0, 1] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 1, 2] = 1;
                    break;                                                                   
                case PIECES.S:                                                               
                    piece[0, 1, 3] = 1;    piece[1, 0, 1] = 1;    piece[2, 0, 1] = 1;    piece[3, 1, 3] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 1] = 1;    piece[2, 1, 1] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 2, 2] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 2, 2] = 1;
                    piece[0, 2, 1] = 1;    piece[1, 2, 2] = 1;    piece[2, 2, 2] = 1;    piece[3, 2, 1] = 1;
                    break;                                                                  
                case PIECES.T:                                                               
                    piece[0, 1, 0] = 1;    piece[1, 0, 1] = 1;    piece[2, 0, 1] = 1;    piece[3, 0, 1] = 1;
                    piece[0, 1, 1] = 1;    piece[1, 1, 0] = 1;    piece[2, 1, 0] = 1;    piece[3, 1, 1] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 1] = 1;    piece[2, 1, 1] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 2, 1] = 1;    piece[1, 2, 1] = 1;    piece[2, 1, 2] = 1;    piece[3, 2, 1] = 1;
                    break;                                                                   
                case PIECES.Z:                                                               
                    piece[0, 1, 1] = 1;    piece[1, 0, 2] = 1;    piece[2, 0, 2] = 1;    piece[3, 1, 1] = 1;
                    piece[0, 1, 2] = 1;    piece[1, 1, 1] = 1;    piece[2, 1, 1] = 1;    piece[3, 1, 2] = 1;
                    piece[0, 2, 2] = 1;    piece[1, 1, 2] = 1;    piece[2, 1, 2] = 1;    piece[3, 2, 2] = 1;
                    piece[0, 2, 3] = 1;    piece[1, 2, 1] = 1;    piece[2, 2, 1] = 1;    piece[3, 2, 3] = 1;
                    break;
            }
        }

        public void rotate_right()
        {
            this.rot = (this.rot + 1) % 4;
        }

        public void rotate_left()
        {
            this.rot = (this.rot + 3) % 4;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public int[,] getPiece()
        {
            int[,] _piece = new int[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    _piece[i, j] = this.piece[this.rot, i, j];
            return _piece;
        }

        public string getColor()
        {
            return this.color;
        }
    }
}
