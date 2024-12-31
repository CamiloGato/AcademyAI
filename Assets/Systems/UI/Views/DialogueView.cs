using UnityEngine;

namespace Systems.UI.Views
{
    public class DialogueView : BaseView
    {
        [SerializeField] private int multiplier;
        [SerializeField] private int duration;
        [SerializeField] private RectTransform backgroundContainer;
        [SerializeField] private RectTransform nameContainer;
        [SerializeField] private RectTransform imageContainer;

        private Vector3 _initialNamePosition;
        private Vector3 _initialImagePosition;

        public bool IsTransition { get; private set; }

        private void Awake()
        {
            _initialNamePosition = nameContainer.localPosition;
            _initialImagePosition = imageContainer.localPosition;
        }

        public override void OpenView()
        {
            IsTransition = true;

            backgroundContainer.LeanMoveY(0, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            imageContainer.LeanMoveX(_initialImagePosition.x, duration * 1.5f)
                .setEase(LeanTweenType.easeInOutElastic);
            nameContainer.LeanMoveY(_initialNamePosition.y, duration * 1.5f)
                .setEase(LeanTweenType.easeInOutElastic)
                .setOnComplete(
                    () =>
                    {
                        IsTransition = false;
                    }
                );
        }

        public override void CloseView()
        {
            IsTransition = true;

            backgroundContainer.LeanMoveY(-multiplier, duration)
                .setEase(LeanTweenType.easeInOutElastic);
            imageContainer.LeanMoveX(_initialImagePosition.x - multiplier * 1.5f, duration * 1.5f)
                .setEase(LeanTweenType.easeInOutElastic);
            nameContainer.LeanMoveY(_initialNamePosition.y - multiplier * 1.5f, duration * 1.5f)
                .setEase(LeanTweenType.easeInOutElastic)
                .setOnComplete(
                    () =>
                    {
                        IsTransition = false;
                    }
                );
        }
    }
}