using System;
using UnityEngine;

namespace Systems.OpenAi
{
    [Serializable]
    public class OpenAiResponse
    {
        public string response;
        public string error;

        public OpenAiResponse(string response, string error)
        {
            this.response = response;
            this.error = error;
        }
    }

    [Serializable]
    public class OpenAiResponse<TData>
    {
        public string response;
        public string error;
        public TData data;

        public OpenAiResponse(string response, string error)
        {
            this.response = response;
            this.error = error;

            response = this.response.Replace("```json\n", "");
            response = response.Replace("```", "");

            data = Newtonsoft.Json.JsonConvert.DeserializeObject<TData>(response);
        }
    }
}