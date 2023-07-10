using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt
{
    using GridObjects;
    using RoomGeneration.Utilities;
    using RoomGeneration.Tiles;
    using UnityEngine.InputSystem;

    public class GridTest : MonoBehaviour
    {
        private Grid<GridObject> grid;

        [SerializeField] private Transform gridParentTransform;
        [SerializeField] private GameObject gridObjectPrefab;

        [Space]

        [SerializeField] private Texture2D texture;
        [SerializeField] private TileRuleset tileRuleset;

        private void Start()
        {
            if (gridParentTransform == null)
            {
                gridParentTransform = transform;
            }

            GridObject.GridObjectPrefab = gridObjectPrefab;
            GridObject.GridParentTransform = gridParentTransform;

            grid = new Grid<GridObject>(32, 32, 1, Vector2.one * -16, GridObject.create);

            grid.GenerateRoom(texture, tileRuleset);
        }
    }
}