using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CMM.Mobile.snake
{
    public partial class MainPage : ContentPage
    {
        protected readonly int gridSize = 20;
        public List<Point> snakePositions;
        public Point foodPosition;
        protected IDispatcherTimer gameTimer;
        protected Direction currentDirection = Direction.Right;
        protected bool isGameOver = false;
        protected bool isPaused = false;
        protected int score = 0;
        protected Label scoreLabel;
        protected Grid startScreen;
        protected Grid gameScreen;
        protected bool isGameInitialized = false;
        private const int gameSpeed = 100; // Oyun hızı (ms)

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public MainPage()
        {
            InitializeComponent();
            CreateStartScreen();
            CreateGameScreen();
            ShowStartScreen();
        }

        private void CreateStartScreen()
        {
            startScreen = new Grid
            {
                BackgroundColor = Colors.White,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };

            var startLayout = new VerticalStackLayout
            {
                Spacing = 20,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var titleLabel = new Label
            {
                Text = "Yılan Oyunu",
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };

            var startButton = new Button
            {
                Text = "Oyuna Başla",
                FontSize = 20,
                Padding = new Thickness(20, 10),
                BackgroundColor = Colors.Green,
                TextColor = Colors.White
            };
            startButton.Clicked += OnStartButtonClicked;

            startLayout.Children.Add(titleLabel);
            startLayout.Children.Add(startButton);
            startScreen.Children.Add(startLayout);
        }

        private void CreateGameScreen()
        {
            gameScreen = new Grid
            {
                IsVisible = false,
                BackgroundColor = Colors.White,
                Padding = new Thickness(20),
                   HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var gameLayout = new VerticalStackLayout
            {
                Spacing = 10,
                CascadeInputTransparent = false,
                InputTransparent = false,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            //scoreLabel = new Label
            //{
            //    Text = "Skor: 0",
            //    FontSize = 20,
            //    HorizontalOptions = LayoutOptions.Center,
            //    Margin = new Thickness(0, 10)
            //};

            gameGrid = new Grid
            {
                BackgroundColor = Colors.White,
                Margin = new Thickness(10),
                MinimumHeightRequest = 300,
                MinimumWidthRequest = 300,
                InputTransparent = false,
                CascadeInputTransparent = false,
                   HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            // Pan gesture'ü ekle
            //var panGesture = new SwipeGestureRecognizer();
            //panGesture.Swiped += PanGesture_Swiped;
            //gameLayout.GestureRecognizers.Clear();
            //gameLayout.GestureRecognizers.Add(panGesture);

            // Sağa kaydırma
            var rightSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Right, Threshold = 5 };
            rightSwipe.Swiped += (s, e) => {
                Debug.WriteLine("Sağa kaydırma algılandı");
                if (!isGameOver && !isPaused && !IsOppositeDirection(currentDirection, Direction.Right))
                {
                    currentDirection = Direction.Right;
                }
            };
            // Sola kaydırma
            var leftSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Left, Threshold=5 };
            leftSwipe.Swiped += (s, e) => {
                Debug.WriteLine("Sola kaydırma algılandı");
                if (!isGameOver && !isPaused && !IsOppositeDirection(currentDirection, Direction.Left))
                {
                    currentDirection = Direction.Left;
                }
            };
            // Yukarı kaydırma
            var upSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Up, Threshold = 50 };
            upSwipe.Swiped += (s, e) => {
                Debug.WriteLine("Yukarı kaydırma algılandı");
                if (!isGameOver && !isPaused && !IsOppositeDirection(currentDirection, Direction.Up))
                {
                    currentDirection = Direction.Up;
                }
            };
            // Aşağı kaydırma
            var downSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Down, Threshold = 50 };
            downSwipe.Swiped += (s, e) => {
                Debug.WriteLine("Aşağı kaydırma algılandı");
                if (!isGameOver && !isPaused && !IsOppositeDirection(currentDirection, Direction.Down))
                {
                    currentDirection = Direction.Down;
                }
            };
            // Önceki gesture'ları temizle ve yenilerini ekle
            gameGrid.GestureRecognizers.Clear();
            gameGrid.GestureRecognizers.Add(leftSwipe);
            gameGrid.GestureRecognizers.Add(upSwipe);
            gameGrid.GestureRecognizers.Add(downSwipe);
            gameGrid.GestureRecognizers.Add(rightSwipe);
            // Test için tap gesture
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                Debug.WriteLine($"Grid tapped at: {DateTime.Now}");
                Debug.WriteLine($"Current direction: {currentDirection}");
                Debug.WriteLine($"Grid InputTransparent: {gameGrid.InputTransparent}");
            };
            gameGrid.GestureRecognizers.Add(tapGesture);

            // Test için tap gesture
            //var tapGesture = new TapGestureRecognizer();
            //tapGesture.Tapped += (s, e) => 
            //{
            //    Debug.WriteLine("Grid tapped at: " + DateTime.Now);
            //    Debug.WriteLine($"Current direction: {currentDirection}");
            //};
            //gameGrid.GestureRecognizers.Add(tapGesture);

            var pauseButton = new Button
            {
                Text = "Duraklat",
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 10)
            };
            pauseButton.Clicked += OnPauseButtonClicked;

            gameLayout.Children.Add(scoreLabel);
            gameLayout.Children.Add(pauseButton);
            gameLayout.Children.Add(gameGrid);

            gameScreen.Children.Add(gameLayout);

            // Ana ekrana da pan gesture ekleyelim
            //gameScreen.GestureRecognizers.Add(panGesture);
        }

        private void PanGesture_Swiped(object? sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    currentDirection = Direction.Right;
                    break;
                case SwipeDirection.Left:
                    currentDirection = Direction.Left;
                    break;
                case SwipeDirection.Up:
                    currentDirection = Direction.Up;
                    break;
                case SwipeDirection.Down:
                    currentDirection = Direction.Down;
                    break;
                default:
                    break;
            }
        }

        private async void ShowStartScreen()
        {
            if (gameScreen != null) gameScreen.IsVisible = false;
            if (startScreen != null)
            {
                startScreen.Opacity = 0;
                startScreen.IsVisible = true;
                await startScreen.FadeTo(1, 500);
            }

            if (Content is VerticalStackLayout mainLayout)
            {
                mainLayout.Children.Clear();
                mainLayout.Children.Add(startScreen);
            }
        }

        private async void ShowGameScreen()
        {
            if (startScreen != null) startScreen.IsVisible = false;
            if (gameScreen != null)
            {
                gameScreen.Opacity = 0;
                gameScreen.IsVisible = true;
                await gameScreen.FadeTo(1, 500);
            }

            if (Content is VerticalStackLayout mainLayout)
            {
                mainLayout.Children.Clear();
                mainLayout.Children.Add(gameScreen);
            }
        }

        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            if (!isGameInitialized)
            {
                InitializeGame();
                isGameInitialized = true;
            }

            await Task.WhenAll(
                startScreen.FadeTo(0, 500),
                gameScreen.FadeTo(1, 500)
            );

            ShowGameScreen();
            isGameOver = false;
            score = 0;
            currentDirection = Direction.Right;
            snakePositions.Clear();
            snakePositions.Add(new Point(gridSize / 2, gridSize / 2));
            GenerateFood();
            UpdateGameGrid();
            gameTimer.Start();
        }

        private void OnPauseButtonClicked(object sender, EventArgs e)
        {
            if (gameTimer.IsRunning)
            {
                gameTimer.Stop();
                DisplayAlert("Oyun Duraklatıldı", "Devam etmek için Tamam'a basın", "Tamam")
                    .ContinueWith(_ => MainThread.BeginInvokeOnMainThread(() => gameTimer.Start()));
            }
        }

        protected async void OnGameOver()
        {
            isGameOver = true;
            gameTimer?.Stop();
            
            await DisplayAlert("Oyun Bitti!", $"Skorunuz: {score}", "Tamam");
            
            var result = await DisplayAlert("Yeniden Oyna", "Tekrar oynamak ister misiniz?", "Evet", "Hayır");
            
            if (result)
            {
                OnStartButtonClicked(this, EventArgs.Empty);
            }
            else
            {
                ShowStartScreen();
            }
        }

        private void InitializeGame()
        {
            scoreLabel = new Label
            {
                Text = "Skor: 0",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)
            };

            if (Content is VerticalStackLayout mainLayout)
            {
                mainLayout.Children.Insert(0, scoreLabel);
            }

            snakePositions = new List<Point>
            {
                new Point(gridSize / 2, gridSize / 2)
            };

            GenerateFood();

            gameTimer = Application.Current.Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(200);
            gameTimer.Tick += GameTimer_Tick;
        }

        private void GenerateFood()
        {
            Random random = new Random();
            bool validPosition;
            Point newFood;

            do
            {
                validPosition = true;
                newFood = new Point(
                    random.Next(0, gridSize),
                    random.Next(0, gridSize)
                );

                // Yemeğin yılanın üzerine gelmemesi için kontrol
                foreach (var pos in snakePositions)
                {
                    if (pos.X == newFood.X && pos.Y == newFood.Y)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            foodPosition = newFood;
        }

        private async void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdateSnake();
        }

        private void UpdateSnake()
        {
            if (isGameOver) return;

            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var head = snakePositions[0];
                    Point newHead = new Point(head.X, head.Y);

                    switch (currentDirection)
                    {
                        case Direction.Right:
                            newHead.X = (newHead.X + 1) % gridSize;
                            break;
                        case Direction.Left:
                            newHead.X = (newHead.X - 1 + gridSize) % gridSize;
                            break;
                        case Direction.Up:
                            newHead.Y = (newHead.Y - 1 + gridSize) % gridSize;
                            break;
                        case Direction.Down:
                            newHead.Y = (newHead.Y + 1) % gridSize;
                            break;
                    }

                    // Kendine çarpma kontrolü
                    if (snakePositions.Contains(newHead))
                    {
                        OnGameOver();
                        return;
                    }

                    snakePositions.Insert(0, newHead);

                    // Yemek yeme kontrolü
                    if (newHead == foodPosition)
                    {
                        score += 10;
                        GenerateFood();
                    }
                    else
                    {
                        snakePositions.RemoveAt(snakePositions.Count - 1);
                    }

                    UpdateGameGrid();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateSnake Error: {ex.Message}");
            }
        }

        bool firstDraw = true;
        public List<BoxView> Cells { get; set; } = new List<BoxView>();
        private void UpdateGameGrid()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (firstDraw)
                    {
                        // Grid'i temizle
                        gameGrid.Children.Clear();
                        gameGrid.RowDefinitions.Clear();
                        gameGrid.ColumnDefinitions.Clear();

                        // Grid boyutlarını ayarla
                        for (int i = 0; i < gridSize; i++)
                        {
                            gameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            gameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        }

                        // Arka plan hücreleri ekle
                        for (int row = 0; row < gridSize; row++)
                        {
                            for (int col = 0; col < gridSize; col++)
                            {
                                var cell = new BoxView
                                {
                                    Color = Colors.LightGray,
                                    Margin = 1
                                };
                                gameGrid.Add(cell, col, row);
                                Cells.Add(cell);
                            }
                        }

                        firstDraw = false;
                    }

                    foreach (var cell in Cells)
                    {
                        cell.Color = Colors.LightGray;
                    }

                    //// Yılan parçalarını ekle
                    foreach (var position in snakePositions)
                    {
                        var snakeSegment = new BoxView
                        {
                            Color = Colors.Green,
                            Margin = 1,
                            ZIndex = 1
                        };

                        Cells[(int)position.X * gridSize + (int)position.Y].Color = Colors.Green;

                        Debug.WriteLine($"Yılan parçası eklendi: X={position.X}, Y={position.Y}");
                    }

                    //// Yemeği ekle
                    //var food = new BoxView
                    //{
                    //    Color = Colors.Red,
                    //    Margin = 1,
                    //    ZIndex = 1
                    //};


                    //gameGrid.Add(food, (int)foodPosition.X, (int)foodPosition.Y);
                    //Debug.WriteLine($"Yemek eklendi: X={foodPosition.X}, Y={foodPosition.Y}");

                    // Skoru güncelle
                    if (scoreLabel != null)
                    {
                        scoreLabel.Text = $"Skor: {score}";
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateGameGrid Error: {ex.Message}");
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType != GestureStatus.Running || isGameOver || isPaused) return;

            try
            {
                var totalX = e.TotalX;
                var totalY = e.TotalY;
                var absX = Math.Abs(totalX);
                var absY = Math.Abs(totalY);

                if (Math.Max(absX, absY) < 30f) return;

                var newDirection = currentDirection;
                if (absX > absY)
                {
                    newDirection = totalX > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    newDirection = totalY > 0 ? Direction.Down : Direction.Up;
                }

                if (!IsOppositeDirection(currentDirection, newDirection))
                {
                    currentDirection = newDirection;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Pan error: {ex.Message}");
            }
        }
        private bool IsOppositeDirection(Direction current, Direction next)
        {

            return (current == Direction.Right && next == Direction.Left) ||
                   (current == Direction.Left && next == Direction.Right) ||
                   (current == Direction.Up && next == Direction.Down) ||
                   (current == Direction.Down && next == Direction.Up);
        }

        private async Task GameLoop()
        {
            while (!isGameOver && !isPaused)
            {
                try
                {
                    await Task.Delay(gameSpeed);
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        UpdateSnakePosition();
                        CheckCollisions();
                        UpdateGameGrid();
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Game loop error: {ex.Message}");
                }
            }
        }

        private void UpdateSnakePosition()
        {
            var head = snakePositions[0];
            var newHead = new Point(
                (head.X + GetDirectionOffset(currentDirection).X + gridSize) % gridSize,
                (head.Y + GetDirectionOffset(currentDirection).Y + gridSize) % gridSize
            );

            snakePositions.Insert(0, newHead);
            if (!CheckFoodCollision())
            {
                snakePositions.RemoveAt(snakePositions.Count - 1);
            }
        }

        private bool CheckFoodCollision()
        {
            var head = snakePositions[0];
            return head.X == foodPosition.X && head.Y == foodPosition.Y;
        }

        private Point GetDirectionOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return new Point(1, 0);
                case Direction.Left:
                    return new Point(-1, 0);
                case Direction.Up:
                    return new Point(0, -1);
                case Direction.Down:
                    return new Point(0, 1);
                default:
                    throw new ArgumentException("Invalid direction");
            }
        }

        private void CheckCollisions()
        {
            var head = snakePositions[0];

            // Yılanın kendisiyle çarpışma kontrolü
            for (int i = 1; i < snakePositions.Count; i++)
            {
                if (head.X == snakePositions[i].X && head.Y == snakePositions[i].Y)
                {
                    isGameOver = true;
                    return;
                }
            }

            // Yemek ile çarpışma kontrolü
            if (head.X == foodPosition.X && head.Y == foodPosition.Y)
            {
                score++;
                GenerateFood();
            }
        }

    }
}
