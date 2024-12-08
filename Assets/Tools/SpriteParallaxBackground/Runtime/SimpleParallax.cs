using System;
using UnityEngine;

namespace Tools.SpriteParallaxBackground.Runtime
{
    public class SimpleParallax : MonoBehaviour
    {
        [Serializable]
        public class ParallaxLayer
        {
            public Transform layerTransform;
            [Range(0f, 1f)] public float depthMultiplier;
        }

        [SerializeField] private ParallaxLayer[] layers;
        [SerializeField] private float baseSpeed = 1f;
        [SerializeField] private float layerSpeedMultiplier = 0.5f;
        [SerializeField] private Camera mainCamera;

        private Vector3 _previousCameraPosition;

        private void Awake()
        {
            if (!mainCamera)
            {
                mainCamera = Camera.main;
            }

            _previousCameraPosition = mainCamera?.transform.position ?? Vector3.zero;
        }

        private void LateUpdate()
        {
            Vector3 cameraDelta = mainCamera.transform.position - _previousCameraPosition;

            foreach (var layer in layers)
            {
                if (!layer.layerTransform) continue;

                float layerSpeed = baseSpeed * (1f - layer.depthMultiplier) * layerSpeedMultiplier;

                Vector3 newPosition = layer.layerTransform.position;
                newPosition.x += cameraDelta.x * layerSpeed;
                newPosition.y += cameraDelta.y * layerSpeed;
                layer.layerTransform.position = newPosition;
            }

            _previousCameraPosition = mainCamera.transform.position;
        }
    }
}