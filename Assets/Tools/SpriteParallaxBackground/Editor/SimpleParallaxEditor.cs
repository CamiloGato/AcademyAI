using Tools.SpriteParallaxBackground.Data;
using Tools.SpriteParallaxBackground.Runtime;
using UnityEditor;
using UnityEngine;

namespace Tools.SpriteParallaxBackground.Editor
{
    [CustomEditor(typeof(SimpleParallax))]
    public class SimpleParallaxEditor : UnityEditor.Editor
    {
        private SerializedProperty _dataProperty;
        private SerializedProperty _layersProperty;
        private SerializedProperty _mainCameraProperty;
        private SerializedProperty _useMainCamera;

        private void OnEnable()
        {
            _dataProperty = serializedObject.FindProperty(nameof(SimpleParallax.data));
            _layersProperty = serializedObject.FindProperty(nameof(SimpleParallax.layers));
            _mainCameraProperty = serializedObject.FindProperty(nameof(SimpleParallax.mainCamera));
            _useMainCamera = serializedObject.FindProperty(nameof(SimpleParallax.useMainCamera));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawParallaxDataField();
            ParallaxBackgroundData parallaxData = _dataProperty.objectReferenceValue as ParallaxBackgroundData;

            if (!parallaxData)
            {
                DisplayError("You must assign a ParallaxBackgroundData to configure the parallax.");
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EnsureLayersMatch(parallaxData);

            DrawLayersSection(parallaxData);
            DrawCameraSettings();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawParallaxDataField()
        {
            EditorGUILayout.PropertyField(_dataProperty, new GUIContent("Parallax Background Data"));
            EditorGUILayout.Space();
        }

        private void DrawLayersSection(ParallaxBackgroundData parallaxData)
        {
            EditorGUILayout.LabelField("Parallax Layers", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Assign Transforms for each layer to configure the parallax behavior.", MessageType.Info);

            for (int i = 0; i < parallaxData.layers.Length; i++)
            {
                string layerName = parallaxData.layers[i].name;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(layerName, GUILayout.MaxWidth(150));
                _layersProperty.GetArrayElementAtIndex(i).objectReferenceValue =
                    EditorGUILayout.ObjectField(_layersProperty.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Transform), true);
                EditorGUILayout.EndHorizontal();
            }

            if (AreAllLayersAssigned())
            {
                EditorGUILayout.HelpBox("All layers are properly assigned.", MessageType.Info);
            }
            else
            {
                DisplayWarning("Some layers are missing. Please assign all layers.");
            }

            EditorGUILayout.Space();
        }

        private void DrawCameraSettings()
        {
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_useMainCamera, new GUIContent("Use Main Camera"));

            if (!_useMainCamera.boolValue)
            {
                EditorGUILayout.PropertyField(_mainCameraProperty, new GUIContent("Camera"));

                if (!_mainCameraProperty.objectReferenceValue)
                {
                    DisplayWarning("Please assign a camera.");
                }
            }

            EditorGUILayout.Space();
        }

        private void EnsureLayersMatch(ParallaxBackgroundData parallaxData)
        {
            if (_layersProperty.arraySize != parallaxData.layers.Length)
            {
                _layersProperty.arraySize = parallaxData.layers.Length;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private bool AreAllLayersAssigned()
        {
            for (int i = 0; i < _layersProperty.arraySize; i++)
            {
                if (!_layersProperty.GetArrayElementAtIndex(i).objectReferenceValue)
                {
                    return false;
                }
            }
            return true;
        }

        private void DisplayError(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Error);
        }

        private void DisplayWarning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
    }
}
