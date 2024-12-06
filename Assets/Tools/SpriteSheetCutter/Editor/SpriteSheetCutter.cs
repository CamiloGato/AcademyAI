using System;
using Tools.Extension;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteSheetCutter.Editor
{
    public class Texture2DList : ListWrapper<Texture2D>{}

    public class SpriteSheetCutter : EditorWindow
    {
        private Texture2DList _spriteSheets;
        private Texture2DCutData _cutData;

        private SerializedObject _cutDataSerializedObject;

        private SerializedObject _spriteSheetsSerializedObject;
        private SerializedProperty _spriteSheetsProperty;

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

            if (_cutData == null)
            {
                _cutData = CreateInstance<Texture2DCutData>();
            }

            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

            _cutDataSerializedObject = new SerializedObject(_cutData);
        }

        private void OnGUI()
        {
            if (_spriteSheetsSerializedObject == null || _spriteSheetsProperty == null)
            {
                EditorGUILayout.HelpBox("Error initializing Sprite Sheet Cutter", MessageType.Error);
                return;
            }

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

                    if (!spriteSheet.CutSpriteSheet(_cutData))
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