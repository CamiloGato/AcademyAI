using Systems.UI.Views;
using UnityEngine;

namespace Systems.UI.Controllers
{
    public abstract class BaseController<TView> : MonoBehaviour where TView : BaseView
    {
        public TView view;

        public virtual void StartController()
        {
            view.OpenView();
        }
        public virtual void CloseController()
        {
            view.CloseView();
        }

    }
}