using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Data
{
    [Serializable]
    public class SpriteAnimationData
    {
        public string animationName;
        public List<Sprite> spriteSheets;

        public SpriteAnimationData(string animationName, List<Sprite> spriteSheets)
        {
            this.animationName = animationName;
            this.spriteSheets = spriteSheets;
        }
    }

    public class SpriteDynamicRendererData : ScriptableObject
    {
        public Sprite DefaultSprite => spriteAnimationData.Count > 0 ? spriteAnimationData[0].spriteSheets[0] : null;
        private readonly Dictionary<string, List<Sprite>> _spriteSheetDictionary = new Dictionary<string, List<Sprite>>();
        [Tooltip("You should not modify this list directly. Its just for debugging purposes.")]
        [SerializeField] private List<SpriteAnimationData> spriteAnimationData = new List<SpriteAnimationData>();

        private void OnValidate()
        {
            _spriteSheetDictionary.Clear();
            foreach (SpriteAnimationData animationData in spriteAnimationData)
            {
                _spriteSheetDictionary[animationData.animationName] = animationData.spriteSheets;
            }
        }

        public void AddAnimation(string animationName, Sprite[] spriteSheets)
        {
            spriteAnimationData.Add(new SpriteAnimationData(animationName, new List<Sprite>(spriteSheets)));
            _spriteSheetDictionary[animationName] = new List<Sprite>(spriteSheets);
        }

        public void AddAnimation(string animationName, List<Sprite> spriteSheetInfos)
        {
            spriteAnimationData.Add(new SpriteAnimationData(animationName, spriteSheetInfos));
            _spriteSheetDictionary[animationName] = spriteSheetInfos;
        }

        public void RemoveAnimation(string animationName)
        {
            spriteAnimationData.RemoveAll(animation => animation.animationName == animationName);
            _spriteSheetDictionary.Remove(animationName);
        }

        public bool TryGetAnimation(string animationName, out List<Sprite> spriteSheets)
        {
            return _spriteSheetDictionary.TryGetValue(animationName, out spriteSheets);
        }
    }
}