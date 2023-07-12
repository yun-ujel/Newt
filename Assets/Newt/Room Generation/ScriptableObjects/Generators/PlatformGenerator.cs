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
        #region Parameters
        [Header("Logic")]
        [SerializeField] private Direction facing;

        [Header("Objects")]
        [SerializeField] private GameObject platformPrefab;
        [SerializeField] private GameObject visualPrefab;
        
        [Space]
        
        [SerializeField] private Sprite sprite;

        private IDictionary<Grid<GridObject>, GridInformation> gridsInfo = new Dictionary<Grid<GridObject>, GridInformation>();
        #endregion

        #region Classes
        private enum Direction
        {
            north,
            south,
            east,
            west
        }

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

        private class GridInformation
        {
            public List<Platform> platforms;
            public List<Vector2Int> buildPositions;

            public GridInformation()
            {
                platforms = new List<Platform>();
                buildPositions = new List<Vector2Int>();
            }
        }
        #endregion

        #region Override Methods
        public override void Initialize(RoomGeneratorSO roomGenerator, Transform parent, Texture2D texture)
        {
            base.Initialize(roomGenerator, parent, texture);

            roomGenerator.OnGridBuiltEvent += OnGridBuilt;
        }

        public override GridObject CreateGridObject(Grid<GridObject> grid, int x, int y)
        {
            if (!gridsInfo.ContainsKey(grid))
            {
                gridsInfo.Add(grid, new GridInformation());
            }

            gridsInfo[grid].buildPositions.Add(new Vector2Int(x, y));
            return new CosmeticGO(grid, x, y, visualPrefab, transform);
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

            if (!HasBuiltPlatformInPosition(gridsInfo[grid].platforms, x, y))
            {
                BuildPlatform(grid, x, y);
            }
        }

        private void OnGridBuilt(object sender, RoomGeneratorSO.OnGridBuiltEventArgs args)
        {
            for (int i = 0; i < gridsInfo[args.Grid].platforms.Count; i++)
            {
                InstantiatePlatform(gridsInfo[args.Grid].platforms[i], args.Grid);
            }

            _ = gridsInfo.Remove(args.Grid);
        }
        #endregion

        #region Platform Creation Methods
        private void InstantiatePlatform(Platform platform, Grid<GridObject> grid)
        {
            Vector2 startPos = grid.GridToWorldPosition(platform.StartPos, false);
            Vector2 endPos = grid.GridToWorldPosition(platform.EndPos, false);

            GameObject gameObject;
            Vector2 scale;

            if (facing == Direction.north || facing == Direction.south)
            {
                float length = (1 + endPos.x - startPos.x) * grid.CellSize;
                Vector2 midPoint = Vector2.Lerp(startPos, endPos, 0.5f);

                gameObject = Instantiate(platformPrefab, midPoint, Quaternion.identity, transform);

                scale = new Vector2(length, 1);
            }
            else
            {
                float length = (1 + endPos.y - startPos.y) * grid.CellSize;
                Vector2 midPoint = Vector2.Lerp(startPos, endPos, 0.5f);

                gameObject = Instantiate(platformPrefab, midPoint, Quaternion.identity, transform);

                scale = new Vector2(1, length);
            }

            gameObject.name = $"Platform {platform.StartPos} - {platform.EndPos}";
            gameObject.transform.localScale = scale;
            gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = GetPlatformRotationZ(facing);
        }

        private void BuildPlatform(Grid<GridObject> grid, int x, int y)
        {
            int length = 0;
            Platform platform;

            if (facing == Direction.north || facing == Direction.south)
            {
                for (int i = x + 1; i < grid.Width; i++)
                {
                    if (gridsInfo[grid].buildPositions.Contains(new Vector2Int(i, y)))
                    {
                        length++;
                        continue;
                    }
                    break;
                }

                platform = new Platform(new Vector2Int(x, y), new Vector2Int(x + length, y));
            }
            else
            {
                for (int i = y + 1; i < grid.Height; i++)
                {
                    if (gridsInfo[grid].buildPositions.Contains(new Vector2Int(x, i)))
                    {
                        length++;
                        continue;
                    }
                    break;
                }

                platform = new Platform(new Vector2Int(x, y), new Vector2Int(x, y + length));
            }


            gridsInfo[grid].platforms.Add(platform);
        }
        #endregion

        #region Utility Methods
        private bool HasBuiltPlatformInPosition(List<Platform> platforms, int x, int y)
        {
            if (facing == Direction.north || facing == Direction.south)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i].StartPos.x <= x && platforms[i].EndPos.x >= x)
                    {
                        if (platforms[i].StartPos.y == y && platforms[i].EndPos.y == y)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i].StartPos.y <= y && platforms[i].EndPos.y >= y)
                    {
                        if (platforms[i].StartPos.x == x && platforms[i].EndPos.x == x)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private float GetPlatformRotationZ(Direction direction)
        {
            float z = 0;

            switch (direction)
            {
                case Direction.west:
                    z = 90f;
                    break;
                case Direction.south:
                    z = 180f;
                    break;
                case Direction.east:
                    z = 270f;
                    break;
                default:
                    break;
            }

            return z;
        }
        #endregion
    }
}