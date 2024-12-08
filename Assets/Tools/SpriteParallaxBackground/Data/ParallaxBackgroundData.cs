using System;
using UnityEngine;

namespace Tools.SpriteParallaxBackground.Data
{
    [CreateAssetMenu(fileName = "Background", menuName = "ParallaxBackground", order = 0)]
    public class ParallaxBackgroundData : ScriptableObject
    {
        [Serializable]
        public class ParallaxLayer
        {
            public string name;
            public Sprite sprite;
            [Range(0f, 1f)] public float depthMultiplier;
        }

        public ParallaxLayer[] layers;
        public float baseSpeed = 1f;
        public float layerSpeedMultiplier = 0.5f;
    }
}