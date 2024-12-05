using UnityEngine;

namespace Systems.UI.Components
{
    public abstract class BaseComponent : MonoBehaviour
    {
        public abstract void InitComponent();
        public abstract void CloseComponent();
    }
}