using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace MultiplayerTetris.Tetris
{
    class SPGameController
    {
        private Uri[] uris = {
                                 new Uri("ms-appx:///Assets/Blocks/BlueBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/BrownBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/CyanBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/DarkBlueBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/GreenBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/OrangeBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/PinkBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/RedBlock.png"),
                                 new Uri("ms-appx:///Assets/Blocks/YellowBlock.png"),
                             };
        private int[] linesToPoints = { 100, 300, 500, 800 };
        private int level = 0;
        private int lines = 0;
        private int points = 0;
        private int rows = 20;
        private int cols = 10;
        private int multiplier = 0;
        private Board board;
        private Piece p;
        private Piece nextP;
        private Random ran = new Random();

        private int ticks = 0;
        private int[] ticksPerLevel = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        private Canvas canvas;
        private Canvas canvas2;
        private MultiplayerTetris.TetrisSinglePlayer tsp;


        public    SPGameController(MultiplayerTetris.TetrisSinglePlayer tsp)
        {
            this.tsp = tsp;
            this.canvas = (Canvas)tsp.FindName("canvasBoard");
            this.canvas2 = (Canvas)tsp.FindName("nextPieceCanvas");
            this.board = new Board(rows, cols);
            p = new Piece(ran.Next(0,7),0,4);
            nextP = new Piece(ran.Next(0, 7), 0, 4);
            this.drawNext();

        }

        //cuando una piesa cae.. antes de chequear lineas hay que agregarla al mapa :).. 
        private void checkLines(int topRow)
        {
            List<int> lines = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int row = topRow + i;
                if (row >= 0 && row < rows)
                {
                    bool line = true;
                    for (int col = 0; col < cols; col++)
                        if (board.getPosition(row,col) == -1)
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
            ((TextBlock)tsp.FindName("levelText")).Text = "level 0";
            ((TextBlock)tsp.FindName("linesText")).Text = "level "+lines.ToString();
            ((TextBlock)tsp.FindName("pointsText")).Text = "level " + points.ToString();
        }

        public void key(Windows.System.VirtualKey e)
        {
            switch (e)
            {
                case Windows.System.VirtualKey.Space:
                    p = this.board.projection(p);
                    this.moveDown();
                    break;
                case Windows.System.VirtualKey.Up:
                    p.rotate_right();
                    if (this.board.intersects(p, 0, 0))//choca despues de intersectar
                    {
                        if (!this.board.intersects(p, -1, 0))
                        {
                            p.moveLeft();
                            break;
                        }
                        else if (!this.board.intersects(p, 1, 0))
                        {
                            p.moveRight();
                            break;
                        }
                        else if (!this.board.intersects(p, 2, 0))
                        {
                            p.moveRight();
                            p.moveRight();
                            break;
                        }
                        p.rotate_left();
                    }
                    break;
                case Windows.System.VirtualKey.Down:
                    this.moveDown();
                    this.ticks = 0;
                    break;
                case Windows.System.VirtualKey.Left:
                    if (!this.board.intersects(p,-1,0))
                        p.moveLeft();
                    break;
                case Windows.System.VirtualKey.Right:
                    if (!this.board.intersects(p, 1, 0))
                        p.moveRight();
                    break;
                case Windows.System.VirtualKey.Shift:
                    break;
            }
            this.draw();
        }

        public void tick()
        {
            ticks ++;
            if(ticks>= ticksPerLevel[level])
            {
                ticks = 0;
                this.update();
                this.draw();
            }
        }

        private void draw()
        {
            this.drawBoard();
            this.drawPiece(this.board.projection(p), true);
            this.drawPiece(p,false);
        }

        private void update()
        {
            this.moveDown();
        }

        private void moveDown()
        {
            if (board.intersects(p,0,1))
            {
                if (!board.addPiece(p))//game over
                {
                    tsp.gameOver();
                }
                this.checkLines(p.getRow());
                this.p = nextP;
                this.nextP = new Piece(ran.Next(0, 7), 0, 4);
                this.drawNext();

            }else
                p.moveDown();
        }

        private void drawBoard()
        {
            canvas.Children.Clear();
            //draw map
            for(int row = 0;row<rows;row++)
                for(int col=0;col<cols;col++)
                {
                    int type = this.board.getPosition(row, col);
                    if (type != -1)
                    {
                        ImageBrush imgBrush = new ImageBrush();
                        BitmapImage image = new BitmapImage(uris[type]);
                        imgBrush.ImageSource = image;
                        Rectangle r = new Rectangle();
                        r.Height = 30;
                        r.Width = 30;
                        r.Fill = imgBrush;
                        r.Margin = new Thickness(30 * col, 30 * row, 0, 0);
                        canvas.Children.Add(r);
                    }
                }
        }

        private void drawPiece(Piece p,bool projection)
        {
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
            //draw piece
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    int type = p.type;
                    if (p.getPiece()[i, j] == 1)
                    {
                        int col = p.getCol() + j;
                        int row = p.getRow() + i;
                        ImageBrush imgBrush = new ImageBrush();
                        BitmapImage image = new BitmapImage(uris[type]);
                        imgBrush.ImageSource = image;
                        Rectangle r = new Rectangle();
                        r.Height = 30;
                        r.Width = 30;
                        if (!projection)
                            r.Fill = imgBrush;
                        else
                            r.Fill = scb;
                        r.Margin = new Thickness(30 * col, 30 * row, 0, 0);
                        canvas.Children.Add(r);
                    }
                }
        }

        private void drawNext()
        {
            canvas2.Children.Clear();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    int type = nextP.type;
                    if (nextP.getPiece()[i, j] == 1)
                    {
                        int col =j;
                        int row =i;
                        ImageBrush imgBrush = new ImageBrush();
                        BitmapImage image = new BitmapImage(uris[type]);
                        imgBrush.ImageSource = image;
                        Rectangle r = new Rectangle();
                        r.Height = 30;
                        r.Width = 30;
                        r.Fill = imgBrush;
                        r.Margin = new Thickness(30 * col, 30 * row, 0, 0);
                        canvas2.Children.Add(r);
                    }
                }
        }

    }
}
