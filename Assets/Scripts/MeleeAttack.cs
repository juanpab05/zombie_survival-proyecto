using System.Reflection;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private Animator anim;
    private HealthSystem healthSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSystem != null && healthSystem.IsDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack");
        }
    }
}
