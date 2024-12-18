using System;
using UnityEngine;

namespace Systems.UI.Views
{
    public class SelectorView : BaseView
    {
        [SerializeField] private int multiplier;
        [SerializeField] private int duration;
        [SerializeField] private RectTransform contentSection;
        [SerializeField] private RectTransform buttonsSection;
        [SerializeField] private RectTransform titleSection;

        public RectTransform ContentSection => contentSection;

        private Vector3 _initialContentPosition;
        private Vector3 _initialButtonsPosition;
        private Vector3 _initialTitlePosition;

        private void Awake()
        {
            _initialContentPosition = contentSection.localPosition;
            _initialButtonsPosition = buttonsSection.localPosition;
            _initialTitlePosition = titleSection.localPosition;
        }

        public override void OpenView()
        {
            contentSection.LeanMoveX(0, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            buttonsSection.LeanMoveX(_initialButtonsPosition.x, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            titleSection.LeanMoveY(_initialTitlePosition.y, duration)
                .setEase(LeanTweenType.easeInOutElastic);
        }

        public override void CloseView()
        {
            contentSection.LeanMoveX(-multiplier, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            buttonsSection.LeanMoveX(_initialButtonsPosition.x + multiplier, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            titleSection.LeanMoveY(_initialTitlePosition.y + multiplier, duration)
                .setEase(LeanTweenType.easeInOutElastic);
        }
    }
}