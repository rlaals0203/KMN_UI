using System;
using KMN.UI.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KMN.UI.ContextMenu
{
    public class BaseContextAction : InteractableUI { }
    
    [RequireComponent(typeof(Button))]
    public abstract class BaseContextAction<T> : BaseContextAction
    { 
        private TextMeshProUGUI _title;
        private Image _icon;
        private Button _contextButton;
        private T _data;
        
        [field: SerializeField] public ContextActionSO ContextActionSO { get; private set; }
        protected string HelpText => CheckCondition(_data) ? 
                ContextActionSO.actionText : 
                ContextActionSO.inactiveText;
        
        public event Action<T> OnContextAction;
        public event Action OnCallbackInvoked;

        protected override void Awake()
        {
            base.Awake();
            _contextButton = GetComponent<Button>();
            _title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            _icon = transform.Find("Icon").GetComponent<Image>();
        }

        public override void DisableUI(bool hasFade = false)
        {
            base.DisableUI(hasFade);
            gameObject.SetActive(false);
        }

        public override void EnableUI(bool hasFade = false)
        {
            base.EnableUI(hasFade);
            gameObject.SetActive(true);
        }

        public void Init(T data)
        {
            _data = data;

            EnableUI();
            ResetEvents();
            InitAction(data);
            BindTooltip(data);
            
            _title.text = HelpText;
            _icon.sprite = ContextActionSO.actionIcon;
            name = $"{typeof(T).Name}ContextAction";
        }

        private void BindTooltip(T data)
        {
            string tooltipDescription = CheckCondition(data) ? 
                ContextActionSO.activeDescription : 
                ContextActionSO.inactiveDescription;
            
            BindTooltip(() => tooltipDescription, 0.8f);
        }

        public void ResetEvents()
        {
            OnContextAction = null;
            OnCallbackInvoked = null;
            
            UnbindTooltip();
            _contextButton.onClick.RemoveAllListeners();
        }

        private void InitAction(T data)
        {
            OnContextAction += OnAction;
            _contextButton.onClick.AddListener(() =>
            {
                OnCallbackInvoked?.Invoke();
                OnContextAction?.Invoke(data);
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _contextButton.onClick.RemoveAllListeners();
        }

        public abstract bool CheckCondition(T data);
        public abstract void OnAction(T data);
        public virtual bool CanShow(T data) => true;
    }
}