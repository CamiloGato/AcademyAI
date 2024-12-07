using Systems.UI.Components;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class HudPanel : BasePanel
    {
        [Header("Components")]
        [SerializeField] private TimeComponent timeComponent;
        
        public override void OpenPanel()
        {
            timeComponent.InitComponent();
        }

        public override void ClosePanel()
        {
            timeComponent.CloseComponent();
        }
    }
}