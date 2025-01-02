using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crogen.PowerfulInput
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Crogen/InputReader", order = 0)]
    public class InputReader : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
    {
        #region Input Event

        public event Action<Vector2, bool> MoveEvent;
        public event Action DashEvent;
        public event Action JumpEvent;
        public event Action AttackEvent;

        #endregion

        public Vector2 MousePosition { get; private set; }
        
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.UI.SetCallbacks(this);
            }
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.performed)
                DashEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(context.started)
                MoveEvent?.Invoke(context.ReadValue<Vector2>(), true);
            if(context.canceled)
                MoveEvent?.Invoke(context.ReadValue<Vector2>(), false);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                AttackEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                JumpEvent?.Invoke();            
        }

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }
    }
}