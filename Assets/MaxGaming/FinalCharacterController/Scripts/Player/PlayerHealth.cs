using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged; // current, max
    public event Action OnDied;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float invulnTime = 0.35f;
    public float MaxHealth => maxHealth;
    public float CurrentHealth { get; private set; }

    private float _lastHitTime;
    private bool _dead;



    private void Awake()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        if (_dead) return;
        if (amount <= 0f) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount, GameObject source)
    {
        if (Time.time - _lastHitTime < invulnTime) return; // verhindert Multi-Hits in einem Frame/Loop
        _lastHitTime = Time.time;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        Debug.Log($"Player took {amount} damage from {source.name}. HP: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth == 0)
        {
            Debug.Log("Player died");
            // TODO: Death handling
        }
    }
}