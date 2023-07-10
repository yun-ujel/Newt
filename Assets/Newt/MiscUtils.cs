using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Newt
{
    public static class MiscUtils
    {
		public static Texture2D ConvertSpriteToTexture(this Sprite sprite)
		{
			if (sprite == null) { return null; }

			if (sprite.rect.width != sprite.texture.width)
			{
				Texture2D generatedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
															 (int)sprite.textureRect.y,
															 (int)sprite.textureRect.width,
															 (int)sprite.textureRect.height);
				generatedTexture.SetPixels(newColors);
				generatedTexture.Apply();
				return generatedTexture;
			}
			else
				return sprite.texture;
		}
	}
}