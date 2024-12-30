using UnityEngine;
using UnityEngine.UI;

namespace Tools.Extension
{
    public static class GraphicExtensions
    {
        public static async Awaitable CrossFadeAlphaAsync(this Graphic graphic, float alpha, float duration, bool ignoreTimeScale = false)
        {
            graphic.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
            await Awaitable.WaitForSecondsAsync(duration);
        }

        public static async Awaitable CrossFadeColorAsync(this Graphic graphic, Color targetColor, float duration, bool ignoreTimeScale = false, bool useAlpha = true)
        {
            graphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
            await Awaitable.WaitForSecondsAsync(duration);
        }

    }
}