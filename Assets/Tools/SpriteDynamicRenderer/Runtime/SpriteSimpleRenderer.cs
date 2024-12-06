using System;
using System.Collections.Generic;
using Tools.Objects;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSimpleRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteDynamicRendererData spriteData;
        private SpriteRenderer _spriteRenderer;

        private string _currentAnimation;
        private List<Sprite> _currentFrames;
        private int _currentFrameIndex;
        private float _frameTimer;

        [SerializeField] private int frameRate = 12;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(_currentAnimation) || _currentFrames == null || _currentFrames.Count == 0)
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

            _currentAnimation = animationName;
            _currentFrames = animationData.spriteSheets;
            _currentFrameIndex = 0;
        }

        /// <summary>
        /// Stop the animation
        /// </summary>
        public void StopAnimation()
        {
            _currentAnimation = null;
            _currentFrames = null;
            _currentFrameIndex = 0;
        }
    }
}