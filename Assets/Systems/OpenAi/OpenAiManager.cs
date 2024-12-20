using System;
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

        public static async UniTask<OpenAiResponse<TData>> CreateRequest<TData>(List<OpenAiMessageData> messagesData)
        {
            OpenAiOptions openAiOptions = new OpenAiOptions()
            {
                ApiKey = _apiKey
            };
            OpenAIService openAiService = new OpenAIService(openAiOptions);
            List<ChatMessage> messages = new List<ChatMessage>();

            foreach (OpenAiMessageData messageData in messagesData)
            {
                switch (messageData.from)
                {
                    case FromType.System:
                        messages.Add(ChatMessage.FromSystem(messageData.message));
                        break;
                    case FromType.Assistant:
                        messages.Add(ChatMessage.FromAssistant(messageData.message));
                        break;
                    case FromType.User:
                        messages.Add(ChatMessage.FromUser(messageData.message));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            ChatCompletionCreateRequest completionCreateRequest = new ChatCompletionCreateRequest()
            {
                Messages = messages,
                Model = Models.Gpt_4o
            };

            ChatCompletionCreateResponse completionResult = await openAiService.ChatCompletion.CreateCompletion(
                completionCreateRequest
            );

            string success = completionResult.Choices.First().Message.Content;
            string error = completionResult.Successful ? string.Empty : completionResult.Error?.Message;
            OpenAiResponse<TData> openAiResponse = new OpenAiResponse<TData>(success, error);

            return openAiResponse;
        }

    }
}