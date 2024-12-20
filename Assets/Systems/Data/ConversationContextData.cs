using System;

namespace Systems.Data
{
    [Serializable]
    public class ConversationContextData
    {
        public string npc;
        public string rol;
        public string context;
        public string question;
    }

    [Serializable]
    public class ConversationResponseData
    {
        public string npc;
        public string response;
        public string emotion;
    }
}