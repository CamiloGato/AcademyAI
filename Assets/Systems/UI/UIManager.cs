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
        [SerializeField] private LoadingPanel loadingPanel;

        public void Initialize()
        {
            hudPanel.OpenPanel();
            dialogPanel.ClosePanel();
            murderSelectorPanel.ClosePanel();
            loadingPanel.ClosePanel();
        }

    }
}