using UnityEngine;
using Grids;

namespace Newt.RoomGeneration.ScriptableObjects
{
    using GridObjects;

    public abstract class GridObjectGenerator : ScriptableObject
    {
        public abstract GridObject CreateGridObject(Grid<GridObject> grid, int x, int y);
    }
}
