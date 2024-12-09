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

            EditorGUILayout.PropertyField(_dataProperty, new GUIContent("Parallax Background Data"));
            ParallaxBackgroundData parallaxData = _dataProperty.objectReferenceValue as ParallaxBackgroundData;

            if (!parallaxData)
            {
                EditorGUILayout.HelpBox("Debes asignar un ParallaxBackgroundData para configurar el Parallax.", MessageType.Error);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EnsureLayersMatch(parallaxData);

            EditorGUILayout.LabelField("Parallax Layers", EditorStyles.boldLabel);

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
                EditorGUILayout.HelpBox("All layers are assigned.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Not all layers are assigned.", MessageType.Warning);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_useMainCamera, new GUIContent("Use Main Camera"));

            if (!_useMainCamera.boolValue)
            {
                EditorGUILayout.PropertyField(_mainCameraProperty, new GUIContent("Camera"));
            }

            if (!_useMainCamera.boolValue && !_mainCameraProperty.objectReferenceValue)
            {
                EditorGUILayout.HelpBox("Please assign a camera.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
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
    }
}