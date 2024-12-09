using System.Collections.Generic;
using Tools.Extension;
using Tools.Objects;
using Tools.Objects.Wrapper;
using Tools.SpriteAnimator.Editor;
using Tools.SpriteDynamicRenderer.Data;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Editor
{
    public class SpriteDynamicRenderer : EditorWindow
    {
        private SpriteSheetInfo _spriteSheetInfo;

        private Texture2DList _spriteSheets;
        private SerializedObject _spriteSheetsSerializedObject;
        private SerializedProperty _spriteSheetsProperty;

        private AnimationListClipData _animationClipData;
        private SerializedObject _animationClipDataSerializedObject;
        private SerializedProperty _animationClipDataProperty;

        [MenuItem("Tools/CamiloGato/Sprite Dynamic Renderer")]
        private static void ShowWindow()
        {
            SpriteDynamicRenderer window = GetWindow<SpriteDynamicRenderer>();
            window.titleContent = new GUIContent("Sprite Dynamic Renderer");
            window.Show();
        }

        private void OnEnable()
        {
            InitializeData();
            InitializeSerializedObjects();
        }

        private void InitializeData()
        {
            _spriteSheets ??= CreateInstance<Texture2DList>();
            _animationClipData ??= CreateInstance<AnimationListClipData>();
        }

        private void InitializeSerializedObjects()
        {
            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

            _animationClipDataSerializedObject = new SerializedObject(_animationClipData);
            _animationClipDataProperty = _animationClipDataSerializedObject.FindProperty(nameof(AnimationListClipData.elements));
        }

        private void OnGUI()
        {
            DrawSpriteSheetInfoField();

            if (!_spriteSheetInfo)
            {
                DisplayWarning("Please select a Sprite Sheet Info.");
                return;
            }

            DrawSpriteSheetsSection();

            if (_spriteSheets.Count <= 0)
            {
                DisplayWarning("Please add at least one Sprite Sheet.");
                return;
            }

            if (_animationClipData.Count <= 0)
            {
                GenerateAnimationClipFromData();
            }

            DrawAnimationClipDataSection();

            if (GUILayout.Button("Create Render Data for Dynamic Sprite Renderer"))
            {
                CreateRenderDataWithProgressBar();
            }
        }

        private void DrawSpriteSheetInfoField()
        {
            _spriteSheetInfo = (SpriteSheetInfo)EditorGUILayout.ObjectField("Sprite Sheet Info", _spriteSheetInfo, typeof(SpriteSheetInfo), false);
            EditorGUILayout.Space();
        }

        private void DrawSpriteSheetsSection()
        {
            _spriteSheetsSerializedObject.Update();
            EditorGUILayout.LabelField("Sprite Sheets", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spriteSheetsProperty, true);
            _spriteSheetsSerializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
        }

        private void DrawAnimationClipDataSection()
        {
            _animationClipDataSerializedObject.Update();
            EditorGUILayout.LabelField("Animation Clips", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_animationClipDataProperty, true);
            _animationClipDataSerializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
        }

        private void GenerateAnimationClipFromData()
        {
            _animationClipData.elements.Clear();

            int currentElement = 0;
            int currentRow = 0;

            foreach (int element in _spriteSheetInfo.elements)
            {
                AnimationClipData clipData = new AnimationClipData
                {
                    name = $"AnimationClip {++currentRow}",
                    startIndex = currentElement,
                    endIndex = currentElement + (element - 1),
                    frameRate = 12f,
                };

                currentElement += element;
                _animationClipData.Add(clipData);
            }
        }

        private void CreateRenderDataWithProgressBar()
        {
            try
            {
                string path = EditorUtility.SaveFilePanelInProject("Save Render Data", $"Render Data", "asset", "Save Render Data");
                path = path.Substring(0, path.LastIndexOf('/'));

                if (string.IsNullOrEmpty(path)) return;

                int totalSheets = _spriteSheets.elements.Count;
                int currentSheetIndex = 0;

                foreach (Texture2D spriteSheet in _spriteSheets.elements)
                {
                    currentSheetIndex++;
                    UpdateProgressBar(currentSheetIndex, totalSheets, spriteSheet.name);
                    CreateRenderData(spriteSheet, path);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void CreateRenderData(Texture2D spriteSheet, string path)
        {
            try
            {
                float progress = 0f;
                float progressStep = 1f / _animationClipData.elements.Count;

                string filePath = $"{path}/{spriteSheet.name}.asset";

                var spriteDynamicRendererData = CreateInstance<SpriteDynamicRendererData>();

                foreach (AnimationClipData clipData in _animationClipData.elements)
                {
                    List<Sprite> sprites = spriteSheet.GetSpritesFromSheet(clipData.startIndex, clipData.endIndex);
                    spriteDynamicRendererData.AddAnimation(clipData.name, sprites);

                    progress += progressStep;
                    EditorUtility.DisplayProgressBar("Creating Sprites Render Data", $"Creating {clipData.name} Render Data", progress);
                }

                AssetDatabase.CreateAsset(spriteDynamicRendererData, filePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void UpdateProgressBar(int currentIndex, int total, string sheetName)
        {
            float progress = (float)currentIndex / total;
            EditorUtility.DisplayProgressBar("Creating Sprites Render Data", $"Processing {sheetName} ({currentIndex}/{total})", progress);
        }

        private void DisplayWarning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
    }
}
