using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Newt.GridObjects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CosmeticBehaviour : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public void Initialize(CosmeticGO gridObject)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            gridObject.OnVisualChangedEvent += OnVisualChanged;
        }

        private void OnVisualChanged(object sender, CosmeticGO.OnVisualChangedEventArgs args)
        {
            spriteRenderer.forceRenderingOff = args.Sprite == null;
            spriteRenderer.sprite = args.Sprite;
        }
    }
}
