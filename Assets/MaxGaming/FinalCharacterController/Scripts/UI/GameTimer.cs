using System;
using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class GameTimer : MonoBehaviour
    {
        [Header("Timer Settings")]
        [SerializeField] private float startSeconds = 120f; // 2 minutes

        [Header("State (Read Only)")]
        [SerializeField] private float remainingSeconds;
        [SerializeField] private bool isRunning = true;
        [SerializeField] private bool hasFinished = false;

        public float RemainingSeconds => remainingSeconds;
        public float StartSeconds => startSeconds;
        public bool IsRunning => isRunning;
        public bool HasFinished => hasFinished;

        public event Action<float> OnTimerChanged; // remaining seconds
        public event Action OnTimerFinished;

        private void Awake()
        {
            remainingSeconds = Mathf.Max(0f, startSeconds);
            OnTimerChanged?.Invoke(remainingSeconds);
        }

        private void Update()
        {
            if (!isRunning || hasFinished) return;

            remainingSeconds -= Time.deltaTime;

            if (remainingSeconds <= 0f)
            {
                remainingSeconds = 0f;
                hasFinished = true;

                OnTimerChanged?.Invoke(remainingSeconds);
                OnTimerFinished?.Invoke(); // Boss spawn hook
                return;
            }

            OnTimerChanged?.Invoke(remainingSeconds);
        }

        public void StartTimer()
        {
            isRunning = true;
        }

        public void PauseTimer()
        {
            isRunning = false;
        }

        public void ResetTimer()
        {
            hasFinished = false;
            remainingSeconds = Mathf.Max(0f, startSeconds);
            OnTimerChanged?.Invoke(remainingSeconds);
        }

        public void AddTime(float seconds)
        {
            if (seconds <= 0f) return;
            remainingSeconds += seconds;
            OnTimerChanged?.Invoke(remainingSeconds);
        }
    }
}