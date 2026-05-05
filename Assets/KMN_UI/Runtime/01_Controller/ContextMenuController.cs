using System;
using System.Collections.Generic;
using KMN.UI.ContextMenu;
using KMN.UI.Interaction;
using UnityEngine;

namespace KMN.UI.Core
{
    public class ContextMenuData
    {
        public ContextMenuSO menuSO;
        public Func<object> Data;
    }

    public class ContextMenuController : MonoBehaviour
    {
        [SerializeField] private List<ContextMenuSO> menus;
        [SerializeField] private RectTransform menuParent;
        [SerializeField] private RectTransform menuRoot;
        [SerializeField] private ContextMenuPanel panel;

        private readonly Dictionary<InteractableUI, ContextMenuData> _contextMenus = new();
        private readonly Dictionary<ContextMenuSO, BaseContextMenu> _instances = new();
        private BaseContextMenu _currentMenu;

        private void Awake()
        {
            MappingMenus();
            panel.PanelButton.onClick.AddListener(HandleClickPanel);
        }

        private void HandleClickPanel()
        {
            HideCurrentMenu();
        }

        private void MappingMenus()
        {
            foreach (var menu in menus)
            {
                if (menu == null)
                    continue;
                
                BaseContextMenu newMenu = Instantiate(menu.menu, menuRoot);
                newMenu.DisableUI();
                _instances.TryAdd(menu, newMenu);
            }
        }

        private void OnDestroy()
        {
            panel.PanelButton.onClick.RemoveListener(HandleClickPanel);
        }

        public void BindContextMenu<T>(InteractableUI owner, ContextMenuSO menu, Func<T> data)
        {
            _contextMenus[owner] = new ContextMenuData { menuSO = menu, Data = () => data() };
            owner.OnToggleUI += HandleToggleUI;
            
            owner.EventHandler.BindUIEvent(owner, _ =>
            {
                ShowContextMenu(owner);
            }, EUIEvent.RightClick);
        }

        private void HandleToggleUI(UIBase ui, bool isActive)
        {
            if(!isActive)
                HideCurrentMenu();    
        }

        public void UnbindContextMenu(InteractableUI owner)
        {
            HideCurrentMenu();

            if (_contextMenus.ContainsKey(owner))
                _contextMenus.Remove(owner);

            owner.OnToggleUI -= HandleToggleUI;
            owner.EventHandler.ClearUIEvent(owner, EUIEvent.RightClick);
        }

        private void ShowContextMenu(InteractableUI owner)
        {
            if (!_contextMenus.TryGetValue(owner, out ContextMenuData menuData))
                return;

            var data = menuData.Data?.Invoke();
            if (data == null) return;

            HideCurrentMenu();
            
            if (_instances.TryGetValue(menuData.menuSO, out BaseContextMenu menu))
            {
                _currentMenu = menu;
                _currentMenu.ShowMenu(data);
                _currentMenu.OnAction += HideCurrentMenu;
                SetPosition(owner.Rect);
            }
            
            panel.EnableUI();
        }

        public void HideCurrentMenu()
        {
            if (_currentMenu == null)
                return;

            _currentMenu.OnAction -= HideCurrentMenu;
            _currentMenu.CloseMenu();
            _currentMenu = null;
            panel.DisableUI();
        }
        
        public bool HasActiveMenu() => _currentMenu != null;

        private void SetPosition(RectTransform rect)
        {
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            Vector3 topRight = corners[2];

            RectTransformUtility.ScreenPointToLocalPointInRectangle(menuRoot, 
                RectTransformUtility.WorldToScreenPoint(null, topRight), 
                null, out Vector2 localPoint);

            _currentMenu.Rect.anchoredPosition = new Vector2(localPoint.x, localPoint.y);
        }
    }
}