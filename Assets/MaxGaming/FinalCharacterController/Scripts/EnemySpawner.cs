using System.Collections.Generic;
using UnityEngine;

namespace MaxGaming.FinalCharacterController
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject[] enemyPrefabs; // z.B. Slime Prefab

        [Header("Spawn Around")]
        [SerializeField] private Transform player; // oder "center"
        [SerializeField] private float spawnRadius = 15f;
        [SerializeField] private float minSpawnDistance = 6f;

        [Header("Rules")]
        [SerializeField] private int maxAlive = 40;
        [SerializeField] private int startAlive = 3;

        [Header("Time Scaling")]
        [SerializeField] private float relaxedSeconds = 10f;
        [SerializeField] private float minSpawnInterval = 0.25f; // später schneller
        [SerializeField] private float maxSpawnInterval = 2.0f;  // am Anfang langsam

        [Header("Grounding")]
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float groundRayHeight = 50f;
        [SerializeField] private float groundRayDistance = 200f;

        private readonly HashSet<GameObject> _alive = new();
        private float _elapsed;
        private float _spawnTimer;

        public int AliveCount => _alive.Count;
        public float ElapsedSeconds => _elapsed;

        private void OnEnable()
        {
            EnemyHealth.OnAnyEnemyDied += HandleEnemyDied;
        }

        private void OnDisable()
        {
            EnemyHealth.OnAnyEnemyDied -= HandleEnemyDied;
        }

        private void Start()
        {
            // initial spawn
            for (int i = 0; i < startAlive; i++)
                TrySpawn();
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            // time-based continuous spawning
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                // gewünschte "spawn rate" wächst mit der Zeit
                _spawnTimer = GetCurrentSpawnInterval();

                // spawn so lange bis maxAlive erreicht ist (aber nur 1 pro tick)
                if (_alive.Count < GetTargetAliveCount())
                    TrySpawn();
            }

            // cleanup nulls (falls irgendwas destroyed wurde)
            _alive.RemoveWhere(go => go == null);
        }

        private void HandleEnemyDied(EnemyHealth dead)
        {
            // Enemy wird erst später destroyed -> wir zählen trotzdem "respawn sofort"
            // Deshalb: aus alive entfernen (falls drin)
            _alive.Remove(dead.gameObject);

            // sofort respawn, solange maxAlive nicht überschritten
            if (_alive.Count < maxAlive)
                TrySpawn();
        }

        private int GetTargetAliveCount()
        {
            // 0..10s: entspannt => z.B. 3-6 enemies
            // danach ramp up bis maxAlive
            if (_elapsed <= relaxedSeconds)
                return Mathf.Clamp(startAlive + Mathf.FloorToInt(_elapsed * 0.3f), startAlive, 6);

            // nach 10s: wächst schneller Richtung 40
            float t = (_elapsed - relaxedSeconds) / 60f; // nach 60s ~ nahe max
            t = Mathf.Clamp01(t);
            return Mathf.RoundToInt(Mathf.Lerp(6, maxAlive, t));
        }

        private float GetCurrentSpawnInterval()
        {
            // am Anfang langsam, dann schneller
            if (_elapsed <= relaxedSeconds)
                return maxSpawnInterval;

            float t = (_elapsed - relaxedSeconds) / 60f;
            t = Mathf.Clamp01(t);
            return Mathf.Lerp(maxSpawnInterval, minSpawnInterval, t);
        }

        private void TrySpawn()
        {
            if (enemyPrefabs.Length == 0 || player == null) return;
            if (_alive.Count >= maxAlive) return;

            if (!TryGetSpawnPosition(out Vector3 pos))
                return;

            // 🔥 zufälliges Monster aus Array
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
            _alive.Add(enemy);
        }

        private bool TryGetSpawnPosition(out Vector3 pos)
        {
            // ein paar Versuche, gültigen Punkt auf dem Boden zu finden
            for (int i = 0; i < 20; i++)
            {
                Vector2 r = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, spawnRadius);
                Vector3 candidate = player.position + new Vector3(r.x, 0f, r.y);

                // von oben nach unten auf Boden raycasten
                Vector3 rayStart = candidate + Vector3.up * groundRayHeight;
                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundRayDistance, groundLayers, QueryTriggerInteraction.Ignore))
                {
                    pos = hit.point;
                    return true;
                }
            }

            pos = default;
            return false;
        }
    }
}