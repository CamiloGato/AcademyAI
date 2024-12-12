using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteComposeRenderer : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private ImageSimpleRenderer imageRenderer;

        private SpriteSimpleRenderer[] _spriteSimpleRenderers;

        private void Awake()
        {
            InitializeRenderers();
        }

        private void InitializeRenderers()
        {
            _spriteSimpleRenderers = GetComponentsInChildren<SpriteSimpleRenderer>();

            if (_spriteSimpleRenderers.Length == 0)
            {
                Debug.LogWarning("No SpriteSimpleRenderers found as children of this GameObject.");
            }
        }

        public void SetAnimation(string animationName)
        {
            if (!ValidateRenderers()) return;

            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteSimpleRenderers)
            {
                spriteSimpleRenderer.SetAnimation(animationName);
            }
        }

        public void StopAnimation()
        {
            if (!ValidateRenderers()) return;

            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteSimpleRenderers)
            {
                spriteSimpleRenderer.StopAnimation();
            }
        }

        [ContextMenu("Create Render")]
        public void CreateRender()
        {
            if (!ValidateRenderers()) return;

            Texture2D[] textures = GetTexturesFromRenderers(out int defaultAnimationFrames);

            if (textures == null || textures.Length == 0)
            {
                Debug.LogError("Failed to collect textures from SpriteSimpleRenderers.");
                return;
            }

            List<Sprite> combinedSprites = SpriteCombiner.Instance.CombineSprites(
                textures,
                spriteWidth: 80,
                spriteHeight: 64,
                pixelsPerUnit: 32,
                frameAmount: defaultAnimationFrames
            );

            if (combinedSprites == null || combinedSprites.Count == 0)
            {
                Debug.LogError("Failed to combine sprites.");
                return;
            }

            UpdateImageRenderer(combinedSprites);
        }

        private Texture2D[] GetTexturesFromRenderers(out int defaultAnimationFrames)
        {
            defaultAnimationFrames = 0;

            Texture2D[] textures = new Texture2D[_spriteSimpleRenderers.Length];

            for (int i = 0; i < _spriteSimpleRenderers.Length; i++)
            {
                SpriteDynamicRendererData spriteData = _spriteSimpleRenderers[i].SpriteData;

                if (!spriteData || !spriteData.Texture2D)
                {
                    Debug.LogWarning($"Missing SpriteData or Texture2D on renderer: {_spriteSimpleRenderers[i].name}");
                    continue;
                }

                defaultAnimationFrames = defaultAnimationFrames == 0
                    ? spriteData.DefaultAnimationFrames
                    : defaultAnimationFrames;

                textures[i] = spriteData.Texture2D;
            }

            return textures;
        }

        private void UpdateImageRenderer(List<Sprite> combinedSprites)
        {
            if (!imageRenderer)
            {
                Debug.LogError("ImageRenderer is not assigned.");
                return;
            }

            if (_spriteSimpleRenderers.Length == 0)
            {
                Debug.LogWarning("No SpriteSimpleRenderers found for frame rate reference.");
                return;
            }

            imageRenderer.SetFrameRate(_spriteSimpleRenderers[0].FrameRate);
            imageRenderer.SetDefaultAnimation(combinedSprites);
            imageRenderer.SetCurrentFrameIndex(_spriteSimpleRenderers[0].CurrentFrameIndex);
        }

        private bool ValidateRenderers()
        {
            if (_spriteSimpleRenderers == null || _spriteSimpleRenderers.Length == 0)
            {
                Debug.LogWarning("SpriteSimpleRenderers are not initialized or empty. Make sure to call this script after Awake.");
                return false;
            }

            return true;
        }
    }
}