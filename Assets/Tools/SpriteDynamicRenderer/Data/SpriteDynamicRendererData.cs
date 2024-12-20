using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Tools.SpriteDynamicRenderer.Data
{
    public class SpriteDynamicRendererData : ScriptableObject
    {
        [Serializable]
        public class AnimationDictionary : SerializedDictionary<string, List<Sprite>> { }

        [SerializeField] private string animationSectionName;
        [SerializeField, Tooltip("Dictionary mapping animation names to sprite lists.")]
        private AnimationDictionary spriteAnimationDictionary;

        public Texture2D Texture2D { get; private set; }
        public int DefaultAnimationFrames { get; private set; }

        public string AnimationSectionName => animationSectionName;

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (spriteAnimationDictionary == null)
            {
                spriteAnimationDictionary = new AnimationDictionary();
            }

            if (spriteAnimationDictionary.Count > 0)
            {
                DefaultAnimationFrames = 0;
                foreach (KeyValuePair<string, List<Sprite>> entry in spriteAnimationDictionary)
                {
                    if (entry.Value == null || entry.Value.Count == 0) continue;

                    Texture2D ??= entry.Value[0]?.texture;
                    DefaultAnimationFrames = DefaultAnimationFrames == 0 ? entry.Value.Count : DefaultAnimationFrames;
                    break;
                }
            }
        }

        public void SetAnimationName(string animationName)
        {
            animationSectionName = animationName;
        }

        public void AddAnimation(string animationName, List<Sprite> spriteSheets)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                throw new ArgumentException("Animation name cannot be null or empty.");
            }

            if (spriteSheets == null || spriteSheets.Count == 0)
            {
                throw new ArgumentException("Sprite list cannot be null or empty.");
            }

            if (!spriteSheets[0]?.texture)
            {
                throw new ArgumentException("Sprite texture cannot be null.");
            }

            spriteAnimationDictionary[animationName] = spriteSheets;

            Texture2D ??= spriteSheets[0].texture;
            DefaultAnimationFrames = DefaultAnimationFrames == 0 ? spriteSheets.Count : DefaultAnimationFrames;
        }

        public void RemoveAnimation(string animationName)
        {
            if (!spriteAnimationDictionary.ContainsKey(animationName))
            {
                Debug.LogWarning($"Animation '{animationName}' does not exist.");
                return;
            }

            spriteAnimationDictionary.Remove(animationName);
        }

        public bool TryGetAnimation(string animationName, out List<Sprite> spriteSheets)
        {
            return spriteAnimationDictionary.TryGetValue(animationName, out spriteSheets);
        }

        public Sprite GetDefaultSprite(string animationName)
        {
            if (spriteAnimationDictionary.TryGetValue(animationName, out var sprites) && sprites.Count > 0)
            {
                return sprites[0];
            }

            Debug.LogError($"Animation '{animationName}' does not exist or has no sprites.");
            return null;
        }

        public int GetAnimationFrameCount(string animationName)
        {
            return spriteAnimationDictionary.TryGetValue(animationName, out var sprites) ? sprites.Count : 0;
        }

        public override string ToString()
        {
            return animationSectionName;
        }
    }
}
