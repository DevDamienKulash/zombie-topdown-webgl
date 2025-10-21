using UnityEngine;

[DisallowMultipleComponent]
public class SimpleTracer : MonoBehaviour
{
    [SerializeField] float defaultLifetime = 0.06f;
    [SerializeField] float defaultWidth = 0.03f;

    LineRenderer lr;
    float life;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (!lr) lr = gameObject.AddComponent<LineRenderer>();

        lr.enabled = true;
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        // If no material assigned in Inspector, fall back to Sprites/Default (unlit)
        if (lr.sharedMaterial == null)
            lr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));

        if (lr.widthMultiplier <= 0f)
            lr.widthMultiplier = defaultWidth;

        // Performance hygiene
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.generateLightingData = false;
        lr.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        lr.numCapVertices = 0;
        lr.numCornerVertices = 0;

        enabled = false; // only tick while alive
    }

    public void Fire(Vector3 from, Vector3 to, float lifetime = -1f)
    {
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
        life = (lifetime > 0f) ? lifetime : defaultLifetime;
        gameObject.SetActive(true);
        enabled = true;
    }

    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0f)
        {
            enabled = false;
            gameObject.SetActive(false); // ready for pooling reuse
        }
    }
}
