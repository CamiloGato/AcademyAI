using UnityEngine;

namespace Systems.UI.Panels
{
    public abstract class BasePanel : MonoBehaviour
    {
        public abstract void OpenPanel();
        public abstract void ClosePanel();
    }
}