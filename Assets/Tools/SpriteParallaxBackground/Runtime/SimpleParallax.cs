using Tools.SpriteParallaxBackground.Data;
using UnityEngine;

namespace Tools.SpriteParallaxBackground.Runtime
{
    public class SimpleParallax : MonoBehaviour
    {
        public ParallaxBackgroundData data;
        public Transform[] layers;
        public Camera mainCamera;
        public bool useMainCamera = true;

        private Vector3 _previousCameraPosition;

        private void Start()
        {
            if (useMainCamera)
            {
                mainCamera = Camera.main;
            }

            _previousCameraPosition = mainCamera?.transform.position ?? Vector3.zero;

            for (int index = 0; index < data.layers.Length; index++)
            {
                ParallaxBackgroundData.ParallaxLayer parallaxLayer = data.layers[index];
                Transform layerTransform = layers[index];

                if (!layerTransform) continue;

                SpriteRenderer spriteRenderer = layerTransform.GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                {
                    spriteRenderer.sprite = parallaxLayer.sprite;
                }
            }
        }

        private void LateUpdate()
        {
            Vector3 cameraDelta = mainCamera.transform.position - _previousCameraPosition;

            for (int index = 0; index < data.layers.Length; index++)
            {
                ParallaxBackgroundData.ParallaxLayer parallaxLayer = data.layers[index];
                Transform layerTransform = layers[index];

                if (!layerTransform) continue;

                float layerSpeed = data.baseSpeed * (1f - parallaxLayer.depthMultiplier) * data.layerSpeedMultiplier;

                Vector3 newPosition = layerTransform.position;
                newPosition.x += cameraDelta.x * layerSpeed;
                newPosition.y += cameraDelta.y * layerSpeed;
                layerTransform.position = newPosition;
            }

            _previousCameraPosition = mainCamera.transform.position;
        }
    }
}