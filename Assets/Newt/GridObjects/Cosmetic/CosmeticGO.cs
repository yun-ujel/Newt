using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.GridObjects
{
    public class CosmeticGO : GridObject
    {
        public event System.EventHandler<OnVisualChangedEventArgs> OnVisualChangedEvent;

        public class OnVisualChangedEventArgs : System.EventArgs
        {
            public Sprite Sprite { get; private set; }

            public OnVisualChangedEventArgs(Sprite sprite)
            {
                Sprite = sprite;
            }
        }

        public CosmeticGO(Grid<GridObject> grid, int x, int y, GameObject prefab, Transform parent = null) : base(grid, x, y)
        {
            GameObject gameObject = Object.Instantiate(prefab, grid.GridToWorldPosition(x, y, false), Quaternion.identity, parent);
            gameObject.name = $"( {x}, {y} )";

            gameObject.GetComponent<CosmeticBehaviour>().Initialize(this);
        }

        public void TriggerVisualChanged(object sender, OnVisualChangedEventArgs args)
        {
            OnVisualChangedEvent?.Invoke(sender, args);
        }
    }
}
