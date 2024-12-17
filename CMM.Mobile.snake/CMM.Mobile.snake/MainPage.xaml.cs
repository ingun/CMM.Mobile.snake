using System.Collections.Generic;
using System.Timers;

namespace CMM.Mobile.snake
{
    public partial class MainPage : ContentPage
    {
        private List<Point> snake;
        private Point food;
        private System.Timers.Timer gameTimer;
        private int gridSize = 20;
        private int cellSize = 20;
        private string direction = "Right";

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
            var touchEffect = new TouchEffect();
            touchEffect.TouchAction += OnTouchEffectAction;
            this.Effects.Add(touchEffect);
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (args.Type == TouchActionType.Released)
            {
                var touchPoint = args.Location;
                var centerX = this.Width / 2;
                var centerY = this.Height / 2;

                if (Math.Abs(touchPoint.X - centerX) > Math.Abs(touchPoint.Y - centerY))
                {
                    if (touchPoint.X > centerX && direction != "Left")
                        direction = "Right";
                    else if (touchPoint.X < centerX && direction != "Right")
                        direction = "Left";
                }
                else
                {
                    if (touchPoint.Y > centerY && direction != "Up")
                        direction = "Down";
                    else if (touchPoint.Y < centerY && direction != "Down")
                        direction = "Up";
                }
            }
        }

        private void InitializeGame()
        {
            snake = new List<Point> { new Point(5, 5) };
            PlaceFood();
            gameTimer = new System.Timers.Timer(200);
            gameTimer.Elapsed += OnGameTick;
            gameTimer.Start();
        }

        private void PlaceFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(gridSize), rand.Next(gridSize));
        }

        private void OnGameTick(object sender, ElapsedEventArgs e)
        {
            MoveSnake();
            CheckCollisions();
            UpdateGameGrid();
        }

        private void MoveSnake()
        {
            Point head = snake[0];
            Point newHead = direction switch
            {
                "Up" => new Point(head.X, head.Y - 1),
                "Down" => new Point(head.X, head.Y + 1),
                "Left" => new Point(head.X - 1, head.Y),
                "Right" => new Point(head.X + 1, head.Y),
                _ => head
            };
            snake.Insert(0, newHead);
            if (newHead != food)
            {
                snake.RemoveAt(snake.Count - 1);
            }
            else
            {
                PlaceFood();
            }
        }

        private void CheckCollisions()
        {
            Point head = snake[0];
            if (head.X < 0 || head.X >= gridSize || head.Y < 0 || head.Y >= gridSize || snake.Skip(1).Contains(head))
            {
                gameTimer.Stop();
                DisplayAlert("Game Over", "You lost!", "OK");
            }
        }

        private void UpdateGameGrid()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GameGrid.Children.Clear();
                var snakeCopy = snake.ToList();

                foreach (var segment in snakeCopy)
                {
                    BoxView box = new BoxView { Color = Colors.Green };
                    GameGrid.Children.Add(box);
                    Grid.SetColumn(box, (int)segment.X);
                    Grid.SetRow(box, (int)segment.Y);
                }
                BoxView foodBox = new BoxView { Color = Colors.Red };
                GameGrid.Children.Add(foodBox);
                Grid.SetColumn(foodBox, (int)food.X);
                Grid.SetRow(foodBox, (int)food.Y);
            });
        }
    }
}
