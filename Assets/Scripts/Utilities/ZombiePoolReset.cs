using UnityEngine;
using UnityEngine.AI;

public class ZombiePoolReset : MonoBehaviour, IPoolable
{
    [SerializeField] int maxHPOnSpawn = 60;
    [SerializeField] string runState = "Run";

    Animator animator;
    NavMeshAgent agent;
    Collider col;
    Health h;
    ZombieAI ai;
    ZombiePoolToken token;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent    = GetComponent<NavMeshAgent>();
        col      = GetComponent<Collider>();
        h        = GetComponent<Health>();
        ai       = GetComponent<ZombieAI>();
        token    = GetComponent<ZombiePoolToken>();
    }

    // Call this from the spawner right after Get()
    public void SpawnAt(Vector3 position, Quaternion rotation, ZombiePool owningPool)
    {
        // Pool reference
        if (token) token.pool = owningPool;

        // Enable collider first (so raycasts work)
        if (col) col.enabled = true;

        // Ensure agent is enabled, then WARP (not just transform.position!)
        if (agent)
        {
            agent.enabled = true;
            agent.Warp(position);                  // sync internal agent pos to navmesh
            transform.rotation = rotation;
            agent.isStopped = false;
            agent.ResetPath();
        }
        else
        {
            // Fallback if no agent: just set transform
            transform.SetPositionAndRotation(position, rotation);
        }

        // Reset AI logic
        if (ai) ai.enabled = true;

        // Health full
        if (h)
        {
            var method = h.GetType().GetMethod("SetMaxAndFill");
            if (method != null) method.Invoke(h, new object[] { maxHPOnSpawn });
            // else: ensure your Health starts full another way
        }

        // Clean Animator state machine
        if (animator)
        {
            animator.applyRootMotion = false;
            animator.Rebind();
            animator.Update(0f);
            animator.Play(runState, 0, 0f);        // force into Run
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Die");
        }

        // Ensure layer is hittable
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    // If your pool calls these, keep them light
    public void OnSpawned()   { /* no-op; we use SpawnAt for deterministic ordering */ }
    public void OnDespawned() { /* no-op */ }
}
