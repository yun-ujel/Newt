using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.Tiles
{
    [System.Serializable]
    public class TilesetBuilder : ScriptableObject
    {
        [SerializeField] private string tilesetName;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite fallbackSprite;
    }
}
