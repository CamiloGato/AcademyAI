using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Entities
{
    [Serializable]
    public enum EntityRole
    {
        Player,
        Npc,
        Murder
    }

    [CreateAssetMenu(fileName = "Entity", menuName = "Entity/Data", order = 0)]
    public class EntityData : ScriptableObject
    {
        public string entityName;
        public string context;
        public EntityRole entityRole;
        public List<Sprite> entityAnim;
    }
}