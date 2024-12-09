using System;
using Tools.Extension;
using Tools.Objects;
using Tools.Objects.Wrapper;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteAnimator.Editor
{
    [Serializable]
    public class AnimationClipData
    {
        public string name;
        public int startIndex;
        public int endIndex;
        public float frameRate;
    }

    public class AnimationListClipData : ListWrapper<AnimationClipData>{}

    public class SpriteAnimator : EditorWindow
    {
        private AnimationListClipData _animationClipData;
        private SerializedObject _animationClipDataSerializedObject;
        private SerializedProperty _animationClipDataProperty;

        private SpriteSheetInfo _spriteSheetInfo;

        private Texture2DList _spriteSheets;
        private SerializedObject _spriteSheetsSerializedObject;
        private SerializedProperty _spriteSheetsProperty;

        [MenuItem("Tools/CamiloGato/SpriteAnimator")]
        private static void ShowWindow()
        {
            SpriteAnimator window = GetWindow<SpriteAnimator>();
            window.titleContent = new GUIContent("Sprite Animator");
            window.Show();
        }

        private void OnEnable()
        {
            InitializeData();
            InitializeSerializedObjects();
        }

        private void InitializeData()
        {
            _animationClipData ??= CreateInstance<AnimationListClipData>();
            _spriteSheets ??= CreateInstance<Texture2DList>();
        }

        private void InitializeSerializedObjects()
        {
            _animationClipDataSerializedObject = new SerializedObject(_animationClipData);
            _animationClipDataProperty = _animationClipDataSerializedObject.FindProperty(nameof(AnimationListClipData.elements));

            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));
        }

        private void OnGUI()
        {
            DrawSpriteSheetInfoField();
            DrawSpriteSheetsSection();

            if (_spriteSheetInfo)
            {
                EditorGUILayout.HelpBox("Use SpriteSheetInfo to generate animation clips.", MessageType.Info);

                if (GUILayout.Button("Generate Animation Clip Data"))
                {
                    GenerateAnimationClipFromData();
                }
            }

            DrawAnimationClipDataSection();

            if (GUILayout.Button("Create Animation Clips"))
            {
                CreateAnimationsWithProgressBar();
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
                    frameRate = 12f
                };

                currentElement += element;
                _animationClipData.Add(clipData);
            }

            _animationClipDataSerializedObject.Update();
            _animationClipDataSerializedObject.ApplyModifiedProperties();
        }

        private void CreateAnimationsWithProgressBar()
        {
            try
            {
                int totalSheets = _spriteSheets.elements.Count;
                int currentSheetIndex = 0;

                string path = EditorUtility.SaveFilePanelInProject("Save Animation Clip", "Animation Clip", "anim", "Save Animation Clip");
                if (string.IsNullOrEmpty(path)) return;

                foreach (Texture2D spriteSheet in _spriteSheets.elements)
                {
                    currentSheetIndex++;
                    UpdateProgressBar(currentSheetIndex, totalSheets, spriteSheet.name);
                    CreateAnimation(spriteSheet, path);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void CreateAnimation(Texture2D spriteSheet, string path)
        {
            try
            {
                float progress = 0f;
                float progressStep = 1f / _animationClipData.elements.Count;

                string folderPath = $"{path}/{spriteSheet.name}";
                CreateFolderIfNotExists(folderPath);

                foreach (AnimationClipData clipData in _animationClipData.elements)
                {
                    Sprite[] sprites = spriteSheet.GetSpritesFromSheet();
                    AnimationClip animationClip = sprites.CreateAnimationClip(clipData.startIndex, clipData.endIndex, clipData.frameRate);

                    string filePath = $"{folderPath}/{clipData.name}.anim";

                    AssetDatabase.CreateAsset(animationClip, filePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    progress += progressStep;
                    EditorUtility.DisplayProgressBar("Creating Animation Clips", $"Creating {clipData.name} Animation Clip", progress);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void UpdateProgressBar(int currentIndex, int total, string sheetName)
        {
            float progress = (float)currentIndex / total;
            EditorUtility.DisplayProgressBar("Creating Animation Clips", $"Processing {sheetName} ({currentIndex}/{total})", progress);
        }

        private void CreateFolderIfNotExists(string folderPath)
        {
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string parentFolder = folderPath.Substring(0, folderPath.LastIndexOf('/'));
                string newFolderName = folderPath.Substring(folderPath.LastIndexOf('/') + 1);
                AssetDatabase.CreateFolder(parentFolder, newFolderName);
            }
        }
    }
}
