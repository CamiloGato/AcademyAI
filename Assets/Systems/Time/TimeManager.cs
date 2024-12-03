using Systems.Addons;

namespace Systems.Time
{
    public class TimeManager : Singleton<TimeManager>
    {
        public ReactiveVariable<int> currentTime;

        private float _time;

        private void Update()
        {
            _time += UnityEngine.Time.deltaTime;
            currentTime.Value = (int)_time;
        }

    }
}