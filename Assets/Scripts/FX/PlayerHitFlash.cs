using UnityEngine;
using UnityEngine.UI;

public class PlayerHitFlash : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] Image flashImage;
    [SerializeField] float flashAlpha = 0.35f;
    [SerializeField] float fadeSpeed = 3f;

    float t;

    void Awake()
    {
        if (!playerHealth)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerHealth = p.GetComponent<Health>();
        }
    }

    void OnEnable()
    {
        if (playerHealth) playerHealth.onHealthChanged.AddListener(OnHPChanged);
    }

    void OnDisable()
    {
        if (playerHealth) playerHealth.onHealthChanged.RemoveListener(OnHPChanged);
    }

    void OnHPChanged(int current, int max)
    {
        // Only flash on damage (assumes your Health invokes on change for both up/down)
        t = flashAlpha;
        SetAlpha(t);
    }

    void Update()
    {
        if (t > 0f)
        {
            t = Mathf.MoveTowards(t, 0f, fadeSpeed * Time.unscaledDeltaTime);
            SetAlpha(t);
        }
    }

    void SetAlpha(float a)
    {
        if (!flashImage) return;
        var c = flashImage.color; c.a = a; flashImage.color = c;
    }
}
