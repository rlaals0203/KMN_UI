using KMN.UI.Core;
using UnityEngine;

namespace KMN.UI
{
    public class UIToggleUtil : MonoBehaviour
    {
        [UnityEngine.ContextMenu("Show UI")]
        public void ShowUI()
        {
            var canvas  = UIUtility.GetOrAddComponent<CanvasGroup>(gameObject);
            canvas.alpha = 1;
            canvas.interactable = true;
            canvas.blocksRaycasts = true;
        }

        [UnityEngine.ContextMenu("Hide UI")]
        public void HideUI()
        {
            var canvas  = UIUtility.GetOrAddComponent<CanvasGroup>(gameObject);
            canvas.alpha = 0;
            canvas.interactable = false;
            canvas.blocksRaycasts = false;
        }
    }
}