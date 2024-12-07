using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Views
{
    public class ChatUserView : BaseView
    {
        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text userText;
        [SerializeField] private Image userImage;

        public override void OpenView()
        {

        }

        public override void CloseView()
        {

        }
    }
}