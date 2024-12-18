using Systems.UI.Components;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class HudPanel : BasePanel
    {
        [Header("Components")]
        [SerializeField] private TimeComponent timeComponent;
        [SerializeField] private ButtonComponent buttonComponent;

        [Header("Dependencies")]
        [SerializeField] private UIManager uiManager;

        public override void OpenPanel()
        {
            timeComponent.InitComponent();
            buttonComponent.InitComponent();
        }

        public override void ClosePanel()
        {
            timeComponent.CloseComponent();
            buttonComponent.CloseComponent();
        }
    }
}