using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeGame
{
    public enum Directions
    {
        right,
        down,
        left,
        up
    };

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
        private int id;
        private String name;
        private Colors color;
        private TextBlock[] scoretext = new TextBlock[2];
        private Dictionary<String, ImageBrush> pictures = new Dictionary<String, ImageBrush>();
        private List<SnakeElem> snake = new List<SnakeElem>(), snakeTmp = new List<SnakeElem>();
        private IPEndPoint address;
        private Directions direction, disabledDirection;
        private Canvas gameCanvas;
        public int score;

        //globals
        public static int CURPARTICIPANTS = 0, SIZEELEM = 14, STARTLENGTH = 5;
        public static List<SnakePlayer> TODIE = new List<SnakePlayer>();

        //Properties
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Colors Color { get => color; set => color = value; }
        public Dictionary<string, ImageBrush> Pictures { get => pictures; set => pictures = value; }
        public List<SnakeElem> Snake { get => snake; set => snake = value; }
        public Canvas GameCanvas { get => gameCanvas; set => gameCanvas = value; }
        public TextBlock[] Scoretext { get => scoretext; set => scoretext = value; }
        public Directions Direction { get => direction; set => direction = value; }
        public Directions DisabledDirection { get => disabledDirection; set => disabledDirection = value; }
        public IPEndPoint Address { get => address; set => address = value; }

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
            this.score = 0;
            this.color = (Colors)(CURPARTICIPANTS - 1);
            this.scoretext[0] = new TextBlock
            {
                Name = "LblScorePlayer" + this.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Text = name + ":",
                Foreground = (SolidColorBrush)(new BrushConverter().ConvertFromString(this.color.ToString()))
        };
            this.Scoretext[1] = new TextBlock
            {
                Name = "TxtScorePlayer" + this.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Text = this.score.ToString(),
                Foreground = (SolidColorBrush)(new BrushConverter().ConvertFromString(this.color.ToString()))
            };
            this.address = adr;

            this.snake = InitializeSnake();
            this.disabledDirection = ((int)this.snake[0].Direction < 2) ? (Directions)((int)this.snake[0].Direction + 2) : (Directions)((int)this.snake[0].Direction - 2);

            foreach (SnakeElem snk in this.snake)
            {
                this.gameCanvas.Children.Add(snk.Rect);
            }

            this.direction = this.snake[0].Direction;
        }

        //initializations
        public void InitializePictures()
        {
            this.pictures.Add(SnakeGame.Pictures.Elem.ToString(), new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakeElem_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
            });
            foreach (Directions dir in Enum.GetValues(typeof(Directions)))
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
                X = ((this.id % 2 == 0) ? (50) : ((int)this.GameCanvas.ActualWidth - 50)) +  SIZEELEM,
                Y = ((this.id < 2) ? 50 + SIZEELEM : ((int)this.GameCanvas.ActualHeight - 50)),
                Direction = ((this.id < 2) ? Directions.down : Directions.up),
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
                    Y = snakeTmp[0].Y + ((this.id < 2) ? -(i * SIZEELEM) : (i * SIZEELEM)),
                    Direction = ((this.id < 2) ? Directions.down : Directions.up),
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
                Y = this.snakeTmp[0].Y + ((this.id < 2) ? -((STARTLENGTH - 1) * SIZEELEM) : (STARTLENGTH - 1) * SIZEELEM),
                Direction = ((this.id < 2) ? Directions.down : Directions.up),
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
            SnakeElem head = this.snake[0];
            
            if (head.X < 0)
                head.X += (int)this.gameCanvas.ActualWidth;

            else if (head.X > (this.gameCanvas.ActualWidth - head.Rect.ActualWidth))
                head.X -= (int)this.gameCanvas.ActualWidth;

            else if (head.Y < 0)
                head.Y += (int)this.gameCanvas.ActualHeight;

            else if (head.Y > (this.gameCanvas.ActualHeight- head.Rect.ActualHeight))
                head.Y -= (int)this.gameCanvas.ActualHeight;

            if (this.snake == null || this.snake.Count == 0)
                return;

            if (GamepageSnake.STARTED)
            {
                //move head
                head.X += ((head.Direction == Directions.left) ? -SIZEELEM:
                    (head.Direction == Directions.right) ? SIZEELEM : 0);
                head.Y += ((head.Direction == Directions.up) ? -SIZEELEM :
                    (head.Direction == Directions.down) ? SIZEELEM : 0);
                
                //make body follow the head
                for (int i = this.snake.Count - 1; i > 0; i--)
                {
                    this.snake[i].X = this.snake[i - 1].X;
                    this.snake[i].Y = this.snake[i - 1].Y;
                    this.snake[i].Direction = this.snake[i - 1].Direction;
                }
            }
            this.SnakeEat();

            foreach (SnakeElem snk in this.snake) {
                Canvas.SetLeft(snk.Rect, snk.X);
                Canvas.SetTop(snk.Rect, snk.Y);
            }
            
            foreach (SnakeElem snk in this.snake)
            {
                if (!this.gameCanvas.Children.Contains(snk.Rect))
                    this.gameCanvas.Children.Add(snk.Rect);
            }
        }
        public void Render()
        {
            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name) || !GamepageSnake.STARTED)
                return;

            this.MoveSnake();

            //reload head- and tailpic for the new direction
            this.snake[0].Rect.Fill = this.pictures["Head_" + this.snake[0].Direction.ToString() + "_" + this.color.ToString()];
            this.snake[this.snake.Count - 1].Rect.Fill = this.pictures["Tail_" + this.snake[this.snake.Count - 1].Direction.ToString() + "_" + this.color.ToString()];


            foreach (SnakeElem snk in this.snake)
            {
                if (this.gameCanvas.Children.Contains(snk.Rect))
                    this.GameCanvas.Children.Remove(snk.Rect);

                this.gameCanvas.Children.Add(snk.Rect);
            }


            if (GamepageSnake.APPLE == null || this.gameCanvas == null)
                return;

            if (!this.gameCanvas.Children.Contains(GamepageSnake.APPLE.Shape))
                this.gameCanvas.Children.Remove(GamepageSnake.APPLE.Shape);

            if (GamepageSnake.APPLE != null && !gameCanvas.Children.Contains(GamepageSnake.APPLE.Shape) && GamepageSnake.STARTED)
                gameCanvas.Children.Add(GamepageSnake.APPLE.Shape);
        }
        public void SnakeEat()
        {
            if (GamepageSnake.APPLE != null)
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
                    SnakeElem snakeElemTmp = new SnakeElem
                    {
                        X = tail.X + ((this.snake[this.snake.Count - 1].Direction == Directions.right) ? -(int)SIZEELEM : ((this.snake[this.snake.Count - 1].Direction == Directions.left) ? (int)SIZEELEM : 0)),
                        Y = tail.Y + ((this.snake[this.snake.Count - 1].Direction == Directions.down) ? -(int)SIZEELEM : ((this.snake[this.snake.Count - 1].Direction == Directions.up) ? (int)SIZEELEM : 0)),
                        Rect = new Rectangle
                        {
                            Width = SIZEELEM,
                            Height = SIZEELEM
                        }
                    };
                    snakeElemTmp.Rect.Fill = this.pictures["Tail_" + this.snake[this.snake.Count - 1].Direction.ToString() + "_" + this.color.ToString()];
                    this.snake.Add(snakeElemTmp);
                    this.snake[this.snake.Count - 2].Rect.Fill = this.pictures[SnakeGame.Pictures.Elem.ToString()];
                    this.score++;
                }
            }
        }
    }
}
