using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<string, List<Sprite>> _spritesSheetDictionary = new Dictionary<string, List<Sprite>>();
        [Tooltip("You should not modify this list directly. Its just for debugging purposes.")]
        [SerializeField] private List<SpriteAnimationData> spriteAnimationData = new List<SpriteAnimationData>();

        public Texture2D Texture2D { get; private set; } = null;
        public int DefaultAnimationFrames { get; private set; } = 0;

        private void OnValidate()
        {
            DefaultAnimationFrames = 0;
            Texture2D = null;

            _spritesSheetDictionary.Clear();
            foreach (SpriteAnimationData animationData in spriteAnimationData)
            {
                if (DefaultAnimationFrames == 0)
                {
                    DefaultAnimationFrames = animationData.spriteSheets.Count;
                }

                if (!Texture2D)
                {
                    Texture2D = animationData.spriteSheets[0].texture;
                }

                _spritesSheetDictionary[animationData.animationName] = animationData.spriteSheets;
            }
        }

        public void AddAnimation(string animationName, List<Sprite> spriteSheetInfos)
        {
            if (DefaultAnimationFrames == 0)
            {
                DefaultAnimationFrames = spriteSheetInfos.Count;
            }

            if (!Texture2D)
            {
                Texture2D = spriteSheetInfos[0].texture;
            }

            spriteAnimationData.Add(new SpriteAnimationData(animationName, spriteSheetInfos));
            _spritesSheetDictionary[animationName] = spriteSheetInfos;
        }

        public void RemoveAnimation(string animationName)
        {
            spriteAnimationData.RemoveAll(animation => animation.animationName == animationName);
            _spritesSheetDictionary.Remove(animationName);
        }

        public bool TryGetAnimation(string animationName, out List<Sprite> spriteSheets)
        {
            return _spritesSheetDictionary.TryGetValue(animationName, out spriteSheets);
        }

        public Sprite GetDefaultSprite(string animationName)
        {
            return _spritesSheetDictionary[animationName][0];
        }

        public int GetAnimationCount(string animationName)
        {
            return _spritesSheetDictionary[animationName].Count;
        }
    }
}