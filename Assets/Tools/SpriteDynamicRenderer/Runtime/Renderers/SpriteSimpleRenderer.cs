using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime.Renderers
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSimpleRenderer : SimpleRenderer<SpriteRenderer>
    {
        protected override void UpdateComponent(Sprite currentSprite)
        {
            Component.sprite = currentSprite;
        }

        [ContextMenu("Set Default Sprite")]
        public void Test()
        {
            SetAnimation(CurrentAnimation);
        }
    }
}