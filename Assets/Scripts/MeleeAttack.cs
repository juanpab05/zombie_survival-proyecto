using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private Animator anim;
    private HealthSystem healthSystem;
    private Coroutine hitboxRoutine;
    private bool attackInProgress;

    private const float MeleeHitboxDuration = 1f;

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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackInProgress)
        {
            attackInProgress = true;
            anim.SetTrigger("Attack");
            BroadcastMessage(
                "SetMeleeHitboxEnabled",
                true,
                SendMessageOptions.DontRequireReceiver);
            hitboxRoutine = StartCoroutine(DisableMeleeHitboxAfterDelay());
        }
    }

    private IEnumerator DisableMeleeHitboxAfterDelay()
    {
        yield return new WaitForSeconds(MeleeHitboxDuration);
        EndMeleeAttack();
    }

    private void EndMeleeAttack()
    {
        if (hitboxRoutine != null)
        {
            StopCoroutine(hitboxRoutine);
            hitboxRoutine = null;
        }

        BroadcastMessage(
            "SetMeleeHitboxEnabled",
            false,
            SendMessageOptions.DontRequireReceiver);
        attackInProgress = false;
    }
}
