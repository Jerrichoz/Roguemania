using System;
using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class PlayerXP : MonoBehaviour
    {
        [Header("XP Tuning")]
        [SerializeField] private int startLevel = 1;

        [Tooltip("XP required to go from level 1 -> level 2")]
        [SerializeField] private float baseXpToLevelUp = 50f;

        [Tooltip("Linear increase per level (0.10 = +10% of base per level)")]
        [SerializeField] private float linearIncreasePerLevel = 0.10f;

        [Header("Debug / Current State")]
        [SerializeField] private int currentLevel;
        [SerializeField] private float currentXp;

        public int Level => currentLevel;
        public float CurrentXp => currentXp;
        public float XpToNextLevel => GetXpRequiredForNextLevel(currentLevel);

        public event Action<float, float> OnXpChanged; // currentXP, requiredXP
        public event Action<int> OnLevelChanged;

        private void Awake()
        {
            currentLevel = Mathf.Max(1, startLevel);
            currentXp = 0f;

            RaiseAll();
        }

        public void AddXp(float amount)
        {
            if (amount <= 0f) return;

            currentXp += amount;

            // Level up as many times as needed
            while (currentXp >= XpToNextLevel)
            {
                currentXp -= XpToNextLevel;
                currentLevel++;
                OnLevelChanged?.Invoke(currentLevel);
            }

            OnXpChanged?.Invoke(currentXp, XpToNextLevel);
        }

        public void ResetXpAndLevel()
        {
            currentLevel = Mathf.Max(1, startLevel);
            currentXp = 0f;
            RaiseAll();
        }

        private void RaiseAll()
        {
            OnLevelChanged?.Invoke(currentLevel);
            OnXpChanged?.Invoke(currentXp, XpToNextLevel);
        }

        private float GetXpRequiredForNextLevel(int level)
        {
            // Linear scaling: base * (1 + (level-1) * increase)
            // Level 1 -> base * (1 + 0) = base
            int levelIndex = Mathf.Max(0, level - 1);
            float required = baseXpToLevelUp * (1f + levelIndex * linearIncreasePerLevel);

            // Avoid tiny/negative values
            return Mathf.Max(1f, required);
        }
    }
}