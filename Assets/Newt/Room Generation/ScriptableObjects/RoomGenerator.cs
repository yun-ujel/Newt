using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.RoomGeneration.ScriptableObjects
{
    using GridObjects;

    public class RoomGenerator : ScriptableObject
    {
        [System.Serializable]
        public class GridObjectDefinition
        {
            [field: SerializeField] public Color Color { get; set; }
            [field: SerializeField] public GridObjectGenerator Generator { get; set; }
        }

        [SerializeField] private GridObjectDefinition[] generators;
    }

}