using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
[SerializeField] float attackRange = 1.6f;
[SerializeField] float attackCooldown = 0.9f;
[SerializeField] int damage = 10;
[SerializeField] string attackTrigger = "Attack"; // animator trigger name
[SerializeField] Animator animator; // optional


NavMeshAgent agent;
Transform player;
Health playerHealth;
float cd;


void Awake()
{
agent = GetComponent<NavMeshAgent>();
var pObj = GameObject.FindGameObjectWithTag("Player");
if (pObj)
{
player = pObj.transform;
playerHealth = pObj.GetComponent<Health>();
}
}


void OnEnable()
{
cd = 0f;
if (agent) agent.isStopped = false;
}


void Update()
{
if (!player) return;


// Chase
agent.SetDestination(player.position);


// Face target (optional — agent handles rotation too)
Vector3 to = player.position - transform.position; to.y = 0f;
if (to.sqrMagnitude > 0.001f)
transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to), 720f * Time.deltaTime);


// Attack when in range
cd -= Time.deltaTime;
if (cd <= 0f)
{
float dist = Vector3.Distance(transform.position, player.position);
if (dist <= attackRange)
{
cd = attackCooldown;
if (animator && !string.IsNullOrEmpty(attackTrigger)) animator.SetTrigger(attackTrigger);
// If no animation events, do direct hit:
TryHitNow();
}
}
}


// Hook this from an animation event at the impact frame, or rely on the direct call above.
public void TryHitNow()
{
if (!playerHealth) return;
float dist = Vector3.Distance(transform.position, player.position);
if (dist <= attackRange + 0.2f)
playerHealth.TakeDamage(damage);
}
}