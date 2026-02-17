using UnityEngine;
namespace MaxGaming.FinalCharacterController
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Hit Detection")]
        public Transform attackOrigin;        // Empty GameObject vor dem Player
        public float attackRadius = 1.2f;
        public LayerMask enemyLayers;

        [Header("Combat")]
        public float baseDamage = 10f;
        public float attackCooldown = 0.4f;

        private float _cooldownTimer;

        private void Update()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;

            // Quick test input (new input system)
            if (Keyboard.current.leftMouseButton.wasPressedThisFrame)
            {
                TryAttack();
            }
        }

        public void TryAttack()
        {
            if (_cooldownTimer > 0f) return;
            _cooldownTimer = attackCooldown;

            // Detect enemies
            Collider[] hits = Physics.OverlapSphere(attackOrigin.position, attackRadius, enemyLayers, QueryTriggerInteraction.Ignore);

            Debug.Log($"Attack! hits={hits.Length}");

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<EnemyHealth>(out var hp))
                {
                    float damage = CalculateDamage();
                    hp.TakeDamage(damage);
                }
            }
        }

        private float CalculateDamage()
        {
            // MVP: nur BaseDamage, später Stats/Crit/Items
            return baseDamage;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackOrigin == null) return;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}
