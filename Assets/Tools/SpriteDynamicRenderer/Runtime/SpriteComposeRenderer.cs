using UnityEngine;
using UnityEngine.UI;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteComposeRenderer : MonoBehaviour
    {
        public Image image;
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
            Texture2D[] textures = new Texture2D[_spriteSimpleRenderers.Length];
            for (int i = 0; i < _spriteSimpleRenderers.Length; i++)
            {
                textures[i] = _spriteSimpleRenderers[i].GetTexture();
                if (textures[i]) continue;

                Debug.LogWarning($"Texture is null for sprite renderer: {_spriteSimpleRenderers[i].name}");
                return;
            }

            Sprite sprite = SpriteCombiner.Instance.CombineSprites(textures, 80, 64, 32);
            image.sprite = sprite;
        }
    }
}