using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private BoxCollider hitbox;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider>();
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitbox.enabled || !other.CompareTag("Zombie"))
        {
            return;
        }

        HealthSystem zombie = other.GetComponentInParent<HealthSystem>();
        if (zombie == null || zombie.IsDead)
        {
            return;
        }

        zombie.TakeDamage(damage);
        Debug.Log($"Melee damage: {damage}");
        SetMeleeHitboxEnabled(false);
        SendMessageUpwards(
            "EndMeleeAttack",
            SendMessageOptions.DontRequireReceiver);
    }

    private void SetMeleeHitboxEnabled(bool enabled)
    {
        if (hitbox == null)
        {
            hitbox = GetComponent<BoxCollider>();
        }

        if (hitbox != null)
        {
            hitbox.enabled = enabled;
        }
    }
}
