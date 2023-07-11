using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.GridObjects
{
    public abstract class GridObject
    {
        #region Grid Position
        protected Grid<GridObject> grid;

        protected int x;
        protected int y;
        #endregion

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;

            this.x = x;
            this.y = y;
        }
    }
}