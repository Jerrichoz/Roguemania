using UnityEngine;
namespace MaxGaming.FinalCharacterController
{
    [RequireComponent(typeof(CharacterController))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Chase")]
        public float moveSpeed = 2.2f;
        public float followRange = 25f;
        public float stopDistance = 1.6f;

        [Header("Gravity")]
        public float gravity = 30f;
        public float groundedStick = 2f;

        private CharacterController _cc;
        private Transform _player;
        private float _vy;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();

            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.transform;
            else Debug.LogError("EnemyController: No GameObject with tag 'Player' found.");
        }

        private void Update()
        {
            // --- Gravity (always) ---
            if (_cc.isGrounded && _vy < 0f) _vy = -groundedStick;
            _vy -= gravity * Time.deltaTime;

            // --- Horizontal chase ---
            Vector3 horizontal = Vector3.zero;

            if (_player != null)
            {
                Vector3 toPlayer = _player.position - transform.position;
                toPlayer.y = 0f;

                float distSqr = toPlayer.sqrMagnitude;

                if (distSqr <= followRange * followRange && distSqr >= stopDistance * stopDistance)
                {
                    Vector3 dir = toPlayer.normalized;
                    horizontal = dir * moveSpeed;

                    if (dir.sqrMagnitude > 0.0001f)
                        transform.forward = dir;
                }
            }

            // --- Move once per frame ---
            Vector3 v = horizontal;
            v.y = _vy;

            _cc.Move(v * Time.deltaTime);
        }
    }
}