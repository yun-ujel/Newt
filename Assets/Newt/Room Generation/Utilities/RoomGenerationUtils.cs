using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.RoomGeneration.Utilities
{
    using GridObjects;

    public static class RoomGenerationUtils
    {
        public static void GenerateRoom(this Grid<GridObject> grid, Texture2D texture)
        {
            if (!texture.isReadable)
            {
                Debug.Log($"Texture {texture} is not readable. Level loading cancelled.");
                return;
            }

            if (grid.Height != texture.height || grid.Width != grid.Width)
            {
                Debug.Log($"Texture {texture} doesn't match the grid dimensions. Level loading cancelled.");
                return;
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    if (texture.GetPixel(x, y).a <= 0f)
                    {
                        grid.GetObject(x, y).UpdateVisual(Color.clear);
                    }
                    else
                    {
                        grid.GetObject(x, y).UpdateVisual(Color.black);
                    }
                }
            }
        }
    }
}
