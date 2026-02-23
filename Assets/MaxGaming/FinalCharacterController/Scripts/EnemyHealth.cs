using System;
using System.Collections;
using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class EnemyHealth : MonoBehaviour
    {
        public static event Action<EnemyHealth> OnAnyEnemyDied;
        [Header("Health")]
        public float maxHealth = 20f;
        public float currentHealth;
        [Header("Animator")]
        [SerializeField] private Animator animator;
        [SerializeField] private string getHitTrigger = "getHit";
        [SerializeField] private string dizzyTrigger = "dizzy";
        [SerializeField] private string dieTrigger = "die";
        [Header("Tuning")]
        [SerializeField] private float getHitCooldown = 0.15f; // verhindert Hit-Spam
        [SerializeField] private float dizzyThreshold01 = 0.5f;
        [SerializeField] private float dizzyDuration = 1f;
        [SerializeField] private float destroyDelay = 0.6f; // später: anhand Clip-Länge
        private bool _isDead;
        private bool _dizzyUsed;
        private float _lastHitTime;

        private EnemyController _enemyController;
        private CharacterController _cc;

        private void Awake()
        {
            Debug.Log($"EnemyHealth Animator = {(animator != null ? animator.name : "NULL")}", this);

            currentHealth = maxHealth;
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
                Debug.Log($"Animator auto-found: {(animator != null ? animator.name : "NULL")}", this);
            }

            if (animator != null)
            {
                Debug.Log($"Animator={animator.name} Controller={animator.runtimeAnimatorController.name}", this);

                bool has = false;
                foreach (var p in animator.parameters)
                {
                    if (p.name == "getHit")
                    {
                        has = true;
                        Debug.Log($"Found param getHit type={p.type}", this);
                        break;
                    }
                }
                Debug.Log($"Has getHit param? {has}", this);
                animator.SetTrigger("getHit");
                Debug.Log("Triggered getHit", this);
            }

            _enemyController = GetComponent<EnemyController>();
            _cc = GetComponent<CharacterController>();
        }

        public void TakeDamage(float amount)
        {
            if (animator != null)
            {
                animator.SetTrigger("getHit");
                Debug.Log("Triggered getHit");
            }

            if (_isDead) return;
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0f);

            var flash = GetComponentInChildren<HitFlashSwap>();
            if (flash != null) flash.Flash();
            // GetHit trigger (mit cooldown)
            if (animator != null && Time.time - _lastHitTime > getHitCooldown)
            {
                _lastHitTime = Time.time;
                animator.ResetTrigger(getHitTrigger);
                animator.SetTrigger(getHitTrigger);
                StartCoroutine(LogStateNextFrame());
            }
            // Dizzy bei <=50% (einmalig)
            if (!_dizzyUsed && currentHealth <= maxHealth * dizzyThreshold01)
            {
                _dizzyUsed = true;
                StartCoroutine(DizzyRoutine());
            }


            if (currentHealth <= 0f)
                Die();
        }
        private IEnumerator LogStateNextFrame()
        {
            yield return null; // next frame
            var st = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log($"State after trigger: {st.shortNameHash} normalizedTime={st.normalizedTime}", this);
        }
        private IEnumerator DizzyRoutine()
        {
            // movement kurz aus
            if (_enemyController != null) _enemyController.enabled = false;

            if (animator != null)
            {
                animator.ResetTrigger(dizzyTrigger);
                animator.SetTrigger(dizzyTrigger);
            }

            yield return new WaitForSeconds(dizzyDuration);

            if (!_isDead && _enemyController != null)
                _enemyController.enabled = true;
        }
        private void Die()
        {
            if (_isDead) return;
            _isDead = true;

            // stop movement/physics interactions
            if (_enemyController != null) _enemyController.enabled = false;
            if (_cc != null) _cc.enabled = false;

            if (animator != null)
            {
                animator.ResetTrigger(dieTrigger);
                animator.SetTrigger(dieTrigger);
            }
            OnAnyEnemyDied?.Invoke(this);
            Destroy(gameObject, destroyDelay);
        }
    }
}