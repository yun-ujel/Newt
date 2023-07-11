using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grids;

namespace Newt.RoomGeneration.ScriptableObjects
{
    using GridObjects;

    [CreateAssetMenu(fileName = "New Room Generator", menuName = "Scriptable Object/Generators/Room Generator")]
    public class RoomGeneratorSO : ScriptableObject
    {
        [System.Serializable]
        public class GridObjectDefinition
        {
            [field: SerializeField] public Color Color { get; set; }
            [field: SerializeField] public GridObjectGeneratorSO Generator { get; set; }
        }

        [SerializeField] private GridObjectDefinition[] generators;

        public void GenerateRoom(Texture2D texture, Vector2 origin)
        {
            GameObject roomParent = new GameObject("Room");

            for (int i = 0; i < generators.Length; i++)
            {
                generators[i].Generator.Initialize(roomParent.transform);
            }

            Grid<GridObject> grid = new Grid<GridObject>
            (
                texture.width,
                texture.height,
                1,
                origin,
                (Grid<GridObject> g, int x, int y) => GenerateGridObject(g, x, y, texture)
            );

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    for (int i = 0; i < generators.Length; i++)
                    {
                        generators[i].Generator.BuildGridObject(grid, x, y);
                    }
                }
            }
        }

        public GridObject GenerateGridObject(Grid<GridObject> g, int x, int y, Texture2D texture)
        {
            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i].Color == texture.GetPixel(x, y))
                {
                    return generators[i].Generator.CreateGridObject(g, x, y);
                }
            }

            return null;
        }
    }

}