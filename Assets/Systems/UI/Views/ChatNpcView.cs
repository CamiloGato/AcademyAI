using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Views
{
    public class ChatNpcView : BaseView
    {
        [SerializeField] private TMP_Text npcName;
        [SerializeField] private TMP_Text npcText;
        [SerializeField] private Image npcChat;

        public override void OpenView()
        {

        }

        public override void CloseView()
        {

        }
    }
}