using System;
using Systems.UI.Panel;
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