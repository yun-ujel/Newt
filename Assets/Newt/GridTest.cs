using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt
{
    using RoomGeneration.ScriptableObjects;

    public class GridTest : MonoBehaviour
    {
        [SerializeField] private RoomGeneratorSO roomGenerator;
        [SerializeField] private Texture2D texture;

        private void Start()
        {
            Vector2 origin = new Vector2(-texture.width, -texture.height) * 0.5f;
            roomGenerator.GenerateRoom(texture, origin);
        }
    }

}