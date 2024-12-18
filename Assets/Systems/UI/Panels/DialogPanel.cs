using Systems.UI.Controllers;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class DialogPanel : BasePanel
    {
        [SerializeField] private DialogueController dialogueController;

        public override void OpenPanel()
        {
            dialogueController.StartController();
        }

        public override void ClosePanel()
        {
            dialogueController.CloseController();
        }

    }
}