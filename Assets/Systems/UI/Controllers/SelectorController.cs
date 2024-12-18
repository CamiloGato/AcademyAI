using System.Collections.Generic;
using Systems.Entities;
using Systems.UI.Components;
using Systems.UI.Views;
using UnityEngine;

namespace Systems.UI.Controllers
{
    public class SelectorController : BaseController<SelectorView>
    {
        [SerializeField] private NpcComponent npcComponentPrefab;

        private Dictionary<EntityData, NpcComponent> _components = new Dictionary<EntityData, NpcComponent>();

        public EntityData SelectedEntity { get; private set; }
        private NpcComponent SelectedEntityComponent { get; set; }

        // TODO: Create an object pool for the NpcComponent
        public void SetUp(List<EntityData> data)
        {
            _components = new Dictionary<EntityData, NpcComponent>();
            foreach (EntityData entityData in data)
            {
                NpcComponent entityNpcComponent = Instantiate(npcComponentPrefab, view.ContentSection);
                entityNpcComponent.InitComponent();
                entityNpcComponent.SetUpNpc(entityData);
                entityNpcComponent.OnNpcSelected += SelectNpc;
                _components.Add(entityData, entityNpcComponent);
            }
        }

        public void RemoveNpc(EntityData data)
        {
            if (_components.ContainsKey(data))
            {
                NpcComponent entityNpcComponent = _components[data];
                entityNpcComponent.CloseComponent();
                entityNpcComponent.OnNpcSelected -= SelectNpc;
                _components.Remove(data);
                Destroy(entityNpcComponent.gameObject);
            }
        }

        private void SelectNpc(EntityData data)
        {
            if (_components.TryGetValue(data, out NpcComponent entityNpcComponent))
            {
                ChangeEntityComponent(entityNpcComponent, data);
            }
        }

        private void ChangeEntityComponent(NpcComponent entityNpcComponent, EntityData entityData)
        {
            if (SelectedEntityComponent)
            {
                SelectedEntityComponent.DeselectNpc();
            }
            SelectedEntityComponent = entityNpcComponent;
            SelectedEntity = entityData;
        }

        public override void CloseController()
        {
            base.CloseController();

            foreach (KeyValuePair<EntityData, NpcComponent> component in _components)
            {
                component.Value.CloseComponent();
                component.Value.OnNpcSelected -= SelectNpc;
                Destroy(component.Value.gameObject);
            }
        }
    }
}