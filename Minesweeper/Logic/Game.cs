using Minesweeper.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.Logic
{
    public class Game
    {
        #region Attributes
        private int numberOfMines;
        private int boardSize;

        private Field[,] board;

        private Random random = new Random();
        #endregion

        #region Constructor
        public Game(int numberOfMines, int boardSize)
        {
            this.numberOfMines = numberOfMines;
            this.boardSize = boardSize;

            createBoard();
        }
        #endregion

        #region Getters
        public Field[,] getBoard() { return this.board; }

        public int getNumberOfMines() { return this.numberOfMines; }
        #endregion

        #region GameOver
        /*
         * Show all mines
         */
        public async void showMines()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Field field = board[row, column];
                    if (field.getIsMine()) GamePage.ButtonsArray[row, column].Style = Application.Current.Resources["MineButtonStyle"] as Style;
                    await Task.Delay(10);
                }
            }
        }

        /*
         * Open all unopened fields, that are not mines
         */
        public async void openSafeFields()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Field field = board[row, column];
                    Button button = GamePage.ButtonsArray[row, column];

                    if (!field.getIsOpened() && !field.getIsMine())
                    {
                        int mines = countNeighbourMines(getNeighbourFields(row, column));
                        button.Style = Application.Current.Resources["OpenedButtonStyle"] as Style;
                        button.Content = mines == 0 ? "" : mines.ToString();
                        await Task.Delay(10);
                    }
                }
            }
        }

        /*
         * Ckeck if any of conditions for win is fulfilled
         */
        public bool checkIsGameWon()
        {
            bool areAllMinesMarked = true;
            bool areAllSafeOpened = true;

            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Field field = board[row, column];

                    // All mines are marked
                    if (field.getIsMine()) areAllMinesMarked = areAllMinesMarked && field.getIsMarked();

                    //All marked are mines
                    if (field.getIsMarked()) areAllMinesMarked = areAllMinesMarked && field.getIsMine();

                    // All safe fields are opened
                    if (!field.getIsMine()) areAllSafeOpened = areAllSafeOpened && field.getIsOpened() && false;
                }
            }

            return areAllMinesMarked || areAllSafeOpened;
        }

        /*
         * Check if condition for loss is fulfilled
         */
        public bool checkIsGameLost()
        {
            bool isLost = false;

            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Field field = board[row, column];
                    if (field.getIsOpened() && field.getIsMine()) isLost = true;
                }
            }

            return isLost;
        }
        #endregion

        #region Fields
        /*
         * Returns number of neighbour mines 
         */
        public int countNeighbourMines(List<Field> fields)
        {
            int neighbourMines = 0;
            foreach (Field field in fields)
            {
                int x = field.getX();
                int y = field.getY();

                if (board[x, y].getIsMine()) neighbourMines++;
            }
            return neighbourMines;
        }

        /*
         * Returns list of neighbour fields
         */
        public List<Field> getNeighbourFields(int row, int column)
        {
            List<Field> fields = new List<Field>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int fieldRow = row + x;
                    int fieldColumn = column + y;

                    // if row or column index is out of bounds countinue
                    if (fieldRow < 0 || fieldRow >= boardSize || fieldColumn < 0 || fieldColumn >= boardSize) continue;
                    fields.Add(board[fieldRow, fieldColumn]);
                }
            }

            return fields;
        }

        /*
        *  Open selected field and all neighbour fields that are not mines
        */
        public void openField(Button button, int row, int column)
        {
            Field field = board[row, column];
            if (field.getIsOpened()) return;
            else if (field.getIsMarked()) return;
            else if (field.getIsMine())
            {
                field.setIsOpened(true);
                button.Style = Application.Current.Resources["MineButtonStyle"] as Style;
            }
            else
            {
                List<Field> neighbourFields = getNeighbourFields(row, column);
                int neighbourMines = countNeighbourMines(neighbourFields);

                field.setIsOpened(true);
                button.Style = Application.Current.Resources["OpenedButtonStyle"] as Style;

                if (neighbourMines == 0)
                {
                    foreach (Field neighbourField in neighbourFields)
                    {
                        int x = neighbourField.getX();
                        int y = neighbourField.getY();
                        openField(GamePage.ButtonsArray[x, y], x, y);
                    }
                }
                else
                {
                    button.Content = neighbourMines.ToString();
                    return;
                }

            }
        }

        /*
         * Mark field as mine
         */
        public void markField(Button button, int row, int column)
        {
            Field field = board[row, column];

            if (field.getIsOpened()) return;
            else if (field.getIsMarked())
            {
                field.setIsMarked(false);
                button.Style = Application.Current.Resources["UnopenedButtonStyle"] as Style;
                numberOfMines++;
            }
            else
            {
                field.setIsMarked(true);
                button.Style = Application.Current.Resources["MarkedButtonStyle"] as Style;
                numberOfMines--;
            }
        }
        #endregion

        #region Board 
        /*
         * Returns array of positions for each mine
         */
        private int[,] getMinePositions()
        {
            int counter = 0;
            int[,] minePositions = new int[numberOfMines, 2];

            while (numberOfMines > counter)
            {
                int[] position = getRandomPosition(boardSize);
                if (!isContained(minePositions, position))
                {
                    minePositions[counter, 0] = position[0];
                    minePositions[counter, 1] = position[1];
                    counter++;
                }
            }
            return minePositions;
        }

        /*
         * Populates board with mines
         */
        private void populateBoard()
        {
            // Array of positions of mines [x, y]
            int[,] minePositions = getMinePositions();

            for (int i = 0; i < minePositions.Length / 2; i++)
            {
                int x = minePositions[i, 0];
                int y = minePositions[i, 1];

                Field field = board[x, y];

                field.setIsMine(true);
            }
        }

        /*
         * Creates board without mines, then adds mines using populateBoard function
         */
        public void createBoard()
        {
            board = new Field[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    board[row, column] = new Field(row, column, false);
                }
            }

            populateBoard();
        }
        #endregion

        #region Misc
        /*
         * Checks if array is contained in 2d array
         */
        private bool isContained(int[,] array, int[] item)
        {
            bool isContained = false;

            for (int i = 0; i < array.Length / 2; i++)
            {
                if (array[i, 0] == item[0] && array[i, 1] == item[1]) isContained = true;
            }

            return isContained;
        }

        /*
         * Creates random nubmer in range [0, number-1]
         */
        private int[] getRandomPosition(int number)
        {
            int x = random.Next(number);
            int y = random.Next(number);

            int[] position = { x, y };

            return position;
        }
        #endregion
    }
}
