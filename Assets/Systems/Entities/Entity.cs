using System.Collections.Generic;
using Tools.SpriteDynamicRenderer.Data;
using Tools.SpriteDynamicRenderer.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Entities
{
    public class Entity : MonoBehaviour
    {
        public EntityData data;
        public SpriteComposeRenderer composeRenderer;
        public Image image;

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
            image.sprite = data.entityAnim[0];
        }

    }
}