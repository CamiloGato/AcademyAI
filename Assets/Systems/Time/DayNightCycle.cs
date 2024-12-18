using UnityEngine;

namespace Systems.Time
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private DayNightCycleData dayNightCycleData;
        private DayNightCallable[] _dayNightCallables;

        private int _currentHour;

        private void Start()
        {
            TimeManager.Instance.currentTime.OnValueChanged += ChangeCycle;

            _dayNightCallables = FindObjectsByType<DayNightCallable>(FindObjectsSortMode.None);
        }

        private void ChangeCycle(TimeData time)
        {

        }
    }
}