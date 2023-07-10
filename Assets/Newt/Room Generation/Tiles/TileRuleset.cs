using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.Tiles
{
    [CreateAssetMenu(fileName = "Tile Ruleset", menuName = "Scriptable Object/Tiles/Tile Ruleset")]
    public class TileRuleset : ScriptableObject
    {
        [SerializeField] private Tile[] tiles;
        [SerializeField] private Sprite fallbackSprite;

        public Sprite GetValidSprite(bool[] adjacents)
        {
            if (adjacents.Length != 8)
            {
                return fallbackSprite;
            }

            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].IsTileValid(adjacents))
                {
                    return tiles[i].GetSprite();
                }
            }

            return fallbackSprite;
        }
    }

}