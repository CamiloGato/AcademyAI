using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.UI.Components
{
    public sealed class ButtonOptionComponent : ButtonComponent
    {
        [SerializeField] private float multiplier;
        [SerializeField] private float duration;

        private RectTransform _rectTransform;
        private Vector3 _initialPosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialPosition = _rectTransform.position;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            _rectTransform.LeanMoveX(multiplier, 1f)
                .setEase(LeanTweenType.easeInOutElastic);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            _rectTransform.LeanMoveX(-multiplier, 1f)
                .setEase(LeanTweenType.easeInOutElastic);
        }
    }
}