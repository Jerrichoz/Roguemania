using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class EnemyHealth : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 20f;
        public float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log("TakeDamage called");
            Debug.Log($"HitFlash found? {GetComponentInChildren<HitFlash>() != null}");

            var flash = GetComponentInChildren<HitFlash>();
            if (flash != null) flash.Flash();


            if (currentHealth <= 0f)
                Die();
        }

        private void Die()
        {
            Debug.Log($"{name} died");
            Destroy(gameObject);
        }
    }
}