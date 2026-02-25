using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthHUD : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image hpFill; // optional

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateUI;
            playerHealth.OnDied += OnDied;
            UpdateUI(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateUI;
            playerHealth.OnDied -= OnDied;
        }
    }

    private void UpdateUI(float current, float max)
    {
        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";

        if (hpFill != null)
            hpFill.fillAmount = (max <= 0f) ? 0f : current / max;
    }

    private void OnDied()
    {
        // optional: show game over UI
        Debug.Log("Player died -> show Game Over");
    }
}