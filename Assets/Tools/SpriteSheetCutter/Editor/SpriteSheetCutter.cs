using System.Collections.Generic;
using System.Linq;
using Tools.Extension;
using Tools.Objects;
using Tools.Objects.Mappers;
using Tools.Objects.Wrapper;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteSheetCutter.Editor
{
    public class SpriteSheetCutter : EditorWindow
    {
        private Texture2DCutData _cutData;
        private SerializedObject _cutDataSerializedObject;

        private SpriteSheetInfo _spriteSheetInfo;

        private Texture2DList _spriteSheets;
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
            InitializeData();
            InitializeSerializedObjects();
        }

        private void InitializeData()
        {
            _spriteSheets ??= CreateInstance<Texture2DList>();
            _cutData ??= CreateInstance<Texture2DCutData>();
        }

        private void InitializeSerializedObjects()
        {
            _spriteSheetsSerializedObject = new SerializedObject(_spriteSheets);
            _spriteSheetsProperty = _spriteSheetsSerializedObject.FindProperty(nameof(Texture2DList.elements));

            _cutDataSerializedObject = new SerializedObject(_cutData);
        }

        private void OnGUI()
        {
            DrawCutDataFields();
            DrawSpriteSheetInfoField();
            DrawSpriteSheetsField();
            DrawButtons();
        }

        private void DrawCutDataFields()
        {
            _cutDataSerializedObject.Update();

            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.gridWidth)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.gridHeight)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.offsetX)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.offsetY)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.paddingX)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.paddingY)));
            EditorGUILayout.PropertyField(_cutDataSerializedObject.FindProperty(nameof(Texture2DCutData.pixelsPerUnit)));

            _cutDataSerializedObject.ApplyModifiedProperties();
        }

        private void DrawSpriteSheetInfoField()
        {
            _spriteSheetInfo = (SpriteSheetInfo)EditorGUILayout.ObjectField("Sprite Sheet Info", _spriteSheetInfo, typeof(SpriteSheetInfo), false);

            if (GUILayout.Button("Get Sprite Sheet Info"))
            {
                UpdateCutDataFromSpriteSheetInfo();
            }
        }

        private void UpdateCutDataFromSpriteSheetInfo()
        {
            if (!_spriteSheetInfo)
            {
                EditorUtility.DisplayDialog("Error", "Sprite Sheet Info is not assigned.", "OK");
                return;
            }

            _cutDataSerializedObject.Update();
            _cutData = Texture2DCutDataMapper.ToEntity(_spriteSheetInfo.texture2DCutDataDto);
            InitializeSerializedObjects();
            _cutDataSerializedObject.ApplyModifiedProperties();
        }

        private void DrawSpriteSheetsField()
        {
            _spriteSheetsSerializedObject.Update();
            EditorGUILayout.PropertyField(_spriteSheetsProperty, new GUIContent("Sprite Sheets"), true);
            _spriteSheetsSerializedObject.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Cut Sprites"))
            {
                CutSpritesWithProgress();
            }

            if (GUILayout.Button("Create Sprite Sheet Info"))
            {
                CreateSpriteSheetInfo();
            }

            if (_spriteSheets.Count > 0)
            {
                EditorGUILayout.HelpBox("If you create the Sprite Sheet Info, you will save the last sprite sheet.", MessageType.Info);
            }
        }

        private void CreateSpriteSheetInfo()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Sprite Sheet Info", "SpriteSheetInfo", "asset", "Save Sprite Sheet Info");
            if (string.IsNullOrEmpty(path)) return;

            _spriteSheets.elements.FirstOrDefault()?.CutSpriteSheet(_cutData);

            SpriteSheetInfo spriteSheetInfo = CreateInstance<SpriteSheetInfo>();
            spriteSheetInfo.elements = new List<int>(Texture2DExtensions.LastTotalSpritesColumns);
            spriteSheetInfo.texture2DCutDataDto = Texture2DCutDataMapper.ToDto(_cutData);

            AssetDatabase.CreateAsset(spriteSheetInfo, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
                    UpdateProgressBar(currentSheetIndex, totalSheets, spriteSheet.name);

                    ConfigureSpriteTexture(spriteSheet);

                    if (!spriteSheet.CutSpriteSheet(_cutData))
                    {
                        break;
                    }
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
            EditorUtility.DisplayProgressBar("Cutting Sprites", $"Processing {sheetName} ({currentIndex}/{total})", progress);
        }

        private void ConfigureSpriteTexture(Texture2D spriteSheet)
        {
            spriteSheet.SetReadAndWrite(true);
            spriteSheet.SetTextureType(TextureImporterType.Sprite);
            spriteSheet.SetSpriteImporterMode(SpriteImportMode.Multiple);
            spriteSheet.SetFilterMode(FilterMode.Point, _cutData.pixelsPerUnit);
        }
    }
}
