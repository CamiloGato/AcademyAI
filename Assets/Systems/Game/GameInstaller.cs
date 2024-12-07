using System;
using Systems.UI.Panels;
using UnityEngine;

namespace Systems.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private HudPanel hudPanel;

        private void Start()
        {
            hudPanel.OpenPanel();
        }
    }
}