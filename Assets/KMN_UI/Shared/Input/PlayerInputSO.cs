using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KMN.Core
{
    [CreateAssetMenu(fileName = "playerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        private Controls _controls;

        public event Action OnESCPressed; 
        
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public void OnESC(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnESCPressed?.Invoke();
        }
    }
}