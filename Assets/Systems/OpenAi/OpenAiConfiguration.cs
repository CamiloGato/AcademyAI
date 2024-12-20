using System.Collections.Generic;
using UnityEngine;

namespace Systems.OpenAi
{
    [CreateAssetMenu(fileName = "OpenAiConfiguration", menuName = "OpenAi/Configuration", order = 0)]
    public class OpenAiConfiguration : ScriptableObject
    {
        public List<OpenAiMessageDataList> defaultMessages;
        public string apiKey;
    }
}