using Systems.Time;
using TMPro;
using UnityEngine;

namespace Systems.UI.Components
{
    [RequireComponent(typeof(TMP_Text))]
    public class TimeComponent : BaseComponent
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public override void InitComponent()
        {
            TimeManager.Instance.currentTime.OnValueChanged += OnTimeChange;
        }

        public override void CloseComponent()
        {
            TimeManager.Instance.currentTime.OnValueChanged -= OnTimeChange;
        }

        private void OnTimeChange(TimeData time)
        {
            _text.text = $"{time.hour:00}:{time.minute:00}";
        }
    }
}