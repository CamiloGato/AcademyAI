using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Tools.Extension
{
    public class Texture2DCutData : ScriptableObject
    {
        public int gridWidth;
        public int gridHeight;
        public int offsetX;
        public int offsetY;
        public int paddingX;
        public int paddingY;
    }

    public static class Texture2DExtensions
    {
        public static bool IsRectEmpty(this Texture2D spriteSheet, Rect rect)
        {
            int x = Mathf.FloorToInt(rect.x);
            int y = Mathf.FloorToInt(rect.y);
            int width = Mathf.FloorToInt(rect.width);
            int height = Mathf.FloorToInt(rect.height);

            Color[] pixels = spriteSheet.GetPixels(x, y, width, height);

            foreach (Color pixel in pixels)
            {
                if (pixel.a > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static SpriteRect CreateSpriteRectFromCoordinates(this Texture2D spriteSheet, int x, int y, Texture2DCutData cutData)
        {
            int columns = (spriteSheet.width - cutData.offsetX + cutData.paddingX) / (cutData.gridWidth + cutData.paddingX);

            Rect rect = new Rect(
                cutData.offsetX + x * (cutData.gridWidth + cutData.paddingX),
                spriteSheet.height - cutData.offsetY - (y + 1) * cutData.gridHeight - y * cutData.paddingY,
                cutData.gridWidth,
                cutData.gridHeight
            );

            if (spriteSheet.IsRectEmpty(rect))
            {
                return null;
            }

            SpriteRect spriteRect = new SpriteRect
            {
                name = $"{spriteSheet.name}_{y * columns + x}",
                rect = rect,
                alignment = (int)SpriteAlignment.Center,
                pivot = new Vector2(0.5f, 0.5f)
            };

            return spriteRect;
        }

        public static List<SpriteRect> CreateSpriteRects(this Texture2D spriteSheet, Texture2DCutData cutData)
        {
            List<SpriteRect> spriteRects = new List<SpriteRect>();

            int columns = (spriteSheet.width - cutData.offsetX + cutData.paddingX) / (cutData.gridWidth + cutData.paddingX);
            int rows = (spriteSheet.height - cutData.offsetY + cutData.paddingY) / (cutData.gridHeight + cutData.paddingY);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    SpriteRect spriteRect = spriteSheet.CreateSpriteRectFromCoordinates(x, y, cutData);
                    if (spriteRect != null)
                    {
                        spriteRects.Add(spriteRect);
                    }
                }
            }

            return spriteRects;
        }

        public static bool CutSpriteSheet(this Texture2D spriteSheet, Texture2DCutData cutData)
        {
            string path = AssetDatabase.GetAssetPath(spriteSheet);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (!textureImporter) return false;

            textureImporter.spriteImportMode = SpriteImportMode.Multiple;

            ISpriteEditorDataProvider dataProvider = textureImporter.GetSpriteEditorDataProvider();
            dataProvider.InitSpriteEditorDataProvider();

            List<SpriteRect> spriteRects = spriteSheet.CreateSpriteRects(cutData);

            dataProvider.SetSpriteRects(spriteRects.ToArray());
            dataProvider.Apply();

            textureImporter.SaveAndReimport();

            return true;
        }
    }
}