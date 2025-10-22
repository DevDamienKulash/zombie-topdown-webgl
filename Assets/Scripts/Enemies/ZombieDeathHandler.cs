using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class ZombieDeathHandler : MonoBehaviour
{
    [SerializeField] string dieParam = "Die";     // Trigger
    [SerializeField] float dieClipSeconds = 1.1f; // set to actual clip length

    Animator animator;
    NavMeshAgent agent;
    Collider col;
    Health h;

    bool despawned;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent    = GetComponent<NavMeshAgent>();
        col      = GetComponent<Collider>();
        h        = GetComponent<Health>();
    }

    void OnEnable()
    {
        despawned = false;
        if (h) h.onDeath.AddListener(OnDeath);
    }

    void OnDisable()
    {
        if (h) h.onDeath.RemoveListener(OnDeath);
    }

    void OnDeath()
    {
        ScoreManager.Add(1);

        // Stop interactions immediately
        if (agent && agent.enabled)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            // disable agent so it can't move via other calls
            agent.enabled = false;
        }
        if (col) col.enabled = false;

        if (animator)
        {
            animator.ResetTrigger(dieParam);
            animator.SetTrigger(dieParam);
        }

        // Despawn after death animation
        StartCoroutine(DespawnAfter(dieClipSeconds));
    }

    IEnumerator DespawnAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Despawn();
    }

    // Optional: call this from the Die clip end via Animation Event
    public void Despawn()
    {
        if (despawned) return;
        despawned = true;

        var token = GetComponent<ZombiePoolToken>();
        if (token && token.pool)
        {
            token.pool.Release(GetComponent<ZombieAI>());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
