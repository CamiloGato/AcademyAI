using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Views
{
    public class ChatNpcView : BaseView
    {
        [SerializeField] private TMP_Text npcNameText;
        [SerializeField] private TMP_Text npcInfoText;
        [SerializeField] private Image npcImage;

        public override void OpenView()
        {
            npcNameText.gameObject.SetActive(true);
            npcInfoText.gameObject.SetActive(true);
            npcImage.gameObject.SetActive(true);
        }

        public override void CloseView()
        {
            npcNameText.gameObject.SetActive(false);
            npcInfoText.gameObject.SetActive(false);
            npcImage.gameObject.SetActive(false);
        }

        public void SetNpcName(string npcName)
        {
            npcNameText.text = npcName;
        }

        public void SetNpcInfo(string info)
        {
            npcInfoText.text = info;
        }

        public void SetNpcImage(Sprite sprite)
        {
            npcImage.sprite = sprite;
        }

    }
}