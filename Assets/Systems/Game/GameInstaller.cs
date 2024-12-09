using System.Collections.Generic;
using Systems.UI.Panels;
using Tools.SpriteDynamicRenderer.Runtime;
using UnityEngine;

namespace Systems.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private HudPanel hudPanel;
        [SerializeField] private SpriteComposeRenderer[] playerComposeRenderer;

        private void Start()
        {
            hudPanel.OpenPanel();

            foreach (SpriteComposeRenderer spriteComposeRenderer in playerComposeRenderer)
            {
                spriteComposeRenderer.SetAnimation("Idle");
            }

        }
    }
}