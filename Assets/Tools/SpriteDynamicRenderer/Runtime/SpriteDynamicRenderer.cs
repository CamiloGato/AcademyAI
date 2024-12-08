using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteDynamicRenderer : MonoBehaviour
    {
        [SerializeField] private List<SpriteDynamicRendererData> renderers;

        public int frameRate = 12;
        private float _frameTimer;
        private int _currentFrame;

        private Texture2D _combinedTexture;
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private List<SpriteAnimationData> spriteAnimationData;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            Sprite sprite = renderers[0].spriteAnimationData[0].spriteSheets[0];
            (int, int) size = ((int)sprite.rect.width, (int)sprite.rect.height);
            _combinedTexture = new Texture2D(size.Item1, size.Item2);

            spriteAnimationData = new List<SpriteAnimationData>();
            CombineSprites();

            if (spriteAnimationData.Count > 0 && spriteAnimationData[0].spriteSheets.Count > 0)
            {
                _spriteRenderer.sprite = spriteAnimationData[0].spriteSheets[0];
            }
        }

        private void Update()
        {
            _spriteRenderer.sprite = spriteAnimationData[0].spriteSheets[_currentFrame];

            _frameTimer += Time.deltaTime;
            if (_frameTimer >= 1f / frameRate)
            {
                _frameTimer = 0;
                _currentFrame++;
                if (_currentFrame >= spriteAnimationData[0].spriteSheets.Count)
                {
                    _currentFrame = 0;
                }
            }
        }

        private void CombineSprites()
        {
            SpriteDynamicRendererData dynamicRendererData = renderers[0];

            for (int animationDataIndex = 0; animationDataIndex < dynamicRendererData.spriteAnimationData.Count; animationDataIndex++)
            {
                List<Sprite> spriteSheets = new List<Sprite>();
                string animationName = dynamicRendererData.spriteAnimationData[animationDataIndex].animationName;
                int spriteSheetAmount = dynamicRendererData.spriteAnimationData[animationDataIndex].spriteSheets.Count;

                for (int currentFrame = 0; currentFrame < spriteSheetAmount; currentFrame++)
                {
                    Color[] combinedPixels = new Color[_combinedTexture.width * _combinedTexture.height];
                    foreach (SpriteDynamicRendererData spriteDynamicRendererData in renderers)
                    {
                        Sprite currentSprite = spriteDynamicRendererData.spriteAnimationData[animationDataIndex].spriteSheets[currentFrame];
                        Color[] spritePixels = GetSpritePixels(currentSprite);
                        for (int i = 0; i < combinedPixels.Length; i++)
                        {
                            combinedPixels[i] = CombineColors(combinedPixels[i], spritePixels[i]);
                        }
                    }
                    _combinedTexture.SetPixels(combinedPixels);
                    _combinedTexture.Apply();

                    Rect rect = new Rect(0, 0, _combinedTexture.width, _combinedTexture.height);
                    Vector2 pivot = new Vector2(0.5f, 0.5f);
                    Sprite sprite = Sprite.Create(_combinedTexture, rect, pivot);
                    spriteSheets.Add(sprite);
                }

                SpriteAnimationData animationData = new SpriteAnimationData(animationName, spriteSheets);
                spriteAnimationData.Add(animationData);
            }

        }

        private Color[] GetSpritePixels(Sprite sprite)
        {
            return sprite.texture.GetPixels(
                (int)sprite.rect.x,
                (int)sprite.rect.y,
                (int)sprite.rect.width,
                (int)sprite.rect.height
            );
        }

        private Color CombineColors(Color primary, Color second)
        {
            return new Color(
                primary.r * (1 - second.a) + second.r * second.a,
                primary.g * (1 - second.a) + second.g * second.a,
                primary.b * (1 - second.a) + second.b * second.a,
                primary.a * (1 - second.a) + second.a
            );
        }
    }
}