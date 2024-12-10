using Systems.UI.Views;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class DialogPanel : BasePanel
    {
        [SerializeField] private ChatNpcView npcView;
        [SerializeField] private ChatUserView userView;

        public override void OpenPanel()
        {
            npcView.CloseView();
            userView.CloseView();
        }

        public override void ClosePanel()
        {
            npcView.CloseView();
            userView.CloseView();
        }

    }
}