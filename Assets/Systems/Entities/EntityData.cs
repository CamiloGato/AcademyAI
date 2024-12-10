using UnityEngine;

namespace Systems.Entities
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Entity/Data", order = 0)]
    public class EntityData : ScriptableObject
    {
        public string entityName;
    }
}