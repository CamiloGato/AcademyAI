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
        public List<SpriteAnimationData> spriteAnimationData = new List<SpriteAnimationData>();

        public void AddAnimation(string animationName, Sprite[] spriteSheets)
        {
            spriteAnimationData.Add(new SpriteAnimationData(animationName, new List<Sprite>(spriteSheets)));
        }

        public void AddAnimation(string animationName, List<Sprite> spriteSheetInfos)
        {
            spriteAnimationData.Add(new SpriteAnimationData(animationName, spriteSheetInfos));
        }

        public void RemoveAnimation(string animationName)
        {
            spriteAnimationData.RemoveAll(animation => animation.animationName == animationName);
        }
    }
}