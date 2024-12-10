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

        protected SpriteDynamicRendererData SpriteData
        {
            get => spriteData;
            set => spriteData = value;
        }
        protected T Component => _component;
        public string CurrentAnimation => currentAnimation;
        public int FrameRate => frameRate;

        protected virtual void Awake()
        {
            _component = GetComponent<T>();
        }

        protected virtual void Update()
        {
            if (CheckIsValid()) return;
            if (CheckOneShot()) return;

            _frameTimer += Time.deltaTime;
            if (_frameTimer >= 1f / frameRate)
            {
                _frameTimer = 0f;
                _currentFrameIndex = (_currentFrameIndex + 1) % _currentFrames.Count;
                UpdateComponent(_currentFrames[_currentFrameIndex]);
            }
        }

        protected abstract void UpdateComponent(Sprite currentSprite);

        private bool CheckIsValid()
        {
            return string.IsNullOrEmpty(currentAnimation) || _currentFrames == null || _currentFrames.Count == 0 || !spriteData;
        }

        private bool CheckOneShot()
        {
            return _currentFrameIndex >= _currentFrames.Count - 1 && playOneShot;
        }

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
            return spriteData.Texture2D;
        }

        public int GetDefaultAnimationFrames()
        {
            return spriteData.DefaultAnimationFrames;
        }

        public void StopAnimation()
        {
            currentAnimation = string.Empty;
        }
    }
}