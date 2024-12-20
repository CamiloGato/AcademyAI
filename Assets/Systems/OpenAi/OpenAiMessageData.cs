using System;
using UnityEngine;

namespace Systems.OpenAi
{
    [Serializable]
    public enum FromType
    {
        System,
        Assistant,
        User,
    }

    [Serializable]
    public class OpenAiMessageData
    {
        public FromType from;
        [TextArea(3, 10)]
        public string message;
    }

    [Serializable]
    public class OpenAiMessageDataList
    {
        public string conversationId;
        public OpenAiMessageData[] messages;
    }
}