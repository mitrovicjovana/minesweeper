using Minesweeper.Logic;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Pages
{
    public partial class GamePage : Page
    {
        #region Attributes
        public static Button[,] ButtonsArray;

        private int numberOfMines;
        private int boardSize;
        private int previousBoardSize = 0;
        private bool isFirstClick;

        private Level level;
        private Game game;
        private Stopwatch stopwatch;
        private Timer timer;
        #endregion

        #region Constructor
        public GamePage()
        {
            InitializeComponent();

            // Select easy level
            LevelPicker.SelectedIndex = 0;

            stopwatch = new Stopwatch();
            timer = new Timer(1000);
        }
        #endregion

        #region Timer
        /*
         * Start timer and stopwatch
         */
        private void startTime()
        {
            timer.Elapsed += OnTimerElapsed;

            stopwatch.Start();
            timer.Start();
        }

        /*
         * Stop timer and stopwatch
         */
        private void stopTime()
        {
            stopwatch.Stop();
            timer.Stop();
        }
        #endregion

        #region Game
        private void startGame()
        {
            isFirstClick = false;

            if (stopwatch != null) stopwatch.Reset();

            makeBoard();
            makeButtonArray();

            game = new Game(numberOfMines, boardSize);

            NumberOfMinesText.Text = numberOfMines.ToString();
            StopwatchText.Text = "00:00";
        }
        #endregion

        #region GameOver
        /*
         * Check for win/loss
         */
        private void checkIsGameOver()
        {
            bool isWon = game.checkIsGameWon();
            bool isLost = game.checkIsGameLost();

            // Remove all event listeners on buttons
            if (isWon || isLost)
            {
                foreach (Button button in ButtonsArray)
                {
                    button.PreviewMouseLeftButtonDown -= Field_PreviewMouseLeftButtonDown;
                    button.MouseRightButtonDown -= Field_MouseRightButtonDown;
                }

                stopTime();
            }

            // Game won
            if (isWon)
            {
                game.openSafeFields();
                Console.WriteLine("won");
                // NavigationService.Navigate(new WinPage());
            }
            //Game lost
            else if (isLost)
            {
                game.showMines();
                Console.WriteLine("lost");
                // NavigationService.Navigate(new LossPage());
            }
        }
        #endregion

        #region Fields
        /*
         * Creates array of all buttons in grid, so they can be accessed by row and column numbers
         */
        private void makeButtonArray()
        {
            ButtonsArray = new Button[boardSize, boardSize];

            int row = 0;
            int column = 0;

            UIElementCollection buttons = BoardGrid.Children;
            foreach (UIElement element in buttons)
            {
                ButtonsArray[row, column] = element as Button;
                column++;
                if (column == boardSize) { column = 0; row++; }
            }
        }

        /*
        * Calculates row and column of clicked button
        */
        private int[] getPosition(Grid grid)
        {
            var point = Mouse.GetPosition(grid);

            int row = 0;
            int column = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // Calculate row mouse was over
            foreach (var rowDefinition in grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // Calculate column mouse was over
            foreach (var columnDefinition in grid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                column++;

            }
            int[] position = { row, column };

            return position;
        }
        #endregion

        #region Board
        /*
         * Make button with all needed attributes
         */
        private Button makeButton(int row, int column)
        {
            Button button = new Button();
            button.PreviewMouseLeftButtonDown += Field_PreviewMouseLeftButtonDown;
            button.MouseRightButtonDown += Field_MouseRightButtonDown;

            button.Style = Application.Current.Resources["UnopenedButtonStyle"] as Style;

            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);

            return button;
        }

        /*
         * Add button to grid
         */
        private void addButtons()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    BoardGrid.Children.Add(makeButton(row, column));
                }
            }
        }

        /*
         * Create column and row definitions
         */
        private void makeGridLines()
        {
            for (int i = 0; i < boardSize; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(level.getFieldSize());

                BoardGrid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < boardSize; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(level.getFieldSize());

                BoardGrid.ColumnDefinitions.Add(column);
            }
        }

        /*
         * Remove all children, including row and column definitions
         */
        private void removeChildren()
        {
            if (previousBoardSize == 0) return;

            BoardGrid.Children.RemoveRange(0, previousBoardSize * previousBoardSize);
            BoardGrid.RowDefinitions.RemoveRange(0, previousBoardSize);
            BoardGrid.ColumnDefinitions.RemoveRange(0, previousBoardSize);
        }

        /*
         * Make gridlines and populate board with buttons
         */
        private void makeBoard()
        {
            removeChildren();
            makeGridLines();
            addButtons();

            previousBoardSize = boardSize;
        }
        #endregion

        #region Level
        /*
         * Set level depened on selected index
         */
        private void setLevel(int index)
        {
            switch (index)
            {
                case 0: level = new Level(12, 10, 30); break;
                case 1: level = new Level(45, 16, 25); break;
                case 2: level = new Level(99, 21, 25); break;
            }

            numberOfMines = level.getNumberOfMines();
            boardSize = level.getBoardSize();

            level.changeWindowsSize();
            startGame();
        }
        #endregion

        #region EventListeners
        /*
        * Opening field
        */
        private void Field_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isFirstClick) startTime();
            isFirstClick = true;

            // get position of clicked button
            int[] position = getPosition(BoardGrid);
            int row = position[0];
            int column = position[1];

            // logic for opening fields
            game.openField(sender as Button, row, column);

            checkIsGameOver();
        }

        /*
         * Mark field
         */
        private void Field_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isFirstClick) startTime();
            isFirstClick = true;

            // get position of clicked button
            int[] position = getPosition(BoardGrid);
            int row = position[0];
            int column = position[1];

            // logic for marking field
            game.markField(sender as Button, row, column);
            NumberOfMinesText.Text = game.getNumberOfMines().ToString();

            checkIsGameOver();
        }

        /*
         * Change level
         */
        private void LevelPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setLevel(LevelPicker.SelectedIndex);
        }

        /*
         * Close app
         */
        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /*
         * Restart game
         */
        private void RestartButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            startGame();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => StopwatchText.Text = stopwatch.Elapsed.ToString(@"mm\:ss"));
        }
        #endregion

    }
}
