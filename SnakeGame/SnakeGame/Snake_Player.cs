using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private TextBlock scoretext;
        private Dictionary<String, ImageBrush> pictures = new Dictionary<String, ImageBrush>(3);
        private List<SnakeElem> snake = new List<SnakeElem>();
        private IPEndPoint address;
        private bool dead = false;
        private Canvas gameCanvas;
        public int score = 0;
        public List<SnakeElem> snakeTmp = new List<SnakeElem>();

        //globals
        public static int SIZEELEM = 14;
        private int STARTLENGTH = 3;
        public static int AMOUNT_PLAYERS = 0;

        //Properties
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Colors Color { get => color; set => color = value; }
        public Dictionary<string, ImageBrush> Pictures { get => pictures; set => pictures = value; }
        public List<SnakeElem> Snake { get => snake; set => snake = value; }
        public bool Dead { get => dead; set => dead = value; }
        public Canvas GameCanvas { get => gameCanvas; set => gameCanvas = value; }
        public TextBlock Scoretext { get => scoretext; set => scoretext = value; }

        //c'tor
        public SnakePlayer(String name, IPEndPoint adr, Canvas gamecanv)
        {
            if (AMOUNT_PLAYERS > 3)
                return;

            this.id = AMOUNT_PLAYERS;
            this.name = name;
            this.gameCanvas = gamecanv;
            this.dead = false;
            this.score = 0;
            this.scoretext = new TextBlock
            {
                Name = "TxtScorePlayer" + this.Id,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(25, 0, 0, 0)
            };
            this.color = (Colors) (AMOUNT_PLAYERS - 1);
            this.address = adr;
            InitializeColors();

            if (this.gameCanvas != null)
                this.snake = InitializeSnake();

            this.UpdateScore();
            foreach(SnakeElem snk in this.snake)
            {
                this.gameCanvas.Children.Add(snk.Rect);
            }
        }

        //methods
        public void InitializeColors()
        {
            pictures.Add(SnakeGame.Pictures.Elem.ToString(), new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakeElem_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
            });

            pictures.Add(SnakeGame.Pictures.Head.ToString(), new ImageBrush
            {
                ImageSource = ((AMOUNT_PLAYERS % 2 == 0) ? new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakehead_down_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snakehead_up_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute)))
            });

            pictures.Add(SnakeGame.Pictures.Tail.ToString(), new ImageBrush
            {
                ImageSource = ((AMOUNT_PLAYERS % 2 == 0) ? new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snaketail_down_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute)) : new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snaketail_up_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute)))
            });
        }

        public List<SnakeElem> InitializeSnake()
        {
            this.snakeTmp.Add(
                new SnakeElem
                {
                    X = ((AMOUNT_PLAYERS % 2 == 0) ? 100 : (this.gameCanvas.ActualWidth - 100)),
                    Y = ((AMOUNT_PLAYERS % 2 == 0) ? (this.gameCanvas.ActualHeight - 100) : 100),
                    Direction = ((AMOUNT_PLAYERS % 2 == 0) ? GamepageSnake.Directions.up : GamepageSnake.Directions.down),
                    Rect = new Rectangle
                    {
                        Fill = this.pictures["Head"],
                        Width = SIZEELEM,
                        Height = SIZEELEM
                    }
                });

            Canvas.SetLeft(snakeTmp[0].Rect, (int)snakeTmp[0].X);
            Canvas.SetTop(snakeTmp[0].Rect, (int)snakeTmp[0].Y);

            for (int i = 1; i < STARTLENGTH - 1; i++)
            {
                this.snakeTmp.Add(new SnakeElem
                {
                    X = 100,
                    Y = 100 - (i + 1) * SIZEELEM,
                    Direction = ((AMOUNT_PLAYERS % 2 == 0) ? GamepageSnake.Directions.up : GamepageSnake.Directions.down),
                    Rect = new Rectangle
                    {
                        Fill = this.pictures["Elem"],
                        Width = SIZEELEM,
                        Height = SIZEELEM
                    }
                });

                Console.WriteLine(i);
                Canvas.SetLeft(snakeTmp[i].Rect, (int)snakeTmp[i].X);
                Canvas.SetTop(snakeTmp[i].Rect, (int)snakeTmp[i].Y);
            }

            this.snakeTmp.Add(new SnakeElem
            {
                X = 100,
                Y = 100 - (this.snake.Count) * SIZEELEM,
                Direction = ((AMOUNT_PLAYERS % 2 == 0) ? GamepageSnake.Directions.up : GamepageSnake.Directions.down),
                Rect = new Rectangle
                {
                    Fill = this.pictures["Tail"],
                    Width = SIZEELEM,
                    Height = SIZEELEM
                }
            });

            return snakeTmp;
        }

        public void MoveSnake()
        {
            if (App.Current.MainWindow.Content.GetType().Name != (typeof(GamepageSnake).Name))
                return;

            SnakeElem head = snake[0];

            if (GamepageSnake.started)
            {
                //move head
                head.X += ((head.Direction == GamepageSnake.Directions.left) ? -SIZEELEM :
                    (head.Direction == GamepageSnake.Directions.right) ? SIZEELEM : 0);
                head.Y += ((head.Direction == GamepageSnake.Directions.up) ? -SIZEELEM :
                    (head.Direction == GamepageSnake.Directions.down) ? SIZEELEM : 0);
            }

            //detect collision with own body
            foreach (SnakeElem snk in this.snake)
            {
                if (snk == head)
                    continue;

                if ((head.X < snk.X + snk.Rect.ActualWidth)
                    && (head.X + head.Rect.ActualWidth > snk.X)
                    && (head.Y < snk.Y + snk.Rect.ActualHeight)
                    && (head.Y + head.Rect.ActualHeight > snk.Y))
                {
                    this.dead = true;
                }
            }

            if (this.gameCanvas.IsInitialized)
            {
                for (int i = this.snake.Count - 1; i > 0; i--)
                {
                    //make body follow the head
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
        }

        public void Render()
        {
            if (this.snake == null || this.snake.Count == 0 || this.gameCanvas == null)
                return;

            bool helpVar = false;
            SnakeElem head = this.snake[0];
            SnakeElem tail = this.snake[this.snake.Count - 1];

            if (!this.dead && GamepageSnake.started)
            {
                //wrap-around
                if (head.X < 0)
                    head.X = gameCanvas.ActualWidth - head.Rect.ActualWidth;
                if (head.X > gameCanvas.ActualWidth - head.Rect.ActualWidth)
                    head.X = 0;

                if (head.Y < 0)
                    head.Y = gameCanvas.ActualHeight - head.Rect.ActualHeight;
                if (head.Y > (gameCanvas.ActualHeight))
                    head.Y = 0;

                //set Position of snakeElements
                foreach (SnakeElem snk in this.snake)
                {
                    if (snk == tail)
                    {
                        this.pictures[SnakeGame.Pictures.Tail.ToString()] = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("../../Images/" + this.color.ToString() + "/snaketail_" + snk.Direction + "_" + this.color.ToString() + ".png", UriKind.RelativeOrAbsolute))
                        };
                        snk.Rect.Fill = this.pictures[SnakeGame.Pictures.Tail.ToString()];
                    }

                    Canvas.SetZIndex(snk.Rect, 0);
                    Canvas.SetLeft(snk.Rect, (int)snk.X);
                    Canvas.SetTop(snk.Rect, (int)snk.Y);
                    for (int i = 0; i < STARTLENGTH; i++)
                    {
                        if (snk == this.snake[i])
                            helpVar = true;
                    }
                    if(helpVar)
                    {
                        helpVar = false;
                        continue;
                    }
                }
            }
            gameCanvas.Children.Clear();
            if(GamepageSnake.apple != null)
                gameCanvas.Children.Add(GamepageSnake.apple.Shape);

            foreach (SnakeElem snk in this.snake)
            {
                gameCanvas.Children.Add(snk.Rect);
            }
            this.MoveSnake();
            this.SnakeEat();
            this.UpdateScore();
        }

        public void SnakeEat()
        {
            if (this.snake != null && GamepageSnake.apple != null)
            {
                SnakeElem head = this.snake[0];

                if ((head.X <= GamepageSnake.apple.X + GamepageSnake.apple.Shape.ActualWidth)
                    && (head.X + head.Rect.ActualWidth >= GamepageSnake.apple.X)
                    && (head.Y <= GamepageSnake.apple.Y + GamepageSnake.apple.Shape.ActualHeight && head.Y + head.Rect.ActualHeight >= GamepageSnake.apple.Y))
                {
                    gameCanvas.Children.Remove(GamepageSnake.apple.Shape);
                    gameCanvas.Children.Remove(GamepageSnake.apple.Shape);
                    GamepageSnake.apple = null;
                    SnakeElem tail = this.snake[this.snake.Count - 1];
                    SnakeElem snakeTmp = new SnakeElem
                    {
                        X = tail.X + 1,
                        Y = tail.Y + 1,
                        Rect = new Rectangle
                        {
                            Fill = this.pictures[SnakeGame.Pictures.Tail.ToString()],
                            Width = SIZEELEM,
                            Height = SIZEELEM
                        }
                    };
                    this.snake.Add(snakeTmp);
                    this.snake[this.snake.Count - 2].Rect.Fill = this.pictures[SnakeGame.Pictures.Elem.ToString()];
                    this.Render();
                    this.score++;
                    this.UpdateScore();
                }
            }
        }

        public void UpdateScore()
        {
            this.scoretext.Text = this.score.ToString();
            this.scoretext.Foreground = (this.Dead) ? Brushes.Gray : Brushes.Black;
        }

        public void GameOver()
        {
            this.dead = true;
        }
    }
}
