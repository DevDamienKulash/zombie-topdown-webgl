using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] Transform muzzle; 
    [SerializeField] float range = 50f;
    [SerializeField] float fireRate = 8f;
    [SerializeField] int damage = 20;
    [SerializeField] LayerMask hitMask;

    [Header("FX Pool")]
    [SerializeField] TracerPool tracerPool;

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

        Vector3 origin = muzzle ? muzzle.position : transform.position + Vector3.up;
        Vector3 dir = transform.forward;

        Vector3 end = origin + dir * range;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, range, hitMask))
        {
            end = hit.point;
            hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(damage);
        }

        if (tracerPool != null)
        {
            var tracer = tracerPool.Get();
            tracer.SetOwner(tracerPool);
            tracer.Fire(origin, end);
        }
    }
}
