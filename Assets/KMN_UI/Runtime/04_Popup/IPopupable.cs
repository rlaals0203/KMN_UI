using System;
using KMN.UI.Interaction;

namespace KMN.UI.Popup
{
    public interface IPopupProvider
    {
        public event Action<Func<object>, ICallbackData> OnShowPopup;
    }
}