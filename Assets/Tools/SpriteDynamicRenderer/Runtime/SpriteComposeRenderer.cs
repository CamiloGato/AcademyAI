using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    public class SpriteComposeRenderer : MonoBehaviour
    {
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
    }
}