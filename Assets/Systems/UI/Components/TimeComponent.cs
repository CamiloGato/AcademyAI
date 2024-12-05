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

        private void OnTimeChange(int time)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            _text.text = $"{minutes:00}:{seconds:00}";
        }
    }
}