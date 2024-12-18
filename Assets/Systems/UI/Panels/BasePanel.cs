using UnityEngine;

namespace Systems.UI.Panels
{
    public abstract class BasePanel : MonoBehaviour
    {
        public abstract void OpenPanel();
        public abstract void ClosePanel();


        [ContextMenu("Open Panel Test")]
        public void OpenPanelTest()
        {
            OpenPanel();
        }

        [ContextMenu("Close Panel Test")]
        public void ClosePanelTest()
        {
            ClosePanel();
        }
    }
}