using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Logic
{
    public class Game
    {
        #region Attributes
        private int numberOfMines;
        private int boardSize;

        private Field[,] board;

        Random random = new Random();
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
        #endregion

        #region Misc functions
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

        #region Functions to handle board 
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
    }
}
