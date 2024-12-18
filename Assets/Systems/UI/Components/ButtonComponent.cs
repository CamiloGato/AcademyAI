using System;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.UI.Components
{
    [RequireComponent(typeof(ImageSimpleRenderer))]
    public class ButtonComponent : BaseComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string animationName;
        private ImageSimpleRenderer _buttonImageRenderer;

        public Action OnButtonClicked;

        private void Awake()
        {
            _buttonImageRenderer = GetComponent<ImageSimpleRenderer>();
        }

        public override void InitComponent()
        {
            _buttonImageRenderer.SetAnimation(animationName);
            _buttonImageRenderer.SetCurrentFrameIndex(0);
        }

        public override void CloseComponent()
        {
            _buttonImageRenderer.StopAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _buttonImageRenderer.SetAnimation(animationName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _buttonImageRenderer.StopAnimation();
        }
    }
}