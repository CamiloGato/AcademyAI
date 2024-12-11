using System;
using UnityEngine;

namespace Systems.Entities
{
    [Serializable]
    public enum EntityType
    {
        Player,
        Npc,
        Murder
    }

    [CreateAssetMenu(fileName = "Entity", menuName = "Entity/Data", order = 0)]
    public class EntityData : ScriptableObject
    {
        public string entityName;
        public EntityType entityType;
    }
}