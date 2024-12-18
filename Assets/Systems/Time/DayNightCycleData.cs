using System;
using UnityEngine;

namespace Systems.Time
{
    [Serializable]
    public class TimeCycleData
    {
        public string name;
        public int cycleHour;
        public int cycleMinute;
        public Color cycleColor;
    }

    [CreateAssetMenu(fileName = "DayNightCycle", menuName = "Time/DayNightCycle", order = 0)]
    public class DayNightCycleData : ScriptableObject
    {
        public TimeCycleData[] timeCycleData;
    }
}