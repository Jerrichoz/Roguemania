using UnityEngine;
namespace MaxGaming.FinalCharacterController
{
    public class DamageNumberSpawner : MonoBehaviour
    {
        [SerializeField] private DamageNumber damageNumberPrefab;
        private static DamageNumberSpawner _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void Spawn(float amount, Vector3 worldPos)
        {
            if (_instance == null || _instance.damageNumberPrefab == null) return;
            var dn = Instantiate(_instance.damageNumberPrefab, worldPos, Quaternion.identity);
            dn.SetValue(amount);
        }
    }
}