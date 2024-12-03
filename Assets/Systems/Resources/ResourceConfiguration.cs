using UnityEngine;

namespace Systems.Resources
{
    [CreateAssetMenu(fileName = "Resources", menuName = "Resources/Configuration", order = 0)]
    public class ResourceConfiguration : ScriptableObject
    {
        public string resourceName;
        public int maxAmount;
        public Color color;
    }
}