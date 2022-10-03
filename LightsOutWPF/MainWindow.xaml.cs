using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LightsOutWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<LightsOutLevels> _levels;
        private LightsOutGame _game;
        private int _currentLevel = 0;
        private int _wonGames = 0;
        private int _moveCounter = 0;
        private int _cellLength = 100;
        
        public MainWindow()
        {
            InitializeComponent();

            var levelsJsonFile = File.ReadAllText("Resources/Levels/lights-out-levels.json");
            _levels = JsonConvert.DeserializeObject<List<LightsOutLevels>>(levelsJsonFile);

            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Title = $"Lights Out Game - Level {_currentLevel + 1}";

            _wonGames = 0;
            _moveCounter = 0;
            gameTable.Children.Clear();

            var converter = new BrushConverter();
            gameTable.Background = (Brush)converter.ConvertFromString("#FF4848");

            DisplayGameInfo();

            _game = new LightsOutGame(_levels[_currentLevel]);
            _cellLength = (int)this.gameTable.Width / _game.CurrentLevel.Columns;

            for (int row = 0; row < _game.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < _game.CurrentLevel.Columns; column++)
                {
                    Rectangle rect = new Rectangle();

                    rect.Fill = Brushes.White;
                    rect.Width = _cellLength + 1;
                    rect.Height = rect.Width + 1;
                    rect.Tag = new Point(row, column);

                    rect.MouseLeftButtonDown += LightClicked;

                    Canvas.SetTop(rect, row * _cellLength);
                    Canvas.SetLeft(rect, column * _cellLength);

                    gameTable.Children.Add(rect);
                }
            }

            DrawGameTable();
        }

        private void DrawGameTable()
        {
            int index = 0;
            for (int row = 0; row < _game.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < _game.CurrentLevel.Columns; column++)
                {
                    Rectangle rect = gameTable.Children[index] as Rectangle;

                    index++;

                    if (_game.GetGameValue(row, column))
                        rect.Fill = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(@"Resources/Images/IMAGE_on_1.png", UriKind.Relative))
                        };
                    else
                        rect.Fill = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(@"Resources/Images/IMAGE_off_1.png", UriKind.Relative))
                        };
                }
            }
        }

        private void LightClicked(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;

            _game.ProcessLightSwitch(row, col);
            _moveCounter++;

            DrawGameTable();

            if (_game.IsGameOver())
            {
                _wonGames++;
                var converter = new BrushConverter();
                gameTable.Background = (Brush)converter.ConvertFromString("#5ED16B");
                MessageBox.Show(this, "YOU WIN THIS BATTLE..", "Lights Out Game!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            DisplayGameInfo();

            e.Handled = true;
        }

        private void RestartMenu_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Level1Menu_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel = 0;

            InitializeGame();
        }

        private void Level2Mennu_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel = 1;

            InitializeGame();
        }

        private void Level3Menu_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel = 2;

            InitializeGame();
        }

        private void DisplayGameInfo()
        {
            lblTrophy.Content = _wonGames.ToString();
            lblMovieCount.Content = _moveCounter.ToString();
        }
    }
}
