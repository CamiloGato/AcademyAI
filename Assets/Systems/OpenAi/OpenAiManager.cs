using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace Systems.OpenAi
{
    public static class OpenAiManager
    {
        private static string _apiKey;
        private static string _projectID;
        private static string _organizationID;

        public static void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public static void SetProjectID(string projectID)
        {
            _projectID = projectID;
        }

        public static void SetOrganizationID(string organizationID)
        {
            _organizationID = organizationID;
        }

        public static async UniTask<string> CreateRequest(string systemMessage, string userMessage)
        {
            OpenAiOptions openAiOptions = new OpenAiOptions()
            {
                ApiKey = _apiKey
            };
            OpenAIService openAiService = new OpenAIService(openAiOptions);

            ChatCompletionCreateRequest completionCreateRequest = new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>()
                {
                    ChatMessage.FromSystem(systemMessage),
                    ChatMessage.FromUser(userMessage),
                },
                Model = Models.Gpt_4o
            };

            ChatCompletionCreateResponse completionResult = await openAiService.ChatCompletion.CreateCompletion(
                completionCreateRequest
            );

            return completionResult.Successful ? completionResult.Choices.First().Message.Content : string.Empty;
        }

    }
}