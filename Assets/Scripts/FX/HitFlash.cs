using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HitFlash : MonoBehaviour
{
    [SerializeField] Color flashColor = Color.white;
    [SerializeField] float flashTime = 0.08f;

    Renderer r;
    Color original;
    float t;

    void Awake()
    {
        r = GetComponent<Renderer>();
        original = r.material.color; // uses instance so won’t affect shared
    }

    public void Ping()
    {
        t = flashTime;
        r.material.color = flashColor;
    }

    void Update()
    {
        if (t > 0f)
        {
            t -= Time.deltaTime;
            if (t <= 0f) r.material.color = original;
        }
    }
}
