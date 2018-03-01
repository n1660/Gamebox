using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeGame
{
    public enum Pictures
    {
        Elem,
        Head,
        Tail
    };

    public enum Colors
    {
        Green,
        Blue,
        Red,
        Purple
    };
    public class SnakePlayer
    {
        //membervariables
        private int id = 1;
        private String name;
        private Colors color;
        private TextBlock[] scoretext = new TextBlock[2];
        private Dictionary<String, ImageBrush> pictures = new Dictionary<String, ImageBrush>();
        private List<SnakeElem> snake = new List<SnakeElem>();
        private IPEndPoint address;
        private bool dead = false;
        private GamepageSnake.Directions direction, disabledDirection;
        private Canvas gameCanvas;
        public int score = 0;
        public List<SnakeElem> snakeTmp = new List<SnakeElem>();

        //globals
        public static int CURPARTICIPANTS = 0, SIZEELEM = 14, STARTLENGTH = 5, AMOUNT_PLAYERS = 3, SURVIVORS;
        public static List<SnakePlayer> TODIE = new List<SnakePlayer>();

        //Properties
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Colors Color { get => color; set => color = value; }
        public Dictionary<string, ImageBrush> Pictures { get => pictures; set => pictures = value; }
        public List<SnakeElem> Snake { get => snake; set => snake = value; }
        public bool Dead { get => dead; set => dead = value; }
        public Canvas GameCanvas { get => gameCanvas; set => gameCanvas = value; }
        public TextBlock[] Scoretext { get => scoretext; set => scoretext = value; }
        public GamepageSnake.Directions Direction { get => direction; set => direction = value; }
        public GamepageSnake.Directions DisabledDirection { get => disabledDirection; set => disabledDirection = value; }

        //c'tor
        public SnakePlayer(String name, IPEndPoint adr, Canvas gamecanv)
        {
            CURPARTICIPANTS++;
            if (CURPARTICIPANTS > 4)
            {
                CURPARTICIPANTS--;
                return;
            }

            this.id = CURPARTICIPANTS - 1;
            this.name = name;
            this.gameCanvas = gamecanv;
            this.dead = false;
            this.score = 0;
            this.scoretext[0] = new TextBlock
            {
                Name = "LblScorePlayer" + this.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
            };
            this.Scoretext[1] = new TextBlock
            {
                Name = "TxtScorePlayer" + this.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
            };
            this.color = (Colors)(CURPARTICIPANTS - 1);
            this.address = adr;

            if (this.gameCanvas != null) {
                this.snake = InitializeSnake();
                this.disabledDirection = ((int)this.snake[0].Direction < 2) ? (GamepageSnake.Directions)((int)this.snake[0].Direction + 2) : (GamepageSnake.Directions)((int)this.snake[0].Direction - 2);
            }

            UpdateRanking();
            this.UpdateScore();
            foreach (SnakeElem snk in this.snake)
            {
                this.gameCanvas.Children.Add(snk.Rect);
            }
            GamepageSnake.GetDPButtons().Text = "players: " + SnakePlayer.CURPARTICIPANTS.ToString();
        }

        //initializations
        public void InitializePictures()
        {
            this.pictures.Add(SnakeGame.Pictures.Elem.ToString(), new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakeElem_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
            });
            foreach (GamepageSnake.Directions dir in Enum.GetValues(typeof(GamepageSnake.Directions)))
            {
                this.pictures.Add(SnakeGame.Pictures.Head.ToString() + "_" + dir.ToString() + "_" + this.color.ToString(), new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakehead_" + dir.ToString() + "_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                });

                this.pictures.Add(SnakeGame.Pictures.Tail.ToString() + "_" + dir.ToString() + "_" + this.color.ToString(), new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snaketail_" + dir.ToString() + "_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                });
            }
        }

        public List<SnakeElem> InitializeSnake()
        {
            this.snakeTmp.Add(new SnakeElem
            {
                X = ((this.id < 2) ? 50 : ((int)this.GameCanvas.ActualWidth - 50)),
                Y = ((this.id % 2 == 0) ? 50 : ((int)this.GameCanvas.ActualHeight - 50)),
                Direction = ((this.id % 2 == 0) ? GamepageSnake.Directions.down : GamepageSnake.Directions.up),
                Rect = new Rectangle
                {
                    Width = SIZEELEM,
                    Height = SIZEELEM,
        }
            });

            for (int i = 1; i < STARTLENGTH - 1; i++)
            {
                this.snakeTmp.Add(new SnakeElem
                {
                    X = snakeTmp[0].X,
                    Y = snakeTmp[0].Y + ((this.id % 2 == 0) ? -(i * SIZEELEM) : (i * SIZEELEM)),
                    Direction = ((this.id % 2 == 0) ? GamepageSnake.Directions.down : GamepageSnake.Directions.up),
                    Rect = new Rectangle
                    {
                        Width = SIZEELEM,
                        Height = SIZEELEM,
                    }
                });
            }

            this.snakeTmp.Add(new SnakeElem
            {
                X = this.snakeTmp[0].X,
                Y = this.snakeTmp[0].Y + ((this.id % 2 == 0) ? -(2 * SIZEELEM) : 2 * SIZEELEM),
                Direction = ((this.id % 2 == 0) ? GamepageSnake.Directions.down : GamepageSnake.Directions.up),
                Rect = new Rectangle
                {
                    Width = SIZEELEM,
                    Height = SIZEELEM,
                }
            });

            InitializePictures();
            snakeTmp[0].Rect.Fill = this.pictures["Head_" + this.snakeTmp[0].Direction.ToString() + "_" + this.color.ToString()];
            snakeTmp[snakeTmp.Count - 1].Rect.Fill = this.pictures["Tail_" + this.snakeTmp[STARTLENGTH - 1].Direction.ToString() + "_" + this.color.ToString()];

            foreach (SnakeElem snk in snakeTmp)
            {
                if(snk != snakeTmp[0] && snk != snakeTmp[snakeTmp.Count - 1])
                {
                    snk.Rect.Fill = this.Pictures["Elem"];
                }
                Canvas.SetLeft(snk.Rect, (int)snk.X);
                Canvas.SetTop(snk.Rect, (int)snk.Y);
            }

            return snakeTmp;
        }

        //methods
        public void MoveSnake()
        {
            if (this.snake == null || this.snake.Count == 0 || this.dead)
                return;

            SnakeElem head = this.snake[0];

            if (GamepageSnake.STARTED && !this.dead)
            {
                //move head
                head.X += ((head.Direction == GamepageSnake.Directions.left) ? -SIZEELEM:
                    (head.Direction == GamepageSnake.Directions.right) ? SIZEELEM : 0);
                head.Y += ((head.Direction == GamepageSnake.Directions.up) ? -SIZEELEM :
                    (head.Direction == GamepageSnake.Directions.down) ? SIZEELEM : 0);

                //detect collision with any other snakebody
                foreach (SnakePlayer p in GamepageSnake.Snakeplayers)
                {
                    if (p.dead)
                    {
                        continue;
                    }
                    foreach (SnakePlayer pl in GamepageSnake.Snakeplayers)
                    {
                        Console.WriteLine("p: " + p.name + " | pl: " + pl.name);
                        if (pl.dead || p == pl)
                        {
                            if (pl.dead)
                                Console.WriteLine(pl.name + " is dead, continue!");
                            else
                                Console.WriteLine("p = pl, continue!");
                            continue;
                        }

                        foreach (SnakeElem snk in pl.Snake)
                        {
                            if ((p.Snake[0].X < snk.X + snk.Rect.ActualWidth)
                                && (p.Snake[0].X > snk.X)
                                && (p.Snake[0].Y < snk.Y + snk.Rect.ActualHeight)
                                && (p.Snake[0].Y > snk.Y))
                            {
                                Console.WriteLine("in collision-if");

                                if (snk == pl.snake[0] && !TODIE.Contains(pl))
                                    TODIE.Add(pl);

                                if (!TODIE.Contains(p))
                                    TODIE.Add(p);
                            }
                        }
                    }
                }

                //detect collision with own snakebody
                if (!this.dead)
                {
                    foreach (SnakeElem snk in this.snake)
                    {
                        if (snk == head)
                            continue;

                        if ((head.X < snk.X + snk.Rect.ActualWidth)
                            && (head.X > snk.X)
                            && (head.Y < snk.Y + snk.Rect.ActualHeight)
                            && (head.Y > snk.Y))
                        {
                            this.dead = true;
                        }
                    }
                }

                //make body follow the head
                for (int i = this.snake.Count - 1; i > 0; i--)
                {
                    if (i > 1)
                    {
                        this.snake[i].X = this.snake[i - 1].X;
                        this.snake[i].Y = this.snake[i - 1].Y;
                    }

                    if (i == 1)
                    {
                        this.snake[i].X = head.X + ((head.Direction == GamepageSnake.Directions.right) ? -SIZEELEM :
                            (head.Direction == GamepageSnake.Directions.left) ? SIZEELEM : 0);
                        this.snake[i].Y = head.Y + ((head.Direction == GamepageSnake.Directions.down) ? -SIZEELEM :
                            (head.Direction == GamepageSnake.Directions.up) ? SIZEELEM : 0);
                    }
                    this.snake[i].Direction = this.snake[i - 1].Direction;

                }
            }

            this.snake[0] = head;
            this.SnakeEat();
        }

        public void Render()
        {
            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name))
                return;

            if (!this.dead && GamepageSnake.STARTED)
            {
                if (this.snake == null || this.snake.Count == 0 || this.gameCanvas == null || this.pictures == null || this.pictures.Count == 0)
                    return;

                SnakeElem head = this.snake[0];
                SnakeElem tail = this.snake[this.snake.Count - 1];

                //reload headpic for the new direction
                this.Pictures["Head"] = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("../../Images/" + this.Color + "/snakehead_" + this.Snake[0].Direction.ToString() + "_" + this.Color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                };
                this.snake[0].Rect.Fill = this.pictures["Head"];
                /*----------------------------------------------*/

                //wrap-around
                if (head.X < 0)
                    head.X = (int)(gameCanvas.ActualWidth - head.Rect.ActualWidth);
                if (head.X > gameCanvas.ActualWidth - head.Rect.ActualWidth)
                    head.X = 0;

                if (head.Y < 0)
                    head.Y = (int)(gameCanvas.ActualHeight - head.Rect.ActualHeight);
                if (head.Y > (gameCanvas.ActualHeight))
                    head.Y = 0;

                //set Position of snakeElements
                foreach (SnakeElem snk in this.snake)
                {
                    Canvas.SetZIndex(snk.Rect, 0);
                    Canvas.SetLeft(snk.Rect, (int)snk.X);
                    Canvas.SetTop(snk.Rect, (int)snk.Y);
                    if (this.gameCanvas.Children.Contains(snk.Rect))
                        this.gameCanvas.Children.Remove(snk.Rect);
                    if (snk == tail)
                    {
                        this.pictures[SnakeGame.Pictures.Tail.ToString()] = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snaketail_" + snk.Direction + "_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                        };
                        snk.Rect.Fill = this.pictures[SnakeGame.Pictures.Tail.ToString()];
                    }
                    this.gameCanvas.Children.Add(snk.Rect);
                }
            }
            if (GamepageSnake.APPLE == null || this.gameCanvas == null)
                return;

            if (!this.gameCanvas.Children.Contains(GamepageSnake.APPLE.Shape))
                this.gameCanvas.Children.Remove(GamepageSnake.APPLE.Shape);

            if (GamepageSnake.APPLE != null && !gameCanvas.Children.Contains(GamepageSnake.APPLE.Shape) && GamepageSnake.STARTED)
                gameCanvas.Children.Add(GamepageSnake.APPLE.Shape);
            
            this.MoveSnake();
        }

        public void SnakeEat()
        {
            if (this.snake != null && GamepageSnake.APPLE != null)
            {
                SnakeElem head = this.snake[0];

                if ((head.X <= GamepageSnake.APPLE.X + GamepageSnake.APPLE.Shape.ActualWidth)
                    && (head.X + head.Rect.ActualWidth >= GamepageSnake.APPLE.X)
                    && (head.Y <= GamepageSnake.APPLE.Y + GamepageSnake.APPLE.Shape.ActualHeight && head.Y + head.Rect.ActualHeight >= GamepageSnake.APPLE.Y))
                {
                    gameCanvas.Children.Remove(GamepageSnake.APPLE.Shape);
                    gameCanvas.Children.Remove(GamepageSnake.APPLE.Shape);
                    GamepageSnake.APPLE = null;
                    SnakeElem tail = this.snake[this.snake.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = tail.X + (int)SIZEELEM,
                        Y = tail.Y + (int)SIZEELEM,
                        Rect = new Rectangle
                        {
                            Fill = this.pictures[SnakeGame.Pictures.Tail.ToString()],
                            Width = SIZEELEM,
                            Height = SIZEELEM
                        }
                    };
                    this.snake.Add(snakeTmp);
                    this.snake[this.snake.Count - 2].Rect.Fill = this.pictures[SnakeGame.Pictures.Elem.ToString()];
                    this.score++;
                    this.UpdateScore();
                    UpdateRanking();
                }
            }
        }

        public void UpdateScore()
        {
            this.scoretext[0].Text = this.name + ": ";
            this.scoretext[1].Text = this.score.ToString();
            foreach (TextBlock tb in this.scoretext)
            {
                tb.Foreground = (this.Dead) ? Brushes.Gray : Brushes.Black;

                if (MainWindow.GamePage.GetScoreSP().Children.Contains(tb))
                    MainWindow.GamePage.GetScoreSP().Children.Remove(tb);

                MainWindow.GamePage.GetScoreSP().Children.Add(tb);
            }
        }

        public static void UpdateRanking()
        {
            GamepageSnake.Snakeplayers.Sort(delegate (SnakePlayer x, SnakePlayer y)
            {
                if (x.score == y.score)
                    return (x.id.CompareTo(y.id));
                else if (x.score > y.score)
                    return -1;
                else
                    return 1;
            });
            UpdateSurvivors();
        }

        public static void UpdateSurvivors()
        {
            SURVIVORS = 0;
            foreach (SnakePlayer p in GamepageSnake.Snakeplayers)
            {
                if (!p.dead)
                {
                    SURVIVORS++;
                }
            }
        }
    }
}
