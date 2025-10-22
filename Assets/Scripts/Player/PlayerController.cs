using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] LayerMask groundMask; // assign to Ground layer in Inspector
    [SerializeField] Camera cam; // auto-grab if null
    [SerializeField] Animator anim;
    [SerializeField] float animDamp = 0.08f;


    Vector2 moveInput;
    CharacterController cc;


    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!cam) cam = Camera.main;
    }


    // Input System (PlayerInput with Send Messages)
    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();


    void Update()
    {
        // Movement on XZ plane
        Vector3 desired = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        cc.SimpleMove(desired);


        // Face mouse (raycast to Ground)
        if (cam)
        {
            Ray r = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(r, out RaycastHit hit, 2000f, groundMask))
            {
                Vector3 dir = hit.point - transform.position;
                dir.y = 0f;
                if (dir.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(dir);
            }
        }
         if (anim) {
        var v = cc.velocity; // CharacterController velocity in m/s
        float planar = new Vector2(v.x, v.z).magnitude; // 0 when idle, ~moveSpeed when running
        anim.SetFloat("Speed", planar, animDamp, Time.deltaTime);
    }
    }
}