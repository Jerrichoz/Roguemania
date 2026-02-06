using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        public PlayerControls PlayerControls { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        }

        private void Osable()
        {
            PlayerControls.Disable();
        }
    }
}