using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Transform muzzle; // optional; fallback = player pos + up
    [SerializeField] float range = 50f;
    [SerializeField] float fireRate = 8f; // shots per second
    [SerializeField] int damage = 20;
    [SerializeField] LayerMask hitMask; // include Enemy layer (and props if desired)
    [SerializeField] SimpleTracer tracerPrefab; // assign in Inspector


    float nextFire;
    Camera cam;


    void Awake() { cam = Camera.main; }


    public void OnFire(InputValue value)
    {
        if (value.isPressed) TryFire();
    }


    void Update()
    {
        if (Mouse.current.leftButton.isPressed) TryFire();
    }


    void TryFire()
    {
        if (Time.time < nextFire) return;
        nextFire = Time.time + 1f / fireRate;


        Vector3 origin = muzzle ? muzzle.position : transform.position + Vector3.up * 1f;
        Vector3 dir = transform.forward; // player faces mouse already


        Vector3 end = origin + dir * range;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hitMask))
        {
            end = hit.point;
            hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(damage);
            // TODO: spawn damage number + brief hit flash on enemy later
        }


        // Tracer (pooled by simple SetActive off when done)
        if (tracerPrefab)
        {
            var tracer = GetOrSpawnTracer();
            tracer.Fire(origin, end);
        }
    }


    SimpleTracer GetOrSpawnTracer()
    {
        // Minimal pool: try to find inactive child; else instantiate
        foreach (Transform child in transform)
        {
            var t = child.GetComponent<SimpleTracer>();
            if (t && !t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true);
                return t;
            }
        }
        return Instantiate(tracerPrefab, transform);
    }
}