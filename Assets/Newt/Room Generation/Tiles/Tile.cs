using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.Tiles
{
    [CreateAssetMenu(fileName = "New Tile", menuName = "Scriptable Object/Tile")]
    public class Tile : ScriptableObject
    {
        [Tooltip("Set to true if there should be a tile in the space adjacent to this object. Starts at top left and ends at bottom right, skipping the center tile.")]
        [SerializeField] private bool[] adjacentTiles = new bool[8];

        [SerializeField] private Sprite tileSprite;

        private void Awake()
        {
            if (adjacentTiles.Length < 8)
            {
                adjacentTiles = new bool[8];
            }
        }
    }
}