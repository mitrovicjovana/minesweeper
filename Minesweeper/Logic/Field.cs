using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Logic
{
    public class Field
    {
        #region Attributes
        private int x;
        private int y;
        private bool isMine;
        private bool isOpened;
        private bool isMarked;
        #endregion

        #region Constructor
        public Field(int x, int y, bool isMine)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

        #region Getters
        public int getX() { return this.x; }

        public int getY() { return this.y; }

        public bool getIsMine() { return this.isMine; }

        public bool getIsOpened() { return this.isOpened; }

        public bool getIsMarked() { return this.isMarked; }
        #endregion

        #region Setters
        public void setIsMine(bool isMine) { this.isMine = isMine; }

        public void setIsOpened(bool isOpened) { this.isOpened = isOpened; }
        #endregion
    }
}
