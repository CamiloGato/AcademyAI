using Systems.UI;
using Tools.SpriteDynamicRenderer.Runtime;
using UnityEngine;

namespace Systems.Game
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private void Start()
        {
            uiManager.Initialize();
        }
    }
}