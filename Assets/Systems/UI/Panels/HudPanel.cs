using Systems.UI.Components;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class HudPanel : BasePanel
    {
        [Header("Components")]
        [SerializeField] private TimeComponent timeComponent;
        [SerializeField] private ButtonComponent murderSelectorComponent;

        [Header("Dependencies")]
        [SerializeField] private UIManager uiManager;

        private bool _isMurderSelectorPanelOpen;

        public override void OpenPanel()
        {
            timeComponent.InitComponent();
            murderSelectorComponent.InitComponent();

            murderSelectorComponent.OnButtonClicked += OpenMurderSelectorPanel;
        }

        public override void ClosePanel()
        {
            timeComponent.CloseComponent();
            murderSelectorComponent.CloseComponent();

            murderSelectorComponent.OnButtonClicked -= OpenMurderSelectorPanel;
        }

        private void OpenMurderSelectorPanel()
        {
        }
    }
}