using UnityEngine;
using UnityEngine.AI;


public class ZombieSpawner : MonoBehaviour
{
[SerializeField] ZombieAI zombiePrefab;
[SerializeField] Transform player;
[SerializeField] int maxAlive = 30;
[SerializeField] float minRadius = 12f;
[SerializeField] float maxRadius = 22f;
[SerializeField] AnimationCurve spawnRateOverTime; // x=time(s), y=spawns/sec


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
new Keyframe(0f, 0.4f), // 0.4 spawns/sec at start
new Keyframe(30f, 0.8f), // 30s → 0.8/s
new Keyframe(60f, 1.2f), // 60s → 1.2/s
new Keyframe(120f, 2.0f) // 120s → 2.0/s
);
}
InvokeRepeating(nameof(TrySpawnTick), 0.5f, 0.25f);
}


void Update() => elapsed += Time.deltaTime;


void TrySpawnTick()
{
if (!player || alive >= maxAlive) return;
float spawnsPerSec = spawnRateOverTime.Evaluate(elapsed);
float chance = spawnsPerSec * 0.25f; // because tick every 0.25s
if (Random.value < Mathf.Clamp01(chance)) SpawnOne();
}


void SpawnOne()
{
Vector3 pos = RandomRingPosition(player.position, minRadius, maxRadius);
if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 3f, NavMesh.AllAreas))
{
var z = Instantiate(zombiePrefab, hit.position, Quaternion.identity);
z.gameObject.SetActive(true);
alive++;
var h = z.GetComponent<Health>();
if (h)
{
h.onDeath.AddListener(() => { alive = Mathf.Max(0, alive - 1); });
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