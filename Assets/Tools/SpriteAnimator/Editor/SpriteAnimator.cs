using System;
using System.Linq;
using Tools.Extension;
using Tools.Objects;
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
            if (!_animationClipData)
            {
                _animationClipData = CreateInstance<AnimationListClipData>();
            }

            _animationClipDataSerializedObject = new SerializedObject(_animationClipData);
            _animationClipDataProperty = _animationClipDataSerializedObject.FindProperty(nameof(AnimationListClipData.elements));

            if (!_spriteSheets)
            {
                _spriteSheets = CreateInstance<Texture2DList>();
            }

            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

        }

        private void OnGUI()
        {
            _spriteSheetInfo = (SpriteSheetInfo)EditorGUILayout.ObjectField("Sprite Sheet Info", _spriteSheetInfo, typeof(SpriteSheetInfo), false);

            _spriteSheetsSerializedObject.Update();
            EditorGUILayout.PropertyField(_spriteSheetsProperty, new GUIContent("Sprite Sheets"), true);
            _spriteSheetsSerializedObject.ApplyModifiedProperties();

            if (_spriteSheetInfo)
            {
                EditorGUILayout.HelpBox("If you have a SpriteSheetInfo, you can use it to create the animation clip.", MessageType.Info);

                if (GUILayout.Button("Generate Animation Clip from Data"))
                {
                    GenerateAnimationClipFromData();
                }

            }

            _animationClipDataSerializedObject.Update();
            EditorGUILayout.PropertyField(_animationClipDataProperty, true);
            _animationClipDataSerializedObject.ApplyModifiedProperties();

            Texture2D spriteSheet = _spriteSheets.elements.FirstOrDefault();
            if (_spriteSheets.Count <= 0 || !spriteSheet)
            {
                EditorGUILayout.HelpBox("Please assign a sprite sheet.", MessageType.Warning);
                return;
            }

            foreach (AnimationClipData clipData in _animationClipData.elements)
            {
                if (clipData.startIndex < 0 || clipData.endIndex >= spriteSheet.width * spriteSheet.height || clipData.startIndex > clipData.endIndex)
                {
                    EditorGUILayout.HelpBox($"Invalid start and end indexes on {clipData.name}. Or NULL Sprite Sheet", MessageType.Warning);
                    return;
                }
            }

            if (GUILayout.Button("Create Animation Clip"))
            {
                CreateAnimationsWithProgressBar();
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

        private void CreateAnimationsWithProgressBar()
        {
            try
            {
                int totalSheets = _spriteSheets.elements.Count;
                int currentSheetIndex = 0;

                string path = EditorUtility.SaveFilePanelInProject("Save Animation Clip", $"Animation Clip", "anim", "Save Animation Clip");

                foreach (Texture2D spriteSheet in _spriteSheets.elements)
                {
                    currentSheetIndex++;

                    EditorUtility.DisplayProgressBar(
                        "Creating Animation Clips",
                        $"Processing {spriteSheet.name} ({currentSheetIndex}/{totalSheets})",
                        (float)currentSheetIndex / totalSheets
                    );

                    string animationFolder = path.Substring(0, path.LastIndexOf('/'));
                    animationFolder += $"/{spriteSheet.name}";

                    if (!AssetDatabase.IsValidFolder(animationFolder))
                    {
                        string parentFolder = animationFolder.Substring(0, animationFolder.LastIndexOf('/'));
                        string newFolderName = spriteSheet.name;
                        AssetDatabase.CreateFolder(parentFolder, newFolderName);
                    }

                    CreateAnimation(spriteSheet, animationFolder);
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

                if (string.IsNullOrEmpty(path))
                {
                    return;
                }

                foreach (AnimationClipData clipData in _animationClipData.elements)
                {
                    Sprite[] sprites = spriteSheet.GetSpritesFromSheet();
                    AnimationClip animationClip = sprites.CreateAnimationClip(clipData.startIndex, clipData.endIndex, clipData.frameRate);

                    string filePath = $"{path}/{clipData.name}.anim";

                    AssetDatabase.CreateAsset(animationClip, filePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    progress += progressStep;
                    EditorUtility.DisplayProgressBar("Creating Animation Clips", $"Creating {clipData.name} Animation Clip", progress);
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