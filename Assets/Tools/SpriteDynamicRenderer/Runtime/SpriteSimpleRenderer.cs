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
        [SerializeField] private bool playOneShot;

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
            if (CheckIsValid()) return;
            if (CheckOneShot()) return;

            _frameTimer += Time.deltaTime;
            if (_frameTimer >= 1f / frameRate)
            {
                _frameTimer = 0f;
                _currentFrameIndex = (_currentFrameIndex + 1) % _currentFrames.Count;
                _spriteRenderer.sprite = _currentFrames[_currentFrameIndex];
            }

        }

        private bool CheckIsValid()
        {
            return string.IsNullOrEmpty(currentAnimation) || _currentFrames == null || _currentFrames.Count == 0 || !spriteData;
        }

        private bool CheckOneShot()
        {
            return _currentFrameIndex >= _currentFrames.Count - 1 && playOneShot;
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

            if (!spriteData.TryGetAnimation(animationName, out List<Sprite> animationData))
            {
                Debug.LogWarning($"No found the animation with name: {animationName}");
                return;
            }

            currentAnimation = animationName;
            _currentFrames = animationData;
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

        public void SetPlayOneShot(bool value)
        {
            playOneShot = value;
        }

        public Texture2D GetTexture()
        {
            return spriteData.DefaultSprite.texture;
        }

        public void StopAnimation()
        {
            currentAnimation = string.Empty;
        }

        [ContextMenu("Set Default Sprite")]
        public void Test()
        {
            SetAnimation(currentAnimation);
        }
    }
}