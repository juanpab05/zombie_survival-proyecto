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

        //Extraer zombiecontroller
        ZombieController zombieController = other.GetComponentInParent<ZombieController>();
        if (zombieController == null)
        {
            Debug.LogError("ZombieController is missing from the hit zombie.", other);
            return;
        }

        HealthSystem zombie = other.GetComponentInParent<HealthSystem>();
        if (zombie == null || zombie.IsDead)
        {
            return;
        }

        //Preguntar al zombie controller si el zombie esta alertado o no. Si no esta alertado 
        //el ataque sigiloso realiza 100 de daño y mata al zombi instantaneamente.
        float damageToDeal = zombieController.IsAlerted ? damage : 100f;
        zombie.TakeDamage(damageToDeal);
        Debug.Log($"Melee damage: {damageToDeal}");
        
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
