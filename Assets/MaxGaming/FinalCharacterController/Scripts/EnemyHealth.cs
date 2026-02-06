using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 20f;
    private float _hp;

    private void Awake() => _hp = maxHealth;

    public void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0f) Destroy(gameObject);
    }
}
