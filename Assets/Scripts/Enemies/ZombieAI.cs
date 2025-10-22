using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class ZombieAI : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] float attackRange = 1.6f;
    [SerializeField] float attackCooldown = 0.9f;
    [SerializeField] int damage = 10;

    [Header("Animator Params / States")]
    [SerializeField] string attackTrigger = "Attack"; // Trigger used by Any State → Attack
    [SerializeField] string runState = "Run";         // Name of your Run state
    [SerializeField] float attackClipSeconds = 0.6f;  // Length of attack clip

    NavMeshAgent agent;
    Animator animator;
    Health health;

    Transform player;
    Health playerHealth;

    float cd;
    bool isDead;
    Coroutine attackCo;

    void Awake()
    {
        agent    = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health   = GetComponent<Health>();

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) { player = p.transform; playerHealth = p.GetComponent<Health>(); }

        agent.stoppingDistance = Mathf.Max(0f, attackRange - 0.1f);
        animator.applyRootMotion = false;
    }

    void OnEnable()
    {
        isDead = false;
        cd = 0f;
        if (agent) { agent.enabled = true; agent.isStopped = false; }
        if (health) health.onDeath.AddListener(OnDied);
    }

    void OnDisable()
    {
        if (health) health.onDeath.RemoveListener(OnDied);
        StopAttackCo();
    }

    void OnDied()
    {
        isDead = true;
        StopAttackCo();
        if (agent && agent.enabled)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
        enabled = false; // stop Update while dead
    }

    void Update()
    {
        if (isDead || !player) return;

        agent.SetDestination(player.position);

        cd -= Time.deltaTime;
        if (cd <= 0f)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange)
                StartAttack();
        }
    }

    void StartAttack()
    {
        if (isDead) return;

        cd = attackCooldown;

        if (agent && agent.enabled)
            agent.isStopped = true;

        // Drive animation via Trigger (matches your Animator transitions)
        animator.ResetTrigger(attackTrigger);
        animator.SetTrigger(attackTrigger);

        StopAttackCo();
        attackCo = StartCoroutine(CoAttack()); // timed impact + resume (works even w/o anim events)
    }

    IEnumerator CoAttack()
    {
        // Impact about mid-clip
        yield return new WaitForSeconds(attackClipSeconds * 0.45f);
        TryHitNow();

        // Finish and resume
        yield return new WaitForSeconds(attackClipSeconds * 0.55f);

        if (!isDead)
        {
            // Animator will usually transition Attack → Run via exit time, but forcing Run is safe:
            animator.Play(runState, 0, 0f);
            if (agent && agent.enabled) agent.isStopped = false;
        }
        attackCo = null;
    }

    void StopAttackCo()
    {
        if (attackCo != null) { StopCoroutine(attackCo); attackCo = null; }
    }

    // Optional to call from an animation event right at the hit frame
    public void TryHitNow()
    {
        if (isDead || !playerHealth) return;
        Vector3 d = player.position - transform.position; d.y = 0f;
        float max = attackRange + 0.2f;
        if (d.sqrMagnitude <= max * max)
            playerHealth.TakeDamage(damage);
    }
}
