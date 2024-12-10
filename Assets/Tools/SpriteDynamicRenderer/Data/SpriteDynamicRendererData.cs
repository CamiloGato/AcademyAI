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

        [SerializeField, Tooltip("Dictionary mapping animation names to sprite lists.")]
        private AnimationDictionary spriteAnimationDictionary;

        public Texture2D Texture2D { get; private set; }
        public int DefaultAnimationFrames { get; private set; }

        private void OnValidate()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (spriteAnimationDictionary == null || spriteAnimationDictionary.Count == 0)
            {
                Texture2D = null;
                DefaultAnimationFrames = 0;
                return;
            }

            foreach (KeyValuePair<string, List<Sprite>> entry in spriteAnimationDictionary)
            {
                if (entry.Value == null || entry.Value.Count == 0) continue;

                Texture2D ??= entry.Value[0]?.texture;
                DefaultAnimationFrames = DefaultAnimationFrames == 0 ? entry.Value.Count : DefaultAnimationFrames;

                break;
            }
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

            spriteAnimationDictionary[animationName] = spriteSheets;

            if (!Texture2D)
            {
                Texture2D = spriteSheets[0].texture;
            }

            if (DefaultAnimationFrames == 0)
            {
                DefaultAnimationFrames = spriteSheets.Count;
            }
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
            if (!spriteAnimationDictionary.TryGetValue(animationName, out var sprites) || sprites.Count == 0)
            {
                Debug.LogError($"Animation '{animationName}' does not exist or has no sprites.");
                return null;
            }

            return sprites[0];
        }

        public int GetAnimationFrameCount(string animationName)
        {
            return spriteAnimationDictionary.TryGetValue(animationName, out var sprites) ? sprites.Count : 0;
        }

        public int GetTotalAnimations()
        {
            return spriteAnimationDictionary.Count;
        }

        public IEnumerable<string> GetAllAnimationNames()
        {
            return spriteAnimationDictionary.Keys;
        }
    }
}
