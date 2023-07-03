using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newt.RoomGeneration.Tiles
{
    [CustomEditor(typeof(Tile))]
    public class TileEditor : Editor
    {
        private Tile tile;

        private SerializedProperty adjacentTilesProperty;
        private SerializedProperty tileSpriteProperty;
        private SerializedProperty ignoreCornersProperty;

        private Texture2D displayTexture;

        private GUIStyle fixedWidth;

        private void OnEnable()
        {
            tile = (Tile)target;

            adjacentTilesProperty = serializedObject.FindProperty("adjacentTiles");
            tileSpriteProperty = serializedObject.FindProperty("tileSprite");
            ignoreCornersProperty = serializedObject.FindProperty("ignoreCorners");

            fixedWidth = new GUIStyle
            {
                fixedWidth = 1f
            };

            UpdateDisplayTexture();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Object sprite = EditorGUILayout.ObjectField("Sprite", tileSpriteProperty.objectReferenceValue, typeof(Sprite), false);
            EditorGUILayout.Space();

            if (sprite != tileSpriteProperty.objectReferenceValue)
            {
                tileSpriteProperty.objectReferenceValue = sprite;
                UpdateDisplayTexture();
            }

            DrawAdjacentTiles();

            EditorGUILayout.Space();
            ignoreCornersProperty.boolValue = EditorGUILayout.Toggle("Ignore Corners", ignoreCornersProperty.boolValue);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAdjacentTiles()
        {
            EditorGUILayout.LabelField("Adjacent Tiles", EditorStyles.boldLabel);

            int i = 0;

            for (int height = 0; height < 3; height++)
            {
                _ = EditorGUILayout.BeginHorizontal(fixedWidth);
                for (int width = 0; width < 3; width++)
                {
                    if (width == 1 && height == 1)
                    {
                        _ = EditorGUILayout.Toggle(true);
                        continue;
                    }
                    SerializedProperty property = adjacentTilesProperty.GetArrayElementAtIndex(i);
                    property.boolValue = EditorGUILayout.Toggle(property.boolValue);

                    i++;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void UpdateDisplayTexture()
        {
            Sprite sprite = (Sprite)tileSpriteProperty.objectReferenceValue;
            displayTexture = sprite.ConvertSpriteToTexture();
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            return displayTexture;
        }
    }
}