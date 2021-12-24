using Minesweeper.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Pages
{
    public partial class GamePage : Page
    {
        #region Attributes
        private int numberOfMines;
        private int boardSize;
        private int previousBoardSize = 0;

        private Level level;
        #endregion

        #region Constructor
        public GamePage()
        {
            InitializeComponent();

            // Select easy level
            LevelPicker.SelectedIndex = 0;
        }
        #endregion

        #region Game
        private void startGame()
        {
            makeBoard();
        }
        #endregion

        #region Board
        /*
         * Make button with all needed attributes
         */
        private Button makeButton(int row, int column)
        {
            Button button = new Button();
            button.PreviewMouseLeftButtonDown += Cell_PreviewMouseLeftButtonDown;
            button.MouseRightButtonDown += Cell_MouseRightButtonDown;

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
        private void Cell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        /*
         * Mark field
         */
        private void Cell_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

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
        #endregion

    }
}
