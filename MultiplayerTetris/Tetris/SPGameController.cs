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
        private int totalLines = 0;
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
        private bool gameOverBool = false;
        private int actions = 0;
        private long totalTicks;
        private int tetris = 0;
        private GoalController goalController;
        private int totalApm = 0;
        private int min = 0;
        private int highestTotalAPM = 0;

        public SPGameController(MultiplayerTetris.TetrisSinglePlayer tsp, int level, GoalController goalController)
        {
            totalTicks = 0;
            this.level = level;
            this.tsp = tsp;
            this.goalController = goalController;
            this.canvas = (Canvas)tsp.FindName("canvasBoard");
            this.canvas2 = (Canvas)tsp.FindName("nextPieceCanvas");
            this.board = new Board(rows, cols);
            nextP = new Piece(ran.Next(0, 7), -1, 4);
            this.newPiece();
            this.updateGoals();
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

        private void updateGoals()
        {
            goalController.updatePending(totalLines, totalApm, tetris, min, combo);
        }

        private void linesUpdate(int lines)
        {
            if (lines > 0)
            {
                if (lines == 4)
                    this.tetris++;
                this.points += linesToPoints[lines - 1] * (this.level + 1);
            }
            this.totalLines += lines;
            this.updateGoals();
            ((TextBlock)tsp.FindName("levelText")).Text = "Level  : " + this.level;
            ((TextBlock)tsp.FindName("linesText")).Text =  "Lines  : " + this.totalLines.ToString();
            ((TextBlock)tsp.FindName("pointsText")).Text = "Points : " + this.points.ToString();
            ((TextBlock)tsp.FindName("comboText")).Text =  "Combo  : " + this.combo.ToString();
        }

        public void key(Windows.System.VirtualKey e)
        {
            switch (e)
            {
                case Windows.System.VirtualKey.Space:
                    actions++;
                    Piece proj = this.board.projection(p);
                    this.points += 2*(proj.getRow()-p.getRow());
                    ((TextBlock)tsp.FindName("pointsText")).Text = "Points :" + points.ToString();
                    this.p = proj;
                    this.moveDown();
                    ticks = 0;
                    //this.moveDown();
                    break;
                case Windows.System.VirtualKey.Up:
                    actions++;
                    p.rotate_right();
                    if (this.board.intersectsAndOutOfBounds(p, 0, 0))//choca despues de intersectar
                    {
                        if (!this.board.intersectsAndOutOfBounds(p, -1, 0))
                        {
                            p.moveLeft();
                            break;
                        }
                        else if (!this.board.intersectsAndOutOfBounds(p, 1, 0))
                        {
                            p.moveRight();
                            break;
                        }
                        else if (!this.board.intersectsAndOutOfBounds(p, 2, 0))
                        {
                            p.moveRight();
                            p.moveRight();
                            break;
                        }
                        p.rotate_left();
                    }
                    break;
                case Windows.System.VirtualKey.Down:
                    actions++;
                    this.points += 1;
                    ((TextBlock)tsp.FindName("pointsText")).Text = "Points :" + this.points.ToString();
                    this.moveDown();
                    this.ticks = 0;
                    break;
                case Windows.System.VirtualKey.Left:
                    actions++;
                    if (!this.board.intersectsAndOutOfBounds(p, -1, 0))
                        p.moveLeft();
                    break;
                case Windows.System.VirtualKey.Right:
                    actions++;
                    if (!this.board.intersectsAndOutOfBounds(p, 1, 0))
                        p.moveRight();
                    break;
                case Windows.System.VirtualKey.Shift:
                    break;
            }
            this.draw();
        }

        public void tick()
        {
            this.updateTime();
            totalTicks++;
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

        private void updateTime()
        {
            TimeSpan ts =  tsp.getTime();
            if (this.min < (int)ts.TotalMinutes)
            {
                this.min = (int)ts.TotalMinutes;
                this.updateGoals();
            }
            ((TextBlock)tsp.FindName("timeText")).Text = new DateTime(ts.Ticks).ToString("HH:mm:ss");
            this.totalApm = ((int)((double)actions / (double)ts.TotalMinutes));
            ((TextBlock)tsp.FindName("apmText")).Text = "APM : " + this.totalApm.ToString();
        }

        private void update()
        {
            this.updateGoals();
            this.moveDown();
        }

        private void moveDown()
        {
            this.ticks = 0;
            if (board.intersectsAndOutOfBounds(p, 0, 1))
            {
                if (board.addPiece(p))
                    this.gameOver() ;
                this.checkLines(p.getRow());
                this.newPiece();
            }else
                p.moveDown();
        }

        private void gameOver()
        {
            this.gameOverBool = true;
            tsp.gameOver();
            canvas.Children.Clear();
            canvas2.Children.Clear();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    ImageBrush imgBrush = new ImageBrush();
                    BitmapImage image = new BitmapImage(uris[ran.Next(0,7)]);
                    imgBrush.ImageSource = image;
                    Rectangle r = new Rectangle();
                    r.Height = 30;
                    r.Width = 30;
                    r.Fill = imgBrush;
                    r.Margin = new Thickness(30 * j, 30 * i, 0, 0);
                    canvas.Children.Add(r);
                }
        }

        private void newPiece()
        {
            this.p = nextP;
            if (board.intersects(p,0,0))
                this.gameOver();
            this.nextP = new Piece(ran.Next(0, 7), -2, 4);
            this.drawNext();
        }

        private void drawBoard()
        {
            if (this.gameOverBool)
                return;
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
            if (this.gameOverBool)
                return;
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
                            if (projection)
                                imgBrush.Opacity = .3;
                            r.Fill = imgBrush;
                            r.Margin = new Thickness(30 * col, 30 * row, 0, 0);
                            canvas.Children.Add(r);
                        }
                    }
                }
        }

        private void drawNext()
        {
            if (this.gameOverBool)
                return;
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
