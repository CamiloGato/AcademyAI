using System;
using Tools.Addons;

namespace Systems.Time
{
    [Serializable]
    public struct TimeData
    {
        public int hour;
        public int minute;
    }

    public class TimeManager : Singleton<TimeManager>
    {
        public float multiplier = 1;

        public ReactiveVariable<TimeData> currentTime;
        
        private float _time;
        private bool _isPaused;

        private int _currentHour;
        private int _currentMinute;

        private void Update()
        {
            if (_isPaused) return;
            
            _time += UnityEngine.Time.deltaTime * multiplier;
            if (_time >= 1)
            {
                _time = 0;
                _currentMinute++;
                if (_currentMinute >= 60)
                {
                    _currentMinute = 0;
                    _currentHour++;
                    if (_currentHour >= 24)
                    {
                        _currentHour = 0;
                    }
                }
            }

            currentTime.Value = new TimeData
            {
                hour = _currentHour,
                minute = _currentMinute
            };

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