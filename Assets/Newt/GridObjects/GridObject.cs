using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.GridObjects
{
    public class GridObject
    {
        public static System.Func<Grid<GridObject>, int, int, GridObject> create = (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y);

        #region Grid Position
        private Grid<GridObject> grid;

        private int x;
        private int y;
        #endregion

        public static GameObject GridObjectPrefab { get; set; }
        public static Transform GridParentTransform { get; set; }

        private GameObject gameObject;

        public GridObject(Grid<GridObject> grid, int x, int y)
        {
            this.grid = grid;

            this.x = x;
            this.y = y;

            if (GridObjectPrefab != null)
            {
                gameObject = Object.Instantiate(GridObjectPrefab, grid.GridToWorldPosition(x, y, false), Quaternion.identity, GridParentTransform);
                gameObject.name = $"( {x}, {y} )";
            }
        }
    }
}