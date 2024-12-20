using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Systems.Data;
using Systems.Data.Mappers;
using Systems.Entities;
using Systems.Entities.Cloth;
using Systems.OpenAi;
using Systems.UI;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;

namespace Systems.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private string conversationId;
        [SerializeField] private ClothStorage[] clothsStorage;
        [SerializeField] private OpenAiConfiguration openAiConfiguration;
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private UIManager uiManager;

        private async void Start()
        {
            uiManager.Initialize();

            print("Creating History...");
            HistoryContextData historyContextData = await CreateHistory();
            print("History created!");

            foreach (NpcContextData npcContextData in historyContextData.npcList)
            {
                Entity entity = Instantiate(gameConfiguration.entityPrefab, Vector3.zero, Quaternion.identity);
                EntityData entityData = npcContextData.ToEntityData();
                entity.SetData(entityData);

                foreach (NpcContextClothData npcContextClothData in npcContextData.clothList)
                {
                    // Find the cloth by category and name from clothStorage
                    SpriteDynamicRendererData spriteDynamicRendererData = clothsStorage
                        .SelectMany(clothStorage => clothStorage.cloths)
                        .Where(clothCategory => clothCategory.categoryName == npcContextClothData.category)
                        .SelectMany(clothCategory => clothCategory.renderData)
                        .FirstOrDefault(renderData => renderData.AnimationSectionName == npcContextClothData.cloth);

                }
            }
        }

        public async UniTask<HistoryContextData> CreateHistory()
        {
            OpenAiManager.SetApiKey(openAiConfiguration.apiKey);
            string clothResponse = "Lista de ropas: ";
            foreach (ClothStorage clothStorage in clothsStorage)
            {
                clothResponse += clothStorage.name + "\n";
                clothResponse += clothStorage.ToString();
            }

            OpenAiMessageData clothDatabase = new OpenAiMessageData()
            {
                from = FromType.User,
                message = clothResponse
            };

            List<OpenAiMessageData> openAiMessageData = openAiConfiguration.defaultMessages
                .Find(x => x.conversationId == conversationId).messages
                .ToList();

            openAiMessageData.Add(clothDatabase);

            OpenAiResponse<HistoryContextData> response = await OpenAiManager.CreateRequest<HistoryContextData>(openAiMessageData);
            return response.data;
        }
    }
}