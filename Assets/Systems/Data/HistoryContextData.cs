using System;

namespace Systems.Data
{
    [Serializable]
    public class NpcContextClothData
    {
        public string category;
        public string cloth;
    }

    [Serializable]
    public class NpcContextData
    {
        public string name;
        public string context;
        public string rol;
        public NpcContextClothData[] clothList;
    }

    [Serializable]
    public class HistoryContextData
    {
        public string history;
        public NpcContextData[] npcList;
    }
}