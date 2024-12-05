using UnityEngine;

namespace Systems.UI.Panel
{
    public abstract class BasePanel : MonoBehaviour
    {
        public abstract void OpenPanel();
        public abstract void ClosePanel();
    }
}