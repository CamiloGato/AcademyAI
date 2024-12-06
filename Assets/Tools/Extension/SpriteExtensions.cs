using UnityEngine;

namespace Tools.Extension
{
    public static class SpriteExtensions
    {
        public static (int, int) GetSize(this Sprite sprite)
        {
            return ((int)sprite.rect.width, (int)sprite.rect.height);
        }
    }
}