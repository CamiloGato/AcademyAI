using Systems.UI.Panels;
using UnityEngine;

namespace Systems.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private DialogPanel dialogPanel;
        [SerializeField] private HudPanel hudPanel;
        [SerializeField] private MurderSelectorPanel murderSelectorPanel;

        public void Initialize()
        {
            hudPanel.OpenPanel();
            dialogPanel.ClosePanel();
            murderSelectorPanel.ClosePanel();
        }

    }
}