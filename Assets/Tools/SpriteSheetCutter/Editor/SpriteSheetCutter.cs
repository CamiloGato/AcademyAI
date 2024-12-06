using System.Collections.Generic;
using System.Linq;
using Tools.Extension;
using Tools.Objects;
using Tools.Objects.Mappers;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteSheetCutter.Editor
{
    public class SpriteSheetCutter : EditorWindow
    {
        private Texture2DCutData _cutData;
        private SerializedObject _cutDataSerializedObject;

        private Texture2DList _spriteSheets;
        private SerializedObject _spriteSheetsSerializedObject;
        private SerializedProperty _spriteSheetsProperty;

        private List<int> _totalSpritesColumns;

        [MenuItem("Tools/CamiloGato/Sprite Sheet Cutter")]
        public static void ShowWindow()
        {
            SpriteSheetCutter window = GetWindow<SpriteSheetCutter>();
            window.titleContent = new GUIContent("Sprite Sheet Cutter");
            window.Show();
        }

        private void OnEnable()
        {
            if (!_spriteSheets)
            {
                _spriteSheets = CreateInstance<Texture2DList>();
            }

            if (!_cutData)
            {
                _cutData = CreateInstance<Texture2DCutData>();
            }

            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

            _cutDataSerializedObject = new SerializedObject(_cutData);
        }

        private void OnGUI()
        {
            // TODO: Use from DTO instead of entity and then remove the Mapper

            _cutDataSerializedObject.Update();
            EditorGUILayout.LabelField("Cut Data Settings", EditorStyles.boldLabel);
            SerializedProperty gridWidth = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.gridWidth));
            EditorGUILayout.PropertyField(gridWidth, new GUIContent("Grid Width"));
            SerializedProperty gridHeight = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.gridHeight));
            EditorGUILayout.PropertyField(gridHeight, new GUIContent("Grid Height"));
            SerializedProperty offsetX = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.offsetX));
            EditorGUILayout.PropertyField(offsetX, new GUIContent("Offset X"));
            SerializedProperty offsetY = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.offsetY));
            EditorGUILayout.PropertyField(offsetY, new GUIContent("Offset Y"));
            SerializedProperty paddingX = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.paddingX));
            EditorGUILayout.PropertyField(paddingX, new GUIContent("Padding X"));
            SerializedProperty paddingY = _cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.paddingY));
            EditorGUILayout.PropertyField(paddingY, new GUIContent("Padding Y"));
            _cutDataSerializedObject.ApplyModifiedProperties();

            _spriteSheetsSerializedObject.Update();
            EditorGUILayout.PropertyField(_spriteSheetsProperty, new GUIContent("Sprite Sheets"), true);
            _spriteSheetsSerializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Cut Sprites"))
            {
                CutSpritesWithProgress();
            }

            if (GUILayout.Button("Create Sprite Sheet Info"))
            {
                string path = EditorUtility.SaveFilePanelInProject("Save Sprite Sheet Info", "SpriteSheetInfo", "asset", "Save Sprite Sheet Info");
                if (!string.IsNullOrEmpty(path))
                {
                    _spriteSheets.elements.FirstOrDefault()?.CutSpriteSheet(_cutData);
                    SpriteSheetInfo spriteSheetInfo = CreateInstance<SpriteSheetInfo>();
                    spriteSheetInfo.elements = new List<int>(_totalSpritesColumns);
                    spriteSheetInfo.texture2DCutDataDto = Texture2DCutDataMapper.ToDto(_cutData);
                    AssetDatabase.CreateAsset(spriteSheetInfo, path);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            if (_spriteSheets.Count > 0)
            {
                EditorGUILayout.HelpBox("If you create the Sprite Sheet Info, you will save the last sprite sheet.", MessageType.Info);
            }
        }

        private void CutSpritesWithProgress()
        {
            try
            {
                int totalSheets = _spriteSheets.elements.Count;
                int currentSheetIndex = 0;

                foreach (Texture2D spriteSheet in _spriteSheets.elements)
                {
                    currentSheetIndex++;

                    EditorUtility.DisplayProgressBar(
                        "Cutting Sprites",
                        $"Processing {spriteSheet.name} ({currentSheetIndex}/{totalSheets})",
                        (float)currentSheetIndex / totalSheets
                    );

                    _totalSpritesColumns = new List<int>(Texture2DExtensions.LastTotalSpritesColumns);

                    if(!spriteSheet.CutSpriteSheet(_cutData))
                    {
                        return;
                    }
                }

                EditorUtility.ClearProgressBar();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}