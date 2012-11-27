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
        private int level;
        private int lines = 0;
        private int points = 0;
        private int rows = 20;
        private int cols = 10;
        private int combo = 0;
        private Board board;
        private Piece p;
        private Piece nextP;
        private Random ran = new Random();
        private int ticks = 0;
        private int[] ticksPerLevel = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        private Canvas canvas;
        private Canvas canvas2;
        private MultiplayerTetris.TetrisSinglePlayer tsp;
        private MediaElement snd;

        public SPGameController(MultiplayerTetris.TetrisSinglePlayer tsp,int level)
        {
            this.level = level;
            this.tsp = tsp;
            this.canvas = (Canvas)tsp.FindName("canvasBoard");
            this.canvas2 = (Canvas)tsp.FindName("nextPieceCanvas");
            this.board = new Board(rows, cols);
            p = new Piece(ran.Next(0,7),-1,4);
            nextP = new Piece(ran.Next(0, 7), -1, 4);
            this.draw();
            this.drawNext();
            this.linesUpdate(0);
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
                combo++;
                this.linesUpdate(lines.Count);
                board.lines(lines);
                if (combo >= 3)
                    this.playComboSound();
            }
            else
            {
                combo = 0;
                ((TextBlock)tsp.FindName("comboText")).Text = "Combo  : " + this.combo.ToString();
            }
        }

        public async void playComboSound()
        {
            if (snd!=null)
                snd.Stop();
            var package = Windows.ApplicationModel.Package.Current;
            var installedLocation = package.InstalledLocation;
            var storageFile = await installedLocation.GetFileAsync("Assets\\Sounds\\combo.mp3");
            if (storageFile != null)
            {
                var stream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                snd = new MediaElement();
                snd.SetSource(stream, storageFile.ContentType);
                snd.Play();
            }
        }

        private void linesUpdate(int rows)
        {
            if(rows>0)
                this.points += linesToPoints[rows - 1] * (level + 1);
            this.lines += rows;
            ((TextBlock)tsp.FindName("levelText")).Text = "Level  : " + this.level;
            ((TextBlock)tsp.FindName("linesText")).Text =  "Lines  : " + lines.ToString();
            ((TextBlock)tsp.FindName("pointsText")).Text = "Points : " + points.ToString();
            ((TextBlock)tsp.FindName("comboText")).Text =  "Combo  : " + this.combo.ToString();
        }

        public void key(Windows.System.VirtualKey e)
        {
            switch (e)
            {
                case Windows.System.VirtualKey.Space:
                    Piece proj = this.board.projection(p);
                    this.points += 2*(proj.getRow()-p.getRow());
                    ((TextBlock)tsp.FindName("pointsText")).Text = "Points :" + points.ToString();
                    this.p = proj;
                    this.moveDown();
                    ticks = 0;
                    //this.moveDown();
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
                    this.points += 1;
                    ((TextBlock)tsp.FindName("pointsText")).Text = "Points :" + this.points.ToString();
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
                this.nextP = new Piece(ran.Next(0, 7), -1, 4);
                this.drawNext();
            }else
                p.moveDown();
        }

        private void drawBoard()
        {
            canvas.Children.Clear();
            //green when good red when fucked
            //green 66 223 49
            //red   223 49 49
            double good = 1-((double)(this.rows - this.board.getHighest()))/(double)this.rows;
            SolidColorBrush gridColor = new SolidColorBrush(Color.FromArgb(100, (byte)(255 - (int)(good * 255)), (byte)((int)(good * 255)), 49));
            for (int col = 1; col < cols; col++)
            {
                Line l = new Line();
                l.X1 = col * 30;
                l.Y1 = 0;
                l.X2 = col * 30;
                l.Y2 = rows * 30;
                l.Stroke = gridColor;
                canvas.Children.Add(l);
            } for (int row = 1; row < rows; row++)
            {
                Line l = new Line();
                l.X1 = 0;
                l.Y1 = row*30;
                l.X2 = cols*30;
                l.Y2 = row * 30;
                l.Stroke = gridColor;
                canvas.Children.Add(l);
            }
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
                        if (row >= 0)
                        {
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
                        if (type == 1 || type == 4 || type == 6 || type ==3)
                            col--;
                        r.Margin = new Thickness(30 * col+10, 30 * (row-1)+10, 0, 0);
                        canvas2.Children.Add(r);
                    }
                }
        }

        public void changeLevel(int level)
        {
            this.level = level;
            this.ticks = 0;
        }

    }
}
