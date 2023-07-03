using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt.GridObjects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GridObjectBehaviour : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(GridObject gridObject)
        {
            gridObject.OnUpdateVisualEvent += OnUpdateVisual;
        }

        private void OnUpdateVisual(object sender, GridObject.OnUpdateVisualEventArgs args)
        {
            spriteRenderer.color = args.Color;
            if (args.Sprite != null)
            {
                spriteRenderer.sprite = args.Sprite;
            }
        }
    }
}