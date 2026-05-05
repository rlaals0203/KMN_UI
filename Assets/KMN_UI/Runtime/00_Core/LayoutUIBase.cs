using UnityEngine;
using UnityEngine.UI;

namespace KMN.UI
{
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutUIBase : UIBase
    {
        private LayoutElement _layoutElement;

        protected override void Awake()
        {
            base.Awake();
            _layoutElement = GetComponent<LayoutElement>();
        }

        public override void EnableUI(bool hasFade = false)
        {
            base.EnableUI(hasFade);
            _layoutElement.ignoreLayout = false;
        }

        public override void DisableUI(bool hasFade = false)
        {
            base.DisableUI(hasFade);
            _layoutElement.ignoreLayout = true;
        }
    }
}