using System.Runtime.CompilerServices;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private int damage=10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            HealthSystem zombie = other.GetComponent<HealthSystem>();
            zombie.TakeDamage(damage);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
