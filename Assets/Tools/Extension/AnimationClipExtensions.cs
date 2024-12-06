using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.Extension
{
    public static class AnimationClipExtensions
    {
        public static AnimationClip CreateAnimationClip(this Sprite[] sprites, int startIndex, int endIndex,
            float frameRate = 12f)
        {
            if (sprites == null || sprites.Length == 0)
            {
                throw new System.ArgumentNullException();
            }

            if (startIndex < 0 || endIndex >= sprites.Length || startIndex > endIndex)
            {
                throw new System.ArgumentException();
            }

            Sprite[] selectedSprites = sprites.Skip(startIndex).Take(endIndex - startIndex + 1).ToArray();

            AnimationClip animationClip = new AnimationClip
            {
                frameRate = frameRate
            };

            EditorCurveBinding spriteBinding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[selectedSprites.Length];
            for (int i = 0; i < selectedSprites.Length; i++)
            {
                keyframes[i] = new ObjectReferenceKeyframe
                {
                    time = i / frameRate,
                    value = selectedSprites[i]
                };
            }

            AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, keyframes);

            return animationClip;
        }
    }
}