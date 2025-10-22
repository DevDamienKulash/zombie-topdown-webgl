using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Pools & Targets")]
    [SerializeField] ZombiePool zombiePool;     // <-- assign in scene (Object with ZombiePool component)
    [SerializeField] Transform player;

    [Header("Spawn Limits")]
    [SerializeField] int maxAlive = 30;
    [SerializeField] float minRadius = 12f;
    [SerializeField] float maxRadius = 22f;

    [Header("Spawn Curve (x=time(s), y=spawns/sec)")]
    [SerializeField] AnimationCurve spawnRateOverTime;

    float elapsed;
    int alive;

    void Start()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (spawnRateOverTime == null || spawnRateOverTime.length == 0)
        {
            spawnRateOverTime = new AnimationCurve(
                new Keyframe(0f,   0.4f),
                new Keyframe(30f,  0.8f),
                new Keyframe(60f,  1.2f),
                new Keyframe(120f, 2.0f)
            );
        }

        InvokeRepeating(nameof(TrySpawnTick), 0.5f, 0.25f);
    }

    void Update() => elapsed += Time.deltaTime;

    void TrySpawnTick()
    {
        if (!player || zombiePool == null || alive >= maxAlive) return;

        float spawnsPerSec = spawnRateOverTime.Evaluate(elapsed);
        float chance = spawnsPerSec * 0.25f; // tick every 0.25s
        if (Random.value < Mathf.Clamp01(chance))
            SpawnOne();
    }

    void SpawnOne()
{
    Vector3 pos = RandomRingPosition(player.position, minRadius, maxRadius);
    if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 3f, NavMesh.AllAreas))
    {
        var z = zombiePool.Get(); // pooled instance (ZombieAI root)

        // IMPORTANT: initialize the instance *before* letting it run
        var reset = z.GetComponent<ZombiePoolReset>();
        if (reset) reset.SpawnAt(hit.position, Quaternion.identity, zombiePool);
        else
        {
            // Fallback if missing reset script (not recommended):
            var agent = z.GetComponent<NavMeshAgent>();
            if (agent) { agent.enabled = true; agent.Warp(hit.position); agent.isStopped = false; agent.ResetPath(); }
            else       { z.transform.position = hit.position; }

            var col = z.GetComponent<Collider>();
            if (col) col.enabled = true;

            var anim = z.GetComponent<Animator>();
            if (anim) { anim.Rebind(); anim.Update(0f); anim.Play("Run", 0, 0f); }
        }

        z.gameObject.SetActive(true);
        alive++;

        // decrement alive when THIS instance dies (avoid stacking listeners)
        var h = z.GetComponent<Health>();
        if (h)
        {
            UnityEngine.Events.UnityAction handler = null;
            handler = () =>
            {
                alive = Mathf.Max(0, alive - 1);
                h.onDeath.RemoveListener(handler);
            };
            h.onDeath.AddListener(handler);
        }
    }
}


    static Vector3 RandomRingPosition(Vector3 center, float minR, float maxR)
    {
        float r = Random.Range(minR, maxR);
        float a = Random.Range(0f, Mathf.PI * 2f);
        return center + new Vector3(Mathf.Cos(a) * r, 0f, Mathf.Sin(a) * r);
    }
}
