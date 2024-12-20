using System;
using Systems.Entities;
using UnityEngine;

namespace Systems.Data.Mappers
{
    public static class EntityDataMapper
    {
        public static EntityData ToEntityData(this NpcContextData npcContextData)
        {
            EntityData entityData = ScriptableObject.CreateInstance<EntityData>();
            entityData.entityName = npcContextData.name;
            entityData.context = npcContextData.context;
            entityData.entityRole = Enum.Parse<EntityRole>(char.ToUpper(npcContextData.rol[0]) + npcContextData.rol[1..]);
            return entityData;
        }
    }
}