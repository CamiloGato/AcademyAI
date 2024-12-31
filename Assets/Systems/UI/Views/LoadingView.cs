using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Views
{
    public class LoadingView : BaseView
    {
        [Header("General")]
        [SerializeField] private float loadingDuration;
        [SerializeField] private Image loadingImage;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private TMP_Text messageText;

        [Header("Loading Text")]
        [SerializeField] private string loading;
        [SerializeField] private float loadingTextDuration;

        private bool _isLoading;

        public override void OpenView()
        {
            _isLoading = true;
            loadingImage.CrossFadeAlpha(
                1f,
                loadingDuration,
                true
            );

            loadingText.CrossFadeAlpha(
                1f,
                loadingDuration,
                true
            );

            messageText.CrossFadeAlpha(
                1f,
                loadingDuration,
                true
            );
            messageText.text = "";

            StartCoroutine(UpdateLoadingText());
        }

        public override void CloseView()
        {
            _isLoading = false;

            loadingImage.CrossFadeAlpha(
                0f,
                loadingDuration,
                true
            );

            loadingText.CrossFadeAlpha(
                0f,
                loadingDuration,
                true
            );

            messageText.CrossFadeAlpha(
                0f,
                loadingDuration,
                true
            );
            messageText.text = "";

            StopCoroutine(UpdateLoadingText());
        }

        public async void ChangeText(string text)
        {
            try
            {
                await messageText.CrossFadeAlphaAsync(
                    0f,
                    loadingDuration,
                    true
                );

                messageText.text = text;

                await messageText.CrossFadeAlphaAsync(
                    1f,
                    loadingDuration,
                    true
                );
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private IEnumerator UpdateLoadingText()
        {
            List<string> dots = new List<string> {".", "..", "..."};
            int index = 0;

            while (_isLoading)
            {
                yield return new WaitForSeconds(loadingTextDuration);

                loadingText.text = loading + dots[index];
                index = (index + 1) % dots.Count;
            }
        }
    }
}