using System.Collections.Generic;
using Systems.Entities;
using Systems.UI.Controllers;
using UnityEngine;

namespace Systems.UI.Panels
{
    public class MurderSelectorPanel : BasePanel
    {
        [Header("Components")]
        [SerializeField] private SelectorController selectorController;

        [Header("Data")]
        [SerializeField] private List<EntityData> data;

        public override void OpenPanel()
        {
            selectorController.StartController();
            selectorController.SetUp(data);
        }

        public override void ClosePanel()
        {
            selectorController.CloseController();
        }
    }
}