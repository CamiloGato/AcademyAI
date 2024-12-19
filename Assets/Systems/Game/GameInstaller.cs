using Systems.Entities;
using Systems.OpenAi;
using Systems.UI;
using Tools.SpriteDynamicRenderer.Runtime;
using UnityEngine;

namespace Systems.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private UIManager uiManager;

        private void Start()
        {
            uiManager.Initialize();

            for (int i = 0; i < gameConfiguration.amountNpc; i++)
            {
                // TODO: Use ObjectPool to instantiate entities
                Entity entity = Instantiate(gameConfiguration.entityPrefab);
                EntityData data = ScriptableObject.CreateInstance<EntityData>();
                data.entityName = $"Npc-{i}";
                data.entityType = EntityType.Npc;

                entity.SetData(data);
            }
        }

        [ContextMenu("Create Context")]
        public async void CreateContext()
        {
            OpenAiManager.SetApiKey(gameConfiguration.openAiApiKey);
            string result = await OpenAiManager.CreateRequest("Hello", "How are you?");
            Debug.Log(result);
        }
    }
}