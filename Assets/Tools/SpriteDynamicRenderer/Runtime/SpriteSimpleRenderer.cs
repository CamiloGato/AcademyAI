using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSimpleRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteDynamicRendererData spriteData;
        [SerializeField] private string currentAnimation;
        [SerializeField] private int frameRate = 12;

        private SpriteRenderer _spriteRenderer;
        private List<Sprite> _currentFrames;
        private int _currentFrameIndex;
        private float _frameTimer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(currentAnimation) || _currentFrames == null || _currentFrames.Count == 0 || !spriteData)
            {
                return;
            }

            _frameTimer += Time.deltaTime;
            if (_frameTimer >= 1f / frameRate)
            {
                _frameTimer = 0f;
                _currentFrameIndex = (_currentFrameIndex + 1) % _currentFrames.Count;
                _spriteRenderer.sprite = _currentFrames[_currentFrameIndex];
            }
        }

        /// <summary>
        /// Change the current animation.
        /// </summary>
        /// <param name="animationName">The name of the animation to play.</param>
        public void SetAnimation(string animationName)
        {
            if (!spriteData)
            {
                Debug.LogWarning("SpriteDynamicRendererData no assigned.");
                return;
            }

            SpriteAnimationData animationData = spriteData.spriteAnimationData.Find(anim => anim.animationName == animationName);

            if (animationData == null)
            {
                Debug.LogWarning($"No found the animation with name: {animationName}");
                return;
            }

            currentAnimation = animationName;
            _currentFrames = animationData.spriteSheets;
            _currentFrameIndex = 0;
        }

        /// <summary>
        /// Stop the animation
        /// </summary>
        public void StopAnimation()
        {
            currentAnimation = null;
            _currentFrames = null;
            _currentFrameIndex = 0;
        }

        /// <summary>
        /// Set the frame rate of the animation.
        /// </summary>
        /// <param name="frames">The number of frames per second.</param>
        public void SetFrameRate(int frames)
        {
            frameRate = frames;
        }

        public Texture2D GetTexture()
        {
            return spriteData.DefaultSprite.texture;
        }
    }
}