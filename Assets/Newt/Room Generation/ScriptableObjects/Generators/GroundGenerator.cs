using Grids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.ScriptableObjects.Generators
{
    using GridObjects;
    using Tiles;

    [CreateAssetMenu(fileName = "New Ground Generator", menuName = "Scriptable Object/Generators/GridObject/Ground Generator")]
    public class GroundGenerator : GridObjectGeneratorSO
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private TileRuleset tileset;

        public override void BuildGridObject(Grid<GridObject> grid, int x, int y)
        {
            GridObject gridObject = grid.GetObject(x, y);

            if (!(gridObject is GroundGO))
            {
                return;
            }

            GroundGO ground = (GroundGO)gridObject;
            ground.TriggerVisualChanged(this, new GroundGO.OnVisualChangedEventArgs(tileset.GetValidSprite(GetAdjacents(grid, x, y))));
        }

        public override GridObject CreateGridObject(Grid<GridObject> grid, int x, int y)
        {
            return new GroundGO(grid, x, y, prefab, transform);
        }

        public static bool[] GetAdjacents(Grid<GridObject> grid, int xPos, int yPos)
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
                    adjacents[i] = gridObject is GroundGO || OutOfBounds(x, y);

                    i++;
                }
            }

            bool OutOfBounds(int x, int y)
            {
                int checkX = x + xPos;
                int checkY = y + yPos;

                return checkX < 0 || checkY < 0 || checkX >= grid.Width || checkY >= grid.Height;
            }

            return adjacents;
        }
    }

}