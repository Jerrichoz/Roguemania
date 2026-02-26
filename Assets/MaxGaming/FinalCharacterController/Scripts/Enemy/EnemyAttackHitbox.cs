using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private string targetTag = "Player";

    private bool _active;
    private readonly HashSet<IDamageable> _hitThisSwing = new();

    public void Activate()
    {
        _active = true;
        _hitThisSwing.Clear();
        // Optional: Debug.Log("Hitbox ACTIVE");
    }

    public void Deactivate()
    {
        _active = false;
        // Optional: Debug.Log("Hitbox INACTIVE");
    }

    private void TryHit(Collider other)
    {
        Debug.Log("---- TryHit called ----");
        Debug.Log("Other Object: " + other.name);
        Debug.Log("Other Tag: " + other.tag);
        Debug.Log("Hitbox Active: " + _active);

        if (!_active)
        {
            Debug.Log("ABORT: Hitbox not active");
            return;
        }

        if (!other.CompareTag(targetTag))
        {
            Debug.Log($"ABORT: Tag mismatch. Expected '{targetTag}' but got '{other.tag}'");
            return;
        }

        Debug.Log("Tag matched!");

        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            Debug.Log("IDamageable FOUND on: " + other.name);

            if (_hitThisSwing.Contains(dmg))
            {
                Debug.Log("ABORT: Already hit this swing");
                return;
            }

            Debug.Log("Applying damage: " + damage);
            _hitThisSwing.Add(dmg);

            dmg.TakeDamage(damage, transform.root.gameObject);
        }
        else
        {
            Debug.Log("ABORT: No IDamageable found on object!");
        }
    }

    private void OnTriggerEnter(Collider other) => TryHit(other);
    private void OnTriggerStay(Collider other) => TryHit(other);
}