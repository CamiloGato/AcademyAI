using UnityEngine;

namespace Systems.Time
{
    public abstract class DayNightCallable : MonoBehaviour
    {
        public abstract void ChangeCycle(float time);
    }
}