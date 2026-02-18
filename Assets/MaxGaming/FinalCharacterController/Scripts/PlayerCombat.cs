using UnityEngine;
namespace MaxGaming.FinalCharacterController
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Hit Detection")]
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private float attackRadius = 1.2f;
        [SerializeField] private LayerMask enemyLayers;

        [Header("Damage")]
        [SerializeField] private float baseDamage = 10f;

        // Wird per Animation Event aufgerufen (Hit-Frame)
        public void DoAttackHit()
        {
            if (attackOrigin == null)
            {
                Debug.LogWarning("PlayerCombat: attackOrigin not set.");
                return;
            }

            var hits = Physics.OverlapSphere(
                attackOrigin.position,
                attackRadius,
                enemyLayers,
                QueryTriggerInteraction.Ignore
            );

            Debug.Log($"DoAttackHit: hits={hits.Length}");

            foreach (var c in hits)
            {
                var hp = c.GetComponentInParent<EnemyHealth>();
                if (hp != null)
                {
                    hp.TakeDamage(baseDamage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackOrigin == null) return;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}
