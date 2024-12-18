using System.Collections.Generic;
using Tools.Addons;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteCombiner : Singleton<SpriteCombiner>
    {
        private static readonly int Width = Shader.PropertyToID("width");
        private static readonly int Height = Shader.PropertyToID("height");
        private static readonly int LayerCount = Shader.PropertyToID("num_layers");
        private static readonly int OutputTexture = Shader.PropertyToID("output_texture");
        private const int MaxTextures = 10;
        private static readonly string InputTextures = "input_textures";
        [SerializeField] private ComputeShader combineShader;

        private int _outputWidth;
        private int _outputHeight;

        public List<Sprite> CombineSprites(Texture2D[] inputSprites, int spriteWidth = 0, int spriteHeight = 0, float pixelsPerUnit = 100f, int frameAmount = 1)
        {
            ValidateInputs(inputSprites);

            _outputWidth = inputSprites[0].width;
            _outputHeight = inputSprites[0].height;

            RenderTexture outputTexture = CreateRenderTexture();

            ExecuteComputeShader(inputSprites, outputTexture);
            Texture2D finalTexture = CreateFinalTexture(outputTexture);
            outputTexture.Release();

            return GenerateSprites(finalTexture, spriteWidth, spriteHeight, pixelsPerUnit, frameAmount);
        }

        private void ValidateInputs(Texture2D[] inputSprites)
        {
            if (inputSprites == null || inputSprites.Length == 0)
            {
                Debug.LogError("No sprites provided for combination.");
                throw new System.ArgumentException("Input sprites cannot be null or empty.");
            }

            foreach (Texture2D sprite in inputSprites)
            {
                if (!sprite)
                {
                    Debug.LogWarning("One or more input sprites are null. They will be replaced with a black texture.");
                }
            }
        }

        private RenderTexture CreateRenderTexture()
        {
            var renderTexture = new RenderTexture(_outputWidth, _outputHeight, 0, RenderTextureFormat.ARGB32)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Point,
                useMipMap = false,
                autoGenerateMips = false
            };
            renderTexture.Create();
            return renderTexture;
        }

        private void ExecuteComputeShader(Texture2D[] inputSprites, RenderTexture outputTexture)
        {
            int kernelHandle = combineShader.FindKernel("combine_sprites");
            combineShader.SetInt(Width, _outputWidth);
            combineShader.SetInt(Height, _outputHeight);
            combineShader.SetInt(LayerCount, inputSprites.Length);
            combineShader.SetTexture(kernelHandle, OutputTexture, outputTexture);

            for (int i = 0; i < MaxTextures; i++)
            {
                if (i < inputSprites.Length && inputSprites[i])
                {
                    combineShader.SetTexture(kernelHandle, $"{InputTextures}[{i}]", inputSprites[i]);
                }
                else
                {
                    Texture2D blackTexture = new Texture2D(1, 1);
                    blackTexture.SetPixel(0, 0, Color.black);
                    blackTexture.Apply();
                    combineShader.SetTexture(kernelHandle, $"{InputTextures}[{i}]", blackTexture);
                }
            }

            int threadGroupsX = Mathf.CeilToInt(_outputWidth / 8f);
            int threadGroupsY = Mathf.CeilToInt(_outputHeight / 8f);

            combineShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);
        }

        private Texture2D CreateFinalTexture(RenderTexture outputTexture)
        {
            var finalTexture = new Texture2D(_outputWidth, _outputHeight, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            RenderTexture.active = outputTexture;
            finalTexture.ReadPixels(new Rect(0, 0, _outputWidth, _outputHeight), 0, 0);
            finalTexture.Apply();
            RenderTexture.active = null;

            return finalTexture;
        }

        private List<Sprite> GenerateSprites(Texture2D finalTexture, int spriteWidth, int spriteHeight, float pixelsPerUnit, int frameAmount)
        {
            var sprites = new List<Sprite>();

            if (spriteWidth > 0 && spriteHeight > 0)
            {
                sprites.AddRange(CreateMultipleSprites(finalTexture, spriteWidth, spriteHeight, pixelsPerUnit, frameAmount));
            }
            else
            {
                sprites.Add(CreateFullTextureSprite(finalTexture, pixelsPerUnit));
            }

            return sprites;
        }

        private IEnumerable<Sprite> CreateMultipleSprites(Texture2D texture, int spriteWidth, int spriteHeight, float pixelsPerUnit, int frameAmount)
        {
            var sprites = new List<Sprite>();
            int columns = texture.width / spriteWidth;

            for (int i = 0; i < frameAmount; i++)
            {
                int row = i / columns;
                int column = i % columns;
                Rect spriteRect = new Rect(column * spriteWidth, texture.height - (row + 1) * spriteHeight, spriteWidth, spriteHeight);
                Sprite sprite = Sprite.Create(texture, spriteRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprites.Add(sprite);
            }

            return sprites;
        }

        private Sprite CreateFullTextureSprite(Texture2D texture, float pixelsPerUnit)
        {
            Rect fullRect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, fullRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }
    }
}
