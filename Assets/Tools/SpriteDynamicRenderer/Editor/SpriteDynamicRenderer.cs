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
            if (!_spriteSheets)
            {
                _spriteSheets = CreateInstance<Texture2DList>();
            }

            if (!_animationClipData)
            {
                _animationClipData = CreateInstance<AnimationListClipData>();
            }

            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

            _animationClipDataSerializedObject = new SerializedObject(_animationClipData);
            _animationClipDataProperty = _animationClipDataSerializedObject.FindProperty(nameof(AnimationListClipData.elements));
        }

        private void OnGUI()
        {
            _spriteSheetInfo = (SpriteSheetInfo)EditorGUILayout.ObjectField("Sprite Sheet Info", _spriteSheetInfo, typeof(SpriteSheetInfo), false);

            if (!_spriteSheetInfo)
            {
                EditorGUILayout.HelpBox("Please select a Sprite Sheet Info", MessageType.Warning);
                return;
            }

            _spriteSheetsSerializedObject.Update();
            EditorGUILayout.LabelField("Sprite Sheets", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spriteSheetsProperty, true);
            _spriteSheetsSerializedObject.ApplyModifiedProperties();

            if(_spriteSheets.Count <= 0)
            {
                EditorGUILayout.HelpBox("Please add at least one Sprite Sheet", MessageType.Warning);
                return;
            }

            if (_animationClipData.Count <= 0)
            {
                GenerateAnimationClipFromData();
            }

            _animationClipDataSerializedObject.Update();
            EditorGUILayout.PropertyField(_animationClipDataProperty, true);
            _animationClipDataSerializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Create Render Data for Dynamic Sprite Renderer"))
            {
                CreateRenderDataWithProgressBar();
            }
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
                int totalSheets = _spriteSheets.elements.Count;
                int currentSheetIndex = 0;

                string path = EditorUtility.SaveFilePanelInProject("Save Render Data", $"Render Data", "asset", "Save Render Data");

                foreach (Texture2D spriteSheet in _spriteSheets.elements)
                {
                    currentSheetIndex++;

                    EditorUtility.DisplayProgressBar(
                        "Creating Sprites Render Data",
                        $"Processing {spriteSheet.name} ({currentSheetIndex}/{totalSheets})",
                        (float)currentSheetIndex / totalSheets
                    );

                    string renderDataFolder = path.Substring(0, path.LastIndexOf('/'));

                    if (!AssetDatabase.IsValidFolder(renderDataFolder))
                    {
                        AssetDatabase.CreateFolder("Assets", renderDataFolder);
                    }

                    CreateRenderData(spriteSheet, renderDataFolder);
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

                if (string.IsNullOrEmpty(path))
                {
                    throw new System.Exception("Path is empty");
                }

                string filePath = $"{path}/{spriteSheet.name}.asset";

                SpriteDynamicRendererData spriteDynamicRendererData = CreateInstance<SpriteDynamicRendererData>();

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

                EditorUtility.ClearProgressBar();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}