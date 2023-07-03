using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.RoomGeneration.Utilities
{
    using GridObjects;
    using Tiles;

    public static class RoomGenerationUtils
    {
        public static void GenerateRoom(this Grid<GridObject> grid, Texture2D texture, TileRuleset tileRuleset)
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
                    grid.GetObject(x, y).IsEmpty = texture.GetPixel(x, y).a <= 0f;
                }
            }

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    GridObject gridObject = grid.GetObject(x, y);

                    if (gridObject.IsEmpty)
                    {
                        gridObject.UpdateVisual(Color.clear);
                    }
                    else
                    {
                        gridObject.UpdateVisual(Color.white, tileRuleset.GetValidSprite(grid.GetAdjacentsToTile(x, y)));
                    }
                }
            }
        }

        public static bool[] GetAdjacentsToTile(this Grid<GridObject> grid, int xPos, int yPos)
        {
            bool[] adjacents = new bool[8];
            int i = 0;

            for (int y = 1; y >= -1; y--)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (y == 0 && x == 0)
                    {
                        continue;
                    }
                    GridObject gridObject = grid.GetObject(xPos + x, yPos + y);
                    adjacents[i] = gridObject != null && !gridObject.IsEmpty;

                    i++;
                }
            }

            return adjacents;
        }
    }
}
