using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAnimatorDriver : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float damp = 0.08f;
    CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!anim) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!anim || cc == null) return;
        var v = cc.velocity;                       // m/s
        float speedPlanar = new Vector2(v.x, v.z).magnitude;
        anim.SetFloat("Speed", speedPlanar, damp, Time.deltaTime);
        // Debug: uncomment to verify
        // Debug.Log($"[PlayerAnimatorDriver] Speed={speedPlanar:0.00}");
    }
}
