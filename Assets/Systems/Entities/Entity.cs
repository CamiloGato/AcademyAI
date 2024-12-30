using Tools.SpriteDynamicRenderer.Data;
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

        public void SetSpriteDynamicRendererData(string category, SpriteDynamicRendererData renderData)
        {
            composeRenderer.SetUpRenderer(category, renderData);
        }

        public void Initialize()
        {
            composeRenderer.SetAnimation("Idle");
            composeRenderer.PlayAnimation();
            data.entityAnim = composeRenderer.CreateRender();
        }

    }
}