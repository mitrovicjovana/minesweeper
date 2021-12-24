using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Logic
{
    public class Level
    {
        #region Attributes
        private int numberOfMines;
        private int boardSize;
        #endregion

        #region Constructor
        public Level(int numberOfMines, int boardSize)
        {
            this.numberOfMines = numberOfMines;
            this.boardSize = boardSize;
        }
        #endregion

        #region Getters
        public int getNumberOfMines() { return this.numberOfMines; }

        public int getBoardSize() { return this.boardSize; }
        #endregion

    }
}
