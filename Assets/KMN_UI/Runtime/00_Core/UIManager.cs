using System;
using System.Collections.Generic;
using DG.Tweening;
using KMN.Core;
using KMN.EventsBus;
using UnityEngine;

namespace KMN.UI.Core
{
    public enum EUILayer
    {
        HUD,
        Panel,
        Popup,
        ContextMenu,
        Tooltip,
        None
    }
    
    [DefaultExecutionOrder(-25)]
    public class UIManager : MonoSingleton<UIManager>
    { 
        [SerializeField] private PlayerInputSO playerInput;

        private readonly HashSet<UIBase> _registeredUI = new();
        private readonly Stack<UIBase> _uiStack = new();
        
        public OverlayUIManager OverlayManager => OverlayUIManager.Instance;
        public event Action OnUIStackChanged;

        protected override void Awake()
        {
            playerInput.OnESCPressed += HandlePressEsc;
        }

        protected override void OnDestroy()
        {
            foreach (UIBase ui in _registeredUI)
            {
                ui.OnToggleUI -= HandleChangeUIState;
            }
            
            playerInput.OnESCPressed -= HandlePressEsc;
        }
        
        public void RegisterUI(UIBase ui)
        {
            if (!_registeredUI.Add(ui))
                return;
            
            ui.OnToggleUI += HandleChangeUIState;
        }

        public void UnRegisterUI(UIBase ui)
        {
            if (!_registeredUI.Contains(ui))
                return;
            
            _registeredUI.Remove(ui);
            ui.OnToggleUI -= HandleChangeUIState;
        }
        
        private void HandleChangeUIState(UIBase ui, bool isFade)
        {
            ToggleUI(ui, ui.IsActive, isFade);
            TryStackUI(ui, ui.IsActive);
        }

        private void TryStackUI(UIBase ui, bool isActive)
        {
            if (!CanStack(ui)) return;
            
            if (OverlayManager.HasActiveOverlay())
                OverlayManager.CloseAllOverlays();

            if (isActive)
                PushStack(ui);
            else
                PopStack();
            
            OnUIStackChanged?.Invoke();
        }

        private bool CanStack(UIBase ui)
        {
            return ui.Layer == EUILayer.Panel || ui.Layer == EUILayer.Popup;
        }

        private void HandlePressEsc()
        {
            if (_uiStack.Count == 0)
            {
                EventBus.Raise(new PressESCEvent());
                return;
            }
            
            PopStack();
        }

        public void PushStack(UIBase ui)
        {
            if (_uiStack.Contains(ui)) return;

            if (ui.Layer == EUILayer.Panel)
                ClearStack();

            _uiStack.Push(ui);
        }

        public void ClearStack()
        {
            while (_uiStack.Count > 0)
            {
                UIBase top = _uiStack.Pop();
                top.DisableUI();
            }
        }
        
        public void PopStack()
        {
            if (_uiStack.Count == 0)
                return;
            
            UIBase top = _uiStack.Pop();
            top.DisableUI();
        }

        public bool TryGetCurrentPanel(out UIPanel panel)
        {
            panel = null;
            
            foreach (UIBase ui in _uiStack)
            {
                if (ui.Layer == EUILayer.Panel)
                {
                    panel = ui as UIPanel;
                    return true;
                }
            }

            return false;
        }
        
        private void ToggleUI(UIBase ui, bool isActive, bool useFade)
        {
            var cg = ui.CanvasGroup;
            cg.DOKill(true);

            if (useFade)
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
        
        public bool HasStackUI()
        {
            return _uiStack.Count > 0;
        }
    }
}