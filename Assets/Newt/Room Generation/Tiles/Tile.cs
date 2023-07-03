using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.Tiles
{
    [CreateAssetMenu(fileName = "New Tile", menuName = "Scriptable Object/Tiles/Tile")]
    public class Tile : ScriptableObject
    {
        [Tooltip("Set to true if there should be a tile in the space adjacent to this object. Starts at top left and ends at bottom right, skipping the center tile.")]
        [SerializeField] private bool[] adjacentTiles = new bool[8];
        [SerializeField] private bool ignoreCorners;

        [SerializeField] private Sprite tileSprite;

        private void Awake()
        {
            if (adjacentTiles.Length < 8)
            {
                adjacentTiles = new bool[8];
            }
        }

        public bool IsTileValid(bool[] adjacents)
        {
            if (!ignoreCorners)
            {
                if (adjacents.Length != 8)
                {
                    return false;
                }

                for (int i = 0; i < adjacents.Length; i++)
                {
                    if (adjacents[i] != adjacentTiles[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return adjacents[1] == adjacentTiles[1]
                    && adjacents[3] == adjacentTiles[3]
                    && adjacents[4] == adjacentTiles[4]
                    && adjacents[6] == adjacentTiles[6];
            }
        }

        public bool IsTileValid(bool[] adjacents, out Sprite sprite)
        {
            sprite = tileSprite;

            return IsTileValid(adjacents);
        }

        public Sprite GetSprite()
        {
            return tileSprite;
        }
    }
}