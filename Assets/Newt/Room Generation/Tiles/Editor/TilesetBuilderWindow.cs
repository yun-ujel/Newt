using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Newt.RoomGeneration.Tiles
{
    public class TilesetBuilderWindow : EditorWindow
    {
        private ScriptableObject target;
        private SerializedObject serializedObject;

        private SerializedProperty tilesetNameProperty;
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

            tilesetNameProperty = serializedObject.FindProperty("tilesetName");
            spritesProperty = serializedObject.FindProperty("sprites");
            fallbackSpriteProperty = serializedObject.FindProperty("fallbackSprite");
        }

        private void OnGUI()
        {
            DrawProperties();

            _ = serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("Build Tileset"))
            {
                BuildTileset();
            }
        }

        private void DrawProperties()
        {
            tilesetNameProperty.stringValue = EditorGUILayout.TextField("Name", tilesetNameProperty.stringValue);
                EditorGUILayout.Space();
            _ = EditorGUILayout.PropertyField(spritesProperty, true);
                EditorGUILayout.Space();
            fallbackSpriteProperty.objectReferenceValue = EditorGUILayout.ObjectField("Fallback Sprite", fallbackSpriteProperty.objectReferenceValue, typeof(Sprite), false);
                EditorGUILayout.Space();
        }

        #region Save / Build Methods
        private void BuildTileset()
        {
            string name = tilesetNameProperty.stringValue;

            if (string.IsNullOrWhiteSpace(name))
            {
                EditorUtility.DisplayDialog("Invalid File Name", "Please ensure the file name you've entered is valid", "OK");
                return;
            }

            CreateRootFolders();
            _ = SaveUtility.TryCreateFolder("Assets/ScriptableObjects/Tiles", name, out string guid);
            string folder = AssetDatabase.GUIDToAssetPath(guid);

            Tile[] tiles = new Tile[spritesProperty.arraySize];

            for (int i = 0; i < spritesProperty.arraySize; i++)
            {
                Sprite sprite = (Sprite)spritesProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                string tilePath = $"{folder}/{$"Tile {i}"}.asset";

                tiles[i] = BuildTile(sprite, tilePath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Sprite fallbackSprite = (Sprite)fallbackSpriteProperty.objectReferenceValue;

            string rulesetPath = $"{folder}/{name}.asset";
            BuildRuleset(tiles, fallbackSprite, rulesetPath);
        }

        private void CreateRootFolders()
        {
            _ = SaveUtility.TryCreateFolder("Assets", "ScriptableObjects");
            _ = SaveUtility.TryCreateFolder("Assets/ScriptableObjects", "Tiles");
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
        #endregion
    }
}