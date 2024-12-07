using Systems.UI.Views;
using UnityEngine;

namespace Systems.UI.Controllers
{
    public abstract class BaseController<TView> : MonoBehaviour where TView : BaseView
    {
        public TView view;

        public abstract void StartController();
        public abstract void CloseController();

    }
}