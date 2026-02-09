using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaxGaming.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerActionsInput : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        #region Variables

        public bool AttackPressed { get; private set; }
        public bool InteractPressed { get; private set; }
        #endregion



        #region Startup
        private void Awake()
        {

        }
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not init - can not enable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not init - can not disable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerActionMap.RemoveCallbacks(this);

        }
        #endregion
        #region Update
        private void Update()
        {

        }
        #endregion
        #region LateUpdate
        private void LateUpdate()
        {
            AttackPressed = false;
            InteractPressed = false;
        }


        #endregion
        #region Input Callbacks
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            AttackPressed = true;
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            InteractPressed = true;
        }
        #endregion


    }
}

