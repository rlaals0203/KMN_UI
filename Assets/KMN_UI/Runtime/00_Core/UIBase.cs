using System;
using DG.Tweening;
using KMN.UI.Core;
using UnityEngine;

namespace KMN.UI
{
    [DefaultExecutionOrder(5)]
   public class UIBase : MonoBehaviour
    {
        public CanvasGroup CanvasGroup { get; protected set; }
        public RectTransform Rect { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public virtual EUILayer Layer => EUILayer.None;
        public event Action<UIBase, bool> OnToggleUI;

        protected virtual void Awake()
        {
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            Rect = GetComponent<RectTransform>();
            UIManager.Instance?.RegisterUI(this);
        }

        public virtual void EnableUI(bool hasFade = false)
        {
            if (IsActive) return;
            IsActive = true;
            OnToggleUI?.Invoke(this, hasFade);
        }

        public virtual void DisableUI(bool hasFade = false)
        {
            if (!IsActive) return;
            IsActive = false;
            OnToggleUI?.Invoke(this, hasFade);
        }

        public virtual void ToggleUI(bool hasFade = false)
        {
            if (IsActive)
                DisableUI(hasFade);
            else
                EnableUI(hasFade);
            
            HandleToggle(IsActive, hasFade);
        }
        
        protected virtual void OnDestroy()
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance?.UnRegisterUI(this);
            }
        }
        
        private void HandleToggle(bool isActive, bool hasFade)
        {
            var cg = CanvasGroup;
            cg.DOKill(true);

            if (hasFade)
            {
                if (isActive) {
                    cg.alpha = 0;
                    ToggleCanvasGroup(cg, true);
                    cg.DOFade(1, 0.1f).SetUpdate(true);
                }
                else {
                    cg.DOFade(0, 0.1f).OnComplete(() => {
                        ToggleCanvasGroup(cg, false);
                    }).SetUpdate(true);
                }
            }
            else {
                cg.alpha = isActive ? 1 : 0;
                ToggleCanvasGroup(cg, isActive);
            }
        }
        
        private void ToggleCanvasGroup(CanvasGroup cg, bool isActive)
        {
            cg.interactable = isActive;
            cg.blocksRaycasts = isActive;
        }
        
        [UnityEngine.ContextMenu("Show UI")]
        public void ShowUIOnInspector()
        {
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            IsActive = true;
        }

        [UnityEngine.ContextMenu("Hide UI")]
        public void HideUIOnInspector()
        {
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            IsActive = false;
        }
    }
}