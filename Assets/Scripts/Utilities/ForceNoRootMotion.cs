using UnityEngine;

// Attach to any character that should NOT use root motion.
// Swallows any clip delta so CC/NavMeshAgent stays in control.
[DisallowMultipleComponent]
public class ForceNoRootMotion : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim) anim.applyRootMotion = false;
    }

    // Unity calls this if a clip has root motion; we swallow it.
    void OnAnimatorMove()
    {
        // Intentionally empty: prevents root deltas from moving the transform.
    }

    void Update()
    {
        if (anim && anim.applyRootMotion) anim.applyRootMotion = false; // belts & suspenders
    }
}
