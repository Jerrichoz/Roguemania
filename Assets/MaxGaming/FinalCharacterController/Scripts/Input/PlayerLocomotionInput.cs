using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaxGaming.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region Variables
        [SerializeField] private bool holdToSrpint = true;

        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintToggledOn { get; private set; }
        public bool WalkToggledOn { get; private set; }
        #endregion



        #region Startup
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not init - can not enable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }
        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not init - can not disable");
                return;
            }
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);

        }
        #endregion
        #region Late Update Logic
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion

        #region Input Callbacks
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggledOn = holdToSrpint || !SprintToggledOn;
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSrpint && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                JumpPressed = true;
        }

        public void OnToggleWalk(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            WalkToggledOn = !WalkToggledOn;
        }
        #endregion
    }
}

