using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaxGaming.FinalCharacterController
{
    public class PlayerXPHUD : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private PlayerXP playerXp;

        [Header("UI")]
        [SerializeField] private Image xpFill;          // Image Type = Filled (Horizontal)
        [SerializeField] private TMP_Text levelText;    // "Lv 1"
        [SerializeField] private TMP_Text xpText;       // optional: "10 / 50"

        private void Awake()
        {
            if (playerXp == null)
                playerXp = FindFirstObjectByType<PlayerXP>();
        }

        private void OnEnable()
        {
            if (playerXp == null) return;

            playerXp.OnXpChanged += UpdateXp;
            playerXp.OnLevelChanged += UpdateLevel;

            // initial paint
            UpdateLevel(playerXp.Level);
            UpdateXp(playerXp.CurrentXp, playerXp.XpToNextLevel);
        }

        private void OnDisable()
        {
            if (playerXp == null) return;

            playerXp.OnXpChanged -= UpdateXp;
            playerXp.OnLevelChanged -= UpdateLevel;
        }

        private void UpdateLevel(int level)
        {
            if (levelText != null)
                levelText.text = $"Lv {level}";
        }

        private void UpdateXp(float current, float required)
        {
            float fill = (required <= 0f) ? 0f : current / required;

            if (xpFill != null)
                xpFill.fillAmount = Mathf.Clamp01(fill);

            if (xpText != null)
                xpText.text = $"{Mathf.FloorToInt(current)} / {Mathf.CeilToInt(required)}";
        }
    }
}