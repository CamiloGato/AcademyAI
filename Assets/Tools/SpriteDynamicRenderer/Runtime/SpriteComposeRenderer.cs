using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteComposeRenderer : MonoBehaviour
    {
        public ImageSimpleRenderer imageRenderer;
        private SpriteSimpleRenderer[] _spriteSimpleRenderers;

        private void Awake()
        {
            _spriteSimpleRenderers = GetComponentsInChildren<SpriteSimpleRenderer>();
        }

        public void SetAnimation(string animationName)
        {
            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteSimpleRenderers)
            {
                spriteSimpleRenderer.SetAnimation(animationName);
            }
        }

        public void StopAnimation()
        {
            foreach (SpriteSimpleRenderer spriteSimpleRenderer in _spriteSimpleRenderers)
            {
                spriteSimpleRenderer.StopAnimation();
            }
        }

        [ContextMenu("Create Render")]
        public void CreateRender()
        {
            int defaultAnimationFrames = 0;
            Texture2D[] textures = new Texture2D[_spriteSimpleRenderers.Length];
            for (int i = 0; i < _spriteSimpleRenderers.Length; i++)
            {
                defaultAnimationFrames = defaultAnimationFrames == 0 ? _spriteSimpleRenderers[i].GetDefaultAnimationFrames() : defaultAnimationFrames;
                textures[i] = _spriteSimpleRenderers[i].GetTexture();
                if (textures[i]) continue;

                Debug.LogWarning($"Texture is null for sprite renderer: {_spriteSimpleRenderers[i].name}");
                return;
            }

            List<Sprite> sprite = SpriteCombiner.Instance.CombineSprites(textures, 80, 64, 32, defaultAnimationFrames);
            imageRenderer.SetFrameRate(_spriteSimpleRenderers[0].FrameRate);
            imageRenderer.SetDefaultRenderer(sprite);
            imageRenderer.SetCurrentFrameIndex(_spriteSimpleRenderers[0].CurrentFrameIndex);
        }
    }
}