using System;
using Systems.Entities;
using TMPro;
using Tools.SpriteDynamicRenderer.Runtime.Renderers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Systems.UI.Components
{
    public class NpcComponent : BaseComponent, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ImageSimpleRenderer npcSelectorImageRenderer;
        [SerializeField] private ImageSimpleRenderer npcImageRenderer;
        [SerializeField] private TMP_Text npcNameText;

        private const string DefaultAnimationName = "Default";
        private const string SelectorAnimationName = "Selector";
        private const string NoneAnimationName = "None";
        private readonly Color _defaultColor = Color.white;
        private readonly Color _disabledColor = Color.gray;
        private readonly Color _hoverColor = Color.white;

        private Image _npcImage;
        private bool _isSelected;

        private void OnEnable()
        {
            _npcImage = npcImageRenderer.GetComponent<Image>();
        }

        public override void InitComponent()
        {
            npcSelectorImageRenderer.SetPlayOneShot(true);
            npcSelectorImageRenderer.SetAnimation(NoneAnimationName);
            npcImageRenderer.StopAnimation();
            npcNameText.text = "";
            SetColor(_disabledColor, instant: true);
        }

        public override void CloseComponent()
        {
            npcSelectorImageRenderer.SetPlayOneShot(true);
            npcSelectorImageRenderer.SetAnimation(NoneAnimationName);
            npcImageRenderer.StopAnimation();
            npcNameText.text = "";
            SetColor(_disabledColor, instant: true);
        }

        public void SetUpNpc(EntityData data)
        {
            npcImageRenderer.SetAnimation(DefaultAnimationName);
            npcNameText.text = data.entityName;
            SetColor(_defaultColor, instant: true);
        }

        public void SelectNpc()
        {
            npcSelectorImageRenderer.SetAnimation(SelectorAnimationName);
            SetColor(_defaultColor);
            _isSelected = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SelectNpc();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                SetColor(_hoverColor);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                SetColor(_disabledColor);
            }
        }

        private void SetColor(Color targetColor, bool instant = false)
        {
            if (instant)
            {
                npcImageRenderer.GetComponent<Image>().color = targetColor;
            }
            else
            {
                _npcImage.CrossFadeColor(targetColor, 0.2f, true, true);
            }
        }
    }
}