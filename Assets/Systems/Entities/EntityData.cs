﻿using System;
using System.Collections.Generic;
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
        public List<Sprite> entityAnim;
    }
}