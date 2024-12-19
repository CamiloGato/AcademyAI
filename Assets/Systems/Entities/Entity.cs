using Tools.SpriteDynamicRenderer.Runtime;
using UnityEngine;

namespace Systems.Entities
{
    public class Entity : MonoBehaviour
    {
        public EntityData data;
        public SpriteComposeRenderer composeRenderer;

        public void SetData(EntityData entityData)
        {
            data = entityData;
        }
    }
}