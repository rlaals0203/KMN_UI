using System;
using System.Collections.Generic;
using System.Linq;
using KMN.UI.Core;
using UnityEngine;

namespace KMN.UI.ContextMenu
{
    public abstract class BaseContextMenu : UIBase
    {
        [field: SerializeField] public ContextActionSO[] ContextActions { get; private set; }
        public override EUILayer Layer => EUILayer.ContextMenu;
        public Action OnAction;

        public abstract void ShowMenu(object data);
        public virtual void CloseMenu() => DisableUI(true);
    }
    
    public class BaseContextMenu<T> : BaseContextMenu
    {
        [SerializeField] private Transform root;
        private readonly Dictionary<ContextActionSO, BaseContextAction<T>> _contextCache = new();
        
        public sealed override void ShowMenu(object data)
        {
            EnableUI(true);
            ShowMenu((T)data);
        }

        protected virtual void ShowMenu(T data)
        {
            Clear();

            foreach (ContextActionSO actionSO in ContextActions)
            {
                var action = GetOrCreateAction(actionSO);
                if (!action.CanShow(data))
                {
                    action.DisableUI();
                    continue;
                }
                
                InitAction(action, data);
            }
        }

        private void InitAction(BaseContextAction<T> action, T dataType)
        {
            action.Init(dataType);
            action.OnCallbackInvoked += HandleActionCalled;
        }

        private void HandleActionCalled()
        {
            OnAction?.Invoke();
            CloseMenu();
        }

        private BaseContextAction<T> GetOrCreateAction(ContextActionSO action)
        {
            if (_contextCache.TryGetValue(action, out var result))
                return result;
            
            var prefab = action.contextAction as BaseContextAction<T>;
            var instance = Instantiate(prefab, root);
            _contextCache[action] = instance;
            SortActions();

            return instance;
        }

        private void Clear()
        {
            foreach (var action in _contextCache.Values)
            {
                action.OnCallbackInvoked -= HandleActionCalled;
                action.DisableUI();
            }
        }

        private void SortActions()
        {
            var sorted = _contextCache.Values.ToList();
            sorted.Sort((a, b) => b.ContextActionSO.sortOrder.CompareTo(a.ContextActionSO.sortOrder));
            
            for (int i = 0; i < sorted.Count; i++)
            {
                sorted[i].transform.SetSiblingIndex(i);
            }
        }
    }
}