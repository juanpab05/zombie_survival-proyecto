using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieController : MonoBehaviour
{   
    public float lookRadius = 20f;

    public float attackRange = 1.0f;

    Transform target;
    NavMeshAgent navZombie;

    private Animator anim;
    private BoxCollider attackHitbox;
    private bool isDead;
    private bool attackInProgress;
    private Coroutine disableHitboxRoutine;
    private bool hasHitPlayer;

    [SerializeField] private float attackHitboxDuration = 0.4f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float attackCooldown = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        if (PlayerManager.instance == null)
        {
            Debug.LogError("PlayerManager instance was not found.", this);
            enabled = false;
            return;
        }

        if (PlayerManager.instance.Player == null)
        {
            Debug.LogError("Player is not assigned in PlayerManager.", this);
            enabled = false;
            return;
        }

        target = PlayerManager.instance.Player.transform;
        navZombie= GetComponent<NavMeshAgent>();
        attackHitbox = GetComponent<BoxCollider>();
        /*foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name.Contains("RightHand"))
            {
                attackHitbox = child.GetComponent<BoxCollider>();
                if (attackHitbox != null)
                {
                    break;
                }
            }
        }*/

        if (attackHitbox == null)
        {
            attackHitbox = GetComponentInChildren<BoxCollider>(true);
        }

        if (attackHitbox != null)
        {
            attackHitbox.isTrigger = true;
            attackHitbox.enabled = false;
        }
        else
        {
            Debug.LogError("Zombie attack BoxCollider was not found on this zombie or its bones.", this);
        }

        if (navZombie == null)
        {
            Debug.LogError("NavMeshAgent is missing from this zombie.", this);
            enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (target == null)
        {
            anim.SetFloat("Speed", 0.0f);
            return;
        }

        if (navZombie == null || !navZombie.isActiveAndEnabled || !navZombie.isOnNavMesh)
        {
            anim.SetFloat("Speed", 0.0f);
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            if (distance <= attackRange && !attackInProgress)
            {
                navZombie.isStopped = true;
                navZombie.ResetPath();

                Vector3 direction = target.position - transform.position;
                direction.y = 0f;
                if (direction.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }

                attackInProgress = true;
                StartCoroutine(PerformAttack());
            }
            else if (distance > attackRange)
            {
                navZombie.isStopped = false;
                navZombie.SetDestination(target.position);
            }
        }
        else
        {
            navZombie.isStopped = true;
            navZombie.ResetPath();
        }

        DetectPlayerHit();
        anim.SetFloat("Speed", navZombie.velocity.magnitude);
    }

    public void SetDead(bool dead)
    {
        isDead = dead;

        if (!isDead)
        {
            return;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", 0.0f);
        }

        if (navZombie != null && navZombie.isActiveAndEnabled)
        {
            navZombie.isStopped = true;
            navZombie.ResetPath();

        }

        if (attackHitbox != null)
        {
            DeactivateAttackHitbox();
        }
    }

    private IEnumerator PerformAttack()
    {
        anim.SetTrigger("Attack");
        yield return null;

        while (!isDead && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        if (!isDead && attackHitbox != null)
        {
            ActivateAttackHitbox();
        }

        while (!isDead && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        if (!isDead)
        {
            yield return new WaitForSeconds(attackCooldown);
        }

        attackInProgress = false;
    }

    private void ActivateAttackHitbox()
    {
        hasHitPlayer = false;
        attackHitbox.enabled = true;

        if (disableHitboxRoutine != null)
        {
            StopCoroutine(disableHitboxRoutine);
        }

        disableHitboxRoutine = StartCoroutine(DisableAttackHitboxAfterDelay());
    }

    private IEnumerator DisableAttackHitboxAfterDelay()
    {
        yield return new WaitForSeconds(attackHitboxDuration);
        DeactivateAttackHitbox();
    }

    private void DeactivateAttackHitbox()
    {
        if (disableHitboxRoutine != null)
        {
            StopCoroutine(disableHitboxRoutine);
            disableHitboxRoutine = null;
        }

        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }
        hasHitPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamagePlayer(other);
    }

    private void DetectPlayerHit()
    {
        if (attackHitbox == null || !attackHitbox.enabled)
        {
            return;
        }

        Collider[] overlaps = Physics.OverlapBox(
            attackHitbox.bounds.center,
            attackHitbox.bounds.extents,
            attackHitbox.transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide);

        foreach (Collider overlap in overlaps)
        {
            TryDamagePlayer(overlap);
            if (hasHitPlayer)
            {
                break;
            }
        }
    }

    private void TryDamagePlayer(Collider other)
    {
        if (attackHitbox == null || !attackHitbox.enabled || hasHitPlayer)
        {
            return;
        }

        HealthSystem playerHealth = other.GetComponentInParent<HealthSystem>();
        if (playerHealth == null || !playerHealth.CompareTag("Player") || playerHealth.IsDead)
        {
            return;
        }

        hasHitPlayer = true;
        playerHealth.TakeDamage(damage);
        DeactivateAttackHitbox();
    }
}
