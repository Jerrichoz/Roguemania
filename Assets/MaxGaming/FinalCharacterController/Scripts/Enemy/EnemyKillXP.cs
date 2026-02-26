using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class EnemyKillXP : MonoBehaviour
    {
        [SerializeField] private PlayerXP playerXp;
        [SerializeField] private float xpPerKill = 10f;

        private void Awake()
        {
            if (playerXp == null)
                playerXp = FindFirstObjectByType<PlayerXP>();
        }

        private void OnEnable()
        {
            EnemyHealth.OnAnyEnemyDied += HandleEnemyDied;
        }

        private void OnDisable()
        {
            EnemyHealth.OnAnyEnemyDied -= HandleEnemyDied;
        }

        private void HandleEnemyDied(EnemyHealth dead)
        {
            if (playerXp == null) return;
            playerXp.AddXp(xpPerKill);
        }
    }
}