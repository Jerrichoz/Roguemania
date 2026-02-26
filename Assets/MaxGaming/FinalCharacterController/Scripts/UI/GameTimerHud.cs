using TMPro;
using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class GameTimerHUD : MonoBehaviour
    {
        [SerializeField] private GameTimer gameTimer;
        [SerializeField] private TMP_Text timerText;

        private void Awake()
        {
            if (gameTimer == null)
                gameTimer = FindFirstObjectByType<GameTimer>();
        }

        private void OnEnable()
        {
            if (gameTimer == null) return;
            gameTimer.OnTimerChanged += UpdateUI;

            UpdateUI(gameTimer.RemainingSeconds);
        }

        private void OnDisable()
        {
            if (gameTimer == null) return;
            gameTimer.OnTimerChanged -= UpdateUI;
        }

        private void UpdateUI(float remainingSeconds)
        {
            int total = Mathf.CeilToInt(remainingSeconds);
            int minutes = total / 60;
            int seconds = total % 60;

            if (timerText != null)
                timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}