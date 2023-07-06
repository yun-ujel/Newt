using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Newt.RoomGeneration.Tiles
{
    public class TilesetBuilderWindow : EditorWindow
    {
        private ScriptableObject target;
        private SerializedObject serializedObject;
        
        private SerializedProperty spritesProperty;
        private SerializedProperty fallbackSpriteProperty;

        [MenuItem("Tools/Tileset Builder")]
        public static void Open()
        {
            TilesetBuilderWindow window = GetWindow<TilesetBuilderWindow>("Tileset Builder");
        }

        private void OnEnable()
        {
            target = CreateInstance<TilesetBuilder>();
            serializedObject = new SerializedObject(target);
            
            spritesProperty = serializedObject.FindProperty("sprites");
            fallbackSpriteProperty = serializedObject.FindProperty("fallbackSprite");
        }

        private void OnGUI()
        {
            _ = EditorGUILayout.PropertyField(spritesProperty, true);

            fallbackSpriteProperty.objectReferenceValue = EditorGUILayout.ObjectField("Fallback Sprite", fallbackSpriteProperty.objectReferenceValue, typeof(Sprite), false);

            _ = serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Build Tileset"))
            {
                BuildTileset();
            }
        }

        private void BuildTileset()
        {
            Sprite fallbackSprite = (Sprite)fallbackSpriteProperty.objectReferenceValue;
            string fallbackSpriteName = fallbackSprite.texture.name;
            Debug.Log(fallbackSpriteName);

            string guid = AssetDatabase.CreateFolder("Assets/ScriptableObjects/Tiles", fallbackSpriteName);
            string folder = AssetDatabase.GUIDToAssetPath(guid);

            Tile[] tiles = new Tile[spritesProperty.arraySize];

            for (int i = 0; i < spritesProperty.arraySize; i++)
            {
                Sprite sprite = (Sprite)spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                string fullPath = $"{folder}/{$"Tile {i}"}.asset";

                tiles[i] = BuildTile(sprite, fullPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string rulesetPath = $"{folder}/{fallbackSpriteName}.asset";
            BuildRuleset(tiles, fallbackSprite, rulesetPath);
        }

        private Tile BuildTile(Sprite sprite, string fullPath)
        {
            Tile tile = CreateInstance<Tile>();
            SerializedObject so = new SerializedObject(tile);

            so.FindProperty("tileSprite").objectReferenceValue = sprite;
            _ = so.ApplyModifiedPropertiesWithoutUndo();
            
            AssetDatabase.CreateAsset(tile, fullPath);
            EditorUtility.SetDirty(tile);
            return tile;
        }

        private void BuildRuleset(Tile[] tiles, Sprite fallback, string fullPath)
        {
            ScriptableObject ruleset = CreateInstance<TileRuleset>();
            SerializedObject so = new SerializedObject(ruleset);

            SerializedProperty tilesProperty = so.FindProperty("tiles");
            so.FindProperty("fallbackSprite").objectReferenceValue = fallback;

            for (int i = 0; i < tiles.Length; i++)
            {
                tilesProperty.InsertArrayElementAtIndex(0);
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                tilesProperty.GetArrayElementAtIndex(i).objectReferenceValue = tiles[i];
            }

            _ = so.ApplyModifiedPropertiesWithoutUndo();

            AssetDatabase.CreateAsset(ruleset, fullPath);
            EditorUtility.SetDirty(ruleset);
        }
    }
}