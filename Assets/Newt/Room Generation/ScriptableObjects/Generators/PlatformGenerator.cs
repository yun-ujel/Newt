using Grids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.ScriptableObjects.Generators
{
    using GridObjects;

    [CreateAssetMenu(fileName = "New Platform Generator", menuName = "Scriptable Object/Generators/GridObject/Platform Generator")]
    public class PlatformGenerator : GridObjectGeneratorSO
    {
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject visualPrefab;

        [SerializeField] private Sprite sprite;

        private class Platform
        {
            public Vector2Int StartPos { get; set; }
            public Vector2Int EndPos { get; set; }

            public Platform(Vector2Int startPos, Vector2Int endPos)
            {
                StartPos = startPos;
                EndPos = endPos;
            }
        }
        private List<Platform> platforms;
        private List<Vector2Int> buildPositions;

        public override void Initialize(RoomGeneratorSO roomGenerator, Transform parent, Texture2D texture)
        {
            base.Initialize(roomGenerator, parent, texture);

            platforms = new List<Platform>();
            buildPositions = new List<Vector2Int>();

            roomGenerator.OnGridBuiltEvent += OnGridBuilt;
        }

        private void OnGridBuilt(object sender, RoomGeneratorSO.OnGridBuiltEventArgs args)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                InstantiatePlatform(platforms[i], args.Grid);
            }
        }

        public override void BuildGridObject(Grid<GridObject> grid, int x, int y)
        {
            GridObject gridObject = grid.GetObject(x, y);
            if (gridObject is CosmeticGO cosmetic)
            {
                cosmetic.TriggerVisualChanged(this, new CosmeticGO.OnVisualChangedEventArgs(sprite));
            }
            else
            {
                return;
            }

            if (!HasBuiltPlatformInPosition(x, y))
            {
                BuildPlatform(grid, x, y);
            }
        }

        private void InstantiatePlatform(Platform platform, Grid<GridObject> grid)
        {
            Vector2 startPos = grid.GridToWorldPosition(platform.StartPos, false);
            Vector2 endPos = grid.GridToWorldPosition(platform.EndPos, false);

            float length = (1 + endPos.x - startPos.x) * grid.CellSize;

            Vector2 midPoint = Vector2.Lerp(startPos, endPos, 0.5f);

            GameObject gameObject = Instantiate(platformPrefab, midPoint, Quaternion.identity, transform);
            
            Vector2 scale = Vector2.one;
            scale.x = length;

            gameObject.transform.localScale = scale;
        }

        private void BuildPlatform(Grid<GridObject> grid, int x, int y)
        {
            int length = 0;

            for (int i = x + 1; i < grid.Width; i++)
            {
                if (buildPositions.Contains(new Vector2Int(i, y)))
                {
                    length++;
                    continue;
                }
                break;
            }

            Platform platform = new Platform(new Vector2Int(x, y), new Vector2Int(x + length, y));
            platforms.Add(platform);
        }

        public override GridObject CreateGridObject(Grid<GridObject> grid, int x, int y)
        {
            buildPositions.Add(new Vector2Int(x, y));
            return new CosmeticGO(grid, x, y, visualPrefab, transform);
        }

        private bool HasBuiltPlatformInPosition(int x, int y)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].StartPos.x <= x && platforms[i].EndPos.x >= x)
                {
                    if (platforms[i].StartPos.y == y && platforms[i].EndPos.y == y)
                    {
                        return true;
                    }
                    else if (platforms[i].StartPos.x == x && platforms[i].EndPos.x == x)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

}