using System;
using UnityEngine;

namespace Tools.Extension
{
    public static class AwaitableExtensions
    {
        public static async void OnComplete(this Awaitable awaitable, System.Action onComplete)
        {
            try
            {
                await awaitable;
                onComplete?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}