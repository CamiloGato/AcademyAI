using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Views
{
    public class ChatUserView : BaseView
    {
        [SerializeField] private TMP_Text userNameText;
        [SerializeField] private TMP_Text userInfoText;
        [SerializeField] private TMP_InputField userInput;
        [SerializeField] private TMP_Text characterCountText;
        [SerializeField] private Image userImage;

        private const int MaxCharacterCount = 100;

        public Action<string> OnUserInputChanged;

        public override void OpenView()
        {
            userNameText.gameObject.SetActive(true);
            userInfoText.gameObject.SetActive(true);
            userInput.gameObject.SetActive(true);
            userImage.gameObject.SetActive(true);

            userInput.characterLimit = MaxCharacterCount;

            userInput.onSubmit.AddListener(UserInputChange);
            userInput.onValueChanged.AddListener(UserValueChange);
        }

        public override void CloseView()
        {
            userNameText.gameObject.SetActive(false);
            userInfoText.gameObject.SetActive(false);
            userInput.gameObject.SetActive(false);
            userImage.gameObject.SetActive(false);

            userInput.onSubmit.RemoveListener(UserInputChange);
            userInput.onValueChanged.RemoveListener(UserValueChange);
        }

        public void SetUserName(string userName)
        {
            userNameText.text = userName;
        }

        public void SetUserInfo(string userInfo)
        {
            userInfoText.text = userInfo;
        }

        public void SetUserImage(Sprite userSprite)
        {
            userImage.sprite = userSprite;
        }

        private void UserInputChange(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            OnUserInputChanged?.Invoke(text);
        }

        private void UserValueChange(string text)
        {
            characterCountText.text = $"{text.Length}/{MaxCharacterCount} characters";
        }

    }
}