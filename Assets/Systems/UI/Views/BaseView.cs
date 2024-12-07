using UnityEngine;

namespace Systems.UI.Views
{
    public abstract class BaseView : MonoBehaviour
    {
        public abstract void OpenView();
        public abstract void CloseView();
    }
}