using Systems.UI.Controllers;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class LoadingPanel : BasePanel
    {
        [SerializeField] private LoadingController loadingController;

        public override void OpenPanel()
        {
            loadingController.StartController();
        }

        public override void ClosePanel()
        {
            loadingController.CloseController();
        }
    }
}