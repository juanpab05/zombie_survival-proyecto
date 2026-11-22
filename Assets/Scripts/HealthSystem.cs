using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health = 100;

    private Animator anim;
    private bool deathTriggered;

    public bool IsDead { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0 && !deathTriggered)
        {
            Die();
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0 && !deathTriggered)
        {
            Die();
        }
        Debug.Log(health);
    }

    private void Die()
    {
        IsDead = true;
        deathTriggered = true;
        anim.SetTrigger("Death");
        SendMessage("SetDead", true, SendMessageOptions.DontRequireReceiver);
    }
}
