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
        private static readonly string InputTextures = "input_textures";
        [SerializeField] private ComputeShader combineShader;

        private int _outputWidth;
        private int _outputHeight;

        public Sprite CombineSprites(Texture2D[] inputSprites, int spriteWidth = 0, int spriteHeight = 0, float pixelsPerUnit = 100f)
        {
            if (inputSprites.Length == 0)
            {
                Debug.LogError("No sprites to combine.");
                return null;
            }

            _outputWidth = inputSprites[0].width;
            _outputHeight = inputSprites[0].height;

            RenderTexture outputTexture = new RenderTexture(_outputWidth, _outputHeight, 0, RenderTextureFormat.ARGB32)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Point,
                useMipMap = false,
                autoGenerateMips = false
            };
            outputTexture.Create();

            int kernelHandle = combineShader.FindKernel("combine_sprites");
            combineShader.SetInt(Width, _outputWidth);
            combineShader.SetInt(Height, _outputHeight);
            combineShader.SetInt(LayerCount, inputSprites.Length);
            combineShader.SetTexture(kernelHandle, OutputTexture, outputTexture);

            for (int i = 0; i <= 10; i++)
            {
                if (i < inputSprites.Length && inputSprites[i] != null)
                {
                    combineShader.SetTexture(kernelHandle, $"{InputTextures}[{i}]", inputSprites[i]);
                }
                else
                {
                    combineShader.SetTexture(kernelHandle, $"{InputTextures}[{i}]", Texture2D.blackTexture);
                }
            }

            int threadGroupsX = Mathf.CeilToInt(_outputWidth / 8f);
            int threadGroupsY = Mathf.CeilToInt(_outputHeight / 8f);

            combineShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);

            Texture2D finalTexture = new Texture2D(_outputWidth, _outputHeight, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            RenderTexture.active = outputTexture;
            finalTexture.ReadPixels(new Rect(0, 0, _outputWidth, _outputHeight), 0, 0);
            finalTexture.Apply();

            outputTexture.Release();

            if (spriteWidth > 0 && spriteHeight > 0)
            {
                Rect firstSpriteRect = new Rect(0, finalTexture.height - spriteHeight, spriteWidth, spriteHeight);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                return Sprite.Create(finalTexture, firstSpriteRect, pivot, pixelsPerUnit);
            }

            Rect fullRect = new Rect(0, 0, finalTexture.width, finalTexture.height);
            Vector2 fullPivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(finalTexture, fullRect, fullPivot, pixelsPerUnit);
        }

    }
}