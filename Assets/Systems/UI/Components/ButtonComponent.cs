using System;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.UI.Components
{
    public class ButtonComponent : BaseComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string animationName;
        [SerializeField] private ImageSimpleRenderer buttonImageRenderer;

        public Action OnButtonClicked;

        public override void InitComponent()
        {
            buttonImageRenderer.SetAnimation(animationName);
        }

        public override void CloseComponent()
        {
            buttonImageRenderer.StopAnimation();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClicked?.Invoke();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            buttonImageRenderer.PlayAnimation();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            buttonImageRenderer.StopAnimation();
        }
    }
}