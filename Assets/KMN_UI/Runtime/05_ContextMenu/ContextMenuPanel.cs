using UnityEngine;
using UnityEngine.UI;

namespace KMN.UI.ContextMenu
{
    [RequireComponent(typeof(Button))]
    public class ContextMenuPanel : UIBase
    {
        [field: SerializeField] public Button PanelButton { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DisableUI();
        }
    }
}