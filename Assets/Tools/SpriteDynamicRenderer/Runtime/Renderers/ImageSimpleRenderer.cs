using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.SpriteDynamicRenderer.Runtime.Renderers
{
    [RequireComponent(typeof(Image))]
    public class ImageSimpleRenderer : SimpleRenderer<Image>
    {
        protected override void UpdateComponent(Sprite currentSprite)
        {
            Component.sprite = currentSprite;
        }

        public void SetDefaultAnimation(List<Sprite> sprites)
        {
            SpriteData = ScriptableObject.CreateInstance<SpriteDynamicRendererData>();
            SpriteData.AddAnimation("Default", sprites);

            SetAnimation("Default");
        }
    }
}
