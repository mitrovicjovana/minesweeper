using Minesweeper.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Minesweeper.Logic
{
    public class Level
    {
        #region Attributes
        private int numberOfMines;
        private int boardSize;
        private int fieldSize;
        #endregion

        #region Constructor
        public Level(int numberOfMines, int boardSize, int fieldSize)
        {
            this.numberOfMines = numberOfMines;
            this.boardSize = boardSize;
            this.fieldSize = fieldSize;
        }
        #endregion

        #region Getters
        public int getNumberOfMines() { return this.numberOfMines; }

        public int getBoardSize() { return this.boardSize; }

        public int getFieldSize() { return this.fieldSize; }
        #endregion

        /*
        * Adjust window size to board size
        */
        public void changeWindowsSize()
        {
            // Windows size
            Application.Current.MainWindow.Width = boardSize * fieldSize;
            Application.Current.MainWindow.Height = boardSize * fieldSize + 90;
        }

    }
}
