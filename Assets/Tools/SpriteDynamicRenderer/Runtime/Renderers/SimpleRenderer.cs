using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Runtime.Renderers
{
    public abstract class SimpleRenderer<T> : MonoBehaviour where T : Object
    {
        [SerializeField] private SpriteDynamicRendererData spriteData;
        [SerializeField] private string currentAnimation;
        [SerializeField] private int frameRate = 12;
        [SerializeField] private bool playOneShot;

        private T _component;
        private List<Sprite> _currentFrames;
        private int _currentFrameIndex;
        private float _frameTimer;
        private bool _isPlaying;

        public SpriteDynamicRendererData SpriteData
        {
            get => spriteData;
            protected set => spriteData = value;
        }
        protected T Component => _component;
        public string CurrentAnimation => currentAnimation;
        public int FrameRate => frameRate;
        public int CurrentFrameIndex => _currentFrameIndex;

        protected virtual void Awake()
        {
            _component = GetComponent<T>();
        }

        protected virtual void Update()
        {
            if (!IsValid())
            {
                StopAnimation();
                return;
            }

            if (playOneShot && _currentFrameIndex >= _currentFrames.Count - 1)
            {
                _isPlaying = false;
                return;
            }

            UpdateAnimation();
        }

        protected abstract void UpdateComponent(Sprite currentSprite);

        private bool IsValid()
        {
            if (!spriteData || string.IsNullOrEmpty(currentAnimation))
            {
                return false;
            }

            if (_currentFrames == null || _currentFrames.Count == 0)
            {
                return false;
            }

            return true;
        }

        private void UpdateAnimation()
        {
            if (!_isPlaying) return;

            _frameTimer += Time.deltaTime;

            if (_frameTimer >= 1f / frameRate)
            {
                _frameTimer = 0f;
                _currentFrameIndex = (_currentFrameIndex + 1) % _currentFrames.Count;
                UpdateComponent(_currentFrames[_currentFrameIndex]);
            }
        }

        public void SetAnimation(string animationName)
        {
            if (!spriteData || !spriteData.TryGetAnimation(animationName, out List<Sprite> animationData))
            {
                Debug.LogWarning($"Animation '{animationName}' not found in SpriteData.");
                return;
            }

            currentAnimation = animationName;
            _currentFrames = animationData;
            _currentFrameIndex = 0;
            _isPlaying = true;
        }

        public void StopAnimation()
        {
            currentAnimation = string.Empty;
            _isPlaying = false;
            _currentFrames = null;
            _currentFrameIndex = 0;
        }

        public void SetFrameRate(int newFrameRate)
        {
            frameRate = Mathf.Max(1, newFrameRate);
        }

        public void SetPlayOneShot(bool value)
        {
            playOneShot = value;
        }

        public void SetCurrentFrameIndex(int index)
        {
            _currentFrameIndex = Mathf.Clamp(index, 0, _currentFrames.Count - 1);
        }
    }
}
