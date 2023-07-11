using UnityEngine;
using Grids;

namespace Newt.RoomGeneration.ScriptableObjects
{
    using GridObjects;

    public abstract class GridObjectGeneratorSO : ScriptableObject
    {
        protected Transform transform;

        public virtual void Initialize(RoomGeneratorSO roomGenerator, Transform parent, Texture2D texture)
        {
            transform = parent;
        }

        public abstract GridObject CreateGridObject(Grid<GridObject> grid, int x, int y);

        public abstract void BuildGridObject(Grid<GridObject> grid, int x, int y);
    }
}
