using Systems.Addons;

namespace Systems.Time
{
    public class TimeManager : Singleton<TimeManager>
    {
        public ReactiveVariable<int> currentTime;
    }
}