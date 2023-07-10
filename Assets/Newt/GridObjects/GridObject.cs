using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.GridObjects
{
    public class GridObject
    {
        #region Grid Position
        private Grid<GridObject> grid;

        private int x;
        private int y;
        #endregion

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;

            this.x = x;
            this.y = y;
        }
    }
}