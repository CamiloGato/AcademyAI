using Tools.Addons;

namespace Systems.Time
{
    public class TimeManager : Singleton<TimeManager>
    {
        public ReactiveVariable<int> currentTime;
        
        private float _time;
        private bool _isPaused;

        private void Update()
        {
            if (_isPaused) return;
            
            _time += UnityEngine.Time.deltaTime;
            currentTime.Value = (int)_time;
        }

        public void StartCounter()
        {
            _time = 0;
            _isPaused = false;
        }

        public void PauseCounter()
        {
            _isPaused = true;
        }

        public void ResumeCounter()
        {
            _isPaused = false;
        }

        public void ResetCounter()
        {
            _time = 0;
            _isPaused = true;
        }

    }
}