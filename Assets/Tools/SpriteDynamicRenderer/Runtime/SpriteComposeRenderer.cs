using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Tools.SpriteDynamicRenderer.Data;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteComposeRenderer : MonoBehaviour
    {
        private readonly SerializedDictionary<string, SpriteSimpleRenderer> _spriteRenderers = new SerializedDictionary<string, SpriteSimpleRenderer>();

        private void Awake()
        {
            InitializeRenderers();
        }

        private void InitializeRenderers()
        {
            SpriteSimpleRenderer[] spriteSimpleRenderers = GetComponentsInChildren<SpriteSimpleRenderer>();

            if (spriteSimpleRenderers.Length == 0)
            {
                Debug.LogWarning("No SpriteSimpleRenderers found as children of this GameObject.");
            }

            foreach (SpriteSimpleRenderer spriteSimpleRenderer in spriteSimpleRenderers)
            {
                _spriteRenderers.Add(spriteSimpleRenderer.gameObject.name, spriteSimpleRenderer);
            }
        }

        public void SetUpRenderers()
        {

        }

        public void SetAnimation(string animationName)
        {
            if (!ValidateRenderers()) return;

            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteRenderers.Values)
            {
                spriteSimpleRenderer.SetAnimation(animationName);
            }
        }

        public void StopAnimation()
        {
            if (!ValidateRenderers()) return;

            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteRenderers.Values)
            {
                spriteSimpleRenderer.StopAnimation();
            }
        }

        public List<Sprite> CreateRender()
        {
            if (!ValidateRenderers()) return null;

            Texture2D[] textures = GetTexturesFromRenderers(out int defaultAnimationFrames);

            if (textures == null || textures.Length == 0)
            {
                Debug.LogError("Failed to collect textures from SpriteSimpleRenderers.");
                return null;
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
                return null;
            }

            return combinedSprites;
        }

        private Texture2D[] GetTexturesFromRenderers(out int defaultAnimationFrames)
        {
            defaultAnimationFrames = 0;

            Texture2D[] textures = new Texture2D[_spriteRenderers.Count];

            int index = 0;
            foreach (SpriteSimpleRenderer spriteRenderer in _spriteRenderers.Values)
            {
                SpriteDynamicRendererData spriteData = spriteRenderer.SpriteData;

                if (!spriteData || !spriteData.Texture2D)
                {
                    Debug.LogWarning($"Missing SpriteData or Texture2D on renderer: {spriteRenderer.name}");
                    continue;
                }

                defaultAnimationFrames = defaultAnimationFrames == 0
                    ? spriteData.DefaultAnimationFrames
                    : defaultAnimationFrames;

                textures[index++] = spriteData.Texture2D;
            }

            return textures;
        }

        private bool ValidateRenderers()
        {
            if (_spriteRenderers == null || _spriteRenderers.Count == 0)
            {
                Debug.LogWarning("SpriteSimpleRenderers are not initialized or empty. Make sure to call this script after Awake.");
                return false;
            }

            return true;
        }
    }
}