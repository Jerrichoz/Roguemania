using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MaxGaming.FinalCharacterController
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Image fill;
        [SerializeField] private TMP_Text hpText; // optional
        [SerializeField] private float visibleSecondsAfterHit = 2f;

        private Coroutine _hideRoutine;

        private void Awake()
        {
            if (enemyHealth == null)
                enemyHealth = GetComponentInParent<EnemyHealth>();

            SetVisible(false);
        }

        private void OnEnable()
        {
            if (enemyHealth != null)
            {
                enemyHealth.OnHealthChanged += OnHealthChanged;
                OnHealthChanged(enemyHealth.currentHealth, enemyHealth.maxHealth);
            }
        }

        private void OnDisable()
        {
            if (enemyHealth != null)
                enemyHealth.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float current, float max)
        {
            if (fill != null)
                fill.fillAmount = (max <= 0f) ? 0f : current / max;

            if (hpText != null)
                hpText.text = $"{Mathf.CeilToInt(current)}";

            // show on hit, then auto-hide
            SetVisible(true);
            if (_hideRoutine != null) StopCoroutine(_hideRoutine);
            _hideRoutine = StartCoroutine(HideAfter());
        }

        private IEnumerator HideAfter()
        {
            yield return new WaitForSeconds(visibleSecondsAfterHit);
            SetVisible(false);
        }

        private void SetVisible(bool v)
        {
            gameObject.SetActive(v);
        }
    }
}