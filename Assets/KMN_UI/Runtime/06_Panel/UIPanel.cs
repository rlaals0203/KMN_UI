using KMN.UI.Core;

namespace KMN.UI
{
    public abstract class UIPanel : UIBase
    {
        public override EUILayer Layer => EUILayer.Panel;

        protected override void Awake()
        {
            base.Awake();
            DisableUI();
        }
    }
}